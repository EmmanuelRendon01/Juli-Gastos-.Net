using JuliGastos.Application.Interfaces.Repositories;

namespace JuliGastos.Application.UseCases.FinancialCommitment.GetAll;

public class GetAllFinancialCommitmentsHandler
{
    private readonly IFinancialCommitmentRepository _commitmentRepository;

    public GetAllFinancialCommitmentsHandler(IFinancialCommitmentRepository commitmentRepository)
    {
        _commitmentRepository = commitmentRepository;
    }

    public async Task<GetAllFinancialCommitmentsResponse> Handle(
        long userId,
        CancellationToken cancellationToken = default)
    {
        // 1. Obtener todos los compromisos del usuario
        var commitments = await _commitmentRepository.GetAllByUserIdAsync(userId, cancellationToken);

        // 2. Mapear a DTOs
        var commitmentDtos = commitments.Select(c => new FinancialCommitmentDto(
            c.Uuid,
            c.Name,
            c.Amount,
            c.Frequency.ToString(),
            c.CategoryId,
            c.Category?.Name,
            c.Category?.Icon,
            c.NextDueDate,
            c.IsActive,
            c.Notes,
            c.CreatedAt
        )).ToList();

        // 3. Contar activos
        var activeCount = commitments.Count(c => c.IsActive);

        // 4. Retornar respuesta
        return new GetAllFinancialCommitmentsResponse(
            commitmentDtos,
            commitments.Count,
            activeCount
        );
    }
}
