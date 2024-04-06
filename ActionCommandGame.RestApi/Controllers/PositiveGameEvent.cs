using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Model.Requests;
using Microsoft.AspNetCore.Mvc;

namespace ActionCommandGame.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PositiveGameEvent : ControllerBase
    {
        private readonly IPositiveGameEventService _positiveGameEventService;

        public PositiveGameEvent(IPositiveGameEventService positiveGameEventService)
        {
            _positiveGameEventService = positiveGameEventService;
        }

        /// <summary>
        /// Finds an event
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _positiveGameEventService.Get(id);
            return Ok(result);
        }

        /// <summary>
        /// Returns all events
        /// </summary>
        /// <returns>All events</returns>
        [HttpGet]
        public async Task<IActionResult> Find()
        {
            var result = await _positiveGameEventService.Find();
            return Ok(result);
        }

        /// <summary>
        /// Creates an event
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(PositiveGameEventRequest request)
        {
            var result = await _positiveGameEventService.Create(request);
            return Ok(result);
        }


        /// <summary>
        /// Updates an event
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, PositiveGameEventRequest request)
        {
            var result = await _positiveGameEventService.Update(id, request);
            return Ok(result);
        }

        /// <summary>
        /// Deletes an event
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _positiveGameEventService.Delete(id);
            return Ok();
        }
    }
}
