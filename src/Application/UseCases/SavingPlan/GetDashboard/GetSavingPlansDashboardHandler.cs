using JuliGastos.Application.Interfaces.Repositories;
using JuliGastos.Application.Interfaces.Services;
using JuliGastos.Domain.Enums;

namespace JuliGastos.Application.UseCases.SavingPlan.GetDashboard;

public class GetSavingPlansDashboardHandler
{
    private readonly ISavingPlanRepository _savingPlanRepository;
    private readonly ISavingCalculationService _calculationService;

    public GetSavingPlansDashboardHandler(
        ISavingPlanRepository savingPlanRepository,
        ISavingCalculationService calculationService)
    {
        _savingPlanRepository = savingPlanRepository;
        _calculationService = calculationService;
    }

    public async Task<GetSavingPlansDashboardResponse> Handle(long userId, CancellationToken cancellationToken = default)
    {
        // 1. Obtener todos los planes de ahorro del usuario
        var savingPlans = await _savingPlanRepository.GetAllByUserIdAsync(userId, cancellationToken);

        // 2. Calcular resumen para cada plan
        var planSummaries = new List<SavingPlanSummaryDto>();
        decimal totalSavings = 0;
        decimal totalTargetAmount = 0;
        int activePlansCount = 0;
        int completedPlansCount = 0;

        foreach (var plan in savingPlans)
        {
            // Calcular estado actual del plan
            var currentStatus = await _calculationService.CalculateCurrentStatusAsync(plan.Id, plan.TargetAmount, userId, cancellationToken);
            
            // Calcular proyección para saber si está on track
            var projection = await _calculationService.CalculateProjectionAsync(plan.Id, plan.TargetAmount, plan.TargetDate, userId, cancellationToken);

            // Calcular días restantes
            var daysRemaining = (int)(plan.TargetDate - DateTimeOffset.UtcNow).TotalDays;

            planSummaries.Add(new SavingPlanSummaryDto(
                plan.Uuid,
                plan.Name,
                plan.TargetAmount,
                plan.TargetDate,
                plan.Status.ToString(),
                currentStatus.TotalInAccounts,
                plan.TargetAmount - currentStatus.TotalInAccounts,
                (double)currentStatus.ProgressPercentage,
                daysRemaining > 0 ? daysRemaining : 0,
                projection.WillReachTarget
            ));

            // Acumular totales
            totalSavings += currentStatus.TotalInAccounts;
            totalTargetAmount += plan.TargetAmount;

            if (plan.Status == SavingPlanStatus.Active)
                activePlansCount++;
            else if (plan.Status == SavingPlanStatus.Completed)
                completedPlansCount++;
        }

        // 3. Obtener resumen financiero general
        var financialSummary = await _calculationService.CalculateFinancialSummaryAsync(userId, cancellationToken);

        // 4. Retornar respuesta
        return new GetSavingPlansDashboardResponse(
            planSummaries,
            financialSummary,
            totalSavings,
            totalTargetAmount,
            activePlansCount,
            completedPlansCount
        );
    }
}
