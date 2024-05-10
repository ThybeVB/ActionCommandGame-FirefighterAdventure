using ActionCommandGame.Services.Model.Core;

namespace ActionCommandGame.Services.Model.Results.Identity
{
    public class JwtAuthenticationResult : ServiceResult
    {
        public string? Token { get; set; }
    }
}
