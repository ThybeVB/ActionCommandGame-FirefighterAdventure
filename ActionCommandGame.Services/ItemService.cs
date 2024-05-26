using ActionCommandGame.Model;
using ActionCommandGame.Repository;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActionCommandGame.Services
{
    public class ItemService : IItemService
    {
        private readonly ActionButtonGameDbContext _database;

        public ItemService(ActionButtonGameDbContext database)
        {
            _database = database;
        }

        public async Task<ItemResult> Get(int id)
        {
            return await _database.Items.
                Select(p => new ItemResult
                {
                    Id = p.Id,
                    Name = p.Name,
                    ActionCooldownSeconds = p.ActionCooldownSeconds,
                    Attack = p.Attack,
                    Defense = p.Defense,
                    Description = p.Description,
                    Fuel = p.Fuel,
                    Price = p.Price
                }).
                SingleOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IList<ItemResult>> Find()
        {
            return await _database.Items
                .Select(p => new ItemResult
                {
                    Id = p.Id,
                    Name = p.Name,
                    ActionCooldownSeconds = p.ActionCooldownSeconds,
                    Attack = p.Attack,
                    Defense = p.Defense,
                    Description = p.Description,
                    Fuel = p.Fuel,
                    Price = p.Price
                }).ToListAsync();
        }

        public async Task<ItemResult> Create(ItemRequest request)
        {
            var item = new Item()
            {
                Name = request.Name,
                ActionCooldownSeconds = request.ActionCooldownSeconds,
                Attack = request.Attack,
                Defense = request.Defense,
                Description = request.Description,
                Fuel = request.Fuel,
                //PlayerItems = p.PlayerItems,
                Price = request.Price
            };

            _database.Items.Add(item);
            await _database.SaveChangesAsync();

            return await Get(item.Id);
        }

        public async Task<ItemResult> Update(int id, ItemRequest request)
        {
            var item = await _database.Items.FirstOrDefaultAsync(p => p.Id == id);
            if (item is null)
            {
                return null;
            }

            item.Name = request.Name;
            item.Description = request.Description;
            item.Attack = request.Attack;
            item.Defense = request.Defense;
            item.ActionCooldownSeconds = request.ActionCooldownSeconds;
            item.Price = request.Price;
            item.Fuel = request.Fuel;

            await _database.SaveChangesAsync();
            return await Get(item.Id);
        }

        public async Task Delete(int id)
        {
            var item = await _database.Items.FirstOrDefaultAsync(p => p.Id == id);
            if (item is null)
            {
                return;
            }

            _database.Items.Remove(item);
            await _database.SaveChangesAsync();
        }
    }
}
