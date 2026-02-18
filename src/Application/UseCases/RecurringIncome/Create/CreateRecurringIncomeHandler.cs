using JuliGastos.Application.Interfaces.Repositories;

namespace JuliGastos.Application.UseCases.RecurringIncome.Create;

public class CreateRecurringIncomeHandler
{
    private readonly IRecurringIncomeRepository _recurringIncomeRepository;

    public CreateRecurringIncomeHandler(IRecurringIncomeRepository recurringIncomeRepository)
    {
        _recurringIncomeRepository = recurringIncomeRepository;
    }

    public async Task<CreateRecurringIncomeResponse> Handle(CreateRecurringIncomeCommand command, long userId, CancellationToken cancellationToken = default)
    {
        // 1. Crear entidad de ingreso recurrente
        var recurringIncome = new Domain.Models.RecurringIncome
        {
            Uuid = Guid.NewGuid(),
            UserId = userId,
            Name = command.Name,
            Description = command.Description,
            Amount = command.Amount,
            Frequency = command.Frequency,
            StartDate = command.StartDate,
            EndDate = command.EndDate,
            IsActive = true,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };

        // 2. Guardar en la base de datos
        var savedIncome = await _recurringIncomeRepository.SaveAsync(recurringIncome, cancellationToken);

        // 3. Retornar respuesta
        return new CreateRecurringIncomeResponse(
            savedIncome.Uuid,
            savedIncome.Name,
            savedIncome.Description,
            savedIncome.Amount,
            savedIncome.Frequency.ToString(),
            savedIncome.StartDate,
            savedIncome.EndDate,
            savedIncome.IsActive,
            savedIncome.CreatedAt
        );
    }
}
