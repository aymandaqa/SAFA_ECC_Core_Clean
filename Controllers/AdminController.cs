using System; 
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SAFA_ECC_Core_Clean.Data;
using SAFA_ECC_Core_Clean.Models;
using SAFA_ECC_Core_Clean.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SAFA_ECC_Core_Clean.Services;
using SAFA_ECC_Core_Clean.ViewModels.AdminViewModels;

namespace SAFA_ECC_Core_Clean.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet]
        public async Task<JsonResult> _getPermission(int user, int app)
        {
            var permissions = await _adminService.GetPermission(user, app);
            return Json(permissions);
        }

        [HttpPost]
        public async Task<JsonResult> Logout_u(string id)
        {
            var userName = HttpContext.Session.GetString("UserName");
            var result = await _adminService.LogoutUser(id, userName);
            return Json(new { success = result, message = result ? "Remove Login Session Done" : "Error logging out user" });
        }

        [HttpPost]
        public async Task<JsonResult> saveAuthUser(SaveAuthUserViewModel model)
        {
            var currentUserName = HttpContext.Session.GetString("UserName");
            var currentUserId = HttpContext.Session.GetInt32("UserID") ?? 0; // Assuming UserID is stored in session

            if (string.IsNullOrEmpty(currentUserName))
            {
                return Json(new { success = false, message = "User not logged in." });
            }

            var result = await _adminService.SaveAuthUser(model, currentUserName, currentUserId);
            return Json(new { success = result, message = result ? "Authorization saved successfully." : "Error saving authorization." });
        }

        [HttpGet]
        public async Task<JsonResult> GetAccountInfo(string accountNo, string benName, string currency)
        {
            var userName = HttpContext.Session.GetString("UserName");
            if (string.IsNullOrEmpty(userName))
            {
                return Json(new { ErrorMsg = "User not logged in.", Status = "Faild" });
            }

            var result = await _adminService.GetAccountInfo(accountNo, benName, currency, userName);
            return Json(result);
        }

        // TODO: Move Accept_Reject_user_Permission, Accept_Reject_Finanical_Site, Accept_Reject_Group_Permission to AdminService
        [HttpGet]
        public async Task<IActionResult> GroupPermission(int group, int app)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserName")))
            {
                return RedirectToAction("Login", "Login");
            }

            var userName = HttpContext.Session.GetString("UserName");
            var result = await _adminService.GetGroupPermission(group, app, userName);
            return Json(result);
        }
    }
}
