using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace JuliGastos.API.Extensions;

/// <summary>
/// Extension methods for configuring JWT Authentication and Authorization
/// </summary>
public static class AuthenticationExtensions
{
    /// <summary>
    /// Adds and configures JWT Bearer authentication with cookie support
    /// Tokens can be read from HttpOnly cookies or Authorization header
    /// </summary>
    public static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // 1. Configure JWT Authentication
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]!))
                };

                // 2. Support reading JWT from cookies (HttpOnly) or Authorization header
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        // Primero intentar leer desde cookie
                        var accessToken = context.Request.Cookies["accessToken"];

                        if (!string.IsNullOrEmpty(accessToken))
                        {
                            context.Token = accessToken;
                        }
                        // Si no hay cookie, intentar desde el header Authorization (fallback para testing)
                        else if (context.Request.Headers.ContainsKey("Authorization"))
                        {
                            var authHeader = context.Request.Headers["Authorization"].ToString();
                            if (authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                            {
                                context.Token = authHeader["Bearer ".Length..].Trim();
                            }
                        }

                        return Task.CompletedTask;
                    }
                };
            });

        // 3. Add Authorization
        services.AddAuthorization();

        return services;
    }
}
