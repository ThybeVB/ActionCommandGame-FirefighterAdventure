using ActionCommandGame.Sdk;
using ActionCommandGame.Services.Model.Requests.Identity;
using ActionCommandGame.Ui.Mvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActionCommandGame.Ui.Mvc.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly IdentitySdk _identitySdk;
        private readonly PlayerSdk _playerSdk;

        public SettingsController(IdentitySdk identitySdk, PlayerSdk playerSdk)
        {
            _identitySdk = identitySdk;
            _playerSdk = playerSdk;
        }


        public async Task<IActionResult> Index()
        {
            var uId = User.Claims.FirstOrDefault(c => c.Type == "Id");
            var user = await _playerSdk.Get(uId.Value);
            var current = new RegisterModel()
            {
                Username = user.UserName,
                DisplayName = user.Name,
                Password = "",
                ConfirmPassword = ""
            };

            return View(current);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(RegisterModel model, string? returnUrl)
        {
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = "/Game/";
            }

            if (!ModelState.IsValid)
            {
                ViewBag.ReturnUrl = returnUrl;
                return View(model);
            }

            var request = new UserRegisterRequest
            {
                Username = model.Username,
                DisplayName = model.DisplayName,
                Password = model.Password
            };

            var result = await _identitySdk.Update(request);
            if (result.Messages.Count > 0) //todo: rare workaround
            {
                foreach (var error in result.Messages)
                {
                    ModelState.AddModelError("", error.Message);
                }
                ViewBag.ReturnUrl = returnUrl;
                return View(model);
            }

            return LocalRedirect(returnUrl);
        }
    }
}
