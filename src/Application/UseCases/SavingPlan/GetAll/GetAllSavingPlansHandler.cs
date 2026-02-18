using JuliGastos.Application.Interfaces.Repositories;

namespace JuliGastos.Application.UseCases.SavingPlan.GetAll;

public class GetAllSavingPlansHandler
{
    private readonly ISavingPlanRepository _savingPlanRepository;
    private readonly ISavingPlanAccountRepository _savingPlanAccountRepository;

    public GetAllSavingPlansHandler(
        ISavingPlanRepository savingPlanRepository,
        ISavingPlanAccountRepository savingPlanAccountRepository)
    {
        _savingPlanRepository = savingPlanRepository;
        _savingPlanAccountRepository = savingPlanAccountRepository;
    }

    public async Task<GetAllSavingPlansResponse> Handle(long userId, CancellationToken cancellationToken = default)
    {
        // 1. Obtener todos los planes de ahorro del usuario
        var savingPlans = await _savingPlanRepository.GetAllByUserIdAsync(userId, cancellationToken);

        // 2. Mapear a DTOs y contar cuentas vinculadas
        var savingPlanDtos = new List<SavingPlanDto>();
        foreach (var plan in savingPlans)
        {
            var linkedAccounts = await _savingPlanAccountRepository.GetBySavingPlanIdAsync(plan.Id, cancellationToken);
            
            savingPlanDtos.Add(new SavingPlanDto(
                plan.Uuid,
                plan.Name,
                plan.Description,
                plan.TargetAmount,
                plan.TargetDate,
                plan.Status.ToString(),
                linkedAccounts.Count,
                plan.CreatedAt
            ));
        }

        // 3. Retornar respuesta
        return new GetAllSavingPlansResponse(
            savingPlanDtos,
            savingPlanDtos.Count
        );
    }
}
