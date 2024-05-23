using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ActionCommandGame.Ui.Mvc.Models
{
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        [DisplayName("Username:")]
        public required string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Password:")]
        public required string Password { get; set; }

        [Required]
        [DisplayName("Remember me?")]
        public required bool RememberMe { get; set; }
    }
}
