using System;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.OutwardViewModels
{
    public class CheckImageViewModel
    {
        [Display(Name = "رقم الشيك")]
        public string? ChequeNumber { get; set; }

        [Display(Name = "صورة الشيك الأمامية")]
        public string? FrontImageBase64 { get; set; }

        [Display(Name = "صورة الشيك الخلفية")]
        public string? BackImageBase64 { get; set; }

        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
    }
}
