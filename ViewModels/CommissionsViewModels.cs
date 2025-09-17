using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SAFA_ECC_Core_Clean.ViewModels.CommissionsViewModels
{
    public class CommissionsViewModel
    {
        public string? CommissionType { get; set; }
        public decimal Amount { get; set; }
        public string? Currency { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? Description { get; set; }
        public List<CommissionItemViewModel>? Commissions { get; set; }
    }

    public class CommissionItemViewModel
    {
        public int Id { get; set; }
        public string? CommissionType { get; set; }
        public decimal Amount { get; set; }
        public string? Currency { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? Description { get; set; }
    }

    public class AddCommissionViewModel
    {
        [Required] public string? CommissionType { get; set; }
        [Required] public decimal Amount { get; set; }
        [Required] public string? Currency { get; set; }
        public string? Description { get; set; }
    }

    public class EditCommissionViewModel
    {
        public int Id { get; set; }
        [Required] public string? CommissionType { get; set; }
        [Required] public decimal Amount { get; set; }
        [Required] public string? Currency { get; set; }
        public string? Description { get; set; }
    }

    public class DeleteCommissionViewModel
    {
        public int Id { get; set; }
        public string? CommissionType { get; set; }
    }

    public class CommissionDetailsViewModel
    {
        public int Id { get; set; }
        public string? CommissionType { get; set; }
        public decimal Amount { get; set; }
        public string? Currency { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? Description { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class CommissionReportViewModel
    {
        public string? ReportType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? CommissionType { get; set; }
        public List<CommissionReportItemViewModel>? ReportItems { get; set; }
    }

    public class CommissionReportItemViewModel
    {
        public DateTime TransactionDate { get; set; }
        public string? CommissionType { get; set; }
        public decimal Amount { get; set; }
        public string? Currency { get; set; }
        public string? Description { get; set; }
    }
}


