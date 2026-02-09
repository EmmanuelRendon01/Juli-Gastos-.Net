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
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

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
                .HasDefaultValueSql("gen_random_uuid()");
            
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

        // Configuración de Account
        modelBuilder.Entity<Account>(entity =>
        {
            entity.ToTable("accounts");
            
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();
            
            entity.Property(e => e.Uuid)
                .HasColumnName("uuid")
                .IsRequired()
                .HasDefaultValueSql("gen_random_uuid()");
            
            entity.Property(e => e.UserId)
                .HasColumnName("user_id")
                .IsRequired();
            
            entity.Property(e => e.Name)
                .HasColumnName("name")
                .HasMaxLength(100)
                .IsRequired();
            
            entity.Property(e => e.Type)
                .HasColumnName("type")
                .HasConversion<string>()
                .IsRequired();
            
            entity.Property(e => e.CurrencyCode)
                .HasColumnName("currency_code")
                .HasMaxLength(3)
                .HasDefaultValue("COP");
            
            entity.Property(e => e.CurrentBalance)
                .HasColumnName("current_balance")
                .HasPrecision(19, 4)
                .HasDefaultValue(0.0000m);
            
            entity.Property(e => e.MonthlyMaintenanceFee)
                .HasColumnName("monthly_maintenance_fee")
                .HasPrecision(19, 4)
                .HasDefaultValue(0.00m);
            
            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            
            entity.Property(e => e.IsActive)
                .HasColumnName("is_active")
                .HasDefaultValue(true);
            
            // Relación con User (FK)
            entity.HasOne<User>()
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // Índice para búsquedas por usuario
            entity.HasIndex(e => e.UserId);
            
            // Índice único para UUID
            entity.HasIndex(e => e.Uuid).IsUnique();
        });

        // Configuración de RefreshToken
        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.ToTable("refresh_tokens");
            
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();
            
            entity.Property(e => e.Token)
                .HasColumnName("token")
                .HasMaxLength(100)
                .IsRequired();
            
            entity.Property(e => e.UserId)
                .HasColumnName("user_id")
                .IsRequired();
            
            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            
            entity.Property(e => e.ExpiresAt)
                .HasColumnName("expires_at")
                .IsRequired();
            
            entity.Property(e => e.IsRevoked)
                .HasColumnName("is_revoked")
                .HasDefaultValue(false);
            
            entity.Property(e => e.RevokedAt)
                .HasColumnName("revoked_at");
            
            entity.Property(e => e.IpAddress)
                .HasColumnName("ip_address")
                .HasMaxLength(45);
            
            entity.Property(e => e.UserAgent)
                .HasColumnName("user_agent")
                .HasMaxLength(500);
            
            entity.Property(e => e.ReplacedByToken)
                .HasColumnName("replaced_by_token")
                .HasMaxLength(100);
            
            // FK a User
            entity.HasOne<User>()
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // Índices
            entity.HasIndex(e => e.Token).IsUnique();
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.ExpiresAt);
        });
    }
}
