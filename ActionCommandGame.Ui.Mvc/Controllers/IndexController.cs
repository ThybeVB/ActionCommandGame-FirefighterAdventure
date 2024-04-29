using ActionCommandGame.Ui.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ActionCommandGame.Ui.Mvc.Controllers
{
    public class IndexController : Controller
    {
        private readonly ILogger<Index> _logger;

        public IndexController(ILogger<Index> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
