using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.AdminViewModels
{
    public class SystemSettingsViewModel
    {
        [Display(Name = "اسم الإعداد")]
        public string? SettingName { get; set; }

        [Display(Name = "قيمة الإعداد")]
        public string? SettingValue { get; set; }

        [Display(Name = "الوصف")]
        public string? Description { get; set; }

        [Display(Name = "قابل للتعديل")]
        public bool IsEditable { get; set; }

        [Display(Name = "اسم الموقع")]
        public string? SiteName { get; set; }

        [Display(Name = "البريد الإلكتروني للمسؤول")]
        public string? AdminEmail { get; set; }

        [Display(Name = "رقم الهاتف")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "عنوان الشركة")]
        public string? CompanyAddress { get; set; }

        [Display(Name = "السماح بالتسجيل")]
        public bool AllowRegistration { get; set; }

        [Display(Name = "وضع الصيانة")]
        public bool MaintenanceMode { get; set; }
    }
}
