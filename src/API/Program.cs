using JuliGastos.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Serializar enums como strings en lugar de números
        options.JsonSerializerOptions.Converters.Add(
            new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

// Configure services using extension methods
builder.Services.AddSwaggerConfiguration();
builder.Services.AddCorsConfiguration();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationHandlers();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// CORS Middleware - Debe ir ANTES de Authentication/Authorization
app.UseCors("AllowFrontend");

// Security Middleware (orden importante)
app.UseAuthentication();  // Valida JWT token
app.UseAuthorization();   // Verifica permisos y roles

app.MapControllers();

app.Run();
