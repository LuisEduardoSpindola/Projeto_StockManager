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



// Adiciona controladores com visualiza��es.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configura o pipeline de requisi��es HTTP.
if (!app.Environment.IsDevelopment())
{
    // Manipulador de exce��es para produ��o.
    app.UseExceptionHandler("/Home/Error");
    // Habilita HSTS (Seguran�a de Transporte Estrito HTTP).
    app.UseHsts();
}

// Redireciona automaticamente para HTTPS.
app.UseHttpsRedirection();

// Serve arquivos est�ticos (CSS, JS, imagens, etc.).
app.UseStaticFiles();

// Configura roteamento.
app.UseRouting();

// Habilita a autentica��o antes da autoriza��o.
app.UseAuthentication();

// Habilita a autoriza��o.
app.UseAuthorization();

// Mapeia as rotas para Razor Pages.
app.MapRazorPages();

// Mapeia a rota padr�o para controladores.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "produto-search",
    pattern: "Produto/Search",
    defaults: new { controller = "Produto", action = "GetByName" }
);


// Inicia a aplica��o.
app.Run();

