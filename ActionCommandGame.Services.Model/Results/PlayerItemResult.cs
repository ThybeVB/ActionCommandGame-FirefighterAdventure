using ActionCommandGame.Model;
using ActionCommandGame.Model.Abstractions;

namespace ActionCommandGame.Services.Model.Results
{
    public class PlayerItemResult : IIdentifiable
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public Player? Player { get; set; }

        public int ItemId { get; set; }
        public Item? Item { get; set; }

        public int RemainingAttack { get; set; }
        public int RemainingDefense { get; set; }
        public int RemainingFuel {get; set; }
    }
}
