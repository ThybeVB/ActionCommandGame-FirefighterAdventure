using System;
using System.Collections.Generic;
using System.Linq;
using ActionCommandGame.Model;
using ActionCommandGame.Repository;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Helpers;

namespace ActionCommandGame.Services
{
    public class NegativeGameEventService: INegativeGameEventService
    {
        private readonly ActionButtonGameDbContext _database;

        public NegativeGameEventService(ActionButtonGameDbContext database)
        {
            _database = database;
        }

        public NegativeGameEvent Get(int id)
        {
            throw new NotImplementedException();
        }

        public NegativeGameEvent GetRandomNegativeGameEvent()
        {
            var gameEvents = Find();
            return GameEventHelper.GetRandomNegativeGameEvent(gameEvents);
        }

        public IList<NegativeGameEvent> Find()
        {
            return _database.NegativeGameEvents.ToList();
        }

        public NegativeGameEvent Create(NegativeGameEvent gameEvent)
        {
            throw new NotImplementedException();
        }

        public NegativeGameEvent Update(int id, NegativeGameEvent gameEvent)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
