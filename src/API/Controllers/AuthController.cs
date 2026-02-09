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

            // Establecer cookies HTTP-Only para los tokens
            SetTokenCookies(response.AccessToken, response.RefreshToken, 
                accessTokenExpirationMinutes, refreshTokenExpirationDays);

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

            // Establecer cookies HTTP-Only para los tokens
            SetTokenCookies(response.AccessToken, response.RefreshToken, 
                accessTokenExpirationMinutes, refreshTokenExpirationDays);

            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<RefreshResponse>> Refresh()
    {
        try
        {
            // Leer el refresh token desde la cookie
            var refreshToken = HttpContext.Request.Cookies["refreshToken"];
            
            if (string.IsNullOrEmpty(refreshToken))
            {
                return BadRequest(new { message = "Refresh token not found in cookies" });
            }

            var accessTokenExpirationMinutes = int.Parse(_configuration["Jwt:AccessTokenExpirationMinutes"] ?? "15");
            var refreshTokenExpirationDays = int.Parse(_configuration["Jwt:RefreshTokenExpirationDays"] ?? "7");
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
            
            var command = new RefreshCommand(refreshToken);
            var response = await _refreshHandler.Handle(
                command, 
                accessTokenExpirationMinutes, 
                refreshTokenExpirationDays, 
                ipAddress, 
                userAgent);

            // Establecer nuevas cookies HTTP-Only para los tokens rotados
            SetTokenCookies(response.AccessToken, response.RefreshToken, 
                accessTokenExpirationMinutes, refreshTokenExpirationDays);

            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("logout")]
    public async Task<ActionResult<LogoutResponse>> Logout()
    {
        try
        {
            // Leer el refresh token desde la cookie
            var refreshToken = HttpContext.Request.Cookies["refreshToken"];
            
            if (string.IsNullOrEmpty(refreshToken))
            {
                return BadRequest(new { message = "Refresh token not found in cookies" });
            }

            var command = new LogoutCommand(refreshToken);
            var response = await _logoutHandler.Handle(command);

            // Eliminar las cookies
            ClearTokenCookies();

            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // Métodos auxiliares para manejo de cookies
    private void SetTokenCookies(string accessToken, string refreshToken, int accessTokenMinutes, int refreshTokenDays)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true, // Solo HTTPS en producción
            SameSite = SameSiteMode.Strict,
            Domain = null // Permitir subdominios si es necesario
        };

        // Cookie para access token
        var accessTokenCookieOptions = new CookieOptions
        {
            HttpOnly = cookieOptions.HttpOnly,
            Secure = cookieOptions.Secure,
            SameSite = cookieOptions.SameSite,
            Domain = cookieOptions.Domain,
            Expires = DateTimeOffset.UtcNow.AddMinutes(accessTokenMinutes)
        };
        HttpContext.Response.Cookies.Append("accessToken", accessToken, accessTokenCookieOptions);

        // Cookie para refresh token
        var refreshTokenCookieOptions = new CookieOptions
        {
            HttpOnly = cookieOptions.HttpOnly,
            Secure = cookieOptions.Secure,
            SameSite = cookieOptions.SameSite,
            Domain = cookieOptions.Domain,
            Expires = DateTimeOffset.UtcNow.AddDays(refreshTokenDays)
        };
        HttpContext.Response.Cookies.Append("refreshToken", refreshToken, refreshTokenCookieOptions);
    }

    private void ClearTokenCookies()
    {
        HttpContext.Response.Cookies.Delete("accessToken");
        HttpContext.Response.Cookies.Delete("refreshToken");
    }
}
