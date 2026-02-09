using JuliGastos.Application.UseCases.Auth.Login;
using JuliGastos.Application.UseCases.Auth.Logout;
using JuliGastos.Application.UseCases.Auth.Refresh;
using JuliGastos.Application.UseCases.Auth.Register;
using Microsoft.AspNetCore.Mvc;

namespace JuliGastos.API.Controllers;

public class AuthController : BaseApiController
{
    private readonly RegisterHandler _registerHandler;
    private readonly LoginHandler _loginHandler;
    private readonly RefreshHandler _refreshHandler;
    private readonly LogoutHandler _logoutHandler;
    private readonly IConfiguration _configuration;

    public AuthController(
        RegisterHandler registerHandler,
        LoginHandler loginHandler,
        RefreshHandler refreshHandler,
        LogoutHandler logoutHandler,
        IConfiguration configuration)
    {
        _registerHandler = registerHandler;
        _loginHandler = loginHandler;
        _refreshHandler = refreshHandler;
        _logoutHandler = logoutHandler;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<ActionResult<RegisterResponse>> Register([FromBody] RegisterCommand command)
    {
        try
        {
            var accessTokenExpirationMinutes = int.Parse(_configuration["Jwt:AccessTokenExpirationMinutes"] ?? "15");
            var refreshTokenExpirationDays = int.Parse(_configuration["Jwt:RefreshTokenExpirationDays"] ?? "7");
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
            
            var response = await _registerHandler.Handle(
                command, 
                accessTokenExpirationMinutes, 
                refreshTokenExpirationDays, 
                ipAddress, 
                userAgent);
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
            var accessTokenExpirationMinutes = int.Parse(_configuration["Jwt:AccessTokenExpirationMinutes"] ?? "15");
            var refreshTokenExpirationDays = int.Parse(_configuration["Jwt:RefreshTokenExpirationDays"] ?? "7");
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
            
            var response = await _loginHandler.Handle(
                command, 
                accessTokenExpirationMinutes, 
                refreshTokenExpirationDays, 
                ipAddress, 
                userAgent);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<RefreshResponse>> Refresh([FromBody] RefreshCommand command)
    {
        try
        {
            var accessTokenExpirationMinutes = int.Parse(_configuration["Jwt:AccessTokenExpirationMinutes"] ?? "15");
            var refreshTokenExpirationDays = int.Parse(_configuration["Jwt:RefreshTokenExpirationDays"] ?? "7");
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
            
            var response = await _refreshHandler.Handle(
                command, 
                accessTokenExpirationMinutes, 
                refreshTokenExpirationDays, 
                ipAddress, 
                userAgent);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("logout")]
    public async Task<ActionResult<LogoutResponse>> Logout([FromBody] LogoutCommand command)
    {
        try
        {
            var response = await _logoutHandler.Handle(command);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
