using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.InwardViewModels
{
    public class InwardFixedErrorDetailsViewModel
    {
        [Display(Name = "رقم الشيك")]
        public string? ChequeNumber { get; set; }

        [Display(Name = "تاريخ الشيك")]
        [DataType(DataType.Date)]
        public DateTime? ChequeDate { get; set; }

        [Display(Name = "المبلغ")]
        public decimal Amount { get; set; }

        [Display(Name = "اسم البنك")]
        public string? BankName { get; set; }

        [Display(Name = "سبب الخطأ")]
        public string? ErrorReason { get; set; }

        [Display(Name = "الإجراء المتخذ")]
        public string? ActionTaken { get; set; }

        public bool IsFixed { get; set; }
        public string? Message { get; set; }
    }
}
