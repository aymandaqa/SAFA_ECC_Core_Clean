using System;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.InwardViewModels
{
    public class FixRetCHQViewModel
    {
        [Display(Name = "رقم الشيك")]
        public string? ChequeNumber { get; set; }

        [Display(Name = "تاريخ الإرجاع")]
        [DataType(DataType.Date)]
        public DateTime? ReturnDate { get; set; }

        [Display(Name = "سبب الإرجاع")]
        public string? ReturnReason { get; set; }

        [Display(Name = "الإجراء المتخذ")]
        public string? ActionTaken { get; set; }

        [Display(Name = "ملاحظات")]
        public string? Notes { get; set; }
    }
}
