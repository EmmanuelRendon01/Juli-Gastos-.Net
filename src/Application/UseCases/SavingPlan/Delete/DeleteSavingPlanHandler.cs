using JuliGastos.Application.Interfaces.Repositories;
using JuliGastos.Domain.Exceptions;

namespace JuliGastos.Application.UseCases.SavingPlan.Delete;

public class DeleteSavingPlanHandler
{
    private readonly ISavingPlanRepository _savingPlanRepository;
    private readonly ISavingPlanAccountRepository _savingPlanAccountRepository;

    public DeleteSavingPlanHandler(
        ISavingPlanRepository savingPlanRepository,
        ISavingPlanAccountRepository savingPlanAccountRepository)
    {
        _savingPlanRepository = savingPlanRepository;
        _savingPlanAccountRepository = savingPlanAccountRepository;
    }

    public async Task<DeleteSavingPlanResponse> Handle(Guid uuid, long userId, CancellationToken cancellationToken = default)
    {
        // 1. Buscar el plan de ahorro por UUID
        var savingPlan = await _savingPlanRepository.GetByUuidAsync(uuid, userId, cancellationToken);

        // 2. Validar que existe
        if (savingPlan == null)
        {
            throw new SavingPlanNotFoundException(uuid);
        }

        // 3. Validar que pertenece al usuario autenticado
        if (savingPlan.UserId != userId)
        {
            throw new SavingPlanNotFoundException(uuid);
        }

        // 4. Eliminar las relaciones M:N primero (cascade delete podría manejar esto, pero lo hacemos explícito)
        await _savingPlanAccountRepository.DeleteBySavingPlanIdAsync(savingPlan.Id, cancellationToken);

        // 5. Eliminar el plan de ahorro
        await _savingPlanRepository.DeleteAsync(savingPlan.Id, cancellationToken);

        // 6. Retornar respuesta
        return new DeleteSavingPlanResponse(
            uuid,
            $"Plan de ahorro '{savingPlan.Name}' eliminado correctamente"
        );
    }
}
