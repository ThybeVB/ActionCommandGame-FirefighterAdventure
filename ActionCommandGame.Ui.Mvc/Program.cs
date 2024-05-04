using ActionCommandGame.Sdk;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient("ActionCommandGameApi", options =>
{
	if (!string.IsNullOrWhiteSpace("https://localhost:7065")) //todo add appconnfig later
	{
		options.BaseAddress = new Uri("https://localhost:7065");
	}
});

builder.Services.AddScoped<PlayerSdk>();

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
