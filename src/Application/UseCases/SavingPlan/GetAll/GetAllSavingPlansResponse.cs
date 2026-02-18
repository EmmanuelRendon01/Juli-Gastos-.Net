namespace JuliGastos.Application.UseCases.SavingPlan.GetAll;

public record SavingPlanDto(
    Guid PlanUuid,
    string Name,
    string? Description,
    decimal TargetAmount,
    DateTimeOffset TargetDate,
    string Status,
    int LinkedAccountsCount,
    DateTimeOffset CreatedAt
);

public record GetAllSavingPlansResponse(
    List<SavingPlanDto> SavingPlans,
    int TotalCount
);
