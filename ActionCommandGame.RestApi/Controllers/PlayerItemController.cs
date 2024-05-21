using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Model.Requests;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActionCommandGame.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PlayerItemController : ControllerBase
    {
        private readonly IPlayerItemService _playerItemService;

        public PlayerItemController(IPlayerItemService playerItemService)
        {
            _playerItemService = playerItemService;
        }

        /// <summary>
        /// Finds a playeritem
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _playerItemService.Get(id);
            return Ok(result);
        }

        /// <summary>
        /// Return all items for player
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        [HttpGet("find/{playerId}")]
        public async Task<IActionResult> Find(string playerId)
        {
            var result = await _playerItemService.Find(playerId);
            return Ok(result);
        }

        /// <summary>
        /// Creates a new PlayerItem object
        /// </summary>
        /// <param name="playerItemRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(PlayerItemRequest playerItemRequest)
        {
            var result = await _playerItemService.Create(playerItemRequest.PlayerId, playerItemRequest.ItemId);
            return Ok(result);
        }


        /// <summary>
        /// Updates a playeritem
        /// </summary>
        /// <param name="id"></param>
        /// <param name="playerItemRequest"></param>
        /// <returns></returns>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, PlayerItemRequest playerItemRequest)
        {
            var result = await _playerItemService.Update(id, playerItemRequest);
            return Ok(result);
        }

        /// <summary>
        /// Deletes a playeritem
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _playerItemService.Delete(id);
            return Ok();
        }

    }
}
