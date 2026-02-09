
namespace JuliGastos.Domain.Models;
public class User
{
    public long Id {get; set;}
    public Guid Uuid {get; set;}
    public string Email {get; set;} = string.Empty;
    public string Role {get; set;} = string.Empty;
    public string PasswordHash {get; set;} = string.Empty;
    public string FullName {get; set;} = string.Empty;
    public string CurrencyCode {get; set;} = string.Empty;
    public int EmergencyFundMonths {get; set;}
    public DateTimeOffset CreatedAt {get; set;}
    public DateTimeOffset UpdatedAt {get; set;}


}