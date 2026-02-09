using System.Text;
using JuliGastos.API.BackgroundServices;
using JuliGastos.Application.Interfaces.Repositories;
using JuliGastos.Application.Interfaces.Services;
using JuliGastos.Infrastructure.Persistence;
using JuliGastos.Infrastructure.Repositories;
using JuliGastos.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Definir el esquema de seguridad JWT Bearer
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingresa tu token JWT en el formato: Bearer {tu token}"
    });

    // Requerir el esquema de seguridad globalmente para todos los endpoints
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Authentication & Authorization (ASP.NET Core Security = Spring Security)
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!))
        };
    });

builder.Services.AddAuthorization();

// Database - PostgreSQL con Entity Framework Core
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

// Services
builder.Services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();
builder.Services.AddScoped<ITokenService, JwtTokenService>();

// Background Services
builder.Services.AddHostedService<RefreshTokenCleanupService>();

// UseCases/Handlers - Auth
builder.Services.AddScoped<JuliGastos.Application.UseCases.Auth.Register.RegisterHandler>();
builder.Services.AddScoped<JuliGastos.Application.UseCases.Auth.Login.LoginHandler>();
builder.Services.AddScoped<JuliGastos.Application.UseCases.Auth.Refresh.RefreshHandler>();
builder.Services.AddScoped<JuliGastos.Application.UseCases.Auth.Logout.LogoutHandler>();

// UseCases/Handlers - Account
builder.Services.AddScoped<JuliGastos.Application.UseCases.Account.Create.CreateAccountHandler>();
builder.Services.AddScoped<JuliGastos.Application.UseCases.Account.GetAll.GetAllAccountsHandler>();
builder.Services.AddScoped<JuliGastos.Application.UseCases.Account.GetById.GetAccountByIdHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Security Middleware (orden importante)
app.UseAuthentication();  // Valida JWT token
app.UseAuthorization();   // Verifica permisos y roles

app.MapControllers();

app.Run();
