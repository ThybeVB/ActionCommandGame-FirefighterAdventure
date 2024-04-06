using ActionCommandGame.Model;

namespace ActionCommandGame.Services.Model.Results
{
    public class BuyResult
    {
        public PlayerResult Player { get; set; }
        public Item Item { get; set; }
    }
}
