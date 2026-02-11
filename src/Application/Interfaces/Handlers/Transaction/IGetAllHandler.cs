using JuliGastos.Application.UseCases.Transaction.GetAll;

namespace JuliGastos.Application.Interfaces.Handlers.Transaction;

public interface IGetAllHandler
{
    Task<GetAllTransactionsResponse> Handle(long userId);
}