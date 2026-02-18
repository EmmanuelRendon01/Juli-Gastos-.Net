using JuliGastos.Application.Interfaces.Repositories;

namespace JuliGastos.Application.UseCases.RecurringIncome.GetAll;

public class GetAllRecurringIncomesHandler
{
    private readonly IRecurringIncomeRepository _recurringIncomeRepository;

    public GetAllRecurringIncomesHandler(IRecurringIncomeRepository recurringIncomeRepository)
    {
        _recurringIncomeRepository = recurringIncomeRepository;
    }

    public async Task<GetAllRecurringIncomesResponse> Handle(long userId, CancellationToken cancellationToken = default)
    {
        // 1. Obtener todos los ingresos recurrentes del usuario
        var incomes = await _recurringIncomeRepository.GetAllByUserIdAsync(userId, cancellationToken);

        // 2. Mapear a DTOs
        var incomeDtos = incomes.Select(income => new RecurringIncomeDto(
            income.Uuid,
            income.Name,
            income.Description,
            income.Amount,
            income.Frequency.ToString(),
            income.StartDate,
            income.EndDate,
            income.IsActive,
            income.CreatedAt
        )).ToList();

        // 3. Retornar respuesta
        return new GetAllRecurringIncomesResponse(
            incomeDtos,
            incomeDtos.Count
        );
    }
}
