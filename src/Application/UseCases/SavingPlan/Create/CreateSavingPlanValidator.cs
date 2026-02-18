using FluentValidation;

namespace JuliGastos.Application.UseCases.SavingPlan.Create;

public class CreateSavingPlanValidator : AbstractValidator<CreateSavingPlanCommand>
{
    public CreateSavingPlanValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("El nombre es requerido")
            .MaximumLength(150)
            .WithMessage("El nombre no puede exceder 150 caracteres");

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("La descripción no puede exceder 500 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.TargetAmount)
            .GreaterThan(0)
            .WithMessage("El monto objetivo debe ser mayor a cero");

        RuleFor(x => x.TargetDate)
            .GreaterThan(DateTimeOffset.UtcNow)
            .WithMessage("La fecha objetivo debe ser futura");

        RuleFor(x => x.AccountUuids)
            .NotEmpty()
            .WithMessage("Debe seleccionar al menos una cuenta de ahorro");
    }
}
