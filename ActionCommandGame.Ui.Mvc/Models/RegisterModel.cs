using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ActionCommandGame.Ui.Mvc.Models
{
    public class RegisterModel
    {
        [Required]
        [EmailAddress]
        [DisplayName("Email Address:")]
        public required string Username { get; set; }

        [Required]
        [DisplayName("Username:")]
        public required string DisplayName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Password:")]
        public required string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        [DisplayName("Confirm password:")]
        public required string ConfirmPassword { get; set; }
    }
}
