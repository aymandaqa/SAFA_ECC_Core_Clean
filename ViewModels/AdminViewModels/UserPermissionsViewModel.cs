using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.AdminViewModels
{
    public class UserPermissionsViewModel
    {
        [Display(Name = "معرف المستخدم")]
        public string? UserId { get; set; }

        [Display(Name = "اسم المستخدم")]
        public string? UserName { get; set; }

        [Display(Name = "المجموعة")]
        public string? GroupName { get; set; }

        public List<PermissionItemViewModel>? Permissions { get; set; }
    }

    public class PermissionItemViewModel
    {
        public int PermissionId { get; set; }
        public string? PermissionName { get; set; }
        public bool HasPermission { get; set; }
    }
}
