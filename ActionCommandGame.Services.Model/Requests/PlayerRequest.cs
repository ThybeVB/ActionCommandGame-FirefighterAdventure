namespace ActionCommandGame.Services.Model.Requests
{
    public class PlayerRequest
    {
        public string Name { get; set; }
        public int? CurrentAttackPlayerItemId { get; set; }
        public int? CurrentDefensePlayerItemId { get; set; }
        public int? CurrentFuelPlayerItemId { get; set; }

        public int Money { get; set; }
        public int Experience { get; set; }
    }
}
