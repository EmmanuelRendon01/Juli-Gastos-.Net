namespace JuliGastos.Application.UseCases.Auth.Register;

public record RegisterCommand(
    string Email, 
    string Password, 
    string FullName,
    string CurrencyCode = "COP",
    int EmergencyFundMonths = 3
);

