﻿using System.ComponentModel.DataAnnotations;

namespace AuthAPI903.Dtos
{
    public class LoginDto2
    {
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
