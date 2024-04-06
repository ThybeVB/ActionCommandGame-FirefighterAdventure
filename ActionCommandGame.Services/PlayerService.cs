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

        public PlayerService(ActionButtonGameDbContext database)
        {
            _database = database;
        }

        public async Task<PlayerResult> Get(int id)
        {
            return await _database.Players
                .Select(p => new PlayerResult
                {
                    CurrentFuelPlayerItem = p.CurrentFuelPlayerItem,
                    CurrentAttackPlayerItem = p.CurrentAttackPlayerItem,
                    CurrentDefensePlayerItem = p.CurrentDefensePlayerItem,
                }).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IList<PlayerResult>> Find()
        {
            return await _database.Players
                .Select(p => new PlayerResult
                {
                    CurrentFuelPlayerItem = p.CurrentFuelPlayerItem,
                    CurrentAttackPlayerItem = p.CurrentAttackPlayerItem,
                    CurrentDefensePlayerItem = p.CurrentDefensePlayerItem
                }).ToListAsync();
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
            //eventueel meer dingen TODO

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
