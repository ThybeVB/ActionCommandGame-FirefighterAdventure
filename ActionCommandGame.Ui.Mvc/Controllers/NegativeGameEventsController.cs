using ActionCommandGame.Model;
using ActionCommandGame.Sdk;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActionCommandGame.Ui.Mvc.Controllers
{
    [Authorize(Roles = "Admin")]
    public class NegativeGameEventsController : Controller
    {
        private readonly NegativeGameEventSdk _negativeGameEventSdk;

        public NegativeGameEventsController(NegativeGameEventSdk negativeGameEventSdk)
        {
            _negativeGameEventSdk = negativeGameEventSdk;
        }


        public async Task<IActionResult> Index()
        {
            var negativeGameEvents = await _negativeGameEventSdk.Find();
            return View(negativeGameEvents);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NegativeGameEvent model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _negativeGameEventSdk.Create(model);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var negativeGameEvent = await _negativeGameEventSdk.Get(id);
            return View("Details", negativeGameEvent);
        }

        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            var negativeGameEvent = await _negativeGameEventSdk.Get(id);
            return View(negativeGameEvent);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, NegativeGameEvent negativeGameEvent)
        {
            if (!ModelState.IsValid)
            {
                return View(negativeGameEvent);
            }

            await _negativeGameEventSdk.Update(id, negativeGameEvent);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var negativeGameEvent = await _negativeGameEventSdk.Get(id);
            return View(negativeGameEvent);
        }

        [HttpPost("/negativeEvents/delete/{id:int?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _negativeGameEventSdk.Delete(id);
            return RedirectToAction("Index");
        }

    }
}
