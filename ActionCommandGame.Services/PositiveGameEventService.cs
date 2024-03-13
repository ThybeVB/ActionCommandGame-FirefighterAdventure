using System;
using System.Collections.Generic;
using System.Linq;
using ActionCommandGame.Model;
using ActionCommandGame.Repository;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Helpers;

namespace ActionCommandGame.Services
{
    public class PositiveGameEventService: IPositiveGameEventService
    {
        private readonly ActionButtonGameDbContext _database;

        public PositiveGameEventService(ActionButtonGameDbContext database)
        {
            _database = database;
        }

        public PositiveGameEvent Get(int id)
        {
            throw new NotImplementedException();
        }

        public PositiveGameEvent GetRandomPositiveGameEvent(bool hasAttackItem)
        {
            var query = _database.PositiveGameEvents.AsQueryable();

            //If we don't have an attack item, we can only get low-reward items.
            if (!hasAttackItem)
            {
                query = query.Where(p => p.Money < 50);
            }

            var gameEvents = query.ToList();

            return GameEventHelper.GetRandomPositiveGameEvent(gameEvents);
        }

        public IList<PositiveGameEvent> Find()
        {
            return _database.PositiveGameEvents.ToList();
        }

        public PositiveGameEvent Create(PositiveGameEvent gameEvent)
        {
            throw new NotImplementedException();
        }

        public PositiveGameEvent Update(int id, PositiveGameEvent gameEvent)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
