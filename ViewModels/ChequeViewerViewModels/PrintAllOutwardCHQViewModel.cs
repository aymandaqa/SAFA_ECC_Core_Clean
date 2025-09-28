using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.ChequeViewerViewModels
{
    public class PrintAllOutwardCHQViewModel
    {
        [Display(Name = "من تاريخ")]
        [DataType(DataType.Date)]
        public DateTime? FromDate { get; set; }

        [Display(Name = "إلى تاريخ")]
        [DataType(DataType.Date)]
        public DateTime? ToDate { get; set; }

        [Display(Name = "اسم البنك")]
        public string? BankName { get; set; }

        [Display(Name = "رقم الحساب")]
        public string? AccountNumber { get; set; }

        public List<PrintOutwardChequeItemViewModel>? Cheques { get; set; }
    }

    public class PrintOutwardChequeItemViewModel
    {
        public string? ChequeNumber { get; set; }
        public decimal Amount { get; set; }
        public string? BeneficiaryName { get; set; }
        public string? BankName { get; set; }
        public DateTime ChequeDate { get; set; }
        public string? ImagePath { get; set; }
    }
}
