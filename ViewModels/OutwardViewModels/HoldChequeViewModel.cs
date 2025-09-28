using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.OutwardViewModels
{
    public class HoldChequeViewModel
    {
        [Display(Name = "رقم الشيك")]
        public string? ChequeNumber { get; set; }

        [Display(Name = "تاريخ التعليق")]
        [DataType(DataType.Date)]
        public DateTime? HoldDate { get; set; }

        [Display(Name = "سبب التعليق")]
        public string? HoldReason { get; set; }

        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
    }
}
