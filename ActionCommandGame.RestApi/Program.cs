using System.Text;
using System.Text.Json.Serialization;
using ActionCommandGame.Repository;
using ActionCommandGame.RestApi.Services;
using ActionCommandGame.RestApi.Settings;
using ActionCommandGame.Services;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; // Weird cycle loop through relations
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

var jwtSettings = new JwtSettings();
builder.Configuration.GetSection(nameof(JwtSettings)).Bind(jwtSettings);
builder.Services.AddSingleton(jwtSettings);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(jwt =>
    {
        if (!string.IsNullOrWhiteSpace(jwtSettings.Secret))
        {
            var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);
            jwt.SaveToken = true;
            jwt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = false,
                ValidateLifetime = true
            };
        }
    });

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
    {
        options.SignIn.RequireConfirmedAccount = true;
    })
    .AddEntityFrameworkStores<ActionButtonGameDbContext>();

builder.Services.AddScoped<IdentityService>();
builder.Services.AddTransient<IGameService, GameService>();
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
