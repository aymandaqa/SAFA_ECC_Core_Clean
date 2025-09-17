using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SAFA_ECC_Core_Clean.ViewModels.ReportViewModels
{
    public class ReportViewModel
    {
        public string? ReportName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<ReportDataItemViewModel>? Items { get; set; }
    }

    public class ReportDataItemViewModel
    {
        public string? ItemName { get; set; }
        public decimal Value { get; set; }
        public string? Description { get; set; }
    }

    public class TransactionReportViewModel
    {
        public string? ReportType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? TransactionType { get; set; }
        public List<TransactionReportItemViewModel>? ReportItems { get; set; }
    }

    public class TransactionReportItemViewModel
    {
        public DateTime TransactionDate { get; set; }
        public string? TransactionType { get; set; }
        public string? Description { get; set; }
        public decimal Amount { get; set; }
        public string? Status { get; set; }
    }
}


