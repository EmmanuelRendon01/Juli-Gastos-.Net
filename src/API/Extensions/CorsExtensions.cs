namespace JuliGastos.API.Extensions;

/// <summary>
/// Extension methods for configuring CORS (Cross-Origin Resource Sharing)
/// </summary>
public static class CorsExtensions
{
    /// <summary>
    /// Adds and configures CORS policy to allow frontend origins with credentials (cookies)
    /// </summary>
    public static IServiceCollection AddCorsConfiguration(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend", policy =>
            {
                policy.WithOrigins(
                        "http://localhost:3000",  // React/Next.js
                        "http://localhost:4200",  // Angular
                        "http://localhost:5173"   // Vite
                    )
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials(); // IMPORTANTE: Permite envío de cookies HttpOnly
            });
        });

        return services;
    }
}
