using ActionCommandGame.Model;
using ActionCommandGame.Repository.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ActionCommandGame.Repository
{
    public class ActionButtonGameDbContext : IdentityDbContext<Player, IdentityRole, string>
    {
        public ActionButtonGameDbContext(DbContextOptions<ActionButtonGameDbContext> options) : base(options)
        {

        }

        public DbSet<PositiveGameEvent> PositiveGameEvents { get; set; }
        public DbSet<NegativeGameEvent> NegativeGameEvents { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerItem> PlayerItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ConfigureRelationships();
            base.OnModelCreating(modelBuilder);
        }

        public void Initialize()
        {
            Database.EnsureCreated();

            if (!Items.Any())
            {
                GenerateAttackItems();
                GenerateDefenseItems();
                GenerateFoodItems();
                GenerateDecorativeItems();
            }

            if (!PositiveGameEvents.Any())
            {
                GeneratePositiveGameEvents();
            }

            if (!NegativeGameEvents.Any())
            {
                GenerateNegativeGameEvents();
            }

            SaveChanges();
        }

        private void GeneratePositiveGameEvents()
        {
            // Neutral Events
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Patrol with no incidents", Description = "You went on patrol and found no incidents.", Probability = 1000 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Routine equipment check", Description = "All equipment is in working order. No changes.", Probability = 950 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Neighborhood watch meeting", Description = "Attend a community meeting to discuss fire safety.", Probability = 900, Money = 15, Experience = 20 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Training exercise", Description = "Participate in a routine training exercise. No immediate benefits.", Probability = 850, Experience = 25 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Inspect hydrants", Description = "Perform routine inspections of fire hydrants. Everything is in order.", Probability = 800 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Fire safety drill", Description = "Conduct a fire drill at a local school. Gain experience but no immediate benefits.", Probability = 750, Experience = 20 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Community outreach", Description = "Hand out fire safety pamphlets in the community.", Probability = 700, Experience = 2 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Daily station maintenance", Description = "Clean and maintain the fire station. No immediate benefits.", Probability = 650, Experience = 1 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Administrative duties", Description = "Complete necessary paperwork and administrative tasks.", Probability = 600, Experience = 5 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Restock supplies", Description = "Restock the fire truck with standard supplies. Everything is as expected.", Probability = 550, Experience = 10 });

            // Positive Events
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Rescue a trapped civilian", Description = "You heroically rescued a civilian trapped in a burning building.", Probability = 1000, Money = 50, Experience = 60 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Find a hidden water source", Description = "You discovered a hidden water source that can be used to fight fires.", Probability = 950, Money = 40, Experience = 50 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Extinguish a major fire", Description = "You successfully extinguished a major fire, preventing widespread damage.", Probability = 900, Money = 60, Experience = 70 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Discover a safe zone", Description = "You found a safe zone where the team can regroup and recover.", Probability = 850, Experience = 10, Money = 20});
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Receive backup from a fellow firefighter", Description = "A fellow firefighter joined you, providing much-needed support.", Probability = 800, Money = 100, Experience = 50});
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Find an extra first aid kit", Description = "You found an extra first aid kit to help treat injuries.", Probability = 750, Experience = 20, Money = 5 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Successfully navigate through a smoke-filled room", Description = "You navigated through a smoke-filled room without incident.", Probability = 700, Experience = 35 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Replenish gear at a supply station", Description = "You replenished your gear at a nearby supply station.", Probability = 650, Money = 35, Experience = 10});
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Successfully evacuate a building", Description = "You successfully evacuated a building full of people.", Probability = 600, Money = 35, Experience = 45 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Receive a community hero award", Description = "You received a community hero award for your bravery.", Probability = 550, Money = 40, Experience = 50 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Find a useful tool in the debris", Description = "You found a useful tool in the debris that can aid in firefighting.", Probability = 500, Experience = 15, Money = 20});
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Rescue a fellow firefighter", Description = "You rescued a fellow firefighter who was in danger.", Probability = 450, Experience = 40, Money = 15});
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Receive a morale boost from rescued civilians", Description = "The gratitude of rescued civilians boosted your morale.", Probability = 400, Experience = 30 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Locate an emergency exit quickly", Description = "You quickly located an emergency exit, ensuring a safe retreat.", Probability = 350, Experience = 50, Money = 75});
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Get a commendation from the fire chief", Description = "You received a commendation from the fire chief for your actions.", Probability = 300, Money = 25, Experience = 35 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Discover an efficient firefighting technique", Description = "You discovered an efficient technique for fighting fires.", Probability = 250, Experience = 150 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Save valuable property from fire", Description = "You saved valuable property from being destroyed by fire.", Probability = 200, Money = 100, Experience = 20 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Receive a surprise supply drop", Description = "You received a surprise supply drop with essential equipment.", Probability = 150, Money = 500, Experience = 300 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Uncover a water main", Description = "You uncovered a water main that can be used for firefighting.", Probability = 100, Money = 500, Experience = 350});
        }

        public void GenerateNegativeGameEvents()
        {
            NegativeGameEvents.Add(new NegativeGameEvent
            {
                Name = "Slip on a wet surface",
                Description = "You slip on a wet surface at the station.",
                DefenseWithGearDescription = "Your gear provides some grip, but you still take a minor fall.",
                DefenseWithoutGearDescription = "Without proper footwear, you fall hard, hurting yourself.",
                DefenseLoss = 5,
                Probability = 100
            });

            NegativeGameEvents.Add(new NegativeGameEvent
            {
                Name = "Fall off the fire pole",
                Description = "You lose your grip and fall off the fire pole.",
                DefenseWithGearDescription = "Your protective gear cushions your fall, preventing serious injury.",
                DefenseWithoutGearDescription = "Without proper gear, you hit the ground hard.",
                DefenseLoss = 8,
                Probability = 95
            });

            NegativeGameEvents.Add(new NegativeGameEvent
            {
                Name = "Caught in a flashover",
                Description = "A sudden burst of flames engulfs the area.",
                DefenseWithGearDescription = "Your protective gear saves you from the worst of the flashover, but you still take some damage.",
                DefenseWithoutGearDescription = "Without proper gear, you are severely burned by the flashover.",
                DefenseLoss = 20,
                Probability = 30
            });

            NegativeGameEvents.Add(new NegativeGameEvent
            {
                Name = "Run out of fuel mid-mission",
                Description = "You run out of fuel during a critical moment.",
                DefenseWithGearDescription = "Your backup fuel canister helps you continue, but the delay costs you.",
                DefenseWithoutGearDescription = "Without backup fuel, you are left stranded and vulnerable.",
                DefenseLoss = 10,
                Probability = 60
            });

            NegativeGameEvents.Add(new NegativeGameEvent
            {
                Name = "Encounter a structural collapse",
                Description = "The building starts collapsing around you.",
                DefenseWithGearDescription = "Your helmet and protective gear shield you from most of the falling debris.",
                DefenseWithoutGearDescription = "Without proper protection, you are hit hard by falling debris.",
                DefenseLoss = 35,
                Probability = 20
            });

            NegativeGameEvents.Add(new NegativeGameEvent
            {
                Name = "Suffer smoke inhalation",
                Description = "You inhale a large amount of smoke.",
                DefenseWithGearDescription = "Your breathing apparatus protects you from the worst effects, but you still feel weakened.",
                DefenseWithoutGearDescription = "Without a breathing apparatus, you choke on the smoke, severely weakening you.",
                DefenseLoss = 15,
                Probability = 70
            });

            NegativeGameEvents.Add(new NegativeGameEvent
            {
                Name = "Gear malfunction",
                Description = "Your gear malfunctions at a critical moment.",
                DefenseWithGearDescription = "You manage to fix the malfunction quickly, but not before taking some damage.",
                DefenseWithoutGearDescription = "With no gear to rely on, you are left completely exposed.",
                DefenseLoss = 40,
                Probability = 10
            });
        }

        private void GenerateAttackItems()
        {
            Items.Add(new Item { Name = "Basic Hatchet", Description = "A basic hatchet for breaking through obstacles.", Attack = 50, Price = 50 });
            Items.Add(new Item { Name = "Fire Extinguisher", Description = "A standard fire extinguisher for putting out flames.", Attack = 150, Price = 100 });
            Items.Add(new Item { Name = "Heavy-Duty Crowbar", Description = "A sturdy crowbar for prying open doors and windows.", Attack = 200, Price = 150 });
            Items.Add(new Item { Name = "Fire Axe", Description = "A sharp fire axe for chopping through barriers.", Attack = 300, Price = 200 });
            Items.Add(new Item { Name = "Hydraulic Cutter", Description = "A powerful hydraulic cutter for cutting through metal and debris.", Attack = 500, Price = 300 });
            Items.Add(new Item { Name = "Chainsaw", Description = "A chainsaw for quickly cutting through heavy materials.", Attack = 750, Price = 400 });
            Items.Add(new Item { Name = "Thermal Lance", Description = "A thermal lance for melting through metal and other tough materials.", Attack = 1000, Price = 500 });
            Items.Add(new Item { Name = "Portable Fire Pump", Description = "A portable fire pump for dousing large fires quickly.", Attack = 2000, Price = 600 });
            Items.Add(new Item { Name = "Advanced Fire Extinguisher", Description = "An advanced fire extinguisher with enhanced capabilities.", Attack = 3000, Price = 700 });
            Items.Add(new Item { Name = "High-Power Water Cannon", Description = "A high-power water cannon for maximum fire suppression.", Attack = 5000, Price = 800 });

        }

        private void GenerateDefenseItems()
        {
            Items.Add(new Item { Name = "Basic Firefighter Suit", Description = "A basic suit offering minimal protection.", Defense = 50, Price = 50 });
            Items.Add(new Item { Name = "Protective Gloves", Description = "Gloves that protect your hands from burns and cuts.", Defense = 100, Price = 75 });
            Items.Add(new Item { Name = "Fire-Resistant Helmet", Description = "A helmet designed to protect against falling debris and high heat.", Defense = 150, Price = 100 });
            Items.Add(new Item { Name = "Standard Firefighter Boots", Description = "Boots that provide protection and grip in hazardous environments.", Defense = 200, Price = 150 });
            Items.Add(new Item { Name = "Advanced Firefighter Suit", Description = "A suit offering enhanced protection against heat and flames.", Defense = 300, Price = 200 });
            Items.Add(new Item { Name = "Full Protective Gear Set", Description = "A complete set of protective gear including suit, helmet, and boots.", Defense = 400, Price = 250 });
            Items.Add(new Item { Name = "Breathing Apparatus", Description = "A breathing apparatus to protect against smoke inhalation.", Defense = 500, Price = 300 });
            Items.Add(new Item { Name = "Thermal Imaging Camera", Description = "A camera that helps locate hotspots and navigate through smoke.", Defense = 600, Price = 350 });
            Items.Add(new Item { Name = "Kevlar Reinforced Suit", Description = "A suit reinforced with Kevlar for maximum protection.", Defense = 2000, Price = 1000 });
            Items.Add(new Item { Name = "Ultimate Firefighter Armor", Description = "The ultimate armor providing the highest level of protection.", Defense = 20000, Price = 10000 });
        }

        private void GenerateFoodItems()
        {
            Items.Add(new Item { Name = "Protein Shake", ActionCooldownSeconds = 50, Fuel = 5, Price = 10 });
            Items.Add(new Item { Name = "Granola Bar", ActionCooldownSeconds = 45, Fuel = 10, Price = 15 });
            Items.Add(new Item { Name = "Hydration Pack", ActionCooldownSeconds = 40, Fuel = 15, Price = 20 });
            Items.Add(new Item { Name = "Energy Drink", ActionCooldownSeconds = 35, Fuel = 20, Price = 25 });
            Items.Add(new Item { Name = "Emergency Meal Kit", ActionCooldownSeconds = 30, Fuel = 25, Price = 30 });
            Items.Add(new Item { Name = "High-Calorie Snack", ActionCooldownSeconds = 30, Fuel = 30, Price = 50 });
            Items.Add(new Item { Name = "Firefighter's Lunch Box", ActionCooldownSeconds = 25, Fuel = 50, Price = 75 });
            Items.Add(new Item { Name = "Rehydration Solution", ActionCooldownSeconds = 25, Fuel = 75, Price = 100 });
            Items.Add(new Item { Name = "McDonalds Big Mac menu", ActionCooldownSeconds = 20, Fuel = 100, Price = 150 });
            Items.Add(new Item { Name = "Deluxe Firefighter Feast", ActionCooldownSeconds = 15, Fuel = 500, Price = 1000 });

#if DEBUG
            Items.Add(new Item { Name = "Developer Food", ActionCooldownSeconds = 1, Fuel = 1000, Price = 1 });
            Items.Add(new Item
            {
                Name = "GOD MODE",
                Description = "This is almost how a GOD must feel.",
                Attack = 1000000,
                Defense = 1000000,
                Fuel = 1000000,
                ActionCooldownSeconds = 1,
                Price = 10000000
            });
#endif
        }

        private void GenerateDecorativeItems()
        {
            Items.Add(new Item { Name = "Mini Firetruck Model", Description = "A miniature model of a firetruck. It's just for show.", Price = 50 });
            Items.Add(new Item { Name = "Firefighter Trophy", Description = "A shiny trophy for display. It has no practical use.", Price = 500 });
            Items.Add(new Item { Name = "Commemorative Fire Helmet", Description = "A decorative helmet to celebrate your achievements. It does nothing.", Price = 10000 });
        }

    }
}
