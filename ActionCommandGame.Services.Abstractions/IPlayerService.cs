using ActionCommandGame.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ActionCommandGame.Services.Abstractions
{
    public interface IPlayerService
    {
        Task<Player> Get(string id);
        Task<IList<Player>> Find();
        Task<Player?> Create(Player player);
        Task<Player?> Update(string id, Player player);
        Task Delete(string id);
    }
}
