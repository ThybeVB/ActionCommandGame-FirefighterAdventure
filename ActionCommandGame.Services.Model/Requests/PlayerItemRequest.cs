using ActionCommandGame.Model;

namespace ActionCommandGame.Services.Model.Requests
{
    public class PlayerItemRequest 
    {
        public int PlayerId { get; set; }
        //public Player? Player { get; set; }

        public int ItemId { get; set; }
        //public Item? Item { get; set; }
    }
}
