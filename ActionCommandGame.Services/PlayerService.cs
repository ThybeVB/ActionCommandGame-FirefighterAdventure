﻿using ActionCommandGame.Model;
using ActionCommandGame.Repository;
using ActionCommandGame.Services.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActionCommandGame.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly ActionButtonGameDbContext _database;

        public PlayerService(ActionButtonGameDbContext database)
        {
            _database = database;
        }

        public async Task<Player> Get(string id)
        {
            var player = await _database.Players
                .Include(p => p.CurrentFuelPlayerItem.Item)
                .Include(p => p.CurrentAttackPlayerItem.Item)
                .Include(p => p.CurrentDefensePlayerItem.Item)
                .SingleOrDefaultAsync(p => p.Id == id);

            return player;
        }

        public async Task<IList<Player>> Find()
        {
            var players = await _database.Players
                .Include(p => p.CurrentAttackPlayerItem.Item)
                .Include(p => p.CurrentDefensePlayerItem.Item)
                .Include(p => p.CurrentFuelPlayerItem.Item)
                .Select(p => new Player
                {
                    Id = p.Id,
                    Name = p.Name,
                    Money = p.Money,
                    Experience = p.Experience,
                    Inventory = p.Inventory,
                    LastActionExecutedDateTime = p.LastActionExecutedDateTime,
                    CurrentFuelPlayerItem = p.CurrentFuelPlayerItem,
                    CurrentAttackPlayerItem = p.CurrentAttackPlayerItem,
                    CurrentDefensePlayerItem = p.CurrentDefensePlayerItem,
                    CurrentAttackPlayerItemId = p.CurrentAttackPlayerItemId,
                    CurrentDefensePlayerItemId = p.CurrentDefensePlayerItemId,
                    CurrentFuelPlayerItemId = p.CurrentFuelPlayerItemId
                }).ToListAsync();

            return players;
        }

        public async Task<Player> Create(Player request)
        {
            var player = new Player()
            {
                Name = request.Name,
                Money = request.Money,
                Experience = request.Experience,
                Inventory = request.Inventory,
                LastActionExecutedDateTime = request.LastActionExecutedDateTime,
                CurrentAttackPlayerItemId = request.CurrentAttackPlayerItemId,
                CurrentDefensePlayerItemId = request.CurrentDefensePlayerItemId,
                CurrentFuelPlayerItemId = request.CurrentFuelPlayerItemId
            };

            _database.Players.Add(player);
            await _database.SaveChangesAsync();

            return await Get(player.Id);
        }

        public async Task<Player> Update(string id, Player playerRequest)
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

        public async Task Delete(string id)
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
