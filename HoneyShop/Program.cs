using Domain.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Implementation;
using Repository.Interface;
using Services.Implementation;
using Services.Interface;
using Stripe;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Get connection string from configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Register AppDbContext with EF Core
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register repositories and services
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();

builder.Services.AddTransient<IProductService, Services.Implementation.ProductService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IShoppingCartService, ShoppingCartService>();
builder.Services.AddTransient<IOrderService, OrderService>();

// Authorization policy for Admin role
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy =>
        policy.RequireClaim(ClaimTypes.Role, "Admin"));
});

// Configure session and authentication
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".HoneyShop.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    });

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Products}/{action=Index}/{id?}");

app.Run();
