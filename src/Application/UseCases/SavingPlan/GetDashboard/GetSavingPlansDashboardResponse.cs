using JuliGastos.Application.DTOs;

namespace JuliGastos.Application.UseCases.SavingPlan.GetDashboard;

public record SavingPlanSummaryDto(
    Guid PlanUuid,
    string PlanName,
    decimal TargetAmount,
    DateTimeOffset TargetDate,
    string Status,
    decimal CurrentSavings,
    decimal AmountRemaining,
    double ProgressPercentage,
    int DaysRemaining,
    bool IsOnTrack
);

public record GetSavingPlansDashboardResponse(
    List<SavingPlanSummaryDto> SavingPlans,
    FinancialSummary OverallFinancialSummary,
    decimal TotalSavings,
    decimal TotalTargetAmount,
    int ActivePlansCount,
    int CompletedPlansCount
);
