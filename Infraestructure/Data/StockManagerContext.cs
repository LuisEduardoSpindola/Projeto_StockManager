using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Data;

public class StockManagerContext : IdentityDbContext<StockUser>
{
    public StockManagerContext(DbContextOptions<StockManagerContext> options)
        : base(options)
    {
    }

    public DbSet<Produto> produtos { get; set; }
    public DbSet<Loja> lojas { get; set; }
    public DbSet<ItemEstoque> itensEstoque { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}
