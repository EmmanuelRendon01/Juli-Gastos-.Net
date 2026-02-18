using JuliGastos.Domain.Enums;

namespace JuliGastos.Domain.Models;

public class FinancialCommitment
{
    public long Id { get; set; }
    public Guid Uuid { get; set; }
    public long UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public RecurrenceFrequency Frequency { get; set; }
    public long? CategoryId { get; set; }
    public DateTimeOffset? StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }
    public DateTimeOffset? NextDueDate { get; set; }
    public bool IsActive { get; set; }
    public string? Notes { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    
    // Navigation properties
    public User? User { get; set; }
    public Category? Category { get; set; }
}
