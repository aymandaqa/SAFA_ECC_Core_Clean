using System;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.AdminViewModels
{
    public class SystemMonitoringViewModel
    {
        [Display(Name = "اسم المكون")]
        public string? ComponentName { get; set; }

        [Display(Name = "الحالة")]
        public string? Status { get; set; }

        [Display(Name = "آخر تحديث")]
        public DateTime LastUpdated { get; set; }

        [Display(Name = "رسالة")]
        public string? Message { get; set; }
    }
}
