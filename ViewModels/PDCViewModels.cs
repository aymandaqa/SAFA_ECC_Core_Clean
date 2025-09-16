namespace SAFA_ECC_Core_Clean.ViewModels.PDCViewModels
{
    public class PDCChequesViewModel { public int Id { get; set; } public int ChequeId { get; set; } public string? ChequeNumber { get; set; } public decimal Amount { get; set; } public DateTime ChequeDate { get; set; } public string? Beneficiary { get; set; } public string? BankName { get; set; } public DateTime DueDate { get; set; } public string? Status { get; set; } public string? PayeeName { get; set; } }
    public class PDCIndexViewModel { public PDCIndexSearchModel SearchModel { get; set; } = new PDCIndexSearchModel(); public List<PDCIndexTransactionViewModel> Transactions { get; set; } = new List<PDCIndexTransactionViewModel>(); }
    public class PDCReportViewModel { public DateTime StartDate { get; set; } public DateTime EndDate { get; set; } public List<PDCChequesViewModel> ReportCheques { get; set; } = new List<PDCChequesViewModel>(); }
    public class PDCViewModel { public int Id { get; set; } public string? ChequeNumber { get; set; } public decimal Amount { get; set; } public DateTime DueDate { get; set; } }
        public class PDCWithdrowViewModel 
    {
        public int ChequeId { get; set; }
        public string? PDCNumber { get; set; }
        public DateTime PDCDate { get; set; }
        public decimal Amount { get; set; }
        public string? BeneficiaryName { get; set; }
        public string? ReasonForWithdrow { get; set; }
        public List<WithdrowHistoryItemViewModel>? WithdrowHistory { get; set; }
    }

    public class WithdrowHistoryItemViewModel
    {
        public string? PDCNumber { get; set; }
        public DateTime WithdrowDate { get; set; }
        public decimal Amount { get; set; }
        public string? BeneficiaryName { get; set; }
        public string? Reason { get; set; }
    }
    public class G1G14ReportItemViewModel
    {
        public DateTime Date { get; set; }
        public string? Description { get; set; }
        public decimal Amount { get; set; }
        public string? Currency { get; set; }
    }

    public class PDC_GET_G1_G14_REPORT_ViewModel { public List<G1G14ReportItemViewModel>? ReportData { get; set; } }
    public class PDC_INWORD_ViewModel { public int ChequeId { get; set; } public string? ChequeNumber { get; set; } }
    public class OutwordDocumentItemViewModel { public int Id { get; set; } public string? DocumentNumber { get; set; } public DateTime DocumentDate { get; set; } public string? Subject { get; set; } public string? Sender { get; set; } public string? Receiver { get; set; } }
    public class PDC_OUTWORD_ViewModel { public string? DocumentNumber { get; set; } public DateTime? DocumentDate { get; set; } public string? Subject { get; set; } public string? Sender { get; set; } public string? Receiver { get; set; } public List<OutwordDocumentItemViewModel>? OutwordDocuments { get; set; } }
    public class PDC_PDCCheques20240429ViewModel { public List<PDCChequesViewModel> Cheques { get; set; } = new List<PDCChequesViewModel>(); }
    public class PDC_ReportViewModel { public string? ReportDetails { get; set; } }
    public class PostDateVerificationViewModel { public int ChequeId { get; set; } public bool IsVerified { get; set; } public string? Field1 { get; set; } public string? Field2 { get; set; } public DateTime? DateInput { get; set; } public string? DropdownSelection { get; set; } public List<VerificationResultItemViewModel>? VerificationResults { get; set; } }
    public class PDCCHQStatusViewModel { public int ChequeId { get; set; } public string? Status { get; set; } }
    public class FaildPostDateVerficationDetailsViewModel { public string? Details { get; set; } public string? DocumentId { get; set; } public DateTime? ActualPostDate { get; set; } public DateTime? ExpectedPostDate { get; set; } public string? ErrorMessage { get; set; } public DateTime? VerificationTimestamp { get; set; } public string? TransactionId { get; set; } public DateTime? VerificationDate { get; set; } public string? Status { get; set; } public string? Reason { get; set; } public string? Notes { get; set; } public List<ChangeLogItemViewModel>? ChangeLog { get; set; } }
    public class AuthrizationCodeViewModel { public string? Code { get; set; } public string? Description { get; set; } public bool IsActive { get; set; } }
    public class OutwardParameterViewModel { public string? ParameterName { get; set; } public string? ParameterValue { get; set; } public string? Description { get; set; } public List<OutwardParameterItemViewModel>? OutwardParameters { get; set; } }
    public class OutwardParameterItemViewModel { public int Id { get; set; } public string? ParameterName { get; set; } public string? ParameterValue { get; set; } public string? Description { get; set; } }
    public class ChequesViewModel { public int ChequeId { get; set; } public string? ChequeNumber { get; set; } public decimal Amount { get; set; } public List<PDCChequesViewModel> Cheques { get; set; } = new List<PDCChequesViewModel>(); }
    public class PDCChequesAuthViewModel { public int ChequeId { get; set; } public bool IsAuthorized { get; set; } }

    public class FailedT24TransactionViewModel { public string? TransactionId { get; set; } public string? ErrorMessage { get; set; } public DateTime TransactionDate { get; set; } public decimal Amount { get; set; } public string? Currency { get; set; } public string? FailureReason { get; set; } }
}




    public class VerificationResultItemViewModel
    {
        public int Id { get; set; }
        public string? Result1 { get; set; }
        public string? Result2 { get; set; }
        public string? Status { get; set; }
    }



    public class PDCIndexSearchModel
    {
        public string? ChqSequance { get; set; }
        public string? DrwChqNo { get; set; }
        public string? DrwAcctNo { get; set; }
        public string? BenName { get; set; }
        public DateTime? FromDueDate { get; set; }
        public DateTime? ToDueDate { get; set; }
        public bool DueToday { get; set; }
        public bool Overdue { get; set; }
        public string? ChequeNumber { get; set; }
        public string? AccountNumber { get; set; }
        public string? BeneficiaryName { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public decimal? MinAmount { get; set; }
        public decimal? MaxAmount { get; set; }
        public string? Status { get; set; }
    }



    public class PDCIndexTransactionViewModel
    {
        public int Serial { get; set; }
        public string? DrwChqNo { get; set; }
        public string? DrwAcctNo { get; set; }
        public decimal Amount { get; set; }
        public string? Currency { get; set; }
        public string? BenName { get; set; }
        public DateTime? TransDate { get; set; }
        public DateTime? DueDate { get; set; }
        public int? CollDays { get; set; }
        public string? Status { get; set; }
    }
