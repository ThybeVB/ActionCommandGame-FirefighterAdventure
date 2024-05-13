using ActionCommandGame.Ui.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ActionCommandGame.Sdk;
using ActionCommandGame.Services.Model.Requests.Identity;
using ActionCommandGame.Ui.Mvc.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace ActionCommandGame.Ui.Mvc.Controllers
{
    public class IndexController : Controller
    {
        private readonly ILogger<Index> _logger;
        private IdentitySdk _identitySdk;
        private TokenStore _tokenStore;

        public IndexController(ILogger<Index> logger, IdentitySdk identitySdk, TokenStore tokenStore)
        {
            _logger = logger;
            _identitySdk = identitySdk;
            _tokenStore = tokenStore;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Login(string? returnUrl)
        {
            await HttpContext.SignOutAsync();
            ViewBag.ReturnUrl = returnUrl ?? "/Game/";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model, string? returnUrl)
        {
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = "/Game/";
            }

            if (!ModelState.IsValid)
            {
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }

            var loginRequest = new UserSignInRequest
            {
                Username = model.Username,
                Password = model.Password
            };
            
            var loginResult = await _identitySdk.SignIn(loginRequest);
            if (!loginResult.IsSuccess)
            {
                ModelState.AddModelError("", "User/Password combination is wrong.");
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }

            _tokenStore.SaveToken(loginResult.Token);
            var principal = CreatePrincipalFromToken(loginResult.Token);
            await HttpContext.SignInAsync(principal);

            return LocalRedirect(returnUrl);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model, string? returnUrl)
        {
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = "/Game/";
            }

            if (!ModelState.IsValid)
            {
                ViewBag.ReturnUrl = returnUrl;
                return View(model);
            }

            var request = new UserRegisterRequest
            {
                Username = model.Username,
                Password = model.Password
            };

            var result = await _identitySdk.Register(request);

            if (!result.IsSuccess)
            {
                foreach (var error in result.Messages)
                {
                    ModelState.AddModelError("", error.Message);
                }
                ViewBag.ReturnUrl = returnUrl;
                return View(model);
            }

            _tokenStore.SaveToken(result.Token);
            var principal = CreatePrincipalFromToken(result.Token);
            await HttpContext.SignInAsync(principal);

            return LocalRedirect(returnUrl);
        }

        private ClaimsPrincipal CreatePrincipalFromToken(string? bearerToken)
        {
            var identity = CreateIdentityFromToken(bearerToken);

            return new ClaimsPrincipal(identity);
        }

        private ClaimsIdentity CreateIdentityFromToken(string? bearerToken)
        {
            if (string.IsNullOrWhiteSpace(bearerToken))
            {
                return new ClaimsIdentity();
            }

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(bearerToken);

            var claims = new List<Claim>();
            foreach (var claim in token.Claims)
            {
                claims.Add(claim);
            }

            //HttpContext required a "Name" claim to display a User Name
            var usernameClaim = token.Claims.SingleOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
            if (usernameClaim is not null)
            {
                claims.Add(new Claim(ClaimTypes.Name, usernameClaim.Value));
            }

            return new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        }

        [HttpGet]
        public IActionResult Register(string? returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl ?? "/Game/";
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
