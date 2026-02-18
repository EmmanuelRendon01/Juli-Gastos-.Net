namespace JuliGastos.Application.UseCases.SavingPlan.UpdateStatus;

public record UpdateSavingPlanStatusResponse(
    Guid PlanUuid,
    string PlanName,
    string NewStatus,
    string Message
);
