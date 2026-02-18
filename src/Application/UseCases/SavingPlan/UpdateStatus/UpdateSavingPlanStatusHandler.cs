using JuliGastos.Application.Interfaces.Repositories;
using JuliGastos.Domain.Exceptions;

namespace JuliGastos.Application.UseCases.SavingPlan.UpdateStatus;

public class UpdateSavingPlanStatusHandler
{
    private readonly ISavingPlanRepository _savingPlanRepository;

    public UpdateSavingPlanStatusHandler(ISavingPlanRepository savingPlanRepository)
    {
        _savingPlanRepository = savingPlanRepository;
    }

    public async Task<UpdateSavingPlanStatusResponse> Handle(UpdateSavingPlanStatusCommand command, long userId, CancellationToken cancellationToken = default)
    {
        // 1. Buscar el plan de ahorro por UUID
        var savingPlan = await _savingPlanRepository.GetByUuidAsync(command.PlanUuid, userId, cancellationToken);

        // 2. Validar que existe
        if (savingPlan == null)
        {
            throw new SavingPlanNotFoundException(command.PlanUuid);
        }

        // 3. Validar que pertenece al usuario autenticado
        if (savingPlan.UserId != userId)
        {
            throw new SavingPlanNotFoundException(command.PlanUuid);
        }

        // 4. Actualizar el estado
        savingPlan.Status = command.Status;
        savingPlan.UpdatedAt = DateTimeOffset.UtcNow;

        // 5. Guardar cambios
        var updatedPlan = await _savingPlanRepository.UpdateAsync(savingPlan, cancellationToken);

        // 6. Retornar respuesta
        return new UpdateSavingPlanStatusResponse(
            updatedPlan.Uuid,
            updatedPlan.Name,
            updatedPlan.Status.ToString(),
            $"Estado del plan '{updatedPlan.Name}' actualizado a '{updatedPlan.Status}'"
        );
    }
}
