using ActionCommandGame.Sdk;
using Microsoft.AspNetCore.Mvc;

namespace ActionCommandGame.Ui.Mvc.Controllers
{
    public class SettingsController : Controller
    {
        private PlayerSdk _playerSdk;

        public SettingsController(PlayerSdk playerSdk)
        {
            _playerSdk = playerSdk;
        }

        
        public async Task<IActionResult> Index()
        {
            var uId = User.Claims.FirstOrDefault(c => c.Type == "Id");
            var player = await _playerSdk.Get(uId.Value);
            return View(player);
        }
    }
}
