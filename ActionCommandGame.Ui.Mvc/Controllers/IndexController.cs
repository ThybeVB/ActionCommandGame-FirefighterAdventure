using ActionCommandGame.Ui.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ActionCommandGame.Sdk;
using ActionCommandGame.Ui.Mvc.Stores;
using Microsoft.AspNetCore.Authentication;

namespace ActionCommandGame.Ui.Mvc.Controllers
{
    /// <summary>
    /// Dit zou de account login pagina moeten worden, die nu nog niet bestaat. Er wordt geredirect naar de game met playerid 1
    /// </summary>
    public class IndexController : Controller
    {
        private readonly ILogger<Index> _logger;
        private IdentitySdk _identitySdk;
        private TokenStore _tokenStore;

        public IndexController(ILogger<Index> logger, IdentitySdk identitySdk, TokenStore tokenStore)
        {
            _logger = logger;
            _identitySdk = identitySdk;
            _tokenStore = tokenStore;
        }

        public IActionResult Index()
        {
            return View("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Login(string? returnUrl)
        {
            await HttpContext.SignOutAsync();
            ViewBag.ReturnUrl = returnUrl ?? "/";
            return View();
        }

        [HttpGet]
        public IActionResult Register(string? returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl ?? "/";
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
