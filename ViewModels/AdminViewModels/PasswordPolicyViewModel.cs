using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.AdminViewModels
{
    public class PasswordPolicyViewModel
    {
        [Display(Name = "الحد الأدنى لطول كلمة المرور")]
        public int MinimumLength { get; set; }

        [Display(Name = "تتطلب أحرفًا كبيرة")]
        public bool RequireUppercase { get; set; }

        [Display(Name = "تتطلب أحرفًا صغيرة")]
        public bool RequireLowercase { get; set; }

        [Display(Name = "تتطلب أرقامًا")]
        public bool RequireDigit { get; set; }

        [Display(Name = "تتطلب أحرفًا خاصة")]
        public bool RequireNonAlphanumeric { get; set; }

        [Display(Name = "عدد مرات تكرار كلمة المرور المسموح بها")]
        public int PasswordHistoryCount { get; set; }

        [Display(Name = "مدة صلاحية كلمة المرور (بالأيام)")]
        public int PasswordExpirationDays { get; set; }

        [Display(Name = "عدد الأحرف الفريدة المطلوبة")]
        public int RequiredUniqueChars { get; set; }
    }
}
