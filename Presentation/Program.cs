using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Infraestructure.Data;
using Domain.Models;
using Application.Interfaces;
using Infraestructure.Repositories;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConn") ?? throw new InvalidOperationException("Connection string 'StockManagerContextConnection' not found.");

builder.Services.AddDbContext<StockManagerContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDefaultIdentity<StockUser>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<StockManagerContext>();


builder.Services.AddScoped<IProduto, ProdutoRepositories>();
builder.Services.AddScoped<ILoja, LojaRepositories>();
builder.Services.AddScoped<IItemEstoque, ItemEstoqueRepositories>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
