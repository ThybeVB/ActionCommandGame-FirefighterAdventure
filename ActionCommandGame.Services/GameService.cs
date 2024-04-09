using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActionCommandGame.Model;
using ActionCommandGame.Repository;
using ActionCommandGame.Sdk;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Extensions;
using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;
using ActionCommandGame.Settings;

namespace ActionCommandGame.Services
{
    public class GameService : IGameService
    {
        private readonly AppSettings _appSettings;
        private readonly ActionButtonGameDbContext _database;
        private readonly IPlayerService _playerService;
        private readonly IPositiveGameEventService _positiveGameEventService;
        private readonly INegativeGameEventService _negativeGameEventService;
        private readonly IItemService _itemService;
        private readonly IPlayerItemService _playerItemService;
        //private readonly PlayerSdk _playerSdk;
        //private readonly ItemSdk _itemSdk;
        //private readonly PlayerItemSdk _playerItemSdk;

        public GameService(
            AppSettings appSettings,
            ActionButtonGameDbContext database,
            IPlayerService playerService,
            IPositiveGameEventService positiveGameEventService,
            INegativeGameEventService negativeGameEventService
            /*IItemService itemService*/,
            IPlayerItemService playerItemService
            //PlayerSdk playerSdk,
            //ItemSdk itemSdk,
            /*PlayerItemSdk playerItemSdk*/)
        {
            _appSettings = appSettings;
            _database = database;
            _playerService = playerService;
            _positiveGameEventService = positiveGameEventService;
            _negativeGameEventService = negativeGameEventService;
            //_itemService = itemService;
            _playerItemService = playerItemService;
            //_playerSdk = playerSdk;
            //_itemSdk = itemSdk;
            //_playerItemSdk = playerItemSdk;
        }

        public async Task<ServiceResult<GameResult>> PerformAction(int playerId)
        {
            //Check Cooldown
            var player = await _playerService.Get(playerId);
            //var player = await _playerSdk.Get(playerId);

            var elapsedSeconds = DateTime.UtcNow.Subtract(player.LastActionExecutedDateTime).TotalSeconds;
            var cooldownSeconds = _appSettings.DefaultCooldown;
            if (player.CurrentFuelPlayerItem != null)
            {
                cooldownSeconds = player.CurrentFuelPlayerItem.Item.ActionCooldownSeconds;
            }

            if (elapsedSeconds < cooldownSeconds)
            {
                var waitSeconds = Math.Ceiling(cooldownSeconds - elapsedSeconds);
                var waitText = $"You are still a bit tired. You have to wait another {waitSeconds} seconds.";
                return new ServiceResult<GameResult>
                {
                    Data = new GameResult { Player = player },
                    Messages = new List<ServiceMessage> { new ServiceMessage { Code = "Cooldown", Message = waitText } }
                };
            }

            var hasAttackItem = player.CurrentAttackPlayerItem != null;
            var positiveGameEvent = await _positiveGameEventService.GetRandomPositiveGameEvent(hasAttackItem);
            if (positiveGameEvent == null)
            {
                return new ServiceResult<GameResult>{Messages = 
                    new List<ServiceMessage>
                    {
                        new ServiceMessage
                        {
                            Code = "Error",
                            Message = "Something went wrong getting the Positive Game Event.",
                            MessagePriority = MessagePriority.Error
                        }
                    }};
            }

            var negativeGameEvent = await _negativeGameEventService.GetRandomNegativeGameEvent();

            var oldLevel = player.GetLevel();

            player.Money += positiveGameEvent.Money;
            player.Experience += positiveGameEvent.Experience;

            var newLevel = player.GetLevel();

            var levelMessages = new List<ServiceMessage>();
            //Check if we leveled up
            if (oldLevel < newLevel)
            {
                levelMessages = new List<ServiceMessage>{new ServiceMessage{Code="LevelUp", Message = $"Congratulations, you arrived at level {newLevel}"}};
            }

            //Consume fuel
            var fuelMessages = await ConsumeFuel(player);

            var attackMessages = new List<ServiceMessage>();
            //Consume attack when we got some loot
            if (positiveGameEvent.Money > 0)
            {
                attackMessages.AddRange(await ConsumeAttack(player));
            }

            var defenseMessages = new List<ServiceMessage>();
            var negativeGameEventMessages = new List<ServiceMessage>();
            if (negativeGameEvent != null)
            {
                //Check defense consumption
                if (player.CurrentDefensePlayerItem != null)
                {
                    negativeGameEventMessages.Add(new ServiceMessage { Code = "DefenseWithGear", Message = negativeGameEvent.DefenseWithGearDescription });
                    defenseMessages.AddRange(await ConsumeDefense(player, negativeGameEvent.DefenseLoss));
                }
                else
                {
                    negativeGameEventMessages.Add(new ServiceMessage { Code = "DefenseWithoutGear", Message = negativeGameEvent.DefenseWithoutGearDescription });

                    //If we have no defense item, consume the defense loss from Fuel and Attack
                    defenseMessages.AddRange(await ConsumeFuel(player, negativeGameEvent.DefenseLoss));
                    defenseMessages.AddRange(await ConsumeAttack(player, negativeGameEvent.DefenseLoss));
                }
            }

            var warningMessages = GetWarningMessages(player);

            player.LastActionExecutedDateTime = DateTime.UtcNow;

            //Save Player
            //await _playerSdk.Update(playerId, player);
            await _playerService.Update(playerId, player);
            await _database.SaveChangesAsync();

            var gameResult = new GameResult
            {
                Player = player,
                PositiveGameEvent = positiveGameEvent,
                NegativeGameEvent = negativeGameEvent,
                NegativeGameEventMessages = negativeGameEventMessages
            };

            var serviceResult = new ServiceResult<GameResult>
            {
                Data = gameResult
            };

            //Add all the messages to the player
            serviceResult.WithMessages(levelMessages);
            serviceResult.WithMessages(warningMessages);
            serviceResult.WithMessages(fuelMessages);
            serviceResult.WithMessages(attackMessages);
            serviceResult.WithMessages(defenseMessages);

            return serviceResult;
        }

