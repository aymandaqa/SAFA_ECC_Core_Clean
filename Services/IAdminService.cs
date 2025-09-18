using System.Collections.Generic;
using System.Threading.Tasks;
using SAFA_ECC_Core_Clean.ViewModels.AdminViewModels;

namespace SAFA_ECC_Core_Clean.Services
{
    public interface IAdminService
    {
        Task<List<UserPermissionViewModel>> GetUserPermissions(int userId, int appId, string userName, int currentUserId);
        Task<bool> AcceptRejectUserPermission(string id, string status, string app, string page, string userName, int currentUserId);
        Task<bool> AcceptRejectFinancialSite(string id, string status, string userName, int currentUserId);
        Task<bool> AcceptRejectGroupPermission(string id, string status, string app, string page, string userName, int currentUserId);
        Task<bool> SaveAuthUser(SaveAuthUserViewModel model, string currentUserName, int currentUserId);
        Task<bool> LogoutUser(string id, string userName);
        Task<List<SAFA_ECC_Core_Clean.Models.Users_Permissions>> GetPermission(int user, int app);
        Task<AccountInfoResponseViewModel> GetAccountInfo(string accountNo, string benName, string currency, string userName);
        Task<List<SAFA_ECC_Core_Clean.Models.Groups_Permissions>> GetGroupPermission(int group, int app, string userName);
        // Add other methods as identified from AdminController.vb
    }
}

