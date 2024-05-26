using System.Text;
using System.Text.Json.Serialization;
using ActionCommandGame.Model;
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
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Fireman Adventure API", Version = "v1" });

    var filePath = Path.Combine(AppContext.BaseDirectory, "ActionCommandGame.RestApi.xml");
    c.IncludeXmlComments(filePath);
});

var configurationBuilder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
var config = configurationBuilder.Build();

var appSettings = new AppSettings();
builder.Configuration.GetSection(nameof(AppSettings)).Bind(appSettings);
builder.Services.AddSingleton(appSettings);

builder.Services.AddDbContext<ActionButtonGameDbContext>(options =>
{
    options.UseSqlServer(appSettings.ConnectionString);
}, ServiceLifetime.Singleton);


var jwtSettings = new JwtSettings();
builder.Configuration.GetSection(nameof(JwtSettings)).Bind(jwtSettings);
builder.Services.AddSingleton(jwtSettings);

builder.Services.AddAuthorization();
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

builder.Services.AddIdentityCore<Player>(options =>
    {
        options.SignIn.RequireConfirmedAccount = true;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ActionButtonGameDbContext>();

builder.Services.AddTransient<IGameService, GameService>();
builder.Services.AddTransient<IPlayerService, PlayerService>();
builder.Services.AddTransient<IItemService, ItemService>();
builder.Services.AddTransient<IPositiveGameEventService, PositiveGameEventService>();
builder.Services.AddTransient<INegativeGameEventService, NegativeGameEventService>();
builder.Services.AddTransient<IPlayerItemService, PlayerItemService>();
builder.Services.AddScoped<IdentityService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using var scope = app.Services.CreateScope();

    await SeedRolesAndAdminAsync(scope.ServiceProvider);

    var dbContext = scope.ServiceProvider.GetRequiredService<ActionButtonGameDbContext>();
    dbContext.Initialize();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
{
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = serviceProvider.GetRequiredService<UserManager<Player>>();

    var adminExists = await userManager.FindByEmailAsync("vanbeerselthybe@gmail.com");
    if (adminExists == null)
    {
        var adminUser = new Player()
        {
            Email = "vanbeerselthybe@gmail.com",
            Name = "Thybe",
            UserName = "vanbeerselthybe@gmail.com",
            NormalizedUserName = "VANBEERSELTHYBE@GMAIL.COM",
            NormalizedEmail = "VANBEERSELTHYBE@GMAIL.COM",
            PasswordHash = "AQAAAAIAAYagAAAAEBsAuWOMcIei3bmWhcPQD7JjVasnyGbzraUEZq5eMcFJ7I0h5ZwNFxDkiglQlOzAHQ==", // Vives4Lyfe@
            ConcurrencyStamp = "c28d772c-2981-4a5b-b34f-ef3bc342fd16",
            SecurityStamp = "YENCZX7ECN5PSKCCTDPLG6JVY4J5DOXI",
            Money = 25
        };
        await userManager.CreateAsync(adminUser);

        var roleExists = await roleManager.RoleExistsAsync("Admin");
        if (!roleExists)
        { 
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }

        await userManager.AddToRoleAsync(adminUser, "Admin");
    }

    var existingUser = await userManager.FindByEmailAsync("bavo.ketels@vives.be");
    if (existingUser == null)
    {
        var user = new Player()
        {
            Email = "bavo.ketels@vives.be",
            Name = "Bavo Ketels",
            UserName = "bavo.ketels@vives.be",
            NormalizedUserName = "BAVO.KETELS@VIVES.BE",
            NormalizedEmail = "BAVO.KETELS@VIVES.BE",
            PasswordHash = "AQAAAAIAAYagAAAAEJjqloKN/74dMzcWRMKRllvPbsZQ59WHbc2p1xrnt7sqr3fdz4vqIe1d5+5Xz6K7tA==", // Test123$
            ConcurrencyStamp = "0a2a3992-859e-4204-95f0-ea9bc9a96adc",
            SecurityStamp = "YJ7UIFZC3FQHY4JOPZF6MBC4THU3K4IL",
            Money = 25
        };
        await userManager.CreateAsync(user);
    }
}
