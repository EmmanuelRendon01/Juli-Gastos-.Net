namespace JuliGastos.Application.UseCases.SavingPlan.UpdateAccounts;

public record UpdateSavingPlanAccountsResponse(
    Guid PlanUuid,
    string PlanName,
    List<Guid> UpdatedAccountUuids,
    string Message
);
