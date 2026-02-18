namespace JuliGastos.Application.UseCases.SavingPlan.Create;

public record CreateSavingPlanResponse(
    Guid PlanUuid,
    string Name,
    string? Description,
    decimal TargetAmount,
    DateTimeOffset TargetDate,
    string Status,
    List<Guid> LinkedAccountUuids,
    DateTimeOffset CreatedAt
);
