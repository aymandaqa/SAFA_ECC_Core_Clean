using SAFA_ECC_Core_Clean.Models;

namespace SAFA_ECC_Core_Clean.ViewModels.UsersViewModels
{

    public class AddUserPermissionViewModel { public int UserId { get; set; } public int PermissionId { get; set; } public List<SAFA_ECC_Core_Clean.Models.Users_Tbl> Users { get; set; } = new List<SAFA_ECC_Core_Clean.Models.Users_Tbl>(); public List<SAFA_ECC_Core_Clean.Models.Permissions_Tbl> Permissions { get; set; } = new List<SAFA_ECC_Core_Clean.Models.Permissions_Tbl>(); }

    public class UserIndexViewModel 
    {
        public UserSearchModel SearchModel { get; set; } = new UserSearchModel();
        public List<SAFA_ECC_Core_Clean.Models.Companies_Tbl> Companies { get; set; } = new List<SAFA_ECC_Core_Clean.Models.Companies_Tbl>();
        public List<SAFA_ECC_Core_Clean.Models.Groups_Tbl> Groups { get; set; } = new List<SAFA_ECC_Core_Clean.Models.Groups_Tbl>();
        public List<SAFA_ECC_Core_Clean.Models.Users_Tbl> Users { get; set; } = new List<SAFA_ECC_Core_Clean.Models.Users_Tbl>();
    }

    public class UserSearchModel
    {
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public int? CompanyId { get; set; }
        public int? GroupId { get; set; }
        public bool? IsDisabled { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsLockedOut { get; set; }
    }

    public class DeleteUserViewModel
    {
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
    }
}