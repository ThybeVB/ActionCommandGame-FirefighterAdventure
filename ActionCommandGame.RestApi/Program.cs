using System.Text.Json.Serialization;
using ActionCommandGame.Repository;
using ActionCommandGame.Sdk;
using ActionCommandGame.Services;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; // wtf man
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Action Command Game API", Version = "v1" });

    var filePath = Path.Combine(AppContext.BaseDirectory, "ActionCommandGame.RestApi.xml");
    c.IncludeXmlComments(filePath);
});

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

//builder.Services.AddTransient<IGameService, GameService>();
builder.Services.AddTransient<IPlayerService, PlayerService>();
builder.Services.AddTransient<IItemService, ItemService>();
builder.Services.AddTransient<IPositiveGameEventService, PositiveGameEventService>();
builder.Services.AddTransient<INegativeGameEventService, NegativeGameEventService>();
builder.Services.AddTransient<IPlayerItemService, PlayerItemService>();

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
