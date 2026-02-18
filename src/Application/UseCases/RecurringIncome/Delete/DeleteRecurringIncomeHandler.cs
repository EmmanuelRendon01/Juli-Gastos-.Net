using JuliGastos.Application.Interfaces.Repositories;
using JuliGastos.Domain.Exceptions;

namespace JuliGastos.Application.UseCases.RecurringIncome.Delete;

public class DeleteRecurringIncomeHandler
{
    private readonly IRecurringIncomeRepository _recurringIncomeRepository;

    public DeleteRecurringIncomeHandler(IRecurringIncomeRepository recurringIncomeRepository)
    {
        _recurringIncomeRepository = recurringIncomeRepository;
    }

    public async Task<DeleteRecurringIncomeResponse> Handle(Guid uuid, long userId, CancellationToken cancellationToken = default)
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

        // 4. Eliminar el ingreso
        await _recurringIncomeRepository.DeleteAsync(income.Id, cancellationToken);

        // 5. Retornar respuesta
        return new DeleteRecurringIncomeResponse(
            uuid,
            $"Ingreso recurrente '{income.Name}' eliminado correctamente"
        );
    }
}
