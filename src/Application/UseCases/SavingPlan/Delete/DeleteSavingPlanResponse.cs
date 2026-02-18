namespace JuliGastos.Application.UseCases.SavingPlan.Delete;

public record DeleteSavingPlanResponse(
    Guid PlanUuid,
    string Message
);
