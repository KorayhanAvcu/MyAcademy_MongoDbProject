using AspNetCore.Identity.MongoDbCore.Extensions;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Reflection;
using Travel.Web.Entities;
using Travel.Web.Services.BannerServices;
using Travel.Web.Services.RouteServices;
using Travel.Web.Settings;

var builder = WebApplication.CreateBuilder(args);


// =====================================================
// AUTO MAPPER
// =====================================================

builder.Services.AddAutoMapper(
    Assembly.GetExecutingAssembly()
);


// =====================================================
// FLUENT VALIDATION
// =====================================================

builder.Services
    .AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters()
    .AddValidatorsFromAssembly(
        Assembly.GetExecutingAssembly()
    );


// =====================================================
// DATABASE SETTINGS
// =====================================================

builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("DatabaseSettings")
);

builder.Services.AddSingleton<IDatabaseSettings>(sp =>
    sp.GetRequiredService<IOptions<DatabaseSettings>>().Value
);


// =====================================================
// SERVICES
// =====================================================

builder.Services.AddScoped<IBannerService, BannerService>();
builder.Services.AddScoped<IRouteService, RouteService>();


// =====================================================
// MONGODB DATABASE SETTINGS
// =====================================================

var databaseSettings = builder.Configuration
    .GetSection("DatabaseSettings")
    .Get<DatabaseSettings>();


// =====================================================
// ASP.NET CORE IDENTITY + MONGODB
// =====================================================

builder.Services
    .AddIdentity<AppUser, AppRole>(options =>
    {
        // USER
        options.User.RequireUniqueEmail = true;


        // PASSWORD
        options.Password.RequiredLength = 8;
        options.Password.RequireDigit = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;


        // LOCKOUT
        options.Lockout.MaxFailedAccessAttempts = 5;
    })
    .AddMongoDbStores<AppUser, AppRole, string>(
        databaseSettings.ConnectionString,
        databaseSettings.DatabaseName
    )
    .AddDefaultTokenProviders();


// =====================================================
// COOKIE SETTINGS
// =====================================================

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Auth/SignIn";
    options.AccessDeniedPath = "/Auth/AccessDenied";

    options.ExpireTimeSpan = TimeSpan.FromHours(2);
    options.SlidingExpiration = true;
});


// =====================================================
// MVC
// =====================================================

builder.Services.AddControllersWithViews();


// =====================================================
// BUILD
// =====================================================

var app = builder.Build();


// =====================================================
// HTTP PIPELINE
// =====================================================

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


// =====================================================
// ROLE SEED
// =====================================================

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider
        .GetRequiredService<RoleManager<AppRole>>();

    string[] roles =
    {
        "Admin",
        "User"
    };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(
                new AppRole
                {
                    Name = role
                }
            );
        }
    }
}


// =====================================================
// AREA ROUTE
// =====================================================

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);


// =====================================================
// DEFAULT ROUTE
// =====================================================

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);


app.Run();

