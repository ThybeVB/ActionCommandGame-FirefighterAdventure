using ActionCommandGame.Model;
using ActionCommandGame.RestApi.Models;
using ActionCommandGame.RestApi.Services.Helpers;
using ActionCommandGame.RestApi.Settings;
using ActionCommandGame.Services.Model.Requests.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ActionCommandGame.RestApi.Services
{
    public class IdentityService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly UserManager<Player> _userManager;

        public IdentityService(JwtSettings jwtSettings, UserManager<Player> userManager)
        {
            _jwtSettings = jwtSettings;
            _userManager = userManager;
        }

        public async Task<JwtAuthenticationResult> SignIn(UserSignInRequest request)
        {
            if (string.IsNullOrWhiteSpace(_jwtSettings.Secret) || !_jwtSettings.Expiry.HasValue)
            {
                return JwtAuthenticationHelpers.JwtConfigurationError();
            }

            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null)
            {
                return JwtAuthenticationHelpers.LoginFailed();
            }

            var isPassValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!isPassValid)
            {
                return JwtAuthenticationHelpers.LoginFailed();
            }

            var roles = await _userManager.GetRolesAsync(user);
            var token = GenerateJwtToken(user, _jwtSettings.Secret, _jwtSettings.Expiry.Value, roles);
            return new JwtAuthenticationResult
            {
                Token = token
            };
        }

        public async Task<JwtAuthenticationResult> Register(UserRegisterRequest request)
        {
            if (string.IsNullOrWhiteSpace(_jwtSettings.Secret)
                || !_jwtSettings.Expiry.HasValue)
            {
                return JwtAuthenticationHelpers.JwtConfigurationError();
            }

            var existingUser = await _userManager.FindByNameAsync(request.Username);
            if (existingUser is not null)
            {
                return JwtAuthenticationHelpers.UserExists();
            }

            var user = new Player { Name = request.DisplayName, UserName = request.Username, Email = request.Username, Money = 25 };
            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return JwtAuthenticationHelpers.RegisterError(result.Errors);
            }

            var roles = await _userManager.GetRolesAsync(user);
            var token = GenerateJwtToken(user, _jwtSettings.Secret, _jwtSettings.Expiry.Value, roles);

            return new JwtAuthenticationResult()
            {
                Token = token
            };
        }

        public async Task<JwtAuthenticationResult> Update(UserRegisterRequest request)
        {
            if (string.IsNullOrWhiteSpace(_jwtSettings.Secret)
                || !_jwtSettings.Expiry.HasValue)
            {
                return JwtAuthenticationHelpers.JwtConfigurationError();
            }

            var existingUser = await _userManager.FindByEmailAsync(request.Username);

            await _userManager.RemovePasswordAsync(existingUser);
            await _userManager.AddPasswordAsync(existingUser, request.Password);

            existingUser.Name = request.DisplayName;
            var result = await _userManager.UpdateAsync(existingUser);

            if (!result.Succeeded)
            {
                return JwtAuthenticationHelpers.UpdateError(result.Errors);
            }

            var roles = await _userManager.GetRolesAsync(existingUser);
            var token = GenerateJwtToken(existingUser, _jwtSettings.Secret, _jwtSettings.Expiry.Value, roles);

            return new JwtAuthenticationResult()
            {
                Token = token
            };
        }

        private string GenerateJwtToken(Player user, string secret, TimeSpan expiry, IEnumerable<string> roles)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(secret);

            var claims = new List<Claim>()
            {
                new Claim("Id", user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            if (!string.IsNullOrWhiteSpace(user.Email))
            {
                claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Email));
                claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            }

            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(expiry),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            return jwtToken;
        }
    }
}
