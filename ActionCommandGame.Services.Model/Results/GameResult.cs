using ActionCommandGame.Model;
using ActionCommandGame.Services.Model.Core;
using System.Collections.Generic;

namespace ActionCommandGame.Services.Model.Results
{
    public class GameResult
    {
        public Player Player { get; set; }
        public PositiveGameEventResult PositiveGameEvent { get; set; }
        public NegativeGameEventResult NegativeGameEvent { get; set; }
        public IList<ServiceMessage> NegativeGameEventMessages { get; set; }
    }
}
