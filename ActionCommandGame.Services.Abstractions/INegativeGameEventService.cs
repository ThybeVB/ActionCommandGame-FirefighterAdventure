using System.Collections.Generic;
using ActionCommandGame.Model;

namespace ActionCommandGame.Services.Abstractions
{
    public interface INegativeGameEventService
    {
        NegativeGameEvent Get(int id);
        NegativeGameEvent GetRandomNegativeGameEvent();
        IList<NegativeGameEvent> Find();
        NegativeGameEvent Create(NegativeGameEvent gameEvent);
        NegativeGameEvent Update(int id, NegativeGameEvent gameEvent);
        bool Delete(int id);
    }
}
