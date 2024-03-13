using System.Collections.Generic;
using ActionCommandGame.Model;

namespace ActionCommandGame.Services.Abstractions
{
    public interface IPositiveGameEventService
    {
        PositiveGameEvent Get(int id);
        PositiveGameEvent GetRandomPositiveGameEvent(bool hasAttackItem);
        IList<PositiveGameEvent> Find();
        PositiveGameEvent Create(PositiveGameEvent gameEvent);
        PositiveGameEvent Update(int id, PositiveGameEvent gameEvent);
        bool Delete(int id);
    }
}
