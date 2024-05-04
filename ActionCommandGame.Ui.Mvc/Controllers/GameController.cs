using ActionCommandGame.Sdk;
using ActionCommandGame.Ui.Mvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace ActionCommandGame.Ui.Mvc.Controllers
{
    public class GameController : Controller
    {
        private readonly int _playerId = 1;

        private readonly PlayerSdk _playerSdk;

        public GameController(PlayerSdk playerSdk)
        {
            _playerSdk = playerSdk;
        }

        public async Task<IActionResult> Index()
        {
	        var player = await _playerSdk.Get(_playerId);
	        var view = new GameView
	        {
		        Player = player
	        };


	        return View("Index", view);
        }
    }
}
