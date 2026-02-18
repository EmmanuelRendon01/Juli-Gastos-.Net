namespace JuliGastos.Application.UseCases.SavingPlan.UpdateAccounts;

public record UpdateSavingPlanAccountsCommand(
    Guid PlanUuid,
    List<Guid> AccountUuids
);
