using CrunchEconUI.Helpers;
using CrunchEconUI.Interfaces;
using CrunchEconUI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using SteamWebAPI2.Utilities;
using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using Radzen;
using Serilog.Extensions.Logging;
using Serilog;
using System.Reflection;
using AutoMapper.Features;
using Newtonsoft.Json;
using CrunchEconUI.EntityFramework;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    public static string APIKEY = "";
    public static List<ulong> Admins = new List<ulong>();
    public static string DBString;
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddRazorPages();
        builder.Services.AddControllers();
        builder.Services.AddServerSideBlazor();
        builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        // builder.Services.AddSingleton<IUserDataService, UserDataService>();
        APIKEY = builder.Configuration["ApiKey"];
        builder.Services.AddTransient(x => new SteamWebInterfaceFactory(builder.Configuration["Authentication:Steam:ClientSecret"]));
        builder.Services.AddScoped<AuthenticationStateProvider, SteamAuthProvider>();
 
        builder.Services.AddSingleton<PlayerBalanceAndNotifyService>();
        builder.Services.AddSingleton<ValidatedUserService>();
        builder.Services.AddSingleton<EventService>();
        builder.Services.AddSingleton<ProductionUpgradeService>();
        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();
        DBString = builder.Configuration["DBString2"];
        builder.Services.AddDbContext<EconContext>();
        builder.Services.AddTransient<EconContext>();

        builder.Logging.SetMinimumLevel(LogLevel.Information);
        builder.Services.AddScoped<AuthenticatedUserService>();
        builder.Services.AddAuthentication(options => options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/signin";
                options.LogoutPath = "/signout";
                options.AccessDeniedPath = "/";
                options.ExpireTimeSpan = TimeSpan.FromDays(7);
                options.Events.OnSignedIn = ValidationHelper.SignIn;
                options.Events.OnValidatePrincipal = ValidationHelper.Validate;
            }).AddSteam();

        builder.Services.AddRadzenComponents();
        builder.Services.AddHttpClient();
        builder.Services
          .AddBlazorise(options =>
          {
              options.Immediate = true;
          })
          .AddBootstrapProviders()
          .AddFontAwesomeIcons();

        builder.Services.AddSingleton<IListingService>();

        var app = builder.Build();
        app.UseAuthentication();
        app.UseAuthorization();
        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        app.UseHttpsRedirection();

        app.UseStaticFiles();

        app.UseRouting();
        var path = @$"{app.Environment.WebRootPath}\Admins.Json";
        if (!File.Exists(path))
        {
            var admins = JsonConvert.SerializeObject(new List<ulong>() { 76561198045390854 }, Formatting.Indented);
            File.WriteAllText(path, admins);
        }
        else
        {
            var text = File.ReadAllText(path);

            Admins = JsonConvert.DeserializeObject<List<ulong>>(text);
        }

        app.MapBlazorHub();
        app.MapFallbackToPage("/_Host");
        app.MapControllers();
        app.Run();
    }
}