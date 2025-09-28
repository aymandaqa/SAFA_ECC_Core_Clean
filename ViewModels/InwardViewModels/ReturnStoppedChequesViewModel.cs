using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.InwardViewModels
{
    public class ReturnStoppedChequesViewModel
    {
        public string Title { get; set; }
        public object Tree { get; set; }

        [Display(Name = "رقم الشيك")]
        public string? ChequeNumber { get; set; }

        [Display(Name = "اسم البنك")]
        public string? BankName { get; set; }

        [Display(Name = "المبلغ")]
        public decimal Amount { get; set; }

        [Display(Name = "تاريخ الإيقاف")]
        [DataType(DataType.Date)]
        public DateTime? StopDate { get; set; }

        [Display(Name = "سبب الإيقاف")]
        public string? StopReason { get; set; }

        public List<StoppedChequeItemViewModel>? StoppedCheques { get; set; }
    }

    public class StoppedChequeItemViewModel
    {
        public int Id { get; set; }
        public string? ChequeNumber { get; set; }
        public string? BankName { get; set; }
        public decimal Amount { get; set; }
        public DateTime StopDate { get; set; }
        public string? StopReason { get; set; }
    }
}
