namespace JuliGastos.Domain.Models;

public class SavingPlanAccount
{
    public long Id { get; set; }
    public long SavingPlanId { get; set; }
    public long AccountId { get; set; }
    public DateTimeOffset LinkedAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    
    // Navigation properties
    public SavingPlan? SavingPlan { get; set; }
    public Account? Account { get; set; }
}
