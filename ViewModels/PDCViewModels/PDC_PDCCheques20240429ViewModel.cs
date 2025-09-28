using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.PDCViewModels
{
    public class PDC_PDCCheques20240429ViewModel
    {
        [Display(Name = "من تاريخ")]
        [DataType(DataType.Date)]
        public DateTime? FromDate { get; set; }

        [Display(Name = "إلى تاريخ")]
        [DataType(DataType.Date)]
        public DateTime? ToDate { get; set; }

        [Display(Name = "رقم الشيك")]
        public string? ChequeNumber { get; set; }

        [Display(Name = "اسم البنك")]
        public string? BankName { get; set; }

        [Display(Name = "الحالة")]
        public string? Status { get; set; }

        public List<PDC_PDCChequeItem20240429ViewModel>? Cheques { get; set; }
    }

    public class PDC_PDCChequeItem20240429ViewModel
    {
        public string? ChequeNumber { get; set; }
        public decimal Amount { get; set; }
        public string? BeneficiaryName { get; set; }
        public string? BankName { get; set; }
        public DateTime DueDate { get; set; }
        public string? Status { get; set; }
    }
}
