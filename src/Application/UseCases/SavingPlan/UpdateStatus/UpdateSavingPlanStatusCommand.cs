using JuliGastos.Domain.Enums;

namespace JuliGastos.Application.UseCases.SavingPlan.UpdateStatus;

public record UpdateSavingPlanStatusCommand(
    Guid PlanUuid,
    SavingPlanStatus Status
);
