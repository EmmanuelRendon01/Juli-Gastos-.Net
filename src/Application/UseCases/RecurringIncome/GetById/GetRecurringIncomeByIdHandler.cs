using JuliGastos.Application.Interfaces.Repositories;
using JuliGastos.Domain.Exceptions;

namespace JuliGastos.Application.UseCases.RecurringIncome.GetById;

public class GetRecurringIncomeByIdHandler
{
    private readonly IRecurringIncomeRepository _recurringIncomeRepository;

    public GetRecurringIncomeByIdHandler(IRecurringIncomeRepository recurringIncomeRepository)
    {
        _recurringIncomeRepository = recurringIncomeRepository;
    }

    public async Task<GetRecurringIncomeByIdResponse> Handle(Guid uuid, long userId, CancellationToken cancellationToken = default)
    {
        // 1. Buscar el ingreso recurrente por UUID
        var income = await _recurringIncomeRepository.GetByUuidAsync(uuid, userId, cancellationToken);

        // 2. Validar que existe
        if (income == null)
        {
            throw new RecurringIncomeNotFoundException(uuid);
        }

        // 3. Validar que pertenece al usuario autenticado
        if (income.UserId != userId)
        {
            throw new RecurringIncomeNotFoundException(uuid);
        }

        // 4. Retornar respuesta
        return new GetRecurringIncomeByIdResponse(
            income.Uuid,
            income.Name,
            income.Description,
            income.Amount,
            income.Frequency.ToString(),
            income.StartDate,
            income.EndDate,
            income.IsActive,
            income.CreatedAt
        );
    }
}
