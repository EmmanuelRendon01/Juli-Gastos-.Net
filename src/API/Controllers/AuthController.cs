using JuliGastos.Application.UseCases.Auth.Register;
using Microsoft.AspNetCore.Mvc;

namespace JuliGastos.API.Controllers;

public class AuthController : BaseApiController
{
    private readonly RegisterHandler _registerHandler;

    public AuthController(RegisterHandler registerHandler)
    {
        _registerHandler = registerHandler;
    }

    [HttpPost("register")]
    public async Task<ActionResult<RegisterResponse>> Register([FromBody] RegisterCommand command)
    {
        try
        {
            var response = await _registerHandler.Handle(command);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
