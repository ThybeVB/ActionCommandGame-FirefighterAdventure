using ActionCommandGame.Model;
using ActionCommandGame.Sdk;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActionCommandGame.Ui.Mvc.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PositiveGameEventsController : Controller
    {
        private readonly PositiveGameEventSdk _positiveGameEventSdk;

        public PositiveGameEventsController(PositiveGameEventSdk positiveGameEventSdk)
        {
            _positiveGameEventSdk = positiveGameEventSdk;
        }


        public async Task<IActionResult> Index()
        {
            var positiveGameEvents = await _positiveGameEventSdk.Find();
            return View(positiveGameEvents);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PositiveGameEvent model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _positiveGameEventSdk.Create(model);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            var positiveGameEvent = await _positiveGameEventSdk.Get(id);
            return View(positiveGameEvent);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, PositiveGameEvent positiveGameEvent)
        {
            if (!ModelState.IsValid)
            {
                return View(positiveGameEvent);
            }

            await _positiveGameEventSdk.Update(id, positiveGameEvent);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var positiveGameEvent = await _positiveGameEventSdk.Get(id);
            return View(positiveGameEvent);
        }

        [HttpPost("/positiveEvents/delete/{id:int?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _positiveGameEventSdk.Delete(id);
            return RedirectToAction("Index");
        }

    }
}
