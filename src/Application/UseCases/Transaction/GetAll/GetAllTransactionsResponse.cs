using JuliGastos.Domain.Enums;

namespace JuliGastos.Application.UseCases.Transaction.GetAll;

public record TransactionsDto(
        Guid Uuid,
        Guid AccountId,
        TransactionType Type,
        decimal Amount,
        DateTimeOffset Date,
        string Description,
        bool IsRecurring
    );
    
public record GetAllTransactionsResponse(
    List<TransactionsDto> Transactions,
    int TotalCount
);