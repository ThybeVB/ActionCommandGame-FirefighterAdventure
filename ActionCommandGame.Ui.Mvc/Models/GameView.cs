﻿using ActionCommandGame.Model;

namespace ActionCommandGame.Ui.Mvc.Models
{
    public class GameView
    {
        public required Player Player { get; set; }
        public IList<Item> Items { get; set; }
    }
}