namespace JuliGastos.Application.UseCases.RecurringIncome.Delete;

public record DeleteRecurringIncomeResponse(
    Guid IncomeUuid,
    string Message
);
