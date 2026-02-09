using JuliGastos.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace JuliGastos.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // DbSets (equivalente a @Entity en JPA)
    public DbSet<User> Users => Set<User>();
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Transaction> Transactions => Set<Transaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuración de User
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");
            
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();
            
            entity.Property(e => e.Uuid)
                .HasColumnName("uuid")
                .IsRequired()
                .HasDefaultValueSql("uuid_generate_v4()");
            
            entity.Property(e => e.Email)
                .HasColumnName("email")
                .HasMaxLength(255)
                .IsRequired();
            
            entity.Property(e => e.PasswordHash)
                .HasColumnName("password_hash")
                .HasMaxLength(255)
                .IsRequired();
            
            entity.Property(e => e.FullName)
                .HasColumnName("full_name")
                .HasMaxLength(100)
                .IsRequired();
            
            entity.Property(e => e.CurrencyCode)
                .HasColumnName("currency_code")
                .HasMaxLength(3)
                .HasDefaultValue("COP");
            
            entity.Property(e => e.EmergencyFundMonths)
                .HasColumnName("emergency_fund_months")
                .HasDefaultValue(3);
            
            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            
            entity.Property(e => e.UpdatedAt)
                .HasColumnName("updated_at")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            
            entity.HasIndex(e => e.Uuid).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();
        });
    }
}
