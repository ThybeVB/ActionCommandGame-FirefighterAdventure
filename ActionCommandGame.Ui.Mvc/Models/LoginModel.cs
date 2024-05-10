using System.ComponentModel.DataAnnotations;

namespace ActionCommandGame.Ui.Mvc.Models
{
    public class LoginModel
    {
        [Required]
        public required string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [Required]
        public required bool RememberMe { get; set; }
    }
}
