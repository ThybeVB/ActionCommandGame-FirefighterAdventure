using ActionCommandGame.Model;
using ActionCommandGame.Services.Abstractions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActionCommandGame.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerService _playerService;

        public PlayerController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        /// <summary>
        /// Finds a player
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _playerService.Get(id);
            return Ok(result);
        }

        /// <summary>
        /// Returns all players
        /// </summary>
        /// <returns>All players</returns>
        [HttpGet]
        public async Task<IActionResult> Find()
        {
            var result = await _playerService.Find();
            return Ok(result);
        }

        /// <summary>
        /// Creates a player
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(Player player)
        {
            var result = await _playerService.Create(player);
            return Ok(result);
        }


        /// <summary>
        /// Updates a player
        /// </summary>
        /// <param name="id"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, Player player)
        {
            var result = await _playerService.Update(id, player);
            return Ok(result);
        }

        /// <summary>
        /// Deletes a player
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _playerService.Delete(id);
            return Ok();
        }
    }
}
