using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.OutwardViewModels
{
    public class RetunedChequeStatesViewModel
    {
        [Display(Name = "رقم الشيك")]
        public string? ChequeNumber { get; set; }

        [Display(Name = "تاريخ الإرجاع")]
        [DataType(DataType.Date)]
        public DateTime? ReturnDate { get; set; }

        [Display(Name = "سبب الإرجاع")]
        public string? ReturnReason { get; set; }

        [Display(Name = "الحالة")]
        public string? Status { get; set; }

        public List<ReturnedChequeStateItemViewModel>? ChequeStates { get; set; }
    }

    public class ReturnedChequeStateItemViewModel
    {
        public string? ChequeNumber { get; set; }
        public DateTime EventDate { get; set; }
        public string? EventDescription { get; set; }
        public string? CurrentStatus { get; set; }
    }
}
