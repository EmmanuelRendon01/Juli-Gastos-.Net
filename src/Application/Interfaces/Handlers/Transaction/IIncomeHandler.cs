namespace JuliGastos.Application.Interfaces.Handlers.Transaction;
using JuliGastos.Application.UseCases.Transaction.Income;

public interface IIncomeHandler
{
    Task<IncomeResponse> Handle(IncomeCommand command, long userId);
}