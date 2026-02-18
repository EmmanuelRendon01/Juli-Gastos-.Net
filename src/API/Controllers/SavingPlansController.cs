using System.Security.Claims;
using JuliGastos.Application.UseCases.SavingPlan.Create;
using JuliGastos.Application.UseCases.SavingPlan.Delete;
using JuliGastos.Application.UseCases.SavingPlan.GetAll;
using JuliGastos.Application.UseCases.SavingPlan.GetById;
using JuliGastos.Application.UseCases.SavingPlan.GetDashboard;
using JuliGastos.Application.UseCases.SavingPlan.GetProjection;
using JuliGastos.Application.UseCases.SavingPlan.Update;
using JuliGastos.Application.UseCases.SavingPlan.UpdateAccounts;
using JuliGastos.Application.UseCases.SavingPlan.UpdateStatus;
using JuliGastos.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JuliGastos.API.Controllers;

[Authorize]
public class SavingPlansController : BaseApiController
{
    private readonly CreateSavingPlanHandler _createHandler;
    private readonly GetAllSavingPlansHandler _getAllHandler;
    private readonly GetSavingPlanByIdHandler _getByIdHandler;
    private readonly UpdateSavingPlanHandler _updateHandler;
    private readonly DeleteSavingPlanHandler _deleteHandler;
    private readonly UpdateSavingPlanAccountsHandler _updateAccountsHandler;
    private readonly GetSavingPlanProjectionHandler _getProjectionHandler;
    private readonly GetSavingPlansDashboardHandler _getDashboardHandler;
    private readonly UpdateSavingPlanStatusHandler _updateStatusHandler;

    public SavingPlansController(
        CreateSavingPlanHandler createHandler,
        GetAllSavingPlansHandler getAllHandler,
        GetSavingPlanByIdHandler getByIdHandler,
        UpdateSavingPlanHandler updateHandler,
        DeleteSavingPlanHandler deleteHandler,
        UpdateSavingPlanAccountsHandler updateAccountsHandler,
        GetSavingPlanProjectionHandler getProjectionHandler,
        GetSavingPlansDashboardHandler getDashboardHandler,
        UpdateSavingPlanStatusHandler updateStatusHandler)
    {
        _createHandler = createHandler;
        _getAllHandler = getAllHandler;
        _getByIdHandler = getByIdHandler;
        _updateHandler = updateHandler;
        _deleteHandler = deleteHandler;
        _updateAccountsHandler = updateAccountsHandler;
        _getProjectionHandler = getProjectionHandler;
        _getDashboardHandler = getDashboardHandler;
        _updateStatusHandler = updateStatusHandler;
    }

    [HttpPost]
    public async Task<ActionResult<CreateSavingPlanResponse>> Create([FromBody] CreateSavingPlanCommand command)
    {
        try
        {
            var userId = GetUserIdFromToken();
            var response = await _createHandler.Handle(command, userId);
            return CreatedAtAction(nameof(GetByUuid), new { uuid = response.PlanUuid }, response);
        }
        catch (AccountNotOwnedByUserException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet]
    public async Task<ActionResult<GetAllSavingPlansResponse>> GetAll()
    {
        try
        {
            var userId = GetUserIdFromToken();
            var response = await _getAllHandler.Handle(userId);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{uuid:guid}")]
    public async Task<ActionResult<GetSavingPlanByIdResponse>> GetByUuid(Guid uuid)
    {
        try
        {
            var userId = GetUserIdFromToken();
            var response = await _getByIdHandler.Handle(uuid, userId);
            return Ok(response);
        }
        catch (SavingPlanNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{uuid:guid}")]
    public async Task<ActionResult<UpdateSavingPlanResponse>> Update(Guid uuid, [FromBody] UpdateSavingPlanCommand command)
    {
        try
        {
            // Validar que el UUID del comando coincide con el de la ruta
            if (uuid != command.Uuid)
            {
                return BadRequest(new { message = "El UUID de la ruta no coincide con el del comando" });
            }

            var userId = GetUserIdFromToken();
            var response = await _updateHandler.Handle(command, userId);
            return Ok(response);
        }
        catch (SavingPlanNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{uuid:guid}")]
    public async Task<ActionResult<DeleteSavingPlanResponse>> Delete(Guid uuid)
    {
        try
        {
            var userId = GetUserIdFromToken();
            var response = await _deleteHandler.Handle(uuid, userId);
            return Ok(response);
        }
        catch (SavingPlanNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{uuid:guid}/accounts")]
    public async Task<ActionResult<UpdateSavingPlanAccountsResponse>> UpdateAccounts(Guid uuid, [FromBody] UpdateSavingPlanAccountsCommand command)
    {
        try
        {
            // Validar que el UUID del comando coincide con el de la ruta
            if (uuid != command.PlanUuid)
            {
                return BadRequest(new { message = "El UUID de la ruta no coincide con el del comando" });
            }

            var userId = GetUserIdFromToken();
            var response = await _updateAccountsHandler.Handle(command, userId);
            return Ok(response);
        }
        catch (SavingPlanNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (AccountNotOwnedByUserException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{uuid:guid}/projection")]
    public async Task<ActionResult<GetSavingPlanProjectionResponse>> GetProjection(Guid uuid)
    {
        try
        {
            var userId = GetUserIdFromToken();
            var response = await _getProjectionHandler.Handle(uuid, userId);
            return Ok(response);
        }
        catch (SavingPlanNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("dashboard")]
    public async Task<ActionResult<GetSavingPlansDashboardResponse>> GetDashboard()
    {
        try
        {
            var userId = GetUserIdFromToken();
            var response = await _getDashboardHandler.Handle(userId);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPatch("{uuid:guid}/status")]
    public async Task<ActionResult<UpdateSavingPlanStatusResponse>> UpdateStatus(Guid uuid, [FromBody] UpdateSavingPlanStatusCommand command)
    {
        try
        {
            // Validar que el UUID del comando coincide con el de la ruta
            if (uuid != command.PlanUuid)
            {
                return BadRequest(new { message = "El UUID de la ruta no coincide con el del comando" });
            }

            var userId = GetUserIdFromToken();
            var response = await _updateStatusHandler.Handle(command, userId);
            return Ok(response);
        }
        catch (SavingPlanNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    private long GetUserIdFromToken()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                          ?? User.FindFirst("sub")?.Value;
        if (userIdClaim == null)
        {
            throw new UnauthorizedAccessException("User ID not found in token");
        }
        return long.Parse(userIdClaim);
    }
}
