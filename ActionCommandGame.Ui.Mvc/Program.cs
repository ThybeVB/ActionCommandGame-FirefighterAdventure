using ActionCommandGame.Sdk;
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
builder.Services.AddSingleton(appSettings);

builder.Services.AddHttpClient("ActionCommandGameApi", options =>
{
	if (!string.IsNullOrWhiteSpace(appSettings.BaseAddress))
	{
		options.BaseAddress = new Uri(appSettings.BaseAddress);
	}
});

builder.Services.AddScoped<PlayerSdk>();
builder.Services.AddScoped<ItemSdk>();

builder.Services.AddScoped<TokenStore>();
builder.Services.AddScoped<IdentitySdk>();

builder.Services.AddHttpContextAccessor();
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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Index}/{action=Index}/{id?}");

app.Run();
