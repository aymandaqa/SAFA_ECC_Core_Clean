using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SAFA_ECC_Core_Clean.ViewModels.SystemViewModels
{
    public class SystemConfigurationViewModel
    {
        public int Id { get; set; }
        [Required] public string? SettingName { get; set; }
        public string? SettingValue { get; set; }
        public string? Description { get; set; }
    }

    public class RoleManagementViewModel
    {
        public int RoleId { get; set; }
        [Required] public string? RoleName { get; set; }
        public string? Description { get; set; }
        public List<PermissionViewModel>? Permissions { get; set; }
    }

    public class PermissionViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool IsAssigned { get; set; }
    }

    public class PermissionSettingsViewModel
    {
        public List<RoleManagementViewModel>? Roles { get; set; }
        public List<PermissionViewModel>? AvailablePermissions { get; set; }
    }
}


