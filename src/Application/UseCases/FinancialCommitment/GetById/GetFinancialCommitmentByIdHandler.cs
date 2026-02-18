using JuliGastos.Application.Interfaces.Repositories;
using JuliGastos.Domain.Exceptions;

namespace JuliGastos.Application.UseCases.FinancialCommitment.GetById;

public class GetFinancialCommitmentByIdHandler
{
    private readonly IFinancialCommitmentRepository _financialCommitmentRepository;

    public GetFinancialCommitmentByIdHandler(IFinancialCommitmentRepository financialCommitmentRepository)
    {
        _financialCommitmentRepository = financialCommitmentRepository;
    }

    public async Task<GetFinancialCommitmentByIdResponse> Handle(Guid uuid, long userId, CancellationToken cancellationToken = default)
    {
        // 1. Buscar el compromiso financiero por UUID
        var commitment = await _financialCommitmentRepository.GetByUuidAsync(uuid, userId, cancellationToken);

        // 2. Validar que existe
        if (commitment == null)
        {
            throw new FinancialCommitmentNotFoundException(uuid);
        }

        // 3. Validar que pertenece al usuario autenticado
        if (commitment.UserId != userId)
        {
            throw new FinancialCommitmentNotFoundException(uuid);
        }

        // 4. Retornar respuesta
        return new GetFinancialCommitmentByIdResponse(
            commitment.Uuid,
            commitment.Name,
            commitment.Amount,
            commitment.Frequency.ToString(),
            commitment.CategoryId,
            commitment.Category?.Name,
            commitment.Category?.Icon,
            commitment.Category?.Color,
            commitment.NextDueDate,
            commitment.IsActive,
            commitment.Notes,
            commitment.CreatedAt,
            commitment.UpdatedAt
        );
    }
}
