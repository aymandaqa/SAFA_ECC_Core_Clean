using System;
using System.Collections.Generic;

namespace SAFA_ECC_Core_Clean.ViewModels
{
    public class OutwordDetailsViewModel
    {
        public int AuthorizationId { get; set; }
        public string? AuthorizationNumber { get; set; }
        public DateTime AuthorizationDate { get; set; }
        public string? Status { get; set; }
        public string? AuthorizedBy { get; set; }
        public string? AuthorizedTo { get; set; }
        public List<TransactionViewModel>? Transactions { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Notes { get; set; }
    }

    public class TransactionViewModel
    {
        public int? TransactionId { get; set; }
        public string? TransactionNumber { get; set; }
        public string? Type { get; set; }
        public decimal Amount { get; set; }
        public string? Currency { get; set; }
        public string? Description { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? Status { get; set; }
    }

    public class Pendding_OutWord_Request1ViewModel 
    {
        public int RequestId { get; set; }
        public DateTime RequestDate { get; set; }
        public string? RequestStatus { get; set; }
        public string? RequesterName { get; set; }
        public string? Department { get; set; }
        public string? Subject { get; set; }
        public List<AuthorizationDetailViewModel>? AuthorizationDetails { get; set; }
        public List<TransactionDetailViewModel>? TransactionDetails { get; set; }
    }

    public class AuthorizationDetailViewModel
    {
        public string? ApproverName { get; set; }
        public string? ApprovalStatus { get; set; }
        public DateTime ApprovalDate { get; set; }
        public string? Comments { get; set; }
    }

    public class TransactionDetailViewModel
    {
        public int TransactionId { get; set; }
        public string? TransactionType { get; set; }
        public decimal Amount { get; set; }
        public string? Currency { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
    }

    public class PDCDetailsViewModel 
    {
        public int AuthorizationId { get; set; }
        public DateTime AuthorizationDate { get; set; }
        public string? AuthorizationStatus { get; set; }
        public decimal TotalAmount { get; set; }
        public string? GrantingEntity { get; set; }
        public List<TransactionViewModel>? Transactions { get; set; }
    }
    public class PenddingRequestViewModel 
    {
        public string? UserName { get; set; }
        public DateTime RequestDate { get; set; }
        public string? AuthorizationType { get; set; }
        public string? Status { get; set; }
        public string? Notes { get; set; }
        public List<TransactionViewModel>? Transactions { get; set; }
    }
    public class RejectedRequestViewModel 
    {
        public int RequestId { get; set; }
        public DateTime RequestDate { get; set; }
        public string? RequestType { get; set; }
        public string? RejectionReason { get; set; }
        public string? RejectionDetails { get; set; }
        public List<TransactionDetailViewModel>? TransactionDetails { get; set; }
        public string? Status { get; set; }
    }
}



