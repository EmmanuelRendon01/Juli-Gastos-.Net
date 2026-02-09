using FluentValidation;

namespace JuliGastos.Application.UseCases.Auth.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6)
            .MaximumLength(100);

        RuleFor(x => x.FullName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.CurrencyCode)
            .Length(3);

        RuleFor(x => x.EmergencyFundMonths)
            .GreaterThan(0);
    }
}