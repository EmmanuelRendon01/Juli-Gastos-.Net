using JuliGastos.Domain.Enums;

namespace JuliGastos.Domain.Models;

public class Account
{
    public long Id { get; set; }
    public Guid Uuid { get; set; }
    public long UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public AccountType Type { get; set; }
    public string CurrencyCode { get; set; } = string.Empty;
    public decimal CurrentBalance { get; set; }
    public decimal MonthlyMaintenanceFee { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public bool IsActive { get; set; }
}