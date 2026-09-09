using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Reflection;
using Travel.Web.Entities;
using Travel.Web.Services.BannerServices;
using Travel.Web.Services.CategoryServices;
using Travel.Web.Services.DestinationServices;
using Travel.Web.Services.QuestionServices;
using Travel.Web.Services.ReviewServices;
using Travel.Web.Services.RouteServices;
using Travel.Web.Services.TourServices;
using Travel.Web.Services.WhyChooseUsServices;
using Travel.Web.Settings;

var builder = WebApplication.CreateBuilder(args);


// =====================================================
// AUTOMAPPER
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

builder.Services.AddScoped<IDestinationService, DestinationService>();

builder.Services.AddScoped<IWhyChooseUsService, WhyChooseUsService>();

builder.Services.AddScoped<ITourService, TourService>();

builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddScoped<IReviewService, ReviewService>();

builder.Services.AddScoped<IQuestionService, QuestionService>();


// =====================================================
// DATABASE SETTINGS OBJECT
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
        // -------------------------------------------------
        // USER
        // -------------------------------------------------

        options.User.RequireUniqueEmail = true;


        // -------------------------------------------------
        // PASSWORD
        // -------------------------------------------------

        options.Password.RequiredLength = 8;

        options.Password.RequireDigit = true;

        options.Password.RequireUppercase = true;

        options.Password.RequireLowercase = true;

        options.Password.RequireNonAlphanumeric = true;


        // -------------------------------------------------
        // LOCKOUT
        // -------------------------------------------------

        options.Lockout.MaxFailedAccessAttempts = 5;


        // -------------------------------------------------
        // SIGN IN
        // -------------------------------------------------

        options.SignIn.RequireConfirmedAccount = false;
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
    // -------------------------------------------------
    // LOGIN PAGE
    // -------------------------------------------------

    options.LoginPath = "/Auth/SignIn";


    // -------------------------------------------------
    // ACCESS DENIED
    // -------------------------------------------------

    options.AccessDeniedPath = "/Auth/AccessDenied";


    // -------------------------------------------------
    // COOKIE
    // -------------------------------------------------

    options.ExpireTimeSpan = TimeSpan.FromHours(2);

    options.SlidingExpiration = true;
});


// =====================================================
// AUTHORIZATION
// =====================================================

builder.Services.AddAuthorization();


// =====================================================
// MVC
// =====================================================

builder.Services.AddControllersWithViews();


var app = builder.Build();


// =====================================================
// ERROR / HSTS
// =====================================================

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    app.UseHsts();
}


// =====================================================
// HTTPS
// =====================================================

app.UseHttpsRedirection();


// =====================================================
// STATIC FILES
// =====================================================

app.UseStaticFiles();


// =====================================================
// ROUTING
// =====================================================

app.UseRouting();


// =====================================================
// AUTHENTICATION
// =====================================================

app.UseAuthentication();


// =====================================================
// AUTHORIZATION
// =====================================================

app.UseAuthorization();


// =====================================================
// CREATE DEFAULT ROLES
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
    pattern: "{controller=Default}/{action=Index}/{id?}"
);


app.Run();