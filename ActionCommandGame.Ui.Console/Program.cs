using System;
using System.IO;
using System.Threading.Tasks;
using ActionCommandGame.Repository;
using ActionCommandGame.Sdk;
using ActionCommandGame.Services;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ActionCommandGame.Ui.ConsoleApp
{
    class Program
    {
        private static IServiceProvider ServiceProvider { get; set; }
        private static IConfiguration Configuration { get; set; }

        public static void Main(string[] args)
            => MainAsync(args).GetAwaiter().GetResult();

        private static async Task MainAsync(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();

            var database = ServiceProvider.GetRequiredService<ActionButtonGameDbContext>();
            database.Initialize();

            var game = ServiceProvider.GetRequiredService<Game>();
            await game.Start();
        }

        public static void ConfigureServices(IServiceCollection services)

        {
            var appSettings = new AppSettings();
            Configuration.Bind(nameof(AppSettings), appSettings);

            services.AddSingleton(appSettings);

            //Register the EntityFramework database In Memory as a Singleton
            services.AddDbContext<ActionButtonGameDbContext>(options =>
            {
                options.UseInMemoryDatabase(nameof(ActionButtonGameDbContext));
            }, ServiceLifetime.Singleton, ServiceLifetime.Singleton);

            services.AddHttpClient("ActionCommandGameApi", options =>
            {
                if (!string.IsNullOrWhiteSpace(appSettings.BaseAddress))
                {
                    options.BaseAddress = new Uri(appSettings.BaseAddress);
                }
            });

            services.AddScoped<PlayerSdk>();
            services.AddScoped<ItemSdk>();
            services.AddScoped<PositiveGameEventSdk>();
            services.AddScoped<NegativeGameEventSdk>();
            services.AddScoped<PlayerItemSdk>();
            
            services.AddTransient<Game>();

            services.AddTransient<IGameService, GameService>();
            services.AddTransient<IPositiveGameEventService, PositiveGameEventService>();
            services.AddTransient<INegativeGameEventService, NegativeGameEventService>();
            services.AddTransient<IItemService, ItemService>();
            services.AddTransient<IPlayerService, PlayerService>();
            services.AddTransient<IPlayerItemService, PlayerItemService>();
        }
    }
}
