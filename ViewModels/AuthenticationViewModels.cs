using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SAFA_ECC_Core_Clean.ViewModels.AuthenticationViewModels
{
    public class AuthenticationViewModel
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public bool RememberMe { get; set; }
        public string? ReturnUrl { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required] public string? OldPassword { get; set; }
        [Required] public string? NewPassword { get; set; }
        [Required] [Compare("NewPassword")] public string? ConfirmNewPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required] [EmailAddress] public string? Email { get; set; }
        [Required] public string? Password { get; set; }
        [Required] [Compare("Password")] public string? ConfirmPassword { get; set; }
        public string? Token { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required] [EmailAddress] public string? Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required] public string? Username { get; set; }
        [Required] public string? Password { get; set; }
        public bool RememberMe { get; set; }
        public string? ReturnUrl { get; set; }
    }

    public class RegisterViewModel
    {
        [Required] public string? Username { get; set; }
        [Required] [EmailAddress] public string? Email { get; set; }
        [Required] public string? Password { get; set; }
        [Required] [Compare("Password")] public string? ConfirmPassword { get; set; }
    }
}


