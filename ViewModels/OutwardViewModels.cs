
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using SAFA_ECC_Core_Clean.ViewModels.SharedViewModels;

namespace SAFA_ECC_Core_Clean.ViewModels
{
    public class DeleteOutwardChequeViewModel { public int ChequeId { get; set; } }
    public class GetReturnedINHOUSESlipDetailsViewModel { public string? Details { get; set; } }
    public class GetReturnedSlipDetailsViewModel { public string? Details { get; set; } }

    public class OutwardSlipItemViewModel
    {
        public int Id { get; set; }
        public string? SlipNumber { get; set; }
        public DateTime SlipDate { get; set; }
        public string? Status { get; set; }
        public string? CustomerName { get; set; }
        public decimal TotalAmount { get; set; }
        public string? SlipType { get; set; }
    }

    public class ReturnedSlipItemViewModel
    {
        public int Id { get; set; }
        public string? ItemCode { get; set; }
        public string? ItemDescription { get; set; }
        public decimal ItemAmount { get; set; }
        public int Quantity { get; set; }
        public string? Status { get; set; }
    }

    public class ReturnedSlipDetailsViewModel
    {
        public string? SlipDetails { get; set; }
        public decimal TotalAmount { get; set; }
        public List<ReturnedSlipItemViewModel>? Items { get; set; }
    }

    public class OutwardViewModel
    {
        public int Id { get; set; }
        public string? ChequeNumber { get; set; }
        public decimal Amount { get; set; }
        public string? BeneficiaryName { get; set; }
        public string? OutwardNumber { get; set; }
        public DateTime OutwardDate { get; set; }
        public string? Subject { get; set; }
        public string? Description { get; set; }
        public string? Sender { get; set; }
        public string? Recipient { get; set; }
        public DateTime RequestDate { get; set; }
        public string? EmployeeName { get; set; }
        public string? TimeOutType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Status { get; set; }
        public string? ChqNumber { get; set; }
        public DateTime ChqDate { get; set; }
        public string? BankName { get; set; }
        public string? Notes { get; set; }
        public DateTime ChequeDate { get; set; }
        public string? CustomerName { get; set; }
        public string? ReturnStatus { get; set; }
        public DateTime ReturnDate { get; set; }
        public string? RequestNumber { get; set; }
        public string? RejectionReason { get; set; }
        public DateTime IssueDate { get; set; }
        public List<AttachmentViewModel>? OutwardDocuments { get; set; }
        public string? DocumentNumber { get; set; }
        public string? AccountHolderName { get; set; }
        public DateTime HoldDate { get; set; }
        public string? HoldReason { get; set; }
    }

    public class HoldChequeViewModel
    {
        public int ChequeId { get; set; }
        public string? Reason { get; set; }
        public string? ChequeNumber { get; set; }
        public string? BankName { get; set; }
        public string? AccountHolderName { get; set; }
        public decimal Amount { get; set; }
        public string? HoldReason { get; set; }
        public DateTime HoldDate { get; set; }
        public List<OutwardViewModel>? HeldCheques { get; set; }
    }

    public class Out_VerificationDetailsViewModel
    {
        public string? VerificationDetails { get; set; }
        public string? Status { get; set; }
        public string? Description { get; set; }
        public List<TableDataItem>? Items { get; set; }
        public int VerificationId { get; set; }
        public DateTime RequestDate { get; set; }
        public string? RequesterName { get; set; }
    }

    public class Outward { public int Id { get; set; } public string? ChequeNumber { get; set; } }

    public class OutwardChqViewModel
    {
        public int ChequeId { get; set; }
        public string? ChequeNumber { get; set; }
        public decimal Amount { get; set; }
        public string? BeneficiaryName { get; set; }
        public string? Status { get; set; }
        public DateTime IssueDate { get; set; }
        public string? Description { get; set; }
        public List<OutwardViewModel>? Cheques { get; set; }
    }

