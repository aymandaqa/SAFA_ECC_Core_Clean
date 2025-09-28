using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.SystemViewModels
{
    public class UserManagementViewModel
    {
        public string? UserId { get; set; }

        [Display(Name = "اسم المستخدم")]
        [Required(ErrorMessage = "اسم المستخدم مطلوب")]
        public string? UserName { get; set; }

        [Display(Name = "البريد الإلكتروني")]
        [EmailAddress(ErrorMessage = "صيغة البريد الإلكتروني غير صحيحة")]
        public string? Email { get; set; }

        [Display(Name = "الدور")]
        public string? Role { get; set; }

        [Display(Name = "نشط")]
        public bool IsActive { get; set; }

        public List<UserListItemViewModel>? Users { get; set; }
    }

    public class UserListItemViewModel
    {
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public bool IsActive { get; set; }
    }
}
