using ActionCommandGame.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActionCommandGame.Services.Model.Requests
{
    public class PlayerItemRequest //todo net zoals alle andere requests diewss ni werken
    {
        public Player Player { get; set; }

        public int ItemId { get; set; }
        public Item Item { get; set; }

        public int RemainingFuel { get; set; }
        public int RemainingAttack { get; set; }
        public int RemainingDefense { get; set; }

        public IList<Player> FuelPlayers { get; set; }
        public IList<Player> AttackPlayers { get; set; }
        public IList<Player> DefensePlayers { get; set; }
    }
}
