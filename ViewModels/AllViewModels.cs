using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using SAFA_ECC_Core_Clean.Models;
using Microsoft.AspNetCore.Http;

namespace SAFA_ECC_Core_Clean.ViewModels
{



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
        public string? BranchName { get; set; }
        public string? Currency { get; set; }
        public string? DrawerName { get; set; }
        public string? DrawerAccount { get; set; }
        public string? Description { get; set; }
        public string? ImagePath { get; set; }
        public DateTime? ValueDate { get; set; }
        public string? TransactionType { get; set; }
        public string? ClearingHouse { get; set; }
        public string? ReturnReason { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string? BatchNumber { get; set; }
        public int SequenceNumber { get; set; }
        public string? OriginalStatus { get; set; }
        public string? ReferenceNumber { get; set; }
        public string? AdditionalInfo { get; set; }
        public List<ChequeHistoryViewModel>? History { get; set; }
    }

    public class ChequeHistoryViewModel
    {
        public DateTime Date { get; set; }
        public string? Event { get; set; }
        public string? Details { get; set; }
        public string? UserName { get; set; }
    }

    public class ChequeViewModel
    {
        public int Id { get; set; }
        public string? ChequeNumber { get; set; }
        public decimal Amount { get; set; }
        public string? Status { get; set; }
    }

    public class ChequeDetails
    {
        public int Id { get; set; }
        public string? ChequeNumber { get; set; }
        public decimal Amount { get; set; }
        public string? Status { get; set; }
        public string? BankName { get; set; }
        public string? BranchName { get; set; }
        public string? DrawerName { get; set; }
        public string? PayeeName { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public string? Currency { get; set; }
        public string? AccountNumber { get; set; }
        public string? Description { get; set; }
        public string? ImagePath { get; set; }
        public string? TransactionType { get; set; }
        public string? ClearingHouse { get; set; }
        public string? ReturnReason { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string? BatchNumber { get; set; }
        public int SequenceNumber { get; set; }
        public string? OriginalStatus { get; set; }
        public string? ReferenceNumber { get; set; }
        public string? AdditionalInfo { get; set; }
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
        public string? BranchName { get; set; }
        public string? Currency { get; set; }
        public string? DrawerName { get; set; }
        public string? DrawerAccount { get; set; }
        public string? Description { get; set; }
        public string? ImagePath { get; set; }
        public DateTime? ValueDate { get; set; }
        public string? TransactionType { get; set; }
        public string? ClearingHouse { get; set; }
        public string? ReturnReason { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string? BatchNumber { get; set; }
        public int SequenceNumber { get; set; }
        public string? OriginalStatus { get; set; }
        public string? ReferenceNumber { get; set; }
        public string? AdditionalInfo { get; set; }
        public List<ChequeHistoryViewModel>? History { get; set; }
    }

    public class ChequeHistoryViewModel
    {
        public DateTime Date { get; set; }
        public string? Event { get; set; }
        public string? Details { get; set; }
        public string? UserName { get; set; }
    }

    public class ChequeViewModel
    {
        public int Id { get; set; }
        public string? ChequeNumber { get; set; }
        public decimal Amount { get; set; }
        public string? Status { get; set; }
    }

    public class ChequeDetails
    {
        public int Id { get; set; }
        public string? ChequeNumber { get; set; }
        public decimal Amount { get; set; }
        public string? Status { get; set; }
        public string? BankName { get; set; }
        public string? BranchName { get; set; }
        public string? DrawerName { get; set; }
        public string? PayeeName { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public string? Currency { get; set; }
        public string? AccountNumber { get; set; }
        public string? Description { get; set; }
        public string? ImagePath { get; set; }
        public string? TransactionType { get; set; }
        public string? ClearingHouse { get; set; }
        public string? ReturnReason { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string? BatchNumber { get; set; }
        public int SequenceNumber { get; set; }
        public string? OriginalStatus { get; set; }
        public string? ReferenceNumber { get; set; }
        public string? AdditionalInfo { get; set; }
    }



    public class CommissionReportViewModel
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? BankName { get; set; }
        public string? BranchName { get; set; }
        public string? TransactionType { get; set; }
        public List<CommissionDetailViewModel>? CommissionDetails { get; set; }
    }

    public class CommissionDetailViewModel
    {
        public DateTime TransactionDate { get; set; }
        public string? ChequeNumber { get; set; }
        public decimal Amount { get; set; }
        public string? Currency { get; set; }
        public decimal CommissionAmount { get; set; }
        public string? BankName { get; set; }
        public string? BranchName { get; set; }
        public string? TransactionType { get; set; }
    }

    public class CommissionSettingsViewModel
    {
        public int SettingId { get; set; }
        public string? BankName { get; set; }
        public string? TransactionType { get; set; }
        public decimal CommissionRate { get; set; }
        public decimal FixedCommission { get; set; }
        public DateTime EffectiveDate { get; set; }
        public bool IsActive { get; set; }
    }



    public class FacilitiesViewModel
    {
        public List<FacilityItemViewModel>? Facilities { get; set; }
        public string? SearchQuery { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
    }

    public class FacilityItemViewModel
    {
        public int FacilityId { get; set; }
        public string? FacilityName { get; set; }
        public string? FacilityType { get; set; }
        public decimal LimitAmount { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string? Status { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerId { get; set; }
    }

    public class AddFacilityViewModel
    {
        [Required] public string? FacilityName { get; set; }
        [Required] public string? FacilityType { get; set; }
        [Required] public decimal LimitAmount { get; set; }
        [Required] public DateTime ExpiryDate { get; set; }
        public string? CustomerId { get; set; }
        public string? CustomerName { get; set; }
    }

    public class EditFacilityViewModel
    {
        public int FacilityId { get; set; }
        [Required] public string? FacilityName { get; set; }
        [Required] public string? FacilityType { get; set; }
        [Required] public decimal LimitAmount { get; set; }
        [Required] public DateTime ExpiryDate { get; set; }
        public string? Status { get; set; }
        public string? CustomerId { get; set; }
        public string? CustomerName { get; set; }
    }

    public class DeleteFacilityViewModel
    {
        public int FacilityId { get; set; }
        public string? FacilityName { get; set; }
    }

    public class FacilityDetailsViewModel
    {
        public int FacilityId { get; set; }
        public string? FacilityName { get; set; }
        public string? FacilityType { get; set; }
        public decimal LimitAmount { get; set; }
        public decimal UsedAmount { get; set; }
        public decimal AvailableAmount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string? Status { get; set; }
        public string? CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public List<FacilityTransactionViewModel>? Transactions { get; set; }
        public List<FacilityDocumentViewModel>? Documents { get; set; }
    }

    public class FacilityTransactionViewModel
    {
        public int TransactionId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? TransactionType { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
    }

    public class FacilityDocumentViewModel
    {
        public int DocumentId { get; set; }
        public string? DocumentName { get; set; }
        public string? DocumentType { get; set; }
        public string? FilePath { get; set; }
        public DateTime UploadDate { get; set; }
    }

    public class FacilityReportViewModel
    {
        public string? ReportType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? CustomerId { get; set; }
        public string? FacilityType { get; set; }
        public List<FacilityReportItemViewModel>? ReportItems { get; set; }
    }

    public class FacilityReportItemViewModel
    {
        public string? CustomerName { get; set; }
        public string? FacilityName { get; set; }
        public string? FacilityType { get; set; }
        public decimal LimitAmount { get; set; }
        public decimal UsedAmount { get; set; }
        public decimal AvailableAmount { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string? Status { get; set; }
    }



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


