using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.ReportViewModels
{
    public class DailyReportViewModel
    {
        [Display(Name = "تاريخ التقرير")]
        [DataType(DataType.Date)]
        public DateTime ReportDate { get; set; }

        [Display(Name = "إجمالي الشيكات الواردة")]
        public int TotalInwardCheques { get; set; }

        [Display(Name = "إجمالي الشيكات الصادرة")]
        public int TotalOutwardCheques { get; set; }

        [Display(Name = "قيمة الشيكات الواردة")]
        public decimal TotalInwardAmount { get; set; }

        [Display(Name = "قيمة الشيكات الصادرة")]
        public decimal TotalOutwardAmount { get; set; }

        public List<DailyReportItemViewModel>? ReportItems { get; set; }
    }

    public class DailyReportItemViewModel
    {
        public string? ChequeNumber { get; set; }
        public decimal Amount { get; set; }
        public string? Type { get; set; } // Inward or Outward
        public string? Status { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
