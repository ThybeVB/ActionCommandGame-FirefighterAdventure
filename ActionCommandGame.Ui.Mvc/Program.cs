using ActionCommandGame.Sdk;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Settings;
using ActionCommandGame.Ui.Mvc.Stores;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
	.SetBasePath(Directory.GetCurrentDirectory())
	.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.AddControllersWithViews();

var appSettings = new AppSettings();
builder.Configuration.Bind(nameof(AppSettings), appSettings);
//builder.Services.AddSingleton(appSettings);

builder.Services.AddHttpClient("ActionCommandGameApi", options =>
{
	if (!string.IsNullOrWhiteSpace(appSettings.BaseAddress))
	{
		options.BaseAddress = new Uri(appSettings.BaseAddress);
	}
});

builder.Services.AddScoped<PlayerSdk>();
builder.Services.AddScoped<ItemSdk>();
builder.Services.AddScoped<PlayerItemSdk>();
builder.Services.AddScoped<PositiveGameEventSdk>();
builder.Services.AddScoped<NegativeGameEventSdk>();
builder.Services.AddScoped<GameSdk>();

builder.Services.AddScoped<ITokenStore, TokenStore>();
builder.Services.AddScoped<IdentitySdk>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthorization();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/Shared/AccessDenied";
});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = "/Index/Login";
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Index/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "AdminArea",
    pattern: "{area:exists}/{controller=Admin}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Index}/{action=Index}/{id?}");

app.Run();
