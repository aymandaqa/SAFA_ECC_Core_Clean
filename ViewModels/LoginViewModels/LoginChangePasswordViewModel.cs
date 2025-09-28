using System.ComponentModel.DataAnnotations;
using SAFA_ECC_Core_Clean.ViewModels.AuthenticationViewModels;

namespace SAFA_ECC_Core_Clean.ViewModels.LoginViewModels
{
    public class LoginChangePasswordViewModel
    {
        public LoginViewModel Login { get; set; } = new LoginViewModel();
        public ChangePasswordViewModel ChangePassword { get; set; } = new ChangePasswordViewModel();
    }

    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "كلمة المرور الحالية مطلوبة")]
        [DataType(DataType.Password)]
        [Display(Name = "كلمة المرور الحالية")]
        public string? CurrentPassword { get; set; }

        [Required(ErrorMessage = "كلمة المرور الجديدة مطلوبة")]
        [StringLength(100, ErrorMessage = "يجب أن تكون كلمة المرور الجديدة على الأقل {2} أحرف طويلة.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "كلمة المرور الجديدة")]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "تأكيد كلمة المرور الجديدة")]
        [Compare("NewPassword", ErrorMessage = "كلمة المرور الجديدة وتأكيدها غير متطابقين.")]
        public string? ConfirmNewPassword { get; set; }
    }
}
