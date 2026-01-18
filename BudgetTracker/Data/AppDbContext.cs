using BudgetTracker.Converters;
using BudgetTracker.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BudgetTracker.Data;

/// <summary>
/// <inheritdoc cref="IAppDbContext"/>
/// </summary>
public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>(options), IAppDbContext
{
    // DbSets
    public DbSet<Category> Categories { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<HttpRequestLog> HttpRequestLogs { get; set; }

    // Expose base methods via interface
    public override ValueTask<EntityEntry> AddAsync(object entity, CancellationToken cancellationToken = default) => base.AddAsync(entity, cancellationToken);
    public Task<int> ExecuteSqlRawAsync(string sql, params object[] parameters) => base.Database.ExecuteSqlRawAsync(sql, parameters);
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => base.SaveChangesAsync(cancellationToken);

    /// <summary>
    /// Configures the model conventions for the current context.
    /// </summary>
    /// <param name="configurationBuilder">The <see cref="ModelConfigurationBuilder"/> used to configure model conventions.</param>
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        configurationBuilder
            .Properties<DateTime>()
            .HaveConversion<DateTimeConverter>();

        configurationBuilder
            .Properties<DateTime?>()
            .HaveConversion<NullableDateTimeConverter>();
    }

    /// <summary>
    /// Setup the models and their relationships
    /// </summary>
    /// <param name="builder"></param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>(user =>
        {
            user.Property(x => x.FirstName)
                .IsUnicode(true)
                .HasMaxLength(50)
                .IsRequired();

            user.Property(x => x.LastName)
                .IsUnicode(true)
                .HasMaxLength(50)
                .IsRequired();

            // Foreign keys
            user.HasMany(e => e.Categories)
                .WithOne(f => f.User)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Category>(entity =>
        {
            entity.ToTable(table =>
            {
                table.IsTemporal(temp =>
                {
                    temp.HasPeriodStart("ValidFrom");
                    temp.HasPeriodEnd("ValidTo");
                    temp.UseHistoryTable("CategoriesHistory");
                });
            });

            entity.HasKey(e => e.CategoryId);

            entity.HasIndex(e => new { e.UserId });

            entity.Property(e => e.UserId)
                .IsRequired();

            entity.Property(e => e.Name)
                .IsUnicode(true)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(e => e.Description)
                .IsUnicode(true)
                .HasMaxLength(100);

            entity.Property(e => e.MonthlyLimit)
                .HasColumnType("DECIMAL(7,2)");

            entity.Property(e => e.Icon)
                .IsUnicode(true)
                .HasMaxLength(50);

            entity.Property(e => e.LastModifiedDays)
                .HasComputedColumnSql("DATEDIFF(DAY, ValidFrom, SYSUTCDATETIME())", stored: false);

            // Foreign keys
            entity.HasOne(e => e.User)
                .WithMany(f => f.Categories)
                .HasForeignKey(e => e.UserId);

            entity.HasMany(e => e.Transactions)
                .WithOne(f => f.Category)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        builder.Entity<HttpRequestLog>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.RequestUrl)
                .IsUnicode(true)
                .HasMaxLength(2048)
                .IsRequired();

            entity.Property(e => e.RequestMethod)
                .IsUnicode(true)
                .HasMaxLength(10)
                .IsRequired();

            entity.Property(e => e.UserAgent)
                .IsUnicode(true)
                .HasMaxLength(512)
                .IsRequired();

            entity.Property(e => e.ClientIpAddress)
                .IsUnicode(true)
                .HasMaxLength(39)
                .IsRequired();

            entity.Property(e => e.ReferrerUrl)
                .IsUnicode(true)
                .HasMaxLength(2048)
                .IsRequired();

            entity.Property(e => e.ResponseStatusCode)
                .IsRequired();

            entity.Property(e => e.ResponseTime)
                .IsRequired();

            entity.Property(e => e.DateTimeStamp)
                .HasColumnType("DATETIME")
                .IsRequired();
        });

        builder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId);

            entity.Property(e => e.Amount)
                .HasColumnType("DECIMAL(7,2)")
                .IsRequired();

            entity.Property(e => e.TransactionType)
                .IsRequired()
                .HasConversion<string>();

            entity.Property(e => e.DateOccurred)
                .HasColumnType("DATE")
                .IsRequired();

            entity.Property(e => e.Description)
                .IsUnicode(true)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.PlaceOfPurchase)
                .IsUnicode(true)
                .HasMaxLength(100);

            // Foreign keys
            entity.HasOne(e => e.Category)
                .WithMany(f => f.Transactions)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });
    }
}
