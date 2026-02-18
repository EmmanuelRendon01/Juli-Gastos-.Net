using FluentValidation;

namespace JuliGastos.Application.UseCases.SavingPlan.UpdateStatus;

public class UpdateSavingPlanStatusValidator : AbstractValidator<UpdateSavingPlanStatusCommand>
{
    public UpdateSavingPlanStatusValidator()
    {
        RuleFor(x => x.PlanUuid)
            .NotEmpty()
            .WithMessage("El UUID del plan es requerido");

        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage("El estado no es válido");
    }
}
