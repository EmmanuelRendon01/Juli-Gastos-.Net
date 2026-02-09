using FluentValidation;

namespace JuliGastos.Application.UseCases.Account.Create;

public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
{
    public CreateAccountCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Type)
            .IsInEnum();

        RuleFor(x => x.CurrencyCode)
            .NotEmpty()
            .Length(3)
            .Matches("^[A-Z]{3}$")
            .WithMessage("Currency code must be 3 uppercase letters (e.g., COP, USD)");

        RuleFor(x => x.InitialBalance)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Initial balance cannot be negative");

        RuleFor(x => x.MonthlyMaintenanceFee)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Monthly maintenance fee cannot be negative");
    }
}
