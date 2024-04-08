using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActionCommandGame.Model;
using ActionCommandGame.Repository;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Extensions;
using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;
using Microsoft.EntityFrameworkCore;

namespace ActionCommandGame.Services
{
    public class PlayerItemService : IPlayerItemService
    {
        private readonly ActionButtonGameDbContext _database;

        public PlayerItemService(ActionButtonGameDbContext database)
        {
            _database = database;
        }

        public async Task<PlayerItemResult> Get(int id)
        {
            return await _database.PlayerItems.Select(p => new PlayerItemResult()
            {
                ItemId = p.ItemId,
                Item = p.Item,
                PlayerId = p.PlayerId,
                Player = p.Player,
            }).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IList<PlayerItem>> Find(int? playerId = null) //todo
        {
            var query = _database.PlayerItems.AsQueryable();

            if (playerId.HasValue)
            {
                query = query
                    .Where(pi => pi.PlayerId == playerId.Value);

            }

            //enkel de item en player id worden gemapt,niet de obj zelf TODO

            return await query.ToListAsync();
        }

        public async Task<ServiceResult<PlayerItem>> Create(int playerId, int itemId)
        {
            var player = _database.Players.SingleOrDefault(p => p.Id == playerId);
            if (player == null)
            {
                return new ServiceResult<PlayerItem>().PlayerNotFound();
            }

            var item = _database.Items.SingleOrDefault(i => i.Id == itemId);
            if (item == null)
            {
                return new ServiceResult<PlayerItem>().ItemNotFound();
            }

            var playerItem = new PlayerItem
            {
                ItemId = itemId,
                Item = item,
                PlayerId = playerId,
                Player = player
            };
            _database.PlayerItems.Add(playerItem);
            player.Inventory.Add(playerItem);
            item.PlayerItems.Add(playerItem);

            //Auto Equip the item you bought
            if (item.Fuel > 0)
            {
                playerItem.RemainingFuel = item.Fuel;
                player.CurrentFuelPlayerItemId = playerItem.Id;
                player.CurrentFuelPlayerItem = playerItem;
            }
            if (item.Attack > 0)
            {
                playerItem.RemainingAttack = item.Attack;
                player.CurrentAttackPlayerItemId = playerItem.Id;
                player.CurrentAttackPlayerItem = playerItem;
            }
            if (item.Defense > 0)
            {
                playerItem.RemainingDefense = item.Defense;
                player.CurrentDefensePlayerItemId = playerItem.Id;
                player.CurrentDefensePlayerItem = playerItem;
            }

            await _database.SaveChangesAsync();

            return new ServiceResult<PlayerItem>(playerItem);
        }

        public async Task<PlayerItemResult> Update(int id, PlayerItemRequest playerItem)
        {
            var i = await _database.PlayerItems.FirstOrDefaultAsync(pi => pi.Id == id);

            if (i is null)
            {
                return null;
            }

            i.ItemId = playerItem.ItemId;
            //i.Item = playerItem.Item;
            i.PlayerId = playerItem.PlayerId;
            //i.Player = playerItem.Player;

            await _database.SaveChangesAsync();

            return await Get(id);
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var playerItem = await _database.PlayerItems.SingleOrDefaultAsync(pi => pi.Id == id);

            if (playerItem == null)
            {
                return new ServiceResult().NotFound();
            }
            
            var player = playerItem.Player;
            player.Inventory.Remove(playerItem);
            
            var item = playerItem.Item;
            item.PlayerItems.Remove(playerItem);

            //Clear up equipment
            if (player.CurrentFuelPlayerItemId == id)
            {
                player.CurrentFuelPlayerItemId = null;
                player.CurrentFuelPlayerItem = null;
            }
            if (player.CurrentAttackPlayerItemId == id)
            {
                player.CurrentAttackPlayerItemId = null;
                player.CurrentAttackPlayerItem = null;
            }
            if (player.CurrentDefensePlayerItemId == id)
            {
                player.CurrentDefensePlayerItemId = null;
                player.CurrentDefensePlayerItem = null;
            }

            _database.PlayerItems.Remove(playerItem);

            await _database.SaveChangesAsync();

            return new ServiceResult();
        }
        
    }
}
