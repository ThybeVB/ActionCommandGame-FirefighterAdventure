using System.Collections.Generic;
using ActionCommandGame.Model;

namespace ActionCommandGame.Services.Abstractions
{
    public interface IPlayerService
    {
        Player Get(int id);
        IList<Player> Find();
        Player Create(Player player);
        Player Update(int id, Player player);
        bool Delete(int id);
    }
}
