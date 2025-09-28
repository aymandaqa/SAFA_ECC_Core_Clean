using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.SystemViewModels
{
    public class RoleManagementViewModel
    {
        public string? RoleId { get; set; }

        [Display(Name = "اسم الدور")]
        [Required(ErrorMessage = "اسم الدور مطلوب")]
        public string? RoleName { get; set; }

        [Display(Name = "الوصف")]
        public string? Description { get; set; }

        public List<PermissionViewModel>? Permissions { get; set; }
    }

    public class PermissionViewModel
    {
        public string? PermissionId { get; set; }
        public string? PermissionName { get; set; }
        public bool HasPermission { get; set; }
    }
}
