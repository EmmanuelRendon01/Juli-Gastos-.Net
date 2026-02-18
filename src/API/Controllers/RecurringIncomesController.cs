using System.Security.Claims;
using JuliGastos.Application.UseCases.RecurringIncome.Create;
using JuliGastos.Application.UseCases.RecurringIncome.Delete;
using JuliGastos.Application.UseCases.RecurringIncome.GetAll;
using JuliGastos.Application.UseCases.RecurringIncome.GetById;
using JuliGastos.Application.UseCases.RecurringIncome.Update;
using JuliGastos.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JuliGastos.API.Controllers;

[Authorize]
public class RecurringIncomesController : BaseApiController
{
    private readonly CreateRecurringIncomeHandler _createHandler;
    private readonly GetAllRecurringIncomesHandler _getAllHandler;
    private readonly GetRecurringIncomeByIdHandler _getByIdHandler;
    private readonly UpdateRecurringIncomeHandler _updateHandler;
    private readonly DeleteRecurringIncomeHandler _deleteHandler;

    public RecurringIncomesController(
        CreateRecurringIncomeHandler createHandler,
        GetAllRecurringIncomesHandler getAllHandler,
        GetRecurringIncomeByIdHandler getByIdHandler,
        UpdateRecurringIncomeHandler updateHandler,
        DeleteRecurringIncomeHandler deleteHandler)
    {
        _createHandler = createHandler;
        _getAllHandler = getAllHandler;
        _getByIdHandler = getByIdHandler;
        _updateHandler = updateHandler;
        _deleteHandler = deleteHandler;
    }

    [HttpPost]
    public async Task<ActionResult<CreateRecurringIncomeResponse>> Create([FromBody] CreateRecurringIncomeCommand command)
    {
        try
        {
            var userId = GetUserIdFromToken();
            var response = await _createHandler.Handle(command, userId);
            return CreatedAtAction(nameof(GetByUuid), new { uuid = response.IncomeUuid }, response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet]
    public async Task<ActionResult<GetAllRecurringIncomesResponse>> GetAll()
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
    public async Task<ActionResult<GetRecurringIncomeByIdResponse>> GetByUuid(Guid uuid)
    {
        try
        {
            var userId = GetUserIdFromToken();
            var response = await _getByIdHandler.Handle(uuid, userId);
            return Ok(response);
        }
        catch (RecurringIncomeNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{uuid:guid}")]
    public async Task<ActionResult<UpdateRecurringIncomeResponse>> Update(Guid uuid, [FromBody] UpdateRecurringIncomeCommand command)
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
        catch (RecurringIncomeNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{uuid:guid}")]
    public async Task<ActionResult<DeleteRecurringIncomeResponse>> Delete(Guid uuid)
    {
        try
        {
            var userId = GetUserIdFromToken();
            var response = await _deleteHandler.Handle(uuid, userId);
            return Ok(response);
        }
        catch (RecurringIncomeNotFoundException ex)
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
