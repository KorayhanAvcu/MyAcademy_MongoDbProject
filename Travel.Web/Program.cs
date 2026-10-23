using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Microsoft.Extensions.Options;
using System.Reflection;
using Travel.Web.Entities;
using Travel.Web.Services.AboutPageHeadServices;
using Travel.Web.Services.AboutStatServices;
using Travel.Web.Services.AboutStorySectionServices;
using Travel.Web.Services.AboutTeamMemberServices;
using Travel.Web.Services.AboutTimelineServices;
using Travel.Web.Services.AboutValueServices;
using Travel.Web.Services.AltBannerServices;
using Travel.Web.Services.BannerServices;
using Travel.Web.Services.CategoryServices;
using Travel.Web.Services.ContactCardServices;
using Travel.Web.Services.ContactFAQServices;
using Travel.Web.Services.ContactFormServices;
using Travel.Web.Services.ContactMapServices;
using Travel.Web.Services.ContactMessageServices;
using Travel.Web.Services.ContactPageHeadServices;
using Travel.Web.Services.DestinationServices;
using Travel.Web.Services.FavoriteServices;
using Travel.Web.Services.QuestionServices;
using Travel.Web.Services.ReportServices;
using Travel.Web.Services.ReservationServices;
using Travel.Web.Services.ReviewServices;
using Travel.Web.Services.RouteServices;
using Travel.Web.Services.StatisticServices;
using Travel.Web.Services.TourServices;
using Travel.Web.Services.UserServices;
using Travel.Web.Services.WhyChooseUsServices;
using Travel.Web.Settings;

var builder = WebApplication.CreateBuilder(args);

// Enum alanları eski kayıtlardaki metin değerleri de okuyabilsin
Travel.Web.Helpers.EnumSerializerRegistration.Register();

// QuestPDF topluluk lisansı (eğitim / bootcamp kullanımı)
QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;


// =====================================================
// LOCALIZATION (TR / EN)
// =====================================================

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

var supportedCultures = new[]
{
    new CultureInfo("tr-TR"),
    new CultureInfo("en-US")
};

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("tr-TR");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;

    // Öncelik: ?culture=en-US -> çerez -> tarayıcı dili
    options.RequestCultureProviders = new List<IRequestCultureProvider>
    {
        new QueryStringRequestCultureProvider(),
        new CookieRequestCultureProvider()
    };
});


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

builder.Services.AddScoped<IAltBannerService, AltBannerService>();

builder.Services.AddScoped<IAboutPageHeadService, AboutPageHeadService>();

builder.Services.AddScoped<IContactPageHeadService, ContactPageHeadService>();

builder.Services.AddScoped<IContactCardService, ContactCardService>();
builder.Services.AddScoped<IContactFormService, ContactFormService>();
builder.Services.AddScoped<IContactMapService, ContactMapService>();
builder.Services.AddScoped<IContactFAQService, ContactFAQService>();
builder.Services.AddScoped<IContactMessageService, ContactMessageService>();


builder.Services.AddScoped<IAboutStorySectionService,AboutStorySectionService>();

builder.Services.AddScoped<IAboutStatService, AboutStatService>();

builder.Services.AddScoped<IAboutValueService, AboutValueService>();

builder.Services.AddScoped<IAboutTimelineService, AboutTimelineService>();

builder.Services.AddScoped<IAboutTeamMemberService, AboutTeamMemberService>();

builder.Services.AddScoped<IReservationService, ReservationService>();

builder.Services.AddScoped<IFavoriteService, FavoriteService>();

builder.Services.AddScoped<IStatisticService, StatisticService>();

builder.Services.AddScoped<IReportService, ReportService>();

builder.Services.AddScoped<IUserService, UserService>();
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

builder.Services
    .AddControllersWithViews()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization(options =>
        options.DataAnnotationLocalizerProvider = (_, factory) =>
            factory.Create(typeof(Travel.Web.SharedResource)));


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
// LOCALIZATION
// =====================================================

