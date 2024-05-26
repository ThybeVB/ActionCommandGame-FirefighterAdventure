using ActionCommandGame.Sdk;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActionCommandGame.Ui.Mvc.Controllers
{
    [Authorize]
    public class GameController : Controller
    {
        private readonly PlayerSdk _playerSdk;
        private readonly ItemSdk _itemSdk;
        private readonly PlayerItemSdk _playerItemSdk;
        private readonly GameSdk _gameSdk;


        public GameController(PlayerSdk playerSdk, ItemSdk itemSdk, PlayerItemSdk playerItemSdk, GameSdk gameSdk)
        {
            _playerSdk = playerSdk;
            _itemSdk = itemSdk;
            _playerItemSdk = playerItemSdk;
            _gameSdk = gameSdk;
        }

        public async Task<IActionResult> Index()
        {
            var uId = User.Claims.FirstOrDefault(c => c.Type == "Id");
            var user = await _playerSdk.Get(uId.Value);
            ViewData["PlayerName"] = user.Name;
            ViewData["PlayerId"] = uId.Value;

            return View();
        }

        public async Task<IActionResult> PerformAction()
        {
            var uId = User.Claims.FirstOrDefault(c => c.Type == "Id");
            var result = await _gameSdk.PerformAction(uId.Value);
            return PartialView("_ActionPartial", result);
        }

        public async Task<IActionResult> Shop()
        {
            var uId = User.Claims.FirstOrDefault(c => c.Type == "Id");
            var user = await _playerSdk.Get(uId.Value);

            var allItems = await _itemSdk.Find();
            ViewData["Money"] = user.Money;
            return PartialView("_ShopPartial", allItems);
        }

        public async Task<IActionResult> BuyItem(int itemId)
        {
            var uId = User.Claims.FirstOrDefault(c => c.Type == "Id");
            var result = await _gameSdk.Buy(uId.Value, itemId);
            return PartialView("_BuyPartial", result);
        }

        public async Task<IActionResult> Inventory()
        {
            var uId = User.Claims.FirstOrDefault(c => c.Type == "Id");
            var ownedItems = await _playerItemSdk.Find(uId.Value);

            return PartialView("_InventoryPartial", ownedItems);

        }

        public async Task<IActionResult> Stats()
        {
            var uId = User.Claims.FirstOrDefault(c => c.Type == "Id");
            var result = await _playerSdk.Get(uId.Value);

            return PartialView("_StatsPartial", result);
        }

        public async Task<IActionResult> Leaderboard()
        {
            var players = (await _playerSdk.Find()).OrderByDescending(p => p.Experience).ToList();
            return PartialView("_LeaderboardPartial", players);
        }

    }
}
