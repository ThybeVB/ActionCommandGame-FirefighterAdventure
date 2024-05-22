using System.ComponentModel.DataAnnotations;

namespace ActionCommandGame.Services.Model.Requests.Identity
{
    public class UserRegisterRequest
    {
        [Required]
        [EmailAddress]
        public required string Username { get; set; }

        [Required]
        public required string DisplayName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }
    }
}
