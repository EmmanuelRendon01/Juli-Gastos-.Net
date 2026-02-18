using JuliGastos.Application.Interfaces.Repositories;
using JuliGastos.Domain.Exceptions;

namespace JuliGastos.Application.UseCases.SavingPlan.Update;

public class UpdateSavingPlanHandler
{
    private readonly ISavingPlanRepository _savingPlanRepository;

    public UpdateSavingPlanHandler(ISavingPlanRepository savingPlanRepository)
    {
        _savingPlanRepository = savingPlanRepository;
    }

    public async Task<UpdateSavingPlanResponse> Handle(UpdateSavingPlanCommand command, long userId, CancellationToken cancellationToken = default)
    {
        // 1. Buscar el plan de ahorro por UUID
        var savingPlan = await _savingPlanRepository.GetByUuidAsync(command.Uuid, userId, cancellationToken);

        // 2. Validar que existe
        if (savingPlan == null)
        {
            throw new SavingPlanNotFoundException(command.Uuid);
        }

        // 3. Validar que pertenece al usuario autenticado
        if (savingPlan.UserId != userId)
        {
            throw new SavingPlanNotFoundException(command.Uuid);
        }

        // 4. Actualizar propiedades
        savingPlan.Name = command.Name;
        savingPlan.Description = command.Description;
        savingPlan.TargetAmount = command.TargetAmount;
        savingPlan.TargetDate = command.TargetDate;
        savingPlan.UpdatedAt = DateTimeOffset.UtcNow;

        // 5. Guardar cambios
        var updatedPlan = await _savingPlanRepository.UpdateAsync(savingPlan, cancellationToken);

        // 6. Retornar respuesta
        return new UpdateSavingPlanResponse(
            updatedPlan.Uuid,
            updatedPlan.Name,
            updatedPlan.Description,
            updatedPlan.TargetAmount,
            updatedPlan.TargetDate,
            updatedPlan.Status.ToString(),
            updatedPlan.UpdatedAt
        );
    }
}
