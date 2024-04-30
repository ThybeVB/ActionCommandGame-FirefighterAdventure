using ActionCommandGame.Services.Model.Requests.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ActionCommandGame.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        [HttpPost]
        public IActionResult SignIn(UserSignInRequest request)
        {
            //todo
            return Ok();
        }

        [HttpPost]
        public IActionResult Register(UserRegisterRequest request)
        {
            //todo
            return Ok();
        }
    }
}
