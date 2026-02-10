

using JuliGastos.Domain.Enums;

namespace JuliGastos.Domain.Models;

public class Transaction
{
    public long Id { get; set; }
    public Guid Uuid { get; set; }
    public long UserId { get; set; }
    public Guid AccountId { get; set; }
    public long CategoryId { get; set; }
    public TransactionType Type { get; set; }
    public decimal Amount { get; set; }
    public DateTimeOffset Date { get; set; }
    public string Description { get; set; } = string.Empty;
    public NecessityLevel NecessityLevel { get; set; }
    public long? DestinationAccountId { get; set; }
    public bool IsRecurring { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    
    // Navigation properties
    public User? User { get; set; }
    public Account? Account { get; set; }
    public Category? Category { get; set; }
    public Account? DestinationAccount { get; set; }
}