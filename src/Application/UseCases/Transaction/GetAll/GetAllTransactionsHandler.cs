using JuliGastos.Application.Interfaces.Repositories;

namespace JuliGastos.Application.UseCases.Transaction.GetAll;

public class GetAllTransactionsHandler
{
    private readonly ITransactionRepository _transactionRepository;
    
    public GetAllTransactionsHandler(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }
    
    public async Task<GetAllTransactionsResponse> Handle(long userId)
    {
        var transactions = await _transactionRepository.GetAllByUserIdAsync(userId);
        
        var transactionsDtos = transactions
            .OrderByDescending(t => t.CreatedAt)
            .Select(t => 
                
                new TransactionsDto(
                t.Uuid,
                t.AccountId,
                t.Type,
                t.Amount,
                t.Date,
                t.Description,
                t.IsRecurring
            ))
            .ToList();
        
        return new GetAllTransactionsResponse(transactionsDtos, transactionsDtos.Count);
    }
}