using System.Collections.Generic;
using System.Threading.Tasks;
using ActionCommandGame.Model;
using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Results;

namespace ActionCommandGame.Services.Abstractions
{
    public interface IPlayerItemService
    {
        Task<PlayerItemResult> Get(int id);
        Task<IList<PlayerItem>> Find(int? playerId = null);
        Task<ServiceResult<PlayerItem>> Create(int playerId, int itemId);
        Task<PlayerItemResult> Update(int id, PlayerItem playerItem);
        Task<ServiceResult> Delete(int id);
    }
}