app.UseRequestLocalization();


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
// DEFAULT ROLES + SEED USERS
// =====================================================

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider
        .GetRequiredService<RoleManager<AppRole>>();

    var userManager = scope.ServiceProvider
        .GetRequiredService<UserManager<AppUser>>();


    // =================================================
    // CREATE DEFAULT ROLES
    // =================================================

    string[] roles =
    {
        "Admin",
        "User"
    };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            var roleResult = await roleManager.CreateAsync(
                new AppRole
                {
                    Name = role
                }
            );

            if (!roleResult.Succeeded)
            {
                foreach (var error in roleResult.Errors)
                {
                    Console.WriteLine(
                        $"Role oluşturulamadı: {error.Description}"
                    );
                }
            }
        }
    }


    // =================================================
    // CREATE ADMIN USER
    // =================================================

    var adminUser = await userManager.FindByNameAsync("korayhan");

    if (adminUser == null)
    {
        adminUser = new AppUser
        {
            UserName = "korayhan",
            Email = "korayhan@travelio.com",

            FirstName = "Korayhan",
            LastName = "Avcu",

            PhoneNumber = "+90 555 111 2233",

            EmailConfirmed = true,
            PhoneNumberConfirmed = true,

            TermsAccepted = true,
            TermsAcceptedAt = DateTime.UtcNow
        };

        var adminResult = await userManager.CreateAsync(
            adminUser,
            "Password12*"
        );

        if (adminResult.Succeeded)
        {
            var roleResult = await userManager.AddToRoleAsync(
                adminUser,
                "Admin"
            );

            if (roleResult.Succeeded)
            {
                Console.WriteLine(
                    "Admin kullanıcısı başarıyla oluşturuldu."
                );
            }
            else
            {
                foreach (var error in roleResult.Errors)
                {
                    Console.WriteLine(
                        $"Admin rolü atanamadı: {error.Description}"
                    );
                }
            }
        }
        else
        {
            foreach (var error in adminResult.Errors)
            {
                Console.WriteLine(
                    $"Admin kullanıcısı oluşturulamadı: {error.Description}"
                );
            }
        }
    }
    else
    {
        // Kullanıcı zaten varsa tekrar oluşturma.
        // Ancak Admin rolü yoksa tamamla.

        if (!await userManager.IsInRoleAsync(
            adminUser,
            "Admin"))
        {
            await userManager.AddToRoleAsync(
                adminUser,
                "Admin"
            );
        }
    }


    // =================================================
    // CREATE NORMAL USER
    // =================================================

    var normalUser = await userManager.FindByNameAsync("aras");

    if (normalUser == null)
    {
        normalUser = new AppUser
        {
            UserName = "aras",
            Email = "aras@travelio.com",

            FirstName = "Aras",
            LastName = "Yılmaz",

            PhoneNumber = "+90 555 222 3344",

            EmailConfirmed = true,
            PhoneNumberConfirmed = true,

            TermsAccepted = true,
            TermsAcceptedAt = DateTime.UtcNow
        };

        var userResult = await userManager.CreateAsync(
            normalUser,
            "Password12*"
        );

        if (userResult.Succeeded)
        {
            var roleResult = await userManager.AddToRoleAsync(
                normalUser,
                "User"
            );

            if (roleResult.Succeeded)
            {
                Console.WriteLine(
                    "User kullanıcısı başarıyla oluşturuldu."
                );
            }
            else
            {
                foreach (var error in roleResult.Errors)
                {
                    Console.WriteLine(
                        $"User rolü atanamadı: {error.Description}"
                    );
                }
            }
        }
        else
        {
            foreach (var error in userResult.Errors)
            {
                Console.WriteLine(
                    $"User kullanıcısı oluşturulamadı: {error.Description}"
                );
            }
        }
    }
    else
    {
        // Kullanıcı zaten varsa tekrar oluşturma.
        // Ancak User rolü yoksa tamamla.

        if (!await userManager.IsInRoleAsync(
            normalUser,
            "User"))
        {
            await userManager.AddToRoleAsync(
                normalUser,
                "User"
            );
        }
    }
}


// =====================================================
// TUR DURUMU NORMALİZASYONU
// Metin olarak kaydedilmiş durumları ("Published", "Draft" ...) sayıya çevirir;
// böylece Status filtreleri ve aggregation sorguları tüm kayıtlarda çalışır.
// =====================================================

{
    var mongo = new MongoDB.Driver.MongoClient(databaseSettings.ConnectionString)
        .GetDatabase(databaseSettings.DatabaseName)
        .GetCollection<MongoDB.Bson.BsonDocument>(databaseSettings.TourCollectionName);

    var statusMap = Enum.GetValues<Travel.Web.Entities.Enums.TourStatus>()
        .ToDictionary(x => x.ToString(), x => (int)x);

    foreach (var alias in Travel.Web.Helpers.EnumSerializerRegistration.TourStatusAliases)
        statusMap[alias.Key] = (int)alias.Value;

    foreach (var (text, number) in statusMap)
    {
        await mongo.UpdateManyAsync(
            MongoDB.Driver.Builders<MongoDB.Bson.BsonDocument>.Filter.Eq("Status", text),
            MongoDB.Driver.Builders<MongoDB.Bson.BsonDocument>.Update.Set("Status", number));
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