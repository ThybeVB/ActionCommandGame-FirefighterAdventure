using ActionCommandGame.Sdk;
using ActionCommandGame.Ui.Mvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace ActionCommandGame.Ui.Mvc.Controllers
{
    public class GameController : Controller
    {
        private readonly int _playerId = 1;

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
	        var player = await _playerSdk.Get(_playerId);
            var items = await _itemSdk.Find();

            _view = new GameView
	        {
		        Player = player,
                Items = items
	        };


	        return View("Index", _view);
        }

        public async Task<IActionResult> ShowShop()
        {
            return View("Index", _view);
        }
    }
}
