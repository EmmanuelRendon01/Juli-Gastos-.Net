using System.Security.Claims;
using JuliGastos.Application.UseCases.FinancialCommitment.Create;
using JuliGastos.Application.UseCases.FinancialCommitment.Delete;
using JuliGastos.Application.UseCases.FinancialCommitment.GetAll;
using JuliGastos.Application.UseCases.FinancialCommitment.GetById;
using JuliGastos.Application.UseCases.FinancialCommitment.Update;
using JuliGastos.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JuliGastos.API.Controllers;

[Authorize]
public class FinancialCommitmentsController : BaseApiController
{
    private readonly CreateFinancialCommitmentHandler _createHandler;
    private readonly GetAllFinancialCommitmentsHandler _getAllHandler;
    private readonly GetFinancialCommitmentByIdHandler _getByIdHandler;
    private readonly UpdateFinancialCommitmentHandler _updateHandler;
    private readonly DeleteFinancialCommitmentHandler _deleteHandler;

    public FinancialCommitmentsController(
        CreateFinancialCommitmentHandler createHandler,
        GetAllFinancialCommitmentsHandler getAllHandler,
        GetFinancialCommitmentByIdHandler getByIdHandler,
        UpdateFinancialCommitmentHandler updateHandler,
        DeleteFinancialCommitmentHandler deleteHandler)
    {
        _createHandler = createHandler;
        _getAllHandler = getAllHandler;
        _getByIdHandler = getByIdHandler;
        _updateHandler = updateHandler;
        _deleteHandler = deleteHandler;
    }

    [HttpPost]
    public async Task<ActionResult<CreateFinancialCommitmentResponse>> Create([FromBody] CreateFinancialCommitmentCommand command)
    {
        try
        {
            var userId = GetUserIdFromToken();
            var response = await _createHandler.Handle(command, userId);
            return CreatedAtAction(nameof(GetByUuid), new { uuid = response.Uuid }, response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet]
    public async Task<ActionResult<GetAllFinancialCommitmentsResponse>> GetAll()
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
    public async Task<ActionResult<GetFinancialCommitmentByIdResponse>> GetByUuid(Guid uuid)
    {
        try
        {
            var userId = GetUserIdFromToken();
            var response = await _getByIdHandler.Handle(uuid, userId);
            return Ok(response);
        }
        catch (FinancialCommitmentNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{uuid:guid}")]
    public async Task<ActionResult<UpdateFinancialCommitmentResponse>> Update(Guid uuid, [FromBody] UpdateFinancialCommitmentCommand command)
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
        catch (FinancialCommitmentNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{uuid:guid}")]
    public async Task<ActionResult<DeleteFinancialCommitmentResponse>> Delete(Guid uuid)
    {
        try
        {
            var userId = GetUserIdFromToken();
            var response = await _deleteHandler.Handle(uuid, userId);
            return Ok(response);
        }
        catch (FinancialCommitmentNotFoundException ex)
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
