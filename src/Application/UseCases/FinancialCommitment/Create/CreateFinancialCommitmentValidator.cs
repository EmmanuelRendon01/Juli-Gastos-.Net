using FluentValidation;

namespace JuliGastos.Application.UseCases.FinancialCommitment.Create;

public class CreateFinancialCommitmentValidator : AbstractValidator<CreateFinancialCommitmentCommand>
{
    public CreateFinancialCommitmentValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters");
        
        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than 0");
        
        RuleFor(x => x.Frequency)
            .IsInEnum().WithMessage("Invalid frequency value");
        
        RuleFor(x => x.Notes)
            .MaximumLength(500).WithMessage("Notes must not exceed 500 characters")
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }
}
