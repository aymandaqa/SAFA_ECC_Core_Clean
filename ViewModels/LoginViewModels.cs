using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels
{
    public class LoginChangePasswordViewModel 
    {
        public LoginViewModel? Login { get; set; }
        public ChangePasswordViewModel? ChangePassword { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string? CurrentPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string? ConfirmNewPassword { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        public string? Username { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}


