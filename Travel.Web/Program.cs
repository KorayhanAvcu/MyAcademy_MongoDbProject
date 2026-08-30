using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Options;
using System.Reflection;
using Travel.Web.Services.BannerServices;
using Travel.Web.Services.RouteServices;
using Travel.Web.Settings;
using Microsoft.AspNetCore.Identity;
using Travel.Web.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters().AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("DatabaseSettings")
);

builder.Services.AddScoped<IBannerService, BannerService>();
builder.Services.AddScoped<IRouteService, RouteService>();
builder.Services.AddSingleton<IDatabaseSettings>(sp =>
    sp.GetRequiredService<IOptions<DatabaseSettings>>().Value
);

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

app.UseAuthorization();
app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
          );
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
