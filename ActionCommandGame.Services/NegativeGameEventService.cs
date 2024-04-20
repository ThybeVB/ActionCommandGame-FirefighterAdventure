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
    public class NegativeGameEventService : INegativeGameEventService
    {
        private readonly ActionButtonGameDbContext _database;

        public NegativeGameEventService(ActionButtonGameDbContext database)
        {
            _database = database;
        }

        public async Task<NegativeGameEventResult> Get(int id)
        {
            return await _database.NegativeGameEvents.Select(p => new NegativeGameEventResult
            {
                Id = p.Id,
                Description = p.Description,
                DefenseLoss = p.DefenseLoss,
                DefenseWithGearDescription = p.DefenseWithGearDescription,
                DefenseWithoutGearDescription = p.DefenseWithoutGearDescription,
                Name = p.Name,
                Probability = p.Probability
            }).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<NegativeGameEventResult> GetRandomNegativeGameEvent()
        {
            var gameEvents = await Find();
            return GameEventHelper.GetRandomNegativeGameEvent(gameEvents);
        }

        public async Task<IList<NegativeGameEventResult>> Find()
        {
            var events = await _database.NegativeGameEvents
                .Select(p => new NegativeGameEventResult
                {
                    Id = p.Id,
                    Description = p.Description,
                    DefenseLoss = p.DefenseLoss,
                    DefenseWithGearDescription = p.DefenseWithGearDescription,
                    DefenseWithoutGearDescription = p.DefenseWithoutGearDescription,
                    Name = p.Name,
                    Probability = p.Probability
                }).ToListAsync();

            return events;
        }

        public async Task<NegativeGameEventResult> Create(NegativeGameEventRequest gameEvent)
        {
            var events = new NegativeGameEvent()
            {
                Description = gameEvent.Description,
                DefenseLoss = gameEvent.DefenseLoss,
                DefenseWithGearDescription = gameEvent.DefenseWithGearDescription,
                DefenseWithoutGearDescription = gameEvent.DefenseWithoutGearDescription,
                Name = gameEvent.Name,
                Probability = gameEvent.Probability
            };

            _database.NegativeGameEvents.Add(events);
            await _database.SaveChangesAsync();

            return await Get(events.Id);
        }

        public async Task<NegativeGameEventResult> Update(int id, NegativeGameEventRequest gameEvent)
        {
            var negativeGameEvent = await _database.NegativeGameEvents.FirstOrDefaultAsync(e => e.Id == id);
            if (negativeGameEvent is null)
            {
                return null;
            }

            negativeGameEvent.Description = gameEvent.Description;
            negativeGameEvent.DefenseLoss = gameEvent.DefenseLoss;
            negativeGameEvent.DefenseWithGearDescription = gameEvent.DefenseWithGearDescription;
            negativeGameEvent.DefenseWithoutGearDescription = gameEvent.DefenseWithoutGearDescription;
            negativeGameEvent.Name = gameEvent.Name;
            negativeGameEvent.Probability = gameEvent.Probability;

            await _database.SaveChangesAsync();
            return await Get(negativeGameEvent.Id);
        }

        public async Task Delete(int id)
        {
            var negativeGameEvent = await _database.NegativeGameEvents.FirstOrDefaultAsync(e => e.Id == id);
            if (negativeGameEvent is null)
            {
                return;
            }

            _database.NegativeGameEvents.Remove(negativeGameEvent);
            await _database.SaveChangesAsync();
        }
    }
}
