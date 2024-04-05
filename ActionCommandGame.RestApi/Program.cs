using ActionCommandGame.Repository;
using ActionCommandGame.Services;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Settings;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var configurationBuilder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
var config = configurationBuilder.Build();

var appSettings = new AppSettings();
config.Bind(nameof(AppSettings), appSettings);

builder.Services.AddSingleton(appSettings);

builder.Services.AddDbContext<ActionButtonGameDbContext>(options =>
{
    options.UseInMemoryDatabase(nameof(ActionButtonGameDbContext));
    //options.UseSqlServer("Server=.\\SqlExpress;Database=ActionCommandGame;Trusted_Connection=True;TrustServerCertificate=true"); TODO
});

builder.Services.AddTransient<IGameService, GameService>();
builder.Services.AddTransient<IPositiveGameEventService, PositiveGameEventService>();
builder.Services.AddTransient<INegativeGameEventService, NegativeGameEventService>();
builder.Services.AddTransient<IItemService, ItemService>();
builder.Services.AddTransient<IPlayerItemService, PlayerItemService>();
builder.Services.AddTransient<IPlayerService, PlayerService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using var scope = app.Services.CreateScope();

    var dbContext = scope.ServiceProvider.GetRequiredService<ActionButtonGameDbContext>();
    if (dbContext.Database.IsInMemory())
    {
        dbContext.Initialize();
    }
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();