        public async Task<ServiceResult<BuyResult>> Buy(int playerId, int itemId)
        {
            //var player = await _playerSdk.Get(playerId);
            var player = await _playerService.Get(playerId);

            if (player == null)
            {
                return new ServiceResult<BuyResult>().PlayerNotFound();
            }

            //var item = await _itemSdk.Get(itemId);
            var item = await _itemService.Get(itemId);
            if (item == null)
            {
                return new ServiceResult<BuyResult>().ItemNotFound();
            }

            if (item.Price > player.Money)
            {
                return new ServiceResult<BuyResult>().NotEnoughMoney();
            }
            
            await _playerItemService.Create(playerId, itemId);
            //await _playerItemSdk.Create(new PlayerItem
            //{
            //    PlayerId = playerId,
            //    //Player = player,
            //    ItemId = itemId,
            //    //Item = item
            //});

            player.Money -= item.Price;

            //SaveChanges
            await _database.SaveChangesAsync();

            var buyResult = new BuyResult
            {
                Player = player,
                Item = item
            };
            return new ServiceResult<BuyResult> { Data = buyResult };
        }

        private async Task<IList<ServiceMessage>> ConsumeFuel(PlayerResult player, int fuelLoss = 1)
        {
            if (player.CurrentFuelPlayerItem != null && player.CurrentFuelPlayerItemId.HasValue)
            {
                player.CurrentFuelPlayerItem.RemainingFuel -= fuelLoss;
                if (player.CurrentFuelPlayerItem.RemainingFuel <= 0)
                {
                    await _playerItemService.Delete(player.CurrentFuelPlayerItemId.Value);
                    //await _playerItemSdk.Delete(player.CurrentFuelPlayerItemId.Value);

                    //Load a new Fuel Item from inventory
                    var newFuelItem = player.Inventory
                        .Where(pi => pi.Item.Fuel > 0)
                        .OrderByDescending(pi => pi.Item.Fuel).FirstOrDefault();

                    if (newFuelItem != null)
                    {
                        player.CurrentFuelPlayerItem = newFuelItem;
                        player.CurrentFuelPlayerItemId = newFuelItem.Id;
                        return new List<ServiceMessage>{new ServiceMessage
                        {
                            Code = "ReloadedFuel",
                            Message = $"You are hungry and open a new {newFuelItem.Item.Name}. Yummy!"
                        }};
                    }

                    return new List<ServiceMessage>{new ServiceMessage
                    {
                        Code = "NoFood",
                        Message = "You are so hungry. You look into your bag and find ... nothing!",
                        MessagePriority = MessagePriority.Warning
                    }};
                }
            }

            return new List<ServiceMessage>();
        }

