using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ActionCommandGame.Services.Abstractions
{
    public interface INegativeGameEventService
    {
        Task<NegativeGameEventResult> Get(int id);
        Task<NegativeGameEventResult> GetRandomNegativeGameEvent();
        Task<IList<NegativeGameEventResult>> Find();
        Task<NegativeGameEventResult> Create(NegativeGameEventRequest gameEvent);
        Task<NegativeGameEventResult> Update(int id, NegativeGameEventRequest gameEvent);
        Task Delete(int id);
    }
}
