using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    public DbSet<Payment> Payments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Amount).HasColumnType("decimal(18,2)").IsRequired();
            entity.Property(e => e.Currency).HasMaxLength(3).IsRequired();
            entity.Property(e => e.MerchantId).IsRequired();

            entity.HasIndex(e => e.MerchantOrderId).IsUnique();
            entity.Property(e => e.MerchantOrderId).IsRequired();
            entity.Property(e => e.MaskedCardNumber).IsRequired();
            entity.Property(e => e.Status).HasConversion<string>();

        });
    }
}