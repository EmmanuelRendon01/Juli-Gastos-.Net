using FluentValidation;

namespace JuliGastos.Application.UseCases.RecurringIncome.Update;

public class UpdateRecurringIncomeValidator : AbstractValidator<UpdateRecurringIncomeCommand>
{
    public UpdateRecurringIncomeValidator()
    {
        RuleFor(x => x.Uuid)
            .NotEmpty()
            .WithMessage("El UUID del ingreso es requerido");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("El nombre es requerido")
            .MaximumLength(100)
            .WithMessage("El nombre no puede exceder 100 caracteres");

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("La descripción no puede exceder 500 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("El monto debe ser mayor a cero");

        RuleFor(x => x.Frequency)
            .IsInEnum()
            .WithMessage("La frecuencia no es válida");

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate)
            .WithMessage("La fecha de fin debe ser posterior a la fecha de inicio")
            .When(x => x.StartDate.HasValue && x.EndDate.HasValue);
    }
}
