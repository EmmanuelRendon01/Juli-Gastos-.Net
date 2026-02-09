

using System.Reflection.Metadata;
using JuliGastos.Domain.Enums;

namespace JuliGastos.Domain.Models;

public class Category
{
    public long Id {get; set;}
    
    public long UserId {get; set;}

    public string Name {get; set;} = string.Empty;

    public string Icon {get; set;} = string.Empty;

    public string Color {get; set;} = string.Empty;

    public TransactionType Type {get; set;}

    public bool IsSystemDefault {get; set;}



}