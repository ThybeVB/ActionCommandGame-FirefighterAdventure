using ActionCommandGame.Model;

namespace ActionCommandGame.Services.Model.Requests
{
    public class PlayerItemRequest //todo net zoals alle andere requests diewss ni werken
    {
        public int PlayerId { get; set; }
        public Player? Player { get; set; }

        public int ItemId { get; set; }
        public Item? Item { get; set; }
    }
}