    public class OutwardIndexViewModel
    {
        public OutwardSearchModel SearchModel { get; set; } = new OutwardSearchModel();
        public List<SAFA_ECC_Core_Clean.Models.Outward_Trans> Transactions { get; set; } = new List<SAFA_ECC_Core_Clean.Models.Outward_Trans>();
    }

    public class OutwardReceiptViewModel { public string? ReceiptDetails { get; set; } }

    public class OutwardReturnedOutwardSlipsViewModel
    {
        public string? SlipDetails { get; set; }
        public List<OutwardSlipItemViewModel>? OutwardSlips { get; set; }
    }

    public class OutwardUpdateViewModel
    {
        public int OutwardId { get; set; }
        public string? Subject { get; set; }
        public string? Sender { get; set; }
        public string? Recipient { get; set; }
        public DateTime OutwardDate { get; set; }
        public string? Description { get; set; }
    }

    public class PenddingOutWordRequestViewModel { public List<OutwardViewModel> PendingRequests { get; set; } = new List<OutwardViewModel>(); }

    public class PospondingChqViewModel { public int ChequeId { get; set; } public DateTime NewDate { get; set; } }

    public class RejectedOutRequestViewModel { public List<OutwardViewModel> RejectedRequests { get; set; } = new List<OutwardViewModel>(); }

    public class RepresentReturnDisViewModel
    {
        public RepresentReturnDisSearchModel SearchCriteria { get; set; } = new RepresentReturnDisSearchModel();
        public List<RepresentReturnDisResultItemViewModel> Results { get; set; } = new List<RepresentReturnDisResultItemViewModel>();
    }

    public class RetunedChequeStatesViewModel
    {
        public string? StateDescription { get; set; }
        public string? ChequeNumber { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? Status { get; set; }
        public List<OutwardViewModel> Cheques { get; set; } = new List<OutwardViewModel>();
    }

    public class ReturnDiscountChqViewModel
    {
        public int ChequeId { get; set; }
        public List<OutwardViewModel> ReturnDiscountChqs { get; set; } = new List<OutwardViewModel>();
    }

    public class ReturnOutwardViewModel
    {
        public int ChequeId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public string? Reason { get; set; }
        public List<ReturnOutwardItemViewModel>? RecentReturns { get; set; }
    }

    public class TimeOutRequestViewModel
    {
        public DateTime RequestDate { get; set; }
        public string? EmployeeName { get; set; }
        public string? TimeOutType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Reason { get; set; }
        public List<OutwardViewModel> TimeOutRequests { get; set; } = new List<OutwardViewModel>();
    }

    public class UpdatePostOutwardViewModel
    {
        public int Id { get; set; }
        public int ChequeId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime OutwardDate { get; set; }
        public string? Recipient { get; set; }
        public string? ReferenceNumber { get; set; }
        public string? Status { get; set; }
    }

    public class UpdateReverseOutwordViewModel { public int ChequeId { get; set; } public string? Status { get; set; } }

    public class OutwardModule { public string? ModuleName { get; set; } }

    public class ReturnOutwardItemViewModel
    {
        public int ReturnId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public string? Reason { get; set; }
        public DateTime ReturnDate { get; set; }
    }

    public class OutwardSearchModel
    {
        public string? ChqSequance { get; set; }
        public string? DrwChqNo { get; set; }
        public string? DrwAcctNo { get; set; }
        public string? BenName { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public decimal? MinAmount { get; set; }
        public decimal? MaxAmount { get; set; }
        public string? Status { get; set; }
    }

    public class RepresentReturnDisSearchModel
    {
        public string? Field1 { get; set; }
        public string? Field2 { get; set; }
    }

    public class RepresentReturnDisResultItemViewModel
    {
        public int Id { get; set; }
        public string? Column1 { get; set; }
        public string? Column2 { get; set; }
        public string? Column3 { get; set; }
    }
}


