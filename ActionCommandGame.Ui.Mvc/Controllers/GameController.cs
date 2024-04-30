using Microsoft.AspNetCore.Mvc;

namespace ActionCommandGame.Ui.Mvc.Controllers
{
    public class GameController : Controller
    {
        public IActionResult Index()
        {
            return View("Index");
        }
    }
}
