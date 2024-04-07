using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActionCommandGame.Model;
using ActionCommandGame.Repository;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;
using Microsoft.EntityFrameworkCore;

namespace ActionCommandGame.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly ActionButtonGameDbContext _database;
        private readonly IPlayerItemService _playerItemService;

        public PlayerService(ActionButtonGameDbContext database, IPlayerItemService playerItemService)
        {
            _database = database;
            _playerItemService = playerItemService;
        }

        public async Task<PlayerResult> Get(int id)
        {
            var player = await _database.Players
                .Select(p => new PlayerResult
                {
                    Id = p.Id,
                    Name = p.Name,
                    Money = p.Money,
                    Experience = p.Experience,
                    Inventory = p.Inventory,
                    CurrentFuelPlayerItem = p.CurrentFuelPlayerItem,
                    CurrentAttackPlayerItem = p.CurrentAttackPlayerItem,
                    CurrentDefensePlayerItem = p.CurrentDefensePlayerItem,
                }).FirstOrDefaultAsync(p => p.Id == id);

            return player;
        }

        public async Task<IList<PlayerResult>> Find()
        {
            var players = await _database.Players
                .Select(p => new PlayerResult
                {
                    Id = p.Id,
                    Name = p.Name,
                    Money = p.Money,
                    Experience = p.Experience,
                    Inventory = p.Inventory,
                    CurrentFuelPlayerItem = p.CurrentFuelPlayerItem,
                    CurrentAttackPlayerItem = p.CurrentAttackPlayerItem,
                    CurrentDefensePlayerItem = p.CurrentDefensePlayerItem
                }).ToListAsync();

            return players;
        }

        public async Task<PlayerResult> Create(PlayerRequest request)
        {
            var player = new Player()
            {
                Name = request.Name
            };

            _database.Players.Add(player);
            await _database.SaveChangesAsync();

            return await Get(player.Id);
        }

        public async Task<PlayerResult?> Update(int id, PlayerRequest playerRequest)
        {
            var player = await _database.Players.FirstOrDefaultAsync(p => p.Id == id);
            if (player is null)
            {
                return null;
            }

            player.Name = playerRequest.Name;
            player.CurrentAttackPlayerItemId = playerRequest.CurrentAttackPlayerItemId;
            player.CurrentDefensePlayerItemId = playerRequest.CurrentDefensePlayerItemId;
            player.CurrentFuelPlayerItemId = playerRequest.CurrentFuelPlayerItemId;
            player.Experience = playerRequest.Experience;
            player.Money = playerRequest.Money;
            player.Inventory = playerRequest.Inventory;
            player.LastActionExecutedDateTime = playerRequest.LastActionExecutedDateTime;

            await _database.SaveChangesAsync();
            return await Get(player.Id);
        }

        public async Task Delete(int id)
        {
            var player = await _database.Players.FirstOrDefaultAsync(p => p.Id == id);
            if (player is null)
            {
                return;
            }

            _database.Players.Remove(player);
            await _database.SaveChangesAsync();
        }
    }
}
