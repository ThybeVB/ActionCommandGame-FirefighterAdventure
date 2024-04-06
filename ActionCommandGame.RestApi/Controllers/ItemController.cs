using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Model.Requests;
using Microsoft.AspNetCore.Mvc;

namespace ActionCommandGame.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }

        /// <summary>
        /// Finds a player
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _itemService.Get(id);
            return Ok(result);
        }

        /// <summary>
        /// Returns all items
        /// </summary>
        /// <returns>All items</returns>
        [HttpGet]
        public async Task<IActionResult> Find()
        {
            var result = await _itemService.Find();
            return Ok(result);
        }

        /// <summary>
        /// Creates an item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(ItemRequest item)
        {
            var result = await _itemService.Create(item);
            return Ok(result);
        }


        /// <summary>
        /// Updates an item
        /// </summary>
        /// <param name="id"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, ItemRequest item)
        {
            var result = await _itemService.Update(id, item);
            return Ok(result);
        }

        /// <summary>
        /// Deletes a item
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _itemService.Delete(id);
            return Ok();
        }
    }
}
