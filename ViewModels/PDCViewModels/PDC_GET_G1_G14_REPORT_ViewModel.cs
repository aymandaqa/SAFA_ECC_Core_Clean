using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.PDCViewModels
{
    public class PDC_GET_G1_G14_REPORT_ViewModel
    {
        [Display(Name = "من تاريخ")]
        [DataType(DataType.Date)]
        public DateTime? FromDate { get; set; }

        [Display(Name = "إلى تاريخ")]
        [DataType(DataType.Date)]
        public DateTime? ToDate { get; set; }

        [Display(Name = "نوع التقرير")]
        public string? ReportType { get; set; }

        public List<PDCReportItemViewModel>? ReportData { get; set; }
    }

    public class PDCReportItemViewModel
    {
        public string? ItemName { get; set; }
        public decimal Value { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public string? Currency { get; set; }
    }
}
