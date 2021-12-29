using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Vk.Auth.Handlers;
using Vk.Auth.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Configuration.AddJsonFile("appsettings.json");
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddHttpClient();
builder.Services.AddScoped<VkAuthHandler>();
builder.Services.Configure<VkAuthOptions>(builder.Configuration.GetSection("VkAuthOptions"));
builder.Services.AddScoped<VkHandler>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient("vk", f =>
    {
        f.BaseAddress = new Uri("https://api.vk.com/method/");
    })
    .AddHttpMessageHandler<VkHandler>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(x =>
    {
        x.LoginPath = new PathString("/Auth/Login");
    })
    .AddOAuth<VkAuthOptions, VkAuthHandler>("vkontakte", d =>
    {
        builder.Configuration.GetSection("VkAuthOptions").Bind(d);
        d.CallbackPath = "/sign-in-vkontakte";
        d.SaveTokens = true;
        d.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
        d.ClaimActions.MapJsonKey(ClaimTypes.Name, "first_name");
        d.ClaimActions.MapJsonKey(ClaimTypes.GivenName, "last_name");
    });

var app = builder.Build();

app.UseAuthentication();
app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(x =>
{
    x.MapControllers();
});

await app.RunAsync();