using System;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.ChequeViewerViewModels
{
    public class ChequeDetailsViewModel
    {
        [Display(Name = "رقم الشيك")]
        public string? ChequeNumber { get; set; }

        [Display(Name = "المبلغ")]
        public decimal Amount { get; set; }

        [Display(Name = "تاريخ الاستحقاق")]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        [Display(Name = "اسم الساحب")]
        public string? DrawerName { get; set; }

        [Display(Name = "رقم حساب الساحب")]
        public string? DrawerAccountNumber { get; set; }

        [Display(Name = "اسم المستفيد")]
        public string? BeneficiaryName { get; set; }

        [Display(Name = "حالة الشيك")]
        public string? Status { get; set; }

        [Display(Name = "صورة الشيك (أمامية)")]
        public string? FrontImageBase64 { get; set; }

        [Display(Name = "صورة الشيك (خلفية)")]
        public string? BackImageBase64 { get; set; }
    }
}
