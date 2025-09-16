namespace SAFA_ECC_Core_Clean.ViewModels
{

}



    public class AdminDashboard2ViewModel
    {
        public string? DashboardName { get; set; }
        public string? Description { get; set; }
        public SystemStatsViewModel? SystemStats { get; set; }
        public List<RecentActivityViewModel>? RecentActivities { get; set; }
        public UserLoginStatsViewModel? UserLoginStats { get; set; }
    }
    public class SystemStatsViewModel
    {
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int TotalGroups { get; set; }
        public int ActiveGroups { get; set; }
        public int TotalCompanies { get; set; }
        public int ActiveCompanies { get; set; }
        public int PendingAuthorizations { get; set; }
        public int TotalTransactions { get; set; }
        public int ProcessedToday { get; set; }
        public int PendingProcessing { get; set; }
        public int ErrorCount { get; set; }
    }
    public class RecentActivityViewModel
    {
        public DateTime Timestamp { get; set; }
        public string? Activity { get; set; }
        public string? UserName { get; set; }
        public string? ActivityType { get; set; }
        public string? Description { get; set; }
        public DateTime? ActivityDate { get; set; }
    }
    public class UserLoginStatsViewModel
    {
        public string? UserName { get; set; }
        public int LoginCount { get; set; }
        public DateTime LastLogin { get; set; }
        public int TodayLogins { get; set; }
        public int ActiveSessions { get; set; }
        public int FailedAttempts { get; set; }
    }
    public class AddUserViewModel
    {
        [Required] public string? UserName { get; set; }
        [Required] public string? Password { get; set; }
        [Required] [Compare("Password")] public string? ConfirmPassword { get; set; }
        [Required] public string? FullName { get; set; }
        [Required] [EmailAddress] public string? Email { get; set; }
        public int CompanyId { get; set; }
        public int GroupId { get; set; }
        public UserRole Role { get; set; }
        public bool IsActive { get; set; }
        public List<Companies_Tbl>? Companies { get; set; }
        public List<Groups_Tbl>? Groups { get; set; }
    }
    public class EditUserViewModel
    {
        public int UserId { get; set; }
        [Required] public string? UserName { get; set; }
        [Required] public string? FullName { get; set; }
        [Required] [EmailAddress] public string? Email { get; set; }
        public int CompanyId { get; set; }
        public int GroupId { get; set; }
        public bool IsDisabled { get; set; }
        public bool IsActive { get; set; }
        public UserRole Role { get; set; }
        public string? NewPassword { get; set; }
        [Compare("NewPassword")] public string? ConfirmNewPassword { get; set; }
        public int RoleId { get; set; }
        public IEnumerable<SelectListItem>? AvailableRoles { get; set; }
        public List<Companies_Tbl>? Companies { get; set; }
        public List<Groups_Tbl>? Groups { get; set; }
    }
    public class AddGroupViewModel
    {
        [Required] public string? GroupName { get; set; }
        public string? GroupDescription { get; set; }
        public int GroupTypeId { get; set; }
        public List<SelectListItem>? GroupTypes { get; set; }
    }

    public enum GroupType
    {
        Admin,
        User
    }
    public class PasswordPolicyViewModel 
    {
        public int Policy_ID { get; set; }
        [Required] public string? PolicyName { get; set; }
        public int MinimumLength { get; set; }
        public int MaxLength { get; set; }
        public bool RequireUppercase { get; set; }
        public bool RequireLowercase { get; set; }
        public bool RequireNumbers { get; set; }
        public bool RequireSpecialChars { get; set; }
        public int PasswordExpiryDays { get; set; }
        public int PasswordHistoryCount { get; set; }
        public int MaxLoginAttempts { get; set; }
        public int LockoutDurationMinutes { get; set; }
        public int RequiredUniqueChars { get; set; }
        public bool RequireDigit { get; set; }
        public bool RequireNonAlphanumeric { get; set; }
        public List<Password_Policies_TBL>? ExistingPolicies { get; set; }
    }
    public class AuditLogsViewModel 
    {
        public List<AuditLogViewModel>? AuditLogs { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public string? UserNameFilter { get; set; }
        public string? ActionFilter { get; set; }
        public DateTime? StartDateFilter { get; set; }
        public DateTime? EndDateFilter { get; set; }
    }

    public class AuditLogViewModel
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string? UserName { get; set; }
        public string? Action { get; set; }
        public string? EntityType { get; set; }
        public string? EntityId { get; set; }
        public string? IpAddress { get; set; }
        public string? Details { get; set; }
        public string? Description { get; set; }
        public string? IPAddress { get; set; }
    }

    public class AddAuthViewModel
    {
        public List<AuthTrans_User_TBL_Auth>? PendingAuthorizations { get; set; }
    }
    public class BackupRestoreViewModel 
    {
        public string? BackupPath { get; set; }
        public IFormFile? RestoreFile { get; set; }
        public List<BackupRestoreLog>? BackupRestoreLogs { get; set; }
    }

    public class BackupRestoreLog
    {
        public DateTime Timestamp { get; set; }
        public string? Type { get; set; }
        public string? Status { get; set; }
        public string? Message { get; set; }
    }

    public class DataExportViewModel
    {
        public DataExportType DataType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? ExportFormat { get; set; }
        public string? ExportPath { get; set; }
    }

    public enum DataExportType
    {
        Users,
        Logs,
        Transactions
    }


    public class EditGroupViewModel { public int GroupId { get; set; } [Required] public string? GroupName { get; set; } public string? Description { get; set; } }
    public class AddgroupPermissionViewModel
    {
        public int GroupId { get; set; }
        public int PermissionId { get; set; }
        public bool CanView { get; set; }
        public bool CanAdd { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
        public IEnumerable<SelectListItem>? AvailableGroups { get; set; }
        public IEnumerable<SelectListItem>? AvailablePermissions { get; set; }
    }
    public class AdminDashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int TotalGroups { get; set; }
        public int ActiveGroups { get; set; }
        public int TotalCompanies { get; set; }
        public int ActiveCompanies { get; set; }
        public int PendingAuthorizations { get; set; }
        public List<Users_Tbl>? RecentUsers { get; set; }
    }
    public class ReportGenerationViewModel { public string? ReportType { get; set; } public DateTime? StartDate { get; set; } public DateTime? EndDate { get; set; } public string? ValidationMessage { get; set; } }
    public class SystemMonitoringViewModel
    {
        public string? ServerName { get; set; }
        public string? CpuUsage { get; set; }
        public string? MemoryUsage { get; set; }
        public string? DiskUsage { get; set; }
        public List<ProcessInfoViewModel>? RunningProcesses { get; set; }
        public string? Uptime { get; set; }
        public string? AvailableMemory { get; set; }
        public string? TotalMemory { get; set; }
        public List<ServiceStatusViewModel>? ServiceStatuses { get; set; }
        public List<ErrorLogViewModel>? RecentErrors { get; set; }
    }

    public class ProcessInfoViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Cpu { get; set; }
        public string? Memory { get; set; }
    }

    public class ServiceStatusViewModel
    {
        public string? ServiceName { get; set; }
        public string? Status { get; set; }
        public string? LastCheck { get; set; }
        public bool IsRunning { get; set; }
        public DateTime? LastUpdated { get; set; }
    }

    public class ErrorLogViewModel
    {
        public DateTime Timestamp { get; set; }
        public string? Message { get; set; }
        public string? Level { get; set; }
    }

    public class AddAuthUserViewModel
    {
        [Required] public string? UserName { get; set; }
        [Required] [EmailAddress] public string? Email { get; set; }
        [Required] public string? Password { get; set; }
        [Required] [Compare("Password")] public string? ConfirmPassword { get; set; }
        public UserRole Role { get; set; }
    }

    public enum UserRole
    {
        Admin,
        User
    }
    public class AdduserPermissionViewModel
    {
        public int UserId { get; set; }
        public int PermissionId { get; set; }
        public bool CanView { get; set; }
        public bool CanAdd { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
        public IEnumerable<SelectListItem>? Users { get; set; }
        public IEnumerable<SelectListItem>? Permissions { get; set; }
    }
    public class UserListViewModel { public List<Users_Tbl>? Users { get; set; } public string? SearchQuery { get; set; } public int PageNumber { get; set; } public int TotalPages { get; set; } }


    public class GroupViewModel
    {
        public int GroupId { get; set; }
        public string? GroupName { get; set; }
        public string? Description { get; set; }
    }

    public class GroupPermissionsViewModel
    {
        public int GroupId { get; set; }
        public string? GroupName { get; set; }
        public List<PermissionViewModel>? Permissions { get; set; }
    }

    public class PermissionViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool IsAssigned { get; set; }
    }
    public class UserPermissionsViewModel
    {
        public string? SearchTerm { get; set; }
        public List<UserPermissionItemViewModel>? Users { get; set; }
    }

    public class UserPermissionItemViewModel
    {
        public string? Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public List<string>? Roles { get; set; }
        public bool IsActive { get; set; }
        public List<SelectListItem>? AvailableRoles { get; set; }
    }

    public class DeleteGroupViewModel
    {
        public int GroupId { get; set; }
        public string? GroupName { get; set; }
    }



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



    public class ChequeViewerSearchModel
    {
        public string? ChequeNumber { get; set; }
        public string? DrawerName { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? BankId { get; set; }
        public decimal? AmountFrom { get; set; }
        public decimal? AmountTo { get; set; }
        public string? Status { get; set; }
    }

    public class ChequeViewerIndexViewModel
    {
        public ChequeViewerSearchModel SearchModel { get; set; } = new ChequeViewerSearchModel();
        public List<SAFA_ECC_Core_Clean.Models.Bank_Tbl> Banks { get; set; } = new List<SAFA_ECC_Core_Clean.Models.Bank_Tbl>();
        public List<ChequeDetails> Cheques { get; set; } = new List<ChequeDetails>();
    }

    public class ChequesSearchViewModel 
    {
        public List<ChequeDetails>? Cheques { get; set; }
    }
    public class ChequeSearchViewModel 
    {
        public string? ChequeNumber { get; set; }
        public string? BankName { get; set; }
        public string? BeneficiaryName { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public List<ChequeDetails>? Cheques { get; set; }
    }
    
    public class PrintChequeDetailsViewModel 
    {
        public string? ChequeNumber { get; set; }
        public DateTime ChequeDate { get; set; }
        public decimal Amount { get; set; }
        public string? PayeeName { get; set; }
        public string? BankName { get; set; }
        public string? AccountNumber { get; set; }
        public string? Status { get; set; }
        public List<ChequeDetails>? Items { get; set; }
    }
    public class ChequeDetailsViewModel
    {
        public string? ChequeNumber { get; set; }
        public DateTime? ChequeDate { get; set; }
        public decimal? Amount { get; set; }
        public string? PayeeName { get; set; }
        public string? BankName { get; set; }
        public string? AccountNumber { get; set; }
        public string? Status { get; set; }
        public string? Notes { get; set; }
        public List<ChequeDetails>? Cheques { get; set; }
    }

    public class ChequeDetails
    {
        public string? ChequeNumber { get; set; }
        public decimal Amount { get; set; }
        public int Id { get; set; }
        public int ChequeId { get; set; }
        public DateTime? ChequeDate { get; set; }
        public DateTime Date { get; set; }
        public string? BeneficiaryName { get; set; }
        public string? Status { get; set; }
        public string? BankName { get; set; }
        public string? BranchName { get; set; }
        public string? AccountNumber { get; set; }
        public DateTime DueDate { get; set; }
        public string? PayeeName { get; set; }
        public string? Description { get; set; }
        public string? Cheque_No { get; set; }
        public string? Drawer_Name { get; set; }
        public DateTime? Trans_Date { get; set; }
        public SAFA_ECC_Core_Clean.Models.Banks_Tbl? Bank { get; set; }
        public List<SAFA_ECC_Core_Clean.Models.Cheque_Images_Link_Tbl>? ChequeImages { get; set; }
        public int Trans_ID { get; set; }
    }
    public class PrintAllOutwardCHQViewModel 
    {
        public SearchCriteria SearchCriteria { get; set; } = new SearchCriteria();
        public List<ChequeDetails> Cheques { get; set; } = new List<ChequeDetails>();
    }

    public class SearchCriteria
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? ChequeNumber { get; set; }
        public string? PayeeName { get; set; }
    }



    public class EditCommissionRecordViewModel { public int Id { get; set; } public DateTime CommissionDate { get; set; } public decimal Amount { get; set; } public int RecordId { get; set; } public decimal NewAmount { get; set; } public string? Reason { get; set; } public string? AgentName { get; set; } public string? Description { get; set; } }
    public class ExcemptionRecordViewModel { public int ExcemptionId { get; set; } public string? Description { get; set; } public decimal Amount { get; set; } public string? EmployeeName { get; set; } public string? Reason { get; set; } public DateTime StartDate { get; set; } public DateTime EndDate { get; set; } }
    public class ExcemptionsDetailsViewModel { public int Id { get; set; } public int ExcemptionId { get; set; } public string? ExcemptionName { get; set; } public DateTime StartDate { get; set; } public DateTime EndDate { get; set; } public string? Status { get; set; } public List<ExcemptedProductItemViewModel>? ExcemptedProducts { get; set; } }
    public class ExcemptedProductItemViewModel { public string? ProductName { get; set; } public string? ProductCode { get; set; } public decimal ExcemptedAmount { get; set; } }
    public class CommissionsIndexViewModel { public decimal TotalCommissions { get; set; } public decimal ActiveCommissions { get; set; } public decimal TotalAmount { get; set; } public List<CommissionRecordViewModel> Commissions { get; set; } = new List<CommissionRecordViewModel>(); }
    public class CommissionsReportViewModel { public DateTime StartDate { get; set; } public DateTime EndDate { get; set; } public decimal TotalCommissions { get; set; } public string? AgentName { get; set; } public string? CommissionType { get; set; } public List<CommissionReportItemViewModel> Commissions { get; set; } = new List<CommissionReportItemViewModel>(); }
    public class AuthorizeCommissionViewModel { public int CommissionId { get; set; } public string? CustomerName { get; set; } public decimal Amount { get; set; } public DateTime CreatedDate { get; set; } public string? Status { get; set; } public string? AuthorizationNotes { get; set; } public bool IsApproved { get; set; } }
    public class CommissionRecordViewModel { public int Id { get; set; } public string? CustomerName { get; set; } public decimal Amount { get; set; } public DateTime Date { get; set; } public string? CommisionType { get; set; } public int CommTypeId { get; set; } public int CurrencyId { get; set; } public string? ChequeSource { get; set; } public string? Description { get; set; } public DateTime? LastUpdate { get; set; } }

    public class CommissionDetailsViewModel
    {
        public int CommissionId { get; set; }
        public DateTime CommissionDate { get; set; }
        public decimal Amount { get; set; }
        public string? Status { get; set; }
        public List<CommissionItemViewModel> Items { get; set; } = new List<CommissionItemViewModel>();
    }

    public class CommissionItemViewModel
    {
        public string? ItemName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }

    public class CommissionReportItemViewModel
    {
        public int Id { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? AgentName { get; set; }
        public string? CommissionType { get; set; }
        public decimal Amount { get; set; }
        public string? Status { get; set; }
    }



    public class CustomerConcentrateViewModel { }
    public class ReportViewModel { }



    public class FinancialAuthViewModel { }



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

    public class ChequeViewerViewModel { public string? ChequeImageBase64 { get; set; } public string? ChequeNumber { get; set; } }
    public class FixedErrorViewModel { public int ErrorId { get; set; } public string? ErrorDescription { get; set; } }

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
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? AccountNumber { get; set; }
        public string? CustomerName { get; set; }
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


    public class IndextestViewModel
    {
        public string? Message { get; set; }
        public List<string> Items { get; set; } = new List<string>();
        public string? InputField { get; set; }
        public List<SharedViewModels.TableDataItem> TableData { get; set; } = new List<SharedViewModels.TableDataItem>();
    }



    public class LoginChangePasswordViewModel 
    {
        public LoginViewModel? Login { get; set; }
        public ChangePasswordViewModel? ChangePassword { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string? CurrentPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string? ConfirmNewPassword { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        public string? Username { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }


