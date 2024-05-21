using ActionCommandGame.Sdk;
using ActionCommandGame.Ui.Mvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace ActionCommandGame.Ui.Mvc.Controllers
{
    public class GameController : Controller
    {
        private GameView? _view;

        private readonly PlayerSdk _playerSdk;
        private readonly ItemSdk _itemSdk;


        public GameController(PlayerSdk playerSdk, ItemSdk itemSdk)
        {
            _playerSdk = playerSdk;
            _itemSdk = itemSdk;
        }

        public async Task<IActionResult> Index()
        {
            //_playerId = int.Parse(User.FindFirstValue("Id"));
            //Console.WriteLine(User.FindFirstValue("Id"));
	        //var player = await _playerSdk.Get(_playerId);
            //var items = await _itemSdk.Find();
            //
            //_view = new GameView
	        //{
		    //    Player = player,
            //    Items = items
	        //};


	        return View();
        }

        public async Task<IActionResult> PerformAction()
        {
            return View();
        }

        public async Task<IActionResult> ShowShop()
        {
            var allItems = await _itemSdk.Find();
            return PartialView("_ShopPartial", allItems);
        }

        public async Task<IActionResult> ShowStats()
        {
            var uId = User.Claims.FirstOrDefault(c => c.Type == "Id");
            var result = await _playerSdk.Get(uId.Value);
            return PartialView("_StatsPartial", result);
        }
    }
}
