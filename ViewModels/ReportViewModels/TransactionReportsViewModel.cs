using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.ReportViewModels
{
    public class TransactionReportsViewModel
    {
        [Display(Name = "من تاريخ")]
        [DataType(DataType.Date)]
        public DateTime? FromDate { get; set; }

        [Display(Name = "إلى تاريخ")]
        [DataType(DataType.Date)]
        public DateTime? ToDate { get; set; }

        [Display(Name = "نوع المعاملة")]
        public string? TransactionType { get; set; }

        [Display(Name = "اسم المستخدم")]
        public string? UserName { get; set; }

        public List<TransactionReportItemViewModel>? ReportItems { get; set; }
    }

    public class TransactionReportItemViewModel
    {
        public DateTime TransactionDate { get; set; }
        public string? TransactionType { get; set; }
        public string? Description { get; set; }
        public string? UserName { get; set; }
        public decimal Amount { get; set; }
    }
}
