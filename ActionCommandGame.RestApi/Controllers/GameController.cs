using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Model.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActionCommandGame.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }

        /// <summary>
        /// Performs a game routine for specified player
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        [HttpGet("PerformAction/{playerId}")]
        public async Task<IActionResult> PerformAction(string playerId)
        {
            var result = await _gameService.PerformAction(playerId);
            return Ok(result);
        }

        /// <summary>
        /// Buys an item 
        /// </summary>
        /// <param name="buyRequest"></param>
        /// <returns></returns>
        [HttpPost("Buy/{buyRequest}")]
        public async Task<IActionResult> Buy(BuyRequest buyRequest)
        {
            var result = await _gameService.Buy(buyRequest.PlayerId, buyRequest.ItemId);
            return Ok(result);
        }
    }
}