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

        // Configuración de Category
        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("categories");
            
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();
            
            entity.Property(e => e.UserId)
                .HasColumnName("user_id")
                .IsRequired(false); // NULL para categorías del sistema
            
            entity.Property(e => e.Name)
                .HasColumnName("name")
                .HasMaxLength(100)
                .IsRequired();
            
            entity.Property(e => e.Icon)
                .HasColumnName("icon")
                .HasMaxLength(50)
                .HasDefaultValue("💰");
            
            entity.Property(e => e.Color)
                .HasColumnName("color")
                .HasMaxLength(7)
                .HasDefaultValue("#6366F1");
            
            entity.Property(e => e.Type)
                .HasColumnName("type")
                .HasConversion<string>()
                .IsRequired();
            
            entity.Property(e => e.IsSystemDefault)
                .HasColumnName("is_system_default")
                .HasDefaultValue(false);
            
            // FK a User (solo para categorías de usuario)
            entity.HasOne<User>()
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);
            
            // Índices
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => new { e.UserId, e.Name }).IsUnique();
            
            // Seed Data - Categorías predeterminadas del sistema (UserId = NULL)
            entity.HasData(
                // Categorías de Gastos
                new Category { Id = 1, UserId = null, Name = "Alimentación", Icon = "🍔", Color = "#10B981", Type = Domain.Enums.TransactionType.Expense, IsSystemDefault = true },
                new Category { Id = 2, UserId = null, Name = "Transporte", Icon = "🚗", Color = "#3B82F6", Type = Domain.Enums.TransactionType.Expense, IsSystemDefault = true },
                new Category { Id = 3, UserId = null, Name = "Vivienda", Icon = "🏠", Color = "#F59E0B", Type = Domain.Enums.TransactionType.Expense, IsSystemDefault = true },
                new Category { Id = 4, UserId = null, Name = "Servicios", Icon = "💡", Color = "#EF4444", Type = Domain.Enums.TransactionType.Expense, IsSystemDefault = true },
                new Category { Id = 5, UserId = null, Name = "Salud", Icon = "⚕️", Color = "#EC4899", Type = Domain.Enums.TransactionType.Expense, IsSystemDefault = true },
                new Category { Id = 6, UserId = null, Name = "Entretenimiento", Icon = "🎬", Color = "#8B5CF6", Type = Domain.Enums.TransactionType.Expense, IsSystemDefault = true },
                new Category { Id = 7, UserId = null, Name = "Educación", Icon = "📚", Color = "#06B6D4", Type = Domain.Enums.TransactionType.Expense, IsSystemDefault = true },
                new Category { Id = 8, UserId = null, Name = "Ropa", Icon = "👕", Color = "#F97316", Type = Domain.Enums.TransactionType.Expense, IsSystemDefault = true },
                new Category { Id = 9, UserId = null, Name = "Tecnología", Icon = "💻", Color = "#6366F1", Type = Domain.Enums.TransactionType.Expense, IsSystemDefault = true },
                new Category { Id = 10, UserId = null, Name = "Mascotas", Icon = "🐶", Color = "#A855F7", Type = Domain.Enums.TransactionType.Expense, IsSystemDefault = true },
                new Category { Id = 11, UserId = null, Name = "Regalos", Icon = "🎁", Color = "#EC4899", Type = Domain.Enums.TransactionType.Expense, IsSystemDefault = true },
                new Category { Id = 12, UserId = null, Name = "Otros Gastos", Icon = "💸", Color = "#64748B", Type = Domain.Enums.TransactionType.Expense, IsSystemDefault = true },
                
                // Categorías de Ingresos
                new Category { Id = 13, UserId = null, Name = "Salario", Icon = "💰", Color = "#10B981", Type = Domain.Enums.TransactionType.Income, IsSystemDefault = true },
                new Category { Id = 14, UserId = null, Name = "Freelance", Icon = "💼", Color = "#3B82F6", Type = Domain.Enums.TransactionType.Income, IsSystemDefault = true },
                new Category { Id = 15, UserId = null, Name = "Inversiones", Icon = "📈", Color = "#8B5CF6", Type = Domain.Enums.TransactionType.Income, IsSystemDefault = true },
                new Category { Id = 16, UserId = null, Name = "Bonus", Icon = "🎉", Color = "#F59E0B", Type = Domain.Enums.TransactionType.Income, IsSystemDefault = true },
                new Category { Id = 17, UserId = null, Name = "Venta", Icon = "🏷️", Color = "#06B6D4", Type = Domain.Enums.TransactionType.Income, IsSystemDefault = true },
                new Category { Id = 18, UserId = null, Name = "Otros Ingresos", Icon = "💵", Color = "#10B981", Type = Domain.Enums.TransactionType.Income, IsSystemDefault = true }
            );
        });
        
        // Configuración de Transaction
        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.ToTable("transactions");
            
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
            
            entity.Property(e => e.AccountId)
                .HasColumnName("account_id")
                .IsRequired();
            
            entity.Property(e => e.CategoryId)
                .HasColumnName("category_id")
                .IsRequired();
            
            entity.Property(e => e.Type)
                .HasColumnName("type")
                .HasConversion<string>()
                .IsRequired();
            
            entity.Property(e => e.Amount)
                .HasColumnName("amount")
                .HasPrecision(19, 4)
                .IsRequired();
            
            entity.Property(e => e.Date)
                .HasColumnName("date")
                .IsRequired();
            
            entity.Property(e => e.Description)
                .HasColumnName("description")
                .HasMaxLength(500)
                .HasDefaultValue(string.Empty);
            
            entity.Property(e => e.NecessityLevel)
                .HasColumnName("necessity_level")
                .HasConversion<string>()
                .IsRequired();
            
            entity.Property(e => e.DestinationAccountId)
                .HasColumnName("destination_account_id")
                .IsRequired(false);
            
            entity.Property(e => e.IsRecurring)
                .HasColumnName("is_recurring")
                .HasDefaultValue(false);
            
            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            
            // FK a User
            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // FK a Account (por Guid, no por Id)
            entity.HasOne(e => e.Account)
                .WithMany()
                .HasForeignKey(e => e.AccountId)
                .HasPrincipalKey(a => a.Uuid)
                .OnDelete(DeleteBehavior.Restrict);
            
            // FK a Category
            entity.HasOne(e => e.Category)
                .WithMany()
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
            
            // FK a DestinationAccount (opcional, para transferencias)
            entity.HasOne(e => e.DestinationAccount)
                .WithMany()
                .HasForeignKey(e => e.DestinationAccountId)
                .HasPrincipalKey(a => a.Id)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);
            
            // Índices
            entity.HasIndex(e => e.Uuid).IsUnique();
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.AccountId);
            entity.HasIndex(e => e.CategoryId);
            entity.HasIndex(e => e.Date);
        });
    }
}
