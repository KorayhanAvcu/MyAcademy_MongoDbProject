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

builder.Services.AddAutoMapper(
    Assembly.GetExecutingAssembly()
);

builder.Services
    .AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters()
    .AddValidatorsFromAssembly(
        Assembly.GetExecutingAssembly()
    );


builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("DatabaseSettings")
);

builder.Services.AddSingleton<IDatabaseSettings>(sp =>
    sp.GetRequiredService<IOptions<DatabaseSettings>>().Value
);


builder.Services.AddScoped<IBannerService, BannerService>();
builder.Services.AddScoped<IRouteService, RouteService>();


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
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = true;


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
    pattern: "{controller=Default}/{action=Index}/{id?}"
);


app.Run();

