using System.Security.Claims;
using JuliGastos.Application.Interfaces.Handlers.Account;
using JuliGastos.Application.UseCases.Account.Create;
using JuliGastos.Application.UseCases.Account.GetAll;
using JuliGastos.Application.UseCases.Account.GetById;
using JuliGastos.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JuliGastos.API.Controllers;

[Authorize]
public class AccountsController : BaseApiController
{
    private readonly ICreateAccountHandler _createAccountHandler;
    private readonly IGetAllAccountsHandler _getAllAccountsHandler;
    private readonly IGetAccountByIdHandler _getAccountByIdHandler;

    public AccountsController(
        ICreateAccountHandler createAccountHandler,
        IGetAllAccountsHandler getAllAccountsHandler,
        IGetAccountByIdHandler getAccountByIdHandler)
    {
        _createAccountHandler = createAccountHandler;
        _getAllAccountsHandler = getAllAccountsHandler;
        _getAccountByIdHandler = getAccountByIdHandler;
    }

    [HttpPost]
    public async Task<ActionResult<CreateAccountResponse>> Create([FromBody] CreateAccountCommand command)
    {
        try
        {
            var userId = GetUserIdFromToken();
            var response = await _createAccountHandler.Handle(command, userId);
            return CreatedAtAction(nameof(GetByUuid), new { uuid = response.AccountUuid }, response);
        }
        catch (DuplicateAccountNameException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet]
    public async Task<ActionResult<GetAllAccountsResponse>> GetAll()
    {
        try
        {
            var userId = GetUserIdFromToken();
            var response = await _getAllAccountsHandler.Handle(userId);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{uuid:guid}")]
    public async Task<ActionResult<AccountDto>> GetByUuid(Guid uuid)
    {
        try
        {
            var userId = GetUserIdFromToken();
            var response = await _getAccountByIdHandler.Handle(uuid, userId);
            return Ok(response);
        }
        catch (AccountNotFoundException ex)
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
