using FluentValidation;

namespace JuliGastos.Application.UseCases.SavingPlan.UpdateAccounts;

public class UpdateSavingPlanAccountsValidator : AbstractValidator<UpdateSavingPlanAccountsCommand>
{
    public UpdateSavingPlanAccountsValidator()
    {
        RuleFor(x => x.PlanUuid)
            .NotEmpty()
            .WithMessage("El UUID del plan es requerido");

        RuleFor(x => x.AccountUuids)
            .NotEmpty()
            .WithMessage("Debe seleccionar al menos una cuenta de ahorro");
    }
}
