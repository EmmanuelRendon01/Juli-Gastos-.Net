using JuliGastos.Application.Interfaces.Repositories;
using JuliGastos.Domain.Exceptions;

namespace JuliGastos.Application.UseCases.FinancialCommitment.Delete;

public class DeleteFinancialCommitmentHandler
{
    private readonly IFinancialCommitmentRepository _financialCommitmentRepository;

    public DeleteFinancialCommitmentHandler(IFinancialCommitmentRepository financialCommitmentRepository)
    {
        _financialCommitmentRepository = financialCommitmentRepository;
    }

    public async Task<DeleteFinancialCommitmentResponse> Handle(Guid uuid, long userId, CancellationToken cancellationToken = default)
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

        // 4. Eliminar el compromiso
        await _financialCommitmentRepository.DeleteAsync(commitment.Id, cancellationToken);

        // 5. Retornar respuesta
        return new DeleteFinancialCommitmentResponse(
            uuid,
            $"Compromiso financiero '{commitment.Name}' eliminado correctamente"
        );
    }
}
