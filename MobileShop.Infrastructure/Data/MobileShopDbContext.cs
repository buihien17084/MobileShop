using Microsoft.EntityFrameworkCore;
using MobileShop.Domain.Entities;
using MobileShop.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MobileShop.Infrastructure.Data;

public class MobileShopDbContext : DbContext, IApplicationDbContext
{
    public MobileShopDbContext(DbContextOptions<MobileShopDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Cart> Carts => Set<Cart>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.Email).HasMaxLength(150).IsRequired();
            entity.Property(u => u.FullName).HasMaxLength(100).IsRequired();
            entity.Property(u => u.PhoneNumber).HasMaxLength(20).IsRequired();
        });
        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.HasOne(c => c.User)
                  .WithOne(u => u.Cart)
                  .HasForeignKey<Cart>(c => c.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}

