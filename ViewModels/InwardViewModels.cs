using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using SAFA_ECC_Core_Clean.Models;

namespace SAFA_ECC_Core_Clean.ViewModels.InwardViewModels
{
    public class InwardSearchViewModel
    {
        public string? ChequeNumber { get; set; }
        public string? AccountNumber { get; set; }
        public string? BankName { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? Status { get; set; }
        public List<InwardChequeViewModel>? Cheques { get; set; }
    }

    public class InwardChequeViewModel
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

    public class InwardDetailsViewModel
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
        public List<InwardChequeHistoryViewModel>? History { get; set; }
    }

    public class InwardChequeHistoryViewModel
    {
        public DateTime Date { get; set; }
        public string? Event { get; set; }
        public string? Details { get; set; }
        public string? UserName { get; set; }
    }

    public class FixRetCHQViewModel
    {
        public string? ChequeNumber { get; set; }
        public string? BankName { get; set; }
        public string? AccountNumber { get; set; }
        public string? CurrentStatus { get; set; }
        public string? NewStatus { get; set; }
        public string? Reason { get; set; }
        public List<SelectListItem>? AvailableStatuses { get; set; }
    }

    public class PostingRestrictionsViewModel
    {
        public string? AccountNumber { get; set; }
        public string? RestrictionType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Reason { get; set; }
        public List<PostingRestrictionItemViewModel>? Restrictions { get; set; }
    }

    public class PostingRestrictionItemViewModel
    {
        public int Id { get; set; }
        public string? AccountNumber { get; set; }
        public string? RestrictionType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Reason { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class PrintCHQQRViewModel
    {
        public string? ChequeNumber { get; set; }
        public string? BankName { get; set; }
        public string? AccountNumber { get; set; }
        public decimal Amount { get; set; }
        public string? BeneficiaryName { get; set; }
        public string? QRImagePath { get; set; }
    }

    public class InwardCustomerDuesReportViewModel
    {
        public string? CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public List<CustomerDueItemViewModel>? Dues { get; set; }
    }

    public class CustomerDueItemViewModel
    {
        public DateTime DueDate { get; set; }
        public string? ChequeNumber { get; set; }
        public decimal Amount { get; set; }
        public string? Currency { get; set; }
        public string? Status { get; set; }
        public string? Description { get; set; }
    }

    public class PMA_DATAVerficationDetailsViewModel
    {
        public string? ChequeNumber { get; set; }
        public string? BankName { get; set; }
        public string? AccountNumber { get; set; }
        public string? CurrentStatus { get; set; }
        public string? VerificationResult { get; set; }
        public List<ChangeLogItemViewModel>? ChangeLog { get; set; }
        public List<OnUs_Imgs>? OnUsImages { get; set; }
        public List<OnUs_Tbl>? OnUsData { get; set; }
    }

    public class ResendHistoryItemViewModel
    {
        public int Id { get; set; }
        public DateTime ResendDate { get; set; }
        public string? ResentBy { get; set; }
        public string? Status { get; set; }
        public string? Details { get; set; }
    }

    public class RejectChequeViewModel
    {
        public string? ChequeNumber { get; set; }
        public string? BankName { get; set; }
        public string? AccountNumber { get; set; }
        public string? Reason { get; set; }
        public List<SelectListItem>? ReasonsList { get; set; }
    }

    public class ReturnOnUsStoppedChequeDetailsViewModel
    {
        public string? ChequeNumber { get; set; }
        public string? BankName { get; set; }
        public string? AccountNumber { get; set; }
        public string? Status { get; set; }
        public string? StopReason { get; set; }
        public DateTime StopDate { get; set; }
        public string? ReturnReason { get; set; }
        public DateTime ReturnDate { get; set; }
        public List<OnUs_Imgs>? OnUsImages { get; set; }
        public List<OnUs_Tbl>? OnUsData { get; set; }
    }

    public class ADDEmailViewModel
    {
        public string? EmailAddress { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
        public List<EmailItemViewModel>? EmailList { get; set; }
    }

    public class EmailItemViewModel
    {
        public int Id { get; set; }
        public string? EmailAddress { get; set; }
        public bool IsActive { get; set; }
    }
}


