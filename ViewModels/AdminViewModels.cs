using SAFA_ECC_Core_Clean.Models;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
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
}

