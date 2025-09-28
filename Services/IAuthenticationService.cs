using System.Threading.Tasks;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using SAFA_ECC_Core_Clean.ViewModels.AuthenticationViewModels;
using System.Collections.Generic;
using SAFA_ECC_Core_Clean.Models;

namespace SAFA_ECC_Core_Clean.Services
{
    public interface IAuthenticationService
    {
        List<TreeNode> FillRecursive(List<Category> flatObjects, int? parentId = null);
        Task<UserPagePermissionsResultViewModel> getuser_group_permision(string pageid, string applicationid, string userid, string userName, int userId, string groupId);
        Task<DataTable> Getpage(string page, string userName, int userId);
        Task<bool> getPermission(string id, string _page, string _groupid, string userName, int userId);
        Task<bool> getPermission1(string id, string _page, string _groupid, string userName, int userId);
        Task<bool> Ge_t(string x, string userName, int userId);
        Task<string> GetAllCategoriesForTree(string userName, int userId, string groupId);
        Task<LoginResultViewModel> Login(LoginViewModel model);
        // Add other methods from AuthenticationController.vb here as needed
    }
}
