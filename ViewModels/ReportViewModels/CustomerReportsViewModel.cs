using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.ReportViewModels
{
    public class CustomerReportsViewModel
    {
        [Display(Name = "رقم العميل")]
        public string? CustomerId { get; set; }

        [Display(Name = "اسم العميل")]
        public string? CustomerName { get; set; }

        [Display(Name = "من تاريخ")]
        [DataType(DataType.Date)]
        public DateTime? FromDate { get; set; }

        [Display(Name = "إلى تاريخ")]
        [DataType(DataType.Date)]
        public DateTime? ToDate { get; set; }

        public List<CustomerReportItemViewModel>? ReportItems { get; set; }
    }

    public class CustomerReportItemViewModel
    {
        public string? ChequeNumber { get; set; }
        public decimal Amount { get; set; }
        public string? Type { get; set; } // Inward or Outward
        public string? Status { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
