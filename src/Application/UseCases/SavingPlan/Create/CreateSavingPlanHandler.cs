using JuliGastos.Application.Interfaces.Repositories;
using JuliGastos.Domain.Enums;
using JuliGastos.Domain.Exceptions;

namespace JuliGastos.Application.UseCases.SavingPlan.Create;

public class CreateSavingPlanHandler
{
    private readonly ISavingPlanRepository _savingPlanRepository;
    private readonly ISavingPlanAccountRepository _savingPlanAccountRepository;
    private readonly IAccountRepository _accountRepository;

    public CreateSavingPlanHandler(
        ISavingPlanRepository savingPlanRepository,
        ISavingPlanAccountRepository savingPlanAccountRepository,
        IAccountRepository accountRepository)
    {
        _savingPlanRepository = savingPlanRepository;
        _savingPlanAccountRepository = savingPlanAccountRepository;
        _accountRepository = accountRepository;
    }

    public async Task<CreateSavingPlanResponse> Handle(CreateSavingPlanCommand command, long userId, CancellationToken cancellationToken = default)
    {
        // 1. Validar que todas las cuentas existen y pertenecen al usuario
        var accountIds = new List<long>();
        foreach (var accountUuid in command.AccountUuids)
        {
            var account = await _accountRepository.GetByUuidAsync(accountUuid, userId);
            if (account == null)
            {
                throw new AccountNotOwnedByUserException(0, userId); // Account not found
            }
            if (account.UserId != userId)
            {
                throw new AccountNotOwnedByUserException(account.Id, userId);
            }
            accountIds.Add(account.Id);
        }

        // 2. Crear el plan de ahorro
        var savingPlan = new Domain.Models.SavingPlan
        {
            Uuid = Guid.NewGuid(),
            UserId = userId,
            Name = command.Name,
            Description = command.Description,
            TargetAmount = command.TargetAmount,
            TargetDate = command.TargetDate,
            Status = SavingPlanStatus.Active,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };

        // 3. Guardar el plan en la base de datos
        var savedPlan = await _savingPlanRepository.SaveAsync(savingPlan, cancellationToken);

        // 4. Crear las relaciones M:N con las cuentas
        foreach (var accountId in accountIds)
        {
            var savingPlanAccount = new Domain.Models.SavingPlanAccount
            {
                SavingPlanId = savedPlan.Id,
                AccountId = accountId,
                LinkedAt = DateTimeOffset.UtcNow,
                CreatedAt = DateTimeOffset.UtcNow
            };
            await _savingPlanAccountRepository.AddAsync(savingPlanAccount, cancellationToken);
        }

        // 5. Retornar respuesta
        return new CreateSavingPlanResponse(
            savedPlan.Uuid,
            savedPlan.Name,
            savedPlan.Description,
            savedPlan.TargetAmount,
            savedPlan.TargetDate,
            savedPlan.Status.ToString(),
            command.AccountUuids,
            savedPlan.CreatedAt
        );
    }
}
