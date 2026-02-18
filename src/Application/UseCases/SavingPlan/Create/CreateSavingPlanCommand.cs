namespace JuliGastos.Application.UseCases.SavingPlan.Create;

public record CreateSavingPlanCommand(
    string Name,
    string? Description,
    decimal TargetAmount,
    DateTimeOffset TargetDate,
    List<Guid> AccountUuids
);
