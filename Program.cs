using FeezSpeedy.Data;
using FeezSpeedy.Models;
using FeezSpeedy.Web.Data;
using FeezSpeedy.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// --------------------
// Logging
// --------------------
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// --------------------
// Database
// --------------------
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// --------------------
// Identity
// --------------------
builder.Services
    .AddIdentity<Parent, IdentityRole>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 6;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// --------------------
// Cookie Config
// --------------------
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;

    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

// --------------------
// CORS
// --------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactPolicy", policy =>
    {
        policy.WithOrigins(
            "http://localhost:3000",
            "http://localhost:3001",
            "http://localhost:5185",
            "https://localhost:7210"
        )
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

// --------------------
// Middleware
// --------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors("ReactPolicy");

app.UseAuthentication();
app.UseAuthorization();

// --------------------
// MVC
// --------------------
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.MapControllers();

// --------------------
// SPA Fallbacks
// --------------------

app.MapWhen(ctx => ctx.Request.Path.StartsWithSegments("/admin"), adminApp =>
{
    adminApp.Run(async ctx =>
    {
        ctx.Response.ContentType = "text/html";
        await ctx.Response.SendFileAsync("wwwroot/admin/index.html");
    });
});

app.MapWhen(ctx => ctx.Request.Path.StartsWithSegments("/parent"), parentApp =>
{
    parentApp.Run(async ctx =>
    {
        ctx.Response.ContentType = "text/html";
        await ctx.Response.SendFileAsync("wwwroot/parent/index.html");
    });
});

// --------------------
// DB Migration
// --------------------
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
    await DbInitializer.InitializeAsync(scope.ServiceProvider);
}

app.Run();