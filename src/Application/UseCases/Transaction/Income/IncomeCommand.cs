namespace JuliGastos.Application.UseCases.Transaction.Income;

public record IncomeCommand
(
    Guid AccountId,
    long CategoryId,
    decimal Amount,
    DateTime Date,
    string Description,
    bool IsRecurring
);
    
        
