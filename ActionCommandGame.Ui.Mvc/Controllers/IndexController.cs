using ActionCommandGame.Ui.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ActionCommandGame.Ui.Mvc.Controllers
{
    /// <summary>
    /// Dit zou de account login pagina moeten worden, die nu nog niet bestaat. Er wordt geredirect naar de game met playerid 1
    /// </summary>
    public class IndexController : Controller
    {
        private readonly ILogger<Index> _logger;

        public IndexController(ILogger<Index> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Index", "Game");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
