using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SAFA_ECC_Core_Clean.ViewModels.FinancialViewModels
{
    public class FinancialAuthorizationViewModel
    {
        public int AuthorizationId { get; set; }
        public string? RequestType { get; set; }
        public string? RequestedBy { get; set; }
        public DateTime RequestDate { get; set; }
        public decimal Amount { get; set; }
        public string? Currency { get; set; }
        public string? Status { get; set; }
        public string? Description { get; set; }
        public List<FinancialTransactionViewModel>? Transactions { get; set; }
    }

    public class FinancialTransactionViewModel
    {
        public int TransactionId { get; set; }
        public string? TransactionType { get; set; }
        public decimal Amount { get; set; }
        public string? Currency { get; set; }
        public string? Description { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? Status { get; set; }
    }

    public class UserFinancialAuthViewModel
    {
        public string? UserName { get; set; }
        public List<FinancialAuthorizationViewModel>? PendingAuthorizations { get; set; }
        public List<FinancialAuthorizationViewModel>? ApprovedAuthorizations { get; set; }
        public List<FinancialAuthorizationViewModel>? RejectedAuthorizations { get; set; }
    }

    public class FinancialReportViewModel
    {
        public string? ReportType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Currency { get; set; }
        public List<FinancialReportItemViewModel>? ReportItems { get; set; }
    }

    public class FinancialReportItemViewModel
    {
        public DateTime TransactionDate { get; set; }
        public string? TransactionType { get; set; }
        public string? Description { get; set; }
        public decimal Amount { get; set; }
        public string? Currency { get; set; }
        public string? Status { get; set; }
        public string? AuthorizedBy { get; set; }
    }
}


