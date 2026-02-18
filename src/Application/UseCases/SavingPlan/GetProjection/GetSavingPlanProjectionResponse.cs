using JuliGastos.Application.DTOs;

namespace JuliGastos.Application.UseCases.SavingPlan.GetProjection;

public record GetSavingPlanProjectionResponse(
    Guid PlanUuid,
    string PlanName,
    decimal TargetAmount,
    DateTimeOffset TargetDate,
    SavingCurrentStatus CurrentStatus,
    SavingProjection Projection,
    FinancialSummary FinancialSummary
);
