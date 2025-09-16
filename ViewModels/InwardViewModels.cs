using System;
using System.Collections.Generic;
using SAFA_ECC_Core_Clean.ViewModels.SharedViewModels;

namespace SAFA_ECC_Core_Clean.ViewModels
{
    public class AddEmailViewModel { public string? EmailAddress { get; set; } public string? Subject { get; set; } public string? Body { get; set; } }
    public class EmailListViewModel { public List<string> Emails { get; set; } = new List<string>(); }
    public class FixRetCHQViewModel { public int ChequeId { get; set; } public string? NewStatus { get; set; } }
    public class InitialAcceptChequesViewModel { public List<int> ChequeIds { get; set; } = new List<int>(); }
    public class InwTimeoutViewModel { public int ChequeId { get; set; } public DateTime TimeoutDate { get; set; } }
    public class InwardDateVerificationViewModel { public DateTime VerificationDate { get; set; } public List<string> ChequeNumbers { get; set; } = new List<string>(); }
    public class InwardFinanicalWFDetailsONUSViewModel 
    {
        public string? TransactionNumber { get; set; }
        public DateTime CreationDate { get; set; }
        public string? Status { get; set; }
        public decimal Amount { get; set; }
        public string? Currency { get; set; }
        public string? BankName { get; set; }
        public string? Details { get; set; }
        public List<WorkflowHistoryItemViewModel>? WorkflowHistory { get; set; }
    }

    public class WorkflowHistoryItemViewModel
    {
        public DateTime Date { get; set; }
        public string? Action { get; set; }
        public string? User { get; set; }
        public string? Notes { get; set; }
    }
    public class InwardFixedErrorDetailsONUSViewModel { public string? ErrorDetails { get; set; } }
    public class InwardFixedErrorDetailsViewModel { public string? ErrorDetails { get; set; } }
    public class InwardIndexViewModel 
    {
        public InwardSearchViewModel SearchModel { get; set; } = new InwardSearchViewModel();
        public List<InwardViewModel> InwardCheques { get; set; } = new List<InwardViewModel>();
        public List<SAFA_ECC_Core_Clean.Models.Inward_Trans> Transactions { get; set; } = new List<SAFA_ECC_Core_Clean.Models.Inward_Trans>();
    }
        public class InwardSearchViewModel 
    {
        public string? InwardNumber { get; set; }
        public string? Subject { get; set; }
        public string? Sender { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public List<InwardViewModel>? SearchResults { get; set; }
        public bool SearchPerformed { get; set; }
    }
    public class InwardViewModel 
    {
        public int Id { get; set; }
        public string? ChequeNumber { get; set; }
        public decimal Amount { get; set; }
        public string? BeneficiaryName { get; set; }
        public DateTime InwardDate { get; set; }
        public string? InwardSource { get; set; }
        public string? InwardSubject { get; set; }
    }
    public class Inward_DateVerficationDetailsViewModel { public string? Details { get; set; } }
    public class InwordDateVerificationViewModel { public DateTime VerificationDate { get; set; } }
    public class InwordDetailsViewModel 
    {
        public string? Details { get; set; }
        public int AuthorizationId { get; set; }
        public DateTime AuthorizationDate { get; set; }
        public string? Issuer { get; set; }
        public string? Status { get; set; }
        public string? Notes { get; set; }
        public List<TransactionViewModel>? Transactions { get; set; }
    }
    public class InwardDetailsViewModel
    {
        public int Id { get; set; }
        public string? ReferenceNumber { get; set; }
        public DateTime InwardDate { get; set; }
        public string? Subject { get; set; }
        public string? Sender { get; set; }
        public string? Status { get; set; }
        public List<SharedViewModels.AttachmentViewModel>? Attachments { get; set; }
    }

    public class PMADataViewModel 
    {
        public string? Data { get; set; }
        public int TotalFiles { get; set; }
        public int TodayFiles { get; set; }
        public int PendingFiles { get; set; }
        public int RejectedFiles { get; set; }
        public List<PMAFileViewModel>? Files { get; set; }
    }

    public class PMAFileViewModel
    {
        public int Id { get; set; }
        public string? FileName { get; set; }
        public DateTime UploadDate { get; set; }
        public string? Status { get; set; }
    }
    public class PMADataVerificationViewModel 
    {
        public string? VerificationData { get; set; }
        public string? VerificationStatus { get; set; }
        public string? Notes { get; set; }
        public List<PMADataItemViewModel>? PMADataList { get; set; }
    }

