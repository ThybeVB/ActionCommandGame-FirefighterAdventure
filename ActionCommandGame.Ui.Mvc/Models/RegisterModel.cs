﻿using System.ComponentModel.DataAnnotations;

namespace ActionCommandGame.Ui.Mvc.Models
{
    public class RegisterModel
    {
        [Required]
        [EmailAddress]
        public required string Username { get; set; }

        [Required]
        public required string DisplayName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public required string ConfirmPassword { get; set; }
    }
}
