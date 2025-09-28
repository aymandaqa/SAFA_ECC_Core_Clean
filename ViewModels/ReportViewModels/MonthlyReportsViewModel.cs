using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.ReportViewModels
{
    public class MonthlyReportsViewModel
    {
        [Display(Name = "الشهر")]
        public int Month { get; set; }

        [Display(Name = "السنة")]
        public int Year { get; set; }

        [Display(Name = "إجمالي الشيكات الواردة")]
        public int TotalInwardCheques { get; set; }

        [Display(Name = "إجمالي الشيكات الصادرة")]
        public int TotalOutwardCheques { get; set; }

        [Display(Name = "قيمة الشيكات الواردة")]
        public decimal TotalInwardAmount { get; set; }

        [Display(Name = "قيمة الشيكات الصادرة")]
        public decimal TotalOutwardAmount { get; set; }

        public List<MonthlyReportItemViewModel>? ReportItems { get; set; }
    }

    public class MonthlyReportItemViewModel
    {
        public DateTime Date { get; set; }
        public string? ChequeNumber { get; set; }
        public decimal Amount { get; set; }
        public string? Type { get; set; } // Inward or Outward
        public string? Status { get; set; }
    }
}
