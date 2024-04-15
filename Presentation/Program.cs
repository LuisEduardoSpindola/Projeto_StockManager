using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Infraestructure.Data;
using Domain.Models;
using Application.Interfaces;
using Infraestructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConn") ?? throw new InvalidOperationException("Connection string 'StockManagerContextConnection' not found.");

builder.Services.AddHttpClient<ViaCEPService>();


builder.Services.AddDbContext<StockManagerContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDefaultIdentity<StockUser>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<StockManagerContext>();


builder.Services.AddScoped<IProduct, ProductRepositories>();
builder.Services.AddScoped<IStore, StoreRepositories>();
builder.Services.AddScoped<IStockItem, StockItemRepositories>();
builder.Services.AddScoped<TokenService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:JwtIssuer"],
        ValidAudience = builder.Configuration["Jwt:JwtAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:JwtSecurityKey"]))
    };
});


builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


app.UseHttpsRedirection();


app.UseStaticFiles();


app.UseRouting();


app.UseAuthentication();


app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "produto-search",
    pattern: "Produto/Search",
    defaults: new { controller = "Produto", action = "GetByName" }
);


// Inicia a aplicação.
app.Run();

