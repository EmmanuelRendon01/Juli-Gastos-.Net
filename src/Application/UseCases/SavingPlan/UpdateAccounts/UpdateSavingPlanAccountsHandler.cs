using JuliGastos.Application.Interfaces.Repositories;
using JuliGastos.Domain.Exceptions;

namespace JuliGastos.Application.UseCases.SavingPlan.UpdateAccounts;

public class UpdateSavingPlanAccountsHandler
{
    private readonly ISavingPlanRepository _savingPlanRepository;
    private readonly ISavingPlanAccountRepository _savingPlanAccountRepository;
    private readonly IAccountRepository _accountRepository;

    public UpdateSavingPlanAccountsHandler(
        ISavingPlanRepository savingPlanRepository,
        ISavingPlanAccountRepository savingPlanAccountRepository,
        IAccountRepository accountRepository)
    {
        _savingPlanRepository = savingPlanRepository;
        _savingPlanAccountRepository = savingPlanAccountRepository;
        _accountRepository = accountRepository;
    }

    public async Task<UpdateSavingPlanAccountsResponse> Handle(UpdateSavingPlanAccountsCommand command, long userId, CancellationToken cancellationToken = default)
    {
        // 1. Buscar el plan de ahorro por UUID
        var savingPlan = await _savingPlanRepository.GetByUuidAsync(command.PlanUuid, userId, cancellationToken);

        // 2. Validar que existe
        if (savingPlan == null)
        {
            throw new SavingPlanNotFoundException(command.PlanUuid);
        }

        // 3. Validar que pertenece al usuario autenticado
        if (savingPlan.UserId != userId)
        {
            throw new SavingPlanNotFoundException(command.PlanUuid);
        }

        // 4. Validar que todas las cuentas existen y pertenecen al usuario
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

        // 5. Eliminar todas las relaciones existentes
        await _savingPlanAccountRepository.DeleteBySavingPlanIdAsync(savingPlan.Id, cancellationToken);

        // 6. Crear nuevas relaciones M:N con las cuentas
        foreach (var accountId in accountIds)
        {
            var savingPlanAccount = new Domain.Models.SavingPlanAccount
            {
                SavingPlanId = savingPlan.Id,
                AccountId = accountId,
                LinkedAt = DateTimeOffset.UtcNow,
                CreatedAt = DateTimeOffset.UtcNow
            };
            await _savingPlanAccountRepository.AddAsync(savingPlanAccount, cancellationToken);
        }

        // 7. Actualizar timestamp del plan
        savingPlan.UpdatedAt = DateTimeOffset.UtcNow;
        await _savingPlanRepository.UpdateAsync(savingPlan, cancellationToken);

        // 8. Retornar respuesta
        return new UpdateSavingPlanAccountsResponse(
            savingPlan.Uuid,
            savingPlan.Name,
            command.AccountUuids,
            $"Cuentas actualizadas correctamente para el plan '{savingPlan.Name}'"
        );
    }
}
