using System.Collections.Generic;
using System.Threading.Tasks;
using ActionCommandGame.Model;

namespace ActionCommandGame.Services.Abstractions
{
    public interface IPlayerService
    {
        Task<Player> Get(int id);
        Task<IList<Player>> Find();
        Player Create(Player player);
        Player Update(int id, Player player);
        bool Delete(int id);
    }
}
