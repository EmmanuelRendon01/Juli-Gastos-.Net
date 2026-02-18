namespace JuliGastos.Application.UseCases.SavingPlan.Update;

public record UpdateSavingPlanResponse(
    Guid PlanUuid,
    string Name,
    string? Description,
    decimal TargetAmount,
    DateTimeOffset TargetDate,
    string Status,
    DateTimeOffset UpdatedAt
);
