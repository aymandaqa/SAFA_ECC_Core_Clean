using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SAFA_ECC_Core_Clean.Services;
using System.Threading.Tasks;
using System.Text.Json;
using SAFA_ECC_Core_Clean.Models;
using System.Collections.Generic;
using System.Data;

namespace SAFA_ECC_Core_Clean.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(ILogger<AuthenticationController> logger, IAuthenticationService authenticationService)
        {
            _logger = logger;
            _authenticationService = authenticationService;
        }

        // The FillRecursive method will be moved to the service layer
        // The Category and TreeNode models are now in SAFA_ECC_Core_Clean.Models

        public async Task<IActionResult> Index()
        {
            // Assuming User.Identity.Name and HttpContext.Session can provide the necessary data
            string userName = User.Identity.Name;
            int userId = 0; // Placeholder, retrieve actual userId if available
            string groupId = HttpContext.Session.GetString("groupid"); // Assuming groupid is stored in session

            // Helper functions to mimic Session.Item behavior


            var treeHtml = await _authenticationService.GetAllCategoriesForTree(userName, userId, groupId);
            ViewBag.Tree = treeHtml;
            return View();
        }

        public async Task<IActionResult> getuser_group_permision(string pageid, string applicationid, string userid)
        {
            string userName = User.Identity.Name;
            int userId = 0; // Placeholder, retrieve actual userId if available
            string groupId = HttpContext.Session.GetString("groupid"); // Assuming groupid is stored in session

            var result = await _authenticationService.getuser_group_permision(pageid, applicationid, userid, userName, userId, groupId);

            // Now, handle the session updates in the controller layer
            HttpContext.Session.SetString("permission_user_Group", System.Text.Json.JsonSerializer.Serialize(result.GroupPermissions));
            HttpContext.Session.SetString("AccessPage", result.AccessPage);
            HttpContext.Session.SetString("pagename", result.Pagename);

            return Json(result); // Or return a specific view/partial view if needed
        }

        public async Task<DataTable> Getpage(string page)
        {
            return await _authenticationService.Getpage(page, User.Identity.Name, 0); // Placeholder for userId
        }

        public async Task<bool> getPermission(string id, string _page, string _groupid)
        {
            return await _authenticationService.getPermission(id, _page, _groupid, User.Identity.Name, 0); // Placeholder for userId
        }

        public async Task<bool> getPermission1(string id, string _page, string _groupid)
        {
            return await _authenticationService.getPermission1(id, _page, _groupid, User.Identity.Name, 0); // Placeholder for userId
        }

        public async Task<bool> Ge_t(string x)
        {
            return await _authenticationService.Ge_t(x, User.Identity.Name, 0); // Placeholder for userId
        }

        // TODO: Continue migrating other actions from AuthenticationController.vb
    }
}




        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _authenticationService.Login(model);

                if (result.Success)
                {
                    // Set session variables based on the result from the service
                    HttpContext.Session.SetString("UserName", result.UserName);
                    HttpContext.Session.SetString("ID", result.UserID);
                    HttpContext.Session.SetString("groupid", result.GroupID);
                    HttpContext.Session.SetString("BranchID", result.BranchID);
                    HttpContext.Session.SetString("BranchName", result.BranchName);
                    HttpContext.Session.SetString("UserType", result.UserType);
                    HttpContext.Session.SetString("ApplicationID", result.ApplicationID);
                    HttpContext.Session.SetString("CompanyID", result.CompanyID);
                    HttpContext.Session.SetString("CompanyName", result.CompanyName);
                    HttpContext.Session.SetString("CompanyBranchID", result.CompanyBranchID);
                    HttpContext.Session.SetString("CompanyBranchName", result.CompanyBranchName);
                    HttpContext.Session.SetString("CompanyBranchType", result.CompanyBranchType);
                    HttpContext.Session.SetString("CompanyBranchAddress", result.CompanyBranchAddress);
                    HttpContext.Session.SetString("CompanyBranchPhone", result.CompanyBranchPhone);
                    HttpContext.Session.SetString("CompanyBranchFax", result.CompanyBranchFax);
                    HttpContext.Session.SetString("CompanyBranchEmail", result.CompanyBranchEmail);
                    HttpContext.Session.SetString("CompanyBranchWebsite", result.CompanyBranchWebsite);
                    HttpContext.Session.SetString("CompanyBranchLogo", result.CompanyBranchLogo);
                    HttpContext.Session.SetString("CompanyBranchCurrency", result.CompanyBranchCurrency);
                    HttpContext.Session.SetString("CompanyBranchCurrencySymbol", result.CompanyBranchCurrencySymbol);
                    HttpContext.Session.SetString("CompanyBranchCurrencyName", result.CompanyBranchCurrencyName);
                    HttpContext.Session.SetString("CompanyBranchCurrencyDecimalPlaces", result.CompanyBranchCurrencyDecimalPlaces);
                    HttpContext.Session.SetString("CompanyBranchCurrencyRate", result.CompanyBranchCurrencyRate);
                    HttpContext.Session.SetString("CompanyBranchCurrencyRateDate", result.CompanyBranchCurrencyRateDate);
                    HttpContext.Session.SetString("CompanyBranchCurrencyRateTime", result.CompanyBranchCurrencyRateTime);
                    HttpContext.Session.SetString("CompanyBranchCurrencyRateUser", result.CompanyBranchCurrencyRateUser);
                    HttpContext.Session.SetString("CompanyBranchCurrencyRateStatus", result.CompanyBranchCurrencyRateStatus);
                    HttpContext.Session.SetString("CompanyBranchCurrencyRateDescription", result.CompanyBranchCurrencyRateDescription);

                    return RedirectToAction("Index", "Home"); // Redirect to home page on successful login
                }
                ModelState.AddModelError(string.Empty, result.Message);
            }
            return View(model);
        }




        public IActionResult LogOff()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Authentication");
        }

