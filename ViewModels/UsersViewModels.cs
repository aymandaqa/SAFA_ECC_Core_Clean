using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using SAFA_ECC_Core_Clean.Models;

namespace SAFA_ECC_Core_Clean.ViewModels.UsersViewModels
{
    public class UserIndexViewModel
    {
        public List<Users_Tbl>? Users { get; set; }
        public string? SearchQuery { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
    }

    public class UserDetailsViewModel
    {
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? CompanyName { get; set; }
        public string? GroupName { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public List<UserActivityLogViewModel>? ActivityLogs { get; set; }
    }

    public class UserActivityLogViewModel
    {
        public DateTime Timestamp { get; set; }
        public string? Activity { get; set; }
        public string? Description { get; set; }
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
        public bool IsActive { get; set; }
        public IEnumerable<SelectListItem>? Companies { get; set; }
        public IEnumerable<SelectListItem>? Groups { get; set; }
    }

    public class EditUserViewModel
    {
        public int UserId { get; set; }
        [Required] public string? UserName { get; set; }
        [Required] public string? FullName { get; set; }
        [Required] [EmailAddress] public string? Email { get; set; }
        public int CompanyId { get; set; }
        public int GroupId { get; set; }
        public bool IsActive { get; set; }
        public string? NewPassword { get; set; }
        [Compare("NewPassword")] public string? ConfirmNewPassword { get; set; }
        public IEnumerable<SelectListItem>? Companies { get; set; }
        public IEnumerable<SelectListItem>? Groups { get; set; }
    }

    public class DeleteUserViewModel
    {
        public int UserId { get; set; }
        public string? UserName { get; set; }
    }

    public class UserProfileViewModel
    {
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? CompanyName { get; set; }
        public string? GroupName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
    }

    public class ChangeUserPasswordViewModel
    {
        public int UserId { get; set; }
        [Required] public string? OldPassword { get; set; }
        [Required] public string? NewPassword { get; set; }
        [Required] [Compare("NewPassword")] public string? ConfirmNewPassword { get; set; }
    }
}


