﻿using ActionCommandGame.RestApi.Models;
using ActionCommandGame.RestApi.Settings;
using ActionCommandGame.Services.Model.Requests.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ActionCommandGame.RestApi.Services.Helpers;

namespace ActionCommandGame.RestApi.Services
{
    public class IdentityService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly UserManager<IdentityUser> _userManager;

        public IdentityService(JwtSettings jwtSettings, UserManager<IdentityUser> userManager)
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

            var token = GenerateJwtToken(user, _jwtSettings.Secret, _jwtSettings.Expiry.Value);
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

            var user = new IdentityUser(request.Username);
            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return JwtAuthenticationHelpers.RegisterError(result.Errors);
            }

            var token = GenerateJwtToken(user, _jwtSettings.Secret, _jwtSettings.Expiry.Value);

            return new JwtAuthenticationResult()
            {
                Token = token
            };
        }

        private string GenerateJwtToken(IdentityUser user, string secret, TimeSpan expiry)
        {
            // Now its ime to define the jwt token which will be responsible for creating our tokens
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            // We get our secret from the AppSettings
            var key = Encoding.ASCII.GetBytes(secret);

            //Define claims
            var claims = new List<Claim>()
            {
                new Claim("Id", user.Id),
                
                // the JTI is used for our refresh token which we will be converting in the next video
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            if (!string.IsNullOrWhiteSpace(user.Email))
            {
                claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Email));
                claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            }

            // we define our token descriptor
            // We need to utilise claims which are properties in our token which gives information about the token
            // which belong to the specific user who it belongs to
            // ,so it could contain their id, name, email the good part is that this information was
            // generated by our server and identity framework which is valid and trusted
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                // the life span of the token needs to be shorter and utilise refresh token to keep the user signedin
                // but since this is a demo app we can extend it to fit our current need
                Expires = DateTime.UtcNow.Add(expiry),
                // here we are adding the encryption algorithm information which will be used to decrypt our token
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);

            var jwtToken = jwtTokenHandler.WriteToken(token);

            return jwtToken;
        }
    }
}