    public class PMADataItemViewModel
    {
        public int Id { get; set; }
        public string? FileName { get; set; }
        public DateTime UploadDate { get; set; }
        public string? Status { get; set; }
        public int PMAId { get; set; }
        public string? ItemCode { get; set; }
        public int Quantity { get; set; }
        public string? VerificationStatus { get; set; }
    }
    public class PostingRestrictionsViewModel 
    {
        public int RestrictionId { get; set; }
        public string? RestrictionName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public List<PostingRestrictionItemViewModel>? PostingRestrictions { get; set; }
    }

    public class PostingRestrictionItemViewModel
    {
        public int RestrictionId { get; set; }
        public string? RestrictionName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
    }
    public class RejectChequeViewModel { public int ChequeId { get; set; } public string? Reason { get; set; } public string? AccountName { get; set; } public decimal Amount { get; set; } public string? RejectReason { get; set; } }
    public class ChequeViewerViewModel { public string? ChequeImageBase64 { get; set; } public string? ChequeNumber { get; set; } }
    public class FixedErrorViewModel { public int ErrorId { get; set; } public string? ErrorDescription { get; set; } }
    public class StoppedChequeDetailsViewModel { public string? ChequeNumber { get; set; } public string? Reason { get; set; } }
    public class InwardCustomerDuesViewModel 
    {
        public string? CustomerName { get; set; }
        public decimal DueAmount { get; set; }
        public string? Account_No { get; set; }
        public decimal Amount { get; set; }
        public string? Cheque_No { get; set; }
        public DateTime? Cheque_Date { get; set; }
        public string? Bank_Name { get; set; }
        public string? Status { get; set; }
        public DateTime? Due_Date { get; set; }
        public int? Serial { get; set; }
    }
    public class InwardDataChequeViewrViewModel { public string? ChequeData { get; set; } }
    public class WFChqStatusViewModel { public int ChequeId { get; set; } public string? Status { get; set; } }
    public class InwardCustomerDuesReportViewModel 
    {
        public List<InwardCustomerDuesViewModel> CustomerDues { get; set; } = new List<InwardCustomerDuesViewModel>();
        public int? BankId { get; set; }
        public List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>? Banks { get; set; }
        public string? Status { get; set; }
        public List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>? Statuses { get; set; }
        public decimal? MinAmount { get; set; }
        public decimal? MaxAmount { get; set; }
    }
    public class InwardDataChequeViewModel { public string? ChequeData { get; set; } }
    public class InwardFinancialWFDetailsPMADISViewModel { public string? Details { get; set; } }
    public class InsufficientFundsViewModel 
    {
        public string? AccountNumber { get; set; }
        public decimal RequiredAmount { get; set; }
        public decimal AvailableAmount { get; set; }
        public int TransactionId { get; set; }
        public decimal Amount { get; set; }
        public string? Status { get; set; }
        public DateTime TransactionDate { get; set; }
        public List<DetailViewModel>? Details { get; set; }
    }

    public class DetailViewModel
    {
        public string? Item { get; set; }
        public string? Value { get; set; }
    }
    public class InwardModule { public string? ModuleName { get; set; } }
    public class ADDEmailViewModel { public string? EmailAddress { get; set; } public string? Subject { get; set; } public string? Body { get; set; } }
    public class PMA_DATAVerficationDetailsViewModel
    {
        public string? Details { get; set; }
        public List<SharedViewModels.TableDataItem>? Items { get; set; }
        public string? DetailField1 { get; set; }
        public string? DetailField2 { get; set; }
        public string? DetailField3 { get; set; }
    }

    public class IndextestViewModel
    {
        public string? Message { get; set; }
        public List<string> Items { get; set; } = new List<string>();
        public string? InputField { get; set; }
        public List<SharedViewModels.TableDataItem> TableData { get; set; } = new List<SharedViewModels.TableDataItem>();
    }


    public class PMADATAVerficationDetailsViewModel
    {
        public string? ReferenceNumber { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerId { get; set; }
        public string? CustomerAddress { get; set; }
        public string? BankName { get; set; }
        public string? BranchName { get; set; }
        public decimal ChequeAmount { get; set; }
        public string? ChequeNumber { get; set; }
        public string? Currency { get; set; }
        public string? Status { get; set; }
        public string? Details { get; set; }
        public string? Field1 { get; set; } // Added based on build error
        public string? Field2 { get; set; } // Added based on build error
        public string? Notes { get; set; } // Added based on build error
        public string? DetailField1 { get; set; } // Added based on build error
        public string? DetailField2 { get; set; } // Added based on build error
        public string? DetailField3 { get; set; } // Added based on build error
    }
}


