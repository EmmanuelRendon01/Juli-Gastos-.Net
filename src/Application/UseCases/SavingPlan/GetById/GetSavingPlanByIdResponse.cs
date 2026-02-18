namespace JuliGastos.Application.UseCases.SavingPlan.GetById;

public record LinkedAccountDto(
    Guid AccountUuid,
    string AccountName,
    decimal CurrentBalance
);

public record GetSavingPlanByIdResponse(
    Guid PlanUuid,
    string Name,
    string? Description,
    decimal TargetAmount,
    DateTimeOffset TargetDate,
    string Status,
    List<LinkedAccountDto> LinkedAccounts,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt
);
