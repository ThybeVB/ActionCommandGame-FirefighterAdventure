using ActionCommandGame.Model;
using ActionCommandGame.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace ActionCommandGame.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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

        /*
        public Player Create(Player player)
        {
            throw new System.NotImplementedException();
        }
        public Player Update(int id, Player player)
        {
            throw new System.NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new System.NotImplementedException();
        }*/
    }
}
