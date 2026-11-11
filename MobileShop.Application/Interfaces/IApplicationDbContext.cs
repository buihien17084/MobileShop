using Microsoft.EntityFrameworkCore;
using MobileShop.Domain.Entities;


namespace MobileShop.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<Cart> Carts { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}



