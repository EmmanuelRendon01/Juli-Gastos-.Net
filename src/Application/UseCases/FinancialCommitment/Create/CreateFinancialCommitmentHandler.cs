using JuliGastos.Application.Interfaces.Repositories;

namespace JuliGastos.Application.UseCases.FinancialCommitment.Create;

public class CreateFinancialCommitmentHandler
{
    private readonly IFinancialCommitmentRepository _commitmentRepository;
    private readonly ICategoryRepository _categoryRepository;

    public CreateFinancialCommitmentHandler(
        IFinancialCommitmentRepository commitmentRepository,
        ICategoryRepository categoryRepository)
    {
        _commitmentRepository = commitmentRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<CreateFinancialCommitmentResponse> Handle(
        CreateFinancialCommitmentCommand command,
        long userId,
        CancellationToken cancellationToken = default)
    {
        // 1. Validar que la categoría existe si se proporcionó
        string? categoryName = null;
        if (command.CategoryId.HasValue)
        {
            var category = await _categoryRepository.GetByIdAsync(command.CategoryId.Value);
            if (category != null)
            {
                categoryName = category.Name;
            }
        }

        // 2. Crear la entidad FinancialCommitment
        var commitment = new Domain.Models.FinancialCommitment
        {
            Uuid = Guid.NewGuid(),
            UserId = userId,
            Name = command.Name,
            Amount = command.Amount,
            Frequency = command.Frequency,
            CategoryId = command.CategoryId,
            NextDueDate = command.NextDueDate,
            IsActive = true,
            Notes = command.Notes,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };

        // 3. Guardar en la base de datos
        var savedCommitment = await _commitmentRepository.SaveAsync(commitment, cancellationToken);

        // 4. Retornar respuesta
        return new CreateFinancialCommitmentResponse(
            savedCommitment.Uuid,
            savedCommitment.Name,
            savedCommitment.Amount,
            savedCommitment.Frequency.ToString(),
            savedCommitment.CategoryId,
            categoryName,
            savedCommitment.NextDueDate,
            savedCommitment.IsActive,
            savedCommitment.Notes,
            savedCommitment.CreatedAt
        );
    }
}
