using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActionCommandGame.Model;
using ActionCommandGame.Repository;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Helpers;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;
using Microsoft.EntityFrameworkCore;

namespace ActionCommandGame.Services
{
    public class PositiveGameEventService: IPositiveGameEventService
    {
        private readonly ActionButtonGameDbContext _database;

        public PositiveGameEventService(ActionButtonGameDbContext database)
        {
            _database = database;
        }

        public async Task<PositiveGameEventResult> Get(int id)
        {
            return await _database.PositiveGameEvents.Select(p => new PositiveGameEventResult
            {
                Id = p.Id,
                Description = p.Description,
                Experience = p.Experience,
                Money = p.Money,
                Name = p.Name,
                Probability = p.Probability,
            }).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<PositiveGameEvent> GetRandomPositiveGameEvent(bool hasAttackItem)
        {
            var query = _database.PositiveGameEvents.AsQueryable();

            //If we don't have an attack item, we can only get low-reward items.
            if (!hasAttackItem)
            {
                query = query.Where(p => p.Money < 50);
            }

            var gameEvents = await query.ToListAsync();

            return GameEventHelper.GetRandomPositiveGameEvent(gameEvents);
        }

        public async Task<IList<PositiveGameEventResult>> Find()
        {
            var events = await _database.PositiveGameEvents
                .Select(p => new PositiveGameEventResult
                {
                    Id = p.Id,
                    Description = p.Description,
                    Experience = p.Experience,
                    Money = p.Money,
                    Name = p.Name,
                    Probability = p.Probability,
                }).ToListAsync();

            return events;
        }

        public async Task<PositiveGameEventResult> Create(PositiveGameEventRequest gameEvent)
        {
            var events = new PositiveGameEvent()
            {
                Description = gameEvent.Description,
                Experience = gameEvent.Experience,
                Money = gameEvent.Money,
                Name = gameEvent.Name,
                Probability = gameEvent.Probability,
            };

            _database.PositiveGameEvents.Add(events);
            await _database.SaveChangesAsync();

            return await Get(events.Id);
        }

        public async Task<PositiveGameEventResult> Update(int id, PositiveGameEventRequest gameEvent)
        {
            var positiveGameEvent = await _database.PositiveGameEvents.FirstOrDefaultAsync(e => e.Id == id);
            if (positiveGameEvent is null)
            {
                return null;
            }

            positiveGameEvent.Description = gameEvent.Description;
            positiveGameEvent.Experience = gameEvent.Experience;
            positiveGameEvent.Money = gameEvent.Money;
            positiveGameEvent.Name = gameEvent.Name;
            positiveGameEvent.Probability = gameEvent.Probability;

            await _database.SaveChangesAsync();
            return await Get(positiveGameEvent.Id);
        }

        public async Task Delete(int id)
        {
            var positiveGameEvent = await _database.PositiveGameEvents.FirstOrDefaultAsync(e => e.Id == id);
            if (positiveGameEvent is null)
            {
                return;
            }

            _database.PositiveGameEvents.Remove(positiveGameEvent);
            await _database.SaveChangesAsync();
        }
    }
}
