using JuliGastos.API.BackgroundServices;
using JuliGastos.Application.Interfaces.Repositories;
using JuliGastos.Application.Interfaces.Services;
using JuliGastos.Infrastructure.Persistence;
using JuliGastos.Infrastructure.Repositories;
using JuliGastos.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace JuliGastos.API.Extensions;

/// <summary>
/// Extension methods for configuring Infrastructure layer services
/// Includes: Database, Repositories, Domain Services, Background Services
/// </summary>
public static class InfrastructureExtensions
{
    /// <summary>
    /// Adds Infrastructure layer dependencies (Database, Repositories, Services)
    /// </summary>
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // 1. Database - PostgreSQL con Entity Framework Core
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        // 2. Repositories (Data Access Layer)
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();

        // 3. Domain Services
        services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();
        services.AddScoped<ITokenService, JwtTokenService>();

        // 4. Background Services (Hosted Services)
        services.AddHostedService<RefreshTokenCleanupService>();

        return services;
    }
}
