using JuliGastos.Domain.Enums;

namespace JuliGastos.Domain.Models;

public class SavingPlan
{
    public long Id { get; set; }
    public Guid Uuid { get; set; }
    public long UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal TargetAmount { get; set; }
    public DateTimeOffset TargetDate { get; set; }
    public string Icon { get; set; } = "🎯";
    public string Color { get; set; } = "#6366F1";
    public SavingPlanStatus Status { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    
    // Navigation properties
    public User? User { get; set; }
    public ICollection<SavingPlanAccount> SavingPlanAccounts { get; set; } = new List<SavingPlanAccount>();
}
