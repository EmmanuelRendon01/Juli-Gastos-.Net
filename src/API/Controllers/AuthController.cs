using JuliGastos.Application.UseCases.Auth.Login;
using JuliGastos.Application.UseCases.Auth.Register;
using Microsoft.AspNetCore.Mvc;

namespace JuliGastos.API.Controllers;

public class AuthController : BaseApiController
{
    private readonly RegisterHandler _registerHandler;
    private readonly LoginHandler _loginHandler;

    public AuthController(
        RegisterHandler registerHandler,
        LoginHandler loginHandler)
    {
        _registerHandler = registerHandler;
        _loginHandler = loginHandler;
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

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginCommand command)
    {
        try
        {
            var response = await _loginHandler.Handle(command);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
