using JuliGastos.Application.Interfaces.Repositories;
using JuliGastos.Application.Interfaces.Services;
using JuliGastos.Domain.Exceptions;

namespace JuliGastos.Application.UseCases.SavingPlan.GetProjection;

public class GetSavingPlanProjectionHandler
{
    private readonly ISavingPlanRepository _savingPlanRepository;
    private readonly ISavingCalculationService _calculationService;

    public GetSavingPlanProjectionHandler(
        ISavingPlanRepository savingPlanRepository,
        ISavingCalculationService calculationService)
    {
        _savingPlanRepository = savingPlanRepository;
        _calculationService = calculationService;
    }

    public async Task<GetSavingPlanProjectionResponse> Handle(Guid planUuid, long userId, CancellationToken cancellationToken = default)
    {
        // 1. Buscar el plan de ahorro por UUID
        var savingPlan = await _savingPlanRepository.GetByUuidAsync(planUuid, userId, cancellationToken);

        // 2. Validar que existe
        if (savingPlan == null)
        {
            throw new SavingPlanNotFoundException(planUuid);
        }

        // 3. Validar que pertenece al usuario autenticado
        if (savingPlan.UserId != userId)
        {
            throw new SavingPlanNotFoundException(planUuid);
        }

        // 4. Calcular estado actual
        var currentStatus = await _calculationService.CalculateCurrentStatusAsync(savingPlan.Id, savingPlan.TargetAmount, userId, cancellationToken);

        // 5. Calcular proyección
        var projection = await _calculationService.CalculateProjectionAsync(savingPlan.Id, savingPlan.TargetAmount, savingPlan.TargetDate, userId, cancellationToken);

        // 6. Obtener resumen financiero
        var financialSummary = await _calculationService.CalculateFinancialSummaryAsync(userId, cancellationToken);

        // 7. Retornar respuesta
        return new GetSavingPlanProjectionResponse(
            savingPlan.Uuid,
            savingPlan.Name,
            savingPlan.TargetAmount,
            savingPlan.TargetDate,
            currentStatus,
            projection,
            financialSummary
        );
    }
}
