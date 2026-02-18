using JuliGastos.Application.Interfaces.Repositories;
using JuliGastos.Domain.Exceptions;

namespace JuliGastos.Application.UseCases.RecurringIncome.Update;

public class UpdateRecurringIncomeHandler
{
    private readonly IRecurringIncomeRepository _recurringIncomeRepository;

    public UpdateRecurringIncomeHandler(IRecurringIncomeRepository recurringIncomeRepository)
    {
        _recurringIncomeRepository = recurringIncomeRepository;
    }

    public async Task<UpdateRecurringIncomeResponse> Handle(UpdateRecurringIncomeCommand command, long userId, CancellationToken cancellationToken = default)
    {
        // 1. Buscar el ingreso recurrente por UUID
        var income = await _recurringIncomeRepository.GetByUuidAsync(command.Uuid, userId, cancellationToken);

        // 2. Validar que existe
        if (income == null)
        {
            throw new RecurringIncomeNotFoundException(command.Uuid);
        }

        // 3. Validar que pertenece al usuario autenticado
        if (income.UserId != userId)
        {
            throw new RecurringIncomeNotFoundException(command.Uuid);
        }

        // 4. Actualizar propiedades
        income.Name = command.Name;
        income.Description = command.Description;
        income.Amount = command.Amount;
        income.Frequency = command.Frequency;
        income.StartDate = command.StartDate;
        income.EndDate = command.EndDate;
        income.IsActive = command.IsActive;
        income.UpdatedAt = DateTimeOffset.UtcNow;

        // 5. Guardar cambios
        var updatedIncome = await _recurringIncomeRepository.UpdateAsync(income, cancellationToken);

        // 6. Retornar respuesta
        return new UpdateRecurringIncomeResponse(
            updatedIncome.Uuid,
            updatedIncome.Name,
            updatedIncome.Description,
            updatedIncome.Amount,
            updatedIncome.Frequency.ToString(),
            updatedIncome.StartDate,
            updatedIncome.EndDate,
            updatedIncome.IsActive,
            updatedIncome.UpdatedAt
        );
    }
}
