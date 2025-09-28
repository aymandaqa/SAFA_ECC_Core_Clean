using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.AdminViewModels
{
    public class SystemConfigurationViewModel
    {
        [Display(Name = "اسم الإعداد")]
        public string? SettingName { get; set; }

        [Display(Name = "قيمة الإعداد")]
        public string? SettingValue { get; set; }

        [Display(Name = "الوصف")]
        public string? Description { get; set; }
    }
}
