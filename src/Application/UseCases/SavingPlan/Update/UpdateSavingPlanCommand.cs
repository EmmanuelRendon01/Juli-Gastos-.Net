namespace JuliGastos.Application.UseCases.SavingPlan.Update;

public record UpdateSavingPlanCommand(
    Guid Uuid,
    string Name,
    string? Description,
    decimal TargetAmount,
    DateTimeOffset TargetDate
);
