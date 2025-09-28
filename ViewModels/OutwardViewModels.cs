using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using SAFA_ECC_Core_Clean.Models;

namespace SAFA_ECC_Core_Clean.ViewModels.OutwardViewModels
{
    public class OutwardViewModel
    {
        public List<OutwardChequeViewModel>? OutwardCheques { get; set; }
        public string? SearchQuery { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
    }

    public class OutwardChequeViewModel
    {
        public int Id { get; set; }
        public string? ChequeNumber { get; set; }
        public decimal Amount { get; set; }
        public string? Status { get; set; }
        public string? BankName { get; set; }
        public string? AccountNumber { get; set; }
        public DateTime ChequeDate { get; set; }
        public string? BeneficiaryName { get; set; }
    }

    public class OutwardDetailsViewModel
    {
        public int ChequeId { get; set; }
        public string? ChequeNumber { get; set; }
        public decimal Amount { get; set; }
        public string? Currency { get; set; }
        public string? Status { get; set; }
        public string? BankName { get; set; }
        public string? BranchName { get; set; }
        public string? AccountNumber { get; set; }
        public string? BeneficiaryName { get; set; }
        public DateTime ChequeDate { get; set; }
        public DateTime ValueDate { get; set; }
        public string? DrawerName { get; set; }
        public string? DrawerAccount { get; set; }
        public string? Description { get; set; }
        public string? ImagePath { get; set; }
        public List<OutwardChequeHistoryViewModel>? History { get; set; }
    }

    public class OutwardChequeHistoryViewModel
    {
        public DateTime Date { get; set; }
        public string? Event { get; set; }
        public string? Details { get; set; }
        public string? UserName { get; set; }
    }

    public class OutwardReceiptViewModel
    {
        public string? ChequeNumber { get; set; }
        public string? BankName { get; set; }
        public string? AccountNumber { get; set; }
        public decimal Amount { get; set; }
        public string? BeneficiaryName { get; set; }
        public DateTime ReceiptDate { get; set; }
        public string? ReceivedBy { get; set; }
        public string? ReceiptNumber { get; set; }
        public string? Status { get; set; }
        public string? Currency { get; set; }
        public string? DrawerName { get; set; }
        public string? DrawerAccount { get; set; }
        public DateTime ChequeDate { get; set; }
        public string? Description { get; set; }
    }

    public class DeleteOutwardChequeViewModel
    {
        public string? ChequeNumber { get; set; }
        public string? BankName { get; set; }
        public string? AccountNumber { get; set; }
        public string? Reason { get; set; }
        public List<SelectListItem>? ReasonsList { get; set; }
    }

    public class OutwardReportViewModel
    {
        public string? ReportType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? BankName { get; set; }
        public string? Status { get; set; }
        public List<OutwardReportItemViewModel>? ReportItems { get; set; }
    }

    public class OutwardReportItemViewModel
    {
        public DateTime ChequeDate { get; set; }
        public string? ChequeNumber { get; set; }
        public decimal Amount { get; set; }
        public string? BeneficiaryName { get; set; }
        public string? BankName { get; set; }
        public string? Status { get; set; }
    }

    public class Out_VerificationDetailsViewModel
    {
        public string? VerificationId { get; set; }
        public DateTime RequestDate { get; set; }
        public string? RequesterName { get; set; }
        public string? Status { get; set; }
        public string? Description { get; set; }
        public List<Out_VerificationItemViewModel>? Items { get; set; }
    }

    public class Out_VerificationItemViewModel
    {
        public string? ItemName { get; set; }
        public int Quantity { get; set; }
        public string? Notes { get; set; }
    }
}

