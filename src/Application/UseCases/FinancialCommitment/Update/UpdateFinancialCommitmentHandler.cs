using JuliGastos.Application.Interfaces.Repositories;
using JuliGastos.Domain.Exceptions;

namespace JuliGastos.Application.UseCases.FinancialCommitment.Update;

public class UpdateFinancialCommitmentHandler
{
    private readonly IFinancialCommitmentRepository _financialCommitmentRepository;

    public UpdateFinancialCommitmentHandler(IFinancialCommitmentRepository financialCommitmentRepository)
    {
        _financialCommitmentRepository = financialCommitmentRepository;
    }

    public async Task<UpdateFinancialCommitmentResponse> Handle(UpdateFinancialCommitmentCommand command, long userId, CancellationToken cancellationToken = default)
    {
        // 1. Buscar el compromiso financiero por UUID
        var commitment = await _financialCommitmentRepository.GetByUuidAsync(command.Uuid, userId, cancellationToken);

        // 2. Validar que existe
        if (commitment == null)
        {
            throw new FinancialCommitmentNotFoundException(command.Uuid);
        }

        // 3. Validar que pertenece al usuario autenticado
        if (commitment.UserId != userId)
        {
            throw new FinancialCommitmentNotFoundException(command.Uuid);
        }

        // 4. Actualizar propiedades
        commitment.Name = command.Name;
        commitment.Description = command.Description;
        commitment.Amount = command.Amount;
        commitment.Frequency = command.Frequency;
        commitment.StartDate = command.StartDate;
        commitment.EndDate = command.EndDate;
        commitment.IsActive = command.IsActive;
        commitment.UpdatedAt = DateTimeOffset.UtcNow;

        // 5. Guardar cambios
        var updatedCommitment = await _financialCommitmentRepository.UpdateAsync(commitment, cancellationToken);

        // 6. Retornar respuesta
        return new UpdateFinancialCommitmentResponse(
            updatedCommitment.Uuid,
            updatedCommitment.Name,
            updatedCommitment.Description,
            updatedCommitment.Amount,
            updatedCommitment.Frequency.ToString(),
            updatedCommitment.StartDate,
            updatedCommitment.EndDate,
            updatedCommitment.IsActive,
            updatedCommitment.UpdatedAt
        );
    }
}
