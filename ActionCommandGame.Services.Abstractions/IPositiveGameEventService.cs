using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ActionCommandGame.Services.Abstractions
{
    public interface IPositiveGameEventService
    {
        Task<PositiveGameEventResult> Get(int id);
        Task<PositiveGameEventResult> GetRandomPositiveGameEvent(bool hasAttackItem);
        Task<IList<PositiveGameEventResult>> Find();
        Task<PositiveGameEventResult> Create(PositiveGameEventRequest request);
        Task<PositiveGameEventResult> Update(int id, PositiveGameEventRequest request);
        Task Delete(int id);
    }
}
