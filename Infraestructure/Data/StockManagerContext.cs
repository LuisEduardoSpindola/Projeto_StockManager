﻿using Domain.Models;
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

    public DbSet<Product> products { get; set; }
    public DbSet<Store> stores { get; set; }
    public DbSet<StockItem> stockItems { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}
