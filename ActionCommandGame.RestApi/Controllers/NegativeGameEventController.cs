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
    public class NegativeGameEventController : ControllerBase
    {
        private readonly INegativeGameEventService _negativeGameEventService;

        public NegativeGameEventController(INegativeGameEventService negativeGameEventService)
        {
            _negativeGameEventService = negativeGameEventService;
        }

        /// <summary>
        /// Finds an event
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _negativeGameEventService.Get(id);
            return Ok(result);
        }

        /// <summary>
        /// Returns all events
        /// </summary>
        /// <returns>All events</returns>
        [HttpGet]
        public async Task<IActionResult> Find()
        {
            var result = await _negativeGameEventService.Find();
            return Ok(result);
        }

        /// <summary>
        /// Creates an event
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(NegativeGameEventRequest request)
        {
            var result = await _negativeGameEventService.Create(request);
            return Ok(result);
        }


        /// <summary>
        /// Updates an event
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, NegativeGameEventRequest request)
        {
            var result = await _negativeGameEventService.Update(id, request);
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
            await _negativeGameEventService.Delete(id);
            return Ok();
        }
    }
}
