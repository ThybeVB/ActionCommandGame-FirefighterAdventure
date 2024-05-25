using ActionCommandGame.Model;
using ActionCommandGame.Sdk;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActionCommandGame.Ui.Mvc.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ItemController : Controller
    {
        private readonly ItemSdk _itemSdk;

        public ItemController(ItemSdk itemSdk)
        {
            _itemSdk = itemSdk;
        }


        public async Task<IActionResult> Index()
        {
            var items = await _itemSdk.Find();
            return View(items);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int itemId)
        {
            var item = await _itemSdk.Get(itemId);
            return PartialView("_DetailsPartial", item);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Item model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _itemSdk.Create(model);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            var item = await _itemSdk.Get(id);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, Item item)
        {
            if (!ModelState.IsValid)
            {
                return View(item);
            }

            await _itemSdk.Update(id, item);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var item = await _itemSdk.Get(id);
            return View(item);
        }

        [HttpPost("/items/delete/{id:int?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _itemSdk.Delete(id);
            return RedirectToAction("Index");
        }

    }
}
