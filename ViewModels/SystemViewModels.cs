using SAFA_ECC_Core_Clean.Models;
using System.Collections.Generic;
using SAFA_ECC_Core_Clean.ViewModels;

namespace SAFA_ECC_Core_Clean.ViewModels
{
    public class PermissionSettingsViewModel 
    {
        public int PermissionId { get; set; }
        public string? PermissionName { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public string? RoleName { get; set; }
        public List<PermissionItemViewModel>? Permissions { get; set; }
    }

    public class PermissionItemViewModel
    {
        public int ModuleId { get; set; }
        public string? ModuleName { get; set; }
        public bool CanView { get; set; }
        public bool CanAdd { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
    }

    public class RoleManagementViewModel 
    {
        public int RoleId { get; set; }
        public string? RoleName { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public List<RoleManagementViewModel>? Roles { get; set; }
    }
    public class SystemConfigurationViewModel 
    {
        public string? SettingName { get; set; }
        public string? SettingValue { get; set; }
        public string? Description { get; set; }
        public string? Setting1 { get; set; }
        public string? Setting2 { get; set; }
        public bool IsEnabled { get; set; }
        public List<ChangeLogViewModel>? ChangeLogs { get; set; }
        public string? ApplicationName { get; set; }
        public string? AdminEmail { get; set; }
        public int ItemsPerPage { get; set; }
        public bool EnableRegistration { get; set; }
    }
    public class SystemSettingsViewModel 
    {
        public string? SettingKey { get; set; }
        public string? SettingValue { get; set; }
        public string? Category { get; set; }
        public string? SiteName { get; set; }
        public string? AdminEmail { get; set; }
        public string? PhoneNumber { get; set; }
        public string? CompanyAddress { get; set; }
        public bool AllowRegistration { get; set; }
        public bool MaintenanceMode { get; set; }
        public bool EnableTwoFactorAuth { get; set; }
        public int MinimumPasswordLength { get; set; }
        public bool RequireUppercase { get; set; }
        public bool RequireLowercase { get; set; }
        public bool RequireDigit { get; set; }
        public bool RequireSpecialChars { get; set; }
        public bool RequireNonAlphanumeric { get; set; }
        public int PasswordExpiryDays { get; set; }
        public int PasswordHistoryCount { get; set; }
        public int MaxLoginAttempts { get; set; }
        public int LockoutDurationMinutes { get; set; }
        public bool EnableAccountLockout { get; set; }
        public int SessionTimeoutInMinutes { get; set; }
    }
    public class UserManagementViewModel
    {
        public AddUserViewModel NewUser { get; set; } = new AddUserViewModel();
        public EditUserViewModel EditUser { get; set; } = new EditUserViewModel();
        public List<Users_Tbl> Users { get; set; } = new List<Users_Tbl>();
    }

    public class ChangeLogViewModel { public int Id { get; set; } public DateTime Date { get; set; } public string? User { get; set; } public string? Action { get; set; } public string? Details { get; set; } public string? Description { get; set; } }
    public class UserLoginViewModel { public string? UserName { get; set; } public string? Email { get; set; } public DateTime LoginTime { get; set; } }
}
