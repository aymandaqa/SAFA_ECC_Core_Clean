using System;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.OutwardViewModels
{
    public class OutwordDetailsViewModel
    {
        [Display(Name = "رقم الشيك")]
        public string? ChequeNumber { get; set; }

        [Display(Name = "المبلغ")]
        public decimal Amount { get; set; }

        [Display(Name = "تاريخ الاستحقاق")]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        [Display(Name = "اسم البنك")]
        public string? BankName { get; set; }

        [Display(Name = "حالة الشيك")]
        public string? ChequeStatus { get; set; }

        [Display(Name = "صورة الشيك الأمامية")]
        public string? FrontImageBase64 { get; set; }

        [Display(Name = "صورة الشيك الخلفية")]
        public string? RearImageBase64 { get; set; }
    }
}
