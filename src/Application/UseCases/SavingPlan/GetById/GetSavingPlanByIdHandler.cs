using JuliGastos.Application.Interfaces.Repositories;
using JuliGastos.Domain.Exceptions;

namespace JuliGastos.Application.UseCases.SavingPlan.GetById;

public class GetSavingPlanByIdHandler
{
    private readonly ISavingPlanRepository _savingPlanRepository;
    private readonly ISavingPlanAccountRepository _savingPlanAccountRepository;

    public GetSavingPlanByIdHandler(
        ISavingPlanRepository savingPlanRepository,
        ISavingPlanAccountRepository savingPlanAccountRepository)
    {
        _savingPlanRepository = savingPlanRepository;
        _savingPlanAccountRepository = savingPlanAccountRepository;
    }

    public async Task<GetSavingPlanByIdResponse> Handle(Guid uuid, long userId, CancellationToken cancellationToken = default)
    {
        // 1. Buscar el plan de ahorro por UUID
        var savingPlan = await _savingPlanRepository.GetByUuidAsync(uuid, userId, cancellationToken);

        // 2. Validar que existe
        if (savingPlan == null)
        {
            throw new SavingPlanNotFoundException(uuid);
        }

        // 3. Validar que pertenece al usuario autenticado
        if (savingPlan.UserId != userId)
        {
            throw new SavingPlanNotFoundException(uuid);
        }

        // 4. Obtener las cuentas vinculadas con detalles
        var linkedAccountsRelations = await _savingPlanAccountRepository.GetBySavingPlanIdAsync(savingPlan.Id, cancellationToken);
        var linkedAccountDtos = linkedAccountsRelations
            .Where(spa => spa.Account != null)
            .Select(spa => new LinkedAccountDto(
                spa.Account!.Uuid,
                spa.Account.Name,
                spa.Account.CurrentBalance
            )).ToList();

        // 5. Retornar respuesta
        return new GetSavingPlanByIdResponse(
            savingPlan.Uuid,
            savingPlan.Name,
            savingPlan.Description,
            savingPlan.TargetAmount,
            savingPlan.TargetDate,
            savingPlan.Status.ToString(),
            linkedAccountDtos,
            savingPlan.CreatedAt,
            savingPlan.UpdatedAt
        );
    }
}
