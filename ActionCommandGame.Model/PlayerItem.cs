﻿using ActionCommandGame.Model.Abstractions;
using System.Collections.Generic;

namespace ActionCommandGame.Model
{
    public class PlayerItem : IIdentifiable
    {
        public PlayerItem()
        {
            FuelPlayers = new List<Player>();
            AttackPlayers = new List<Player>();
            DefensePlayers = new List<Player>();
        }

        public int Id { get; set; }

        public string PlayerId { get; set; }
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