        private async Task<IList<ServiceMessage>> ConsumeAttack(PlayerResult player, int attackLoss = 1)
        {
            if (player.CurrentAttackPlayerItem != null && player.CurrentAttackPlayerItemId.HasValue)
            {
                var oldAttackItem = player.CurrentAttackPlayerItem;
                player.CurrentAttackPlayerItem.RemainingAttack -= attackLoss;
                if (player.CurrentAttackPlayerItem.RemainingAttack <= 0)
                {
                    await _playerItemService.Delete(player.CurrentAttackPlayerItemId.Value);
                    //await _playerItemSdk.Delete(player.CurrentAttackPlayerItemId.Value);

                    //Load a new Attack Item from inventory
                    var newAttackItem = player.Inventory
                        .Where(pi => pi.Item.Attack > 0)
                        .OrderByDescending(pi => pi.Item.Attack).FirstOrDefault();
                    if (newAttackItem != null)
                    {
                        player.CurrentAttackPlayerItem = newAttackItem;
                        player.CurrentAttackPlayerItemId = newAttackItem.Id;
                        return new List<ServiceMessage>{new ServiceMessage
                        {
                            Code = "ReloadedAttack",
                            Message = $"You just broke {oldAttackItem.Item.Name}. No worries, you swiftly wield a new {newAttackItem.Item.Name}. Yeah!",

                        }};
                    }

                    return new List<ServiceMessage>{new ServiceMessage
                    {
                        Code = "NoAttack",
                        Message = $"You just broke {oldAttackItem.Item.Name}. This was your last tool. Bummer!",
                        MessagePriority = MessagePriority.Warning
                    }};
                }
            }
            else
            {
                //If we don't have any attack tools, just consume more fuel in stead
                await ConsumeFuel(player);
            }

            return new List<ServiceMessage>();
        }

        private async Task<IList<ServiceMessage>> ConsumeDefense(PlayerResult player, int defenseLoss = 1)
        {
            if (player.CurrentDefensePlayerItem != null && player.CurrentDefensePlayerItemId.HasValue)
            {
                var oldDefenseItem = player.CurrentDefensePlayerItem;
                player.CurrentDefensePlayerItem.RemainingDefense -= defenseLoss;
                if (player.CurrentDefensePlayerItem.RemainingDefense <= 0)
                {
                    await _playerItemService.Delete(player.CurrentDefensePlayerItemId.Value);
                    //await _playerItemSdk.Delete(player.CurrentDefensePlayerItemId.Value);

                    //Load a new Defense Item from inventory
                    var newDefenseItem = player.Inventory
                        .Where(pi => pi.Item.Defense > 0)
                        .OrderByDescending(pi => pi.Item.Defense).FirstOrDefault();
                    ;
                    if (newDefenseItem != null)
                    {
                        player.CurrentDefensePlayerItem = newDefenseItem;
                        player.CurrentDefensePlayerItemId = newDefenseItem.Id;

                        return new List<ServiceMessage>{new ServiceMessage
                        {
                            Code = "ReloadedDefense",
                            Message = $"Your {oldDefenseItem.Item.Name} is starting to smell. No worries, you swiftly put on a freshly washed {newDefenseItem.Item.Name}. Yeah!"
                        }};
                    }

                    return new List<ServiceMessage>{new ServiceMessage
                    {
                        Code = "NoAttack",
                        Message = $"You just lost {oldDefenseItem.Item.Name}. You continue without protection. Did I just see something move?",
                        MessagePriority = MessagePriority.Warning
                    }};
                }
            }
            else
            {
                //If we don't have defensive gear, just consume more fuel in stead.
                await ConsumeFuel(player);
            }

            return new List<ServiceMessage>();
        }

        private IList<ServiceMessage> GetWarningMessages(PlayerResult player)
        {
            var serviceMessages = new List<ServiceMessage>();

            if (player.CurrentFuelPlayerItem == null)
            {
                var infoText = "Playing without food is hard. You need a long time to recover. Consider buying food from the shop.";
                serviceMessages.Add(new ServiceMessage { Code = "NoFood", Message = infoText, MessagePriority = MessagePriority.Warning });
            }
            if (player.CurrentAttackPlayerItem == null)
            {
                var infoText = "Playing without tools is hard. You lost extra fuel. Consider buying tools from the shop.";
                serviceMessages.Add(new ServiceMessage { Code = "NoTools", Message = infoText, MessagePriority = MessagePriority.Warning });
            }
            if (player.CurrentDefensePlayerItem == null)
            {
                var infoText = "Playing without gear is hard. You lost extra fuel. Consider buying gear from the shop.";
                serviceMessages.Add(new ServiceMessage { Code = "NoGear", Message = infoText, MessagePriority = MessagePriority.Warning });
            }

            return serviceMessages;
        }
    }
}
