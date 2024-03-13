using System.Collections.Generic;
using ActionCommandGame.Model;
using ActionCommandGame.Services.Model.Core;

namespace ActionCommandGame.Services.Abstractions
{
    public interface IPlayerItemService
    {
        PlayerItem Get(int id);
        IList<PlayerItem> Find(int? playerId = null);
        ServiceResult<PlayerItem> Create(int playerId, int itemId);
        PlayerItem Update(int id, PlayerItem playerItem);
        ServiceResult Delete(int id);
    }
}
