using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.InwardViewModels
{
    public class WFChqStatusViewModel
    {
        [Display(Name = "رقم الشيك")]
        public string? ChequeNumber { get; set; }

        [Display(Name = "اسم البنك")]
        public string? BankName { get; set; }

        [Display(Name = "من تاريخ")]
        [DataType(DataType.Date)]
        public DateTime? FromDate { get; set; }

        [Display(Name = "إلى تاريخ")]
        [DataType(DataType.Date)]
        public DateTime? ToDate { get; set; }

        public List<WFChequeItemViewModel>? Cheques { get; set; }
    }

    public class WFChequeItemViewModel
    {
        public string? ChequeNumber { get; set; }
        public string? BankName { get; set; }
        public DateTime ChequeDate { get; set; }
        public decimal Amount { get; set; }
        public string? Status { get; set; }
    }
}
