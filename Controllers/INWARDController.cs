using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SAFA_ECC_Core_Clean.Models;
using SAFA_ECC_Core_Clean.ViewModels.InwardViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Data.SqlClient;
using System.Data;
using System.Xml.Linq;
using System.Globalization;

namespace SAFA_ECC_Core_Clean.Controllers
{
    public class INWARDController : Controller
    {
        private readonly ILogger<INWARDController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly string _applicationId = "1";
        private readonly string _connectionString;

        public INWARDController(ILogger<INWARDController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
            _connectionString = ""; // ConfigurationManager.AppSettings("CONNECTION_STR_DNS") needs to be configured in appsettings.json
        }

        private string GetUserName() => HttpContext.Session.GetString("UserName");
        private string GetBranchID() => HttpContext.Session.GetString("BranchID");
        private string GetComID() => HttpContext.Session.GetString("ComID");
        private int GetUserID() => Convert.ToInt32(HttpContext.Session.GetString("ID"));

        public async Task<string> GetCustomerDeathDate(string customerId)
        {
            string result = "";
            string _result = "";
            string _dateDeath = "";
            string _deathNotDate = "";

            if (string.IsNullOrEmpty(GetUserName()))
            {
                return ""; // Or redirect to login
            }

            try
            {
                // Simplified for conversion, actual T24 integration would be more complex
                // For now, directly call the stored procedure if result is empty
                if (string.IsNullOrEmpty(result))
                {
                    // This part needs proper ADO.NET implementation or EF Core raw SQL query
                    // Example using raw SQL (needs adjustment for actual stored proc call)
                    // result = await _context.Database.ExecuteSqlRawAsync("SELECT [DBO].[GET_CUSTOMER_DEATH_DATE] ({0})", customerId);

                    // Placeholder for now
                    using (SqlConnection conn = new SqlConnection(_connectionString))
                    {
                        await conn.OpenAsync();
                        using (SqlCommand cmd = new SqlCommand("SELECT [DBO].[GET_CUSTOMER_DEATH_DATE] (@CustomerID)", conn))
                        {
                            cmd.Parameters.AddWithValue("@CustomerID", customerId);
                            var deathDate = await cmd.ExecuteScalarAsync();
                            if (deathDate != null) result = deathDate.ToString();
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Get Date of Death: {Message}", ex.Message);
                // _LogSystem.WriteError(_Logg_Message, _ApplicationID, GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, GetUserName(), GetUserName(), "", "", "");
            }

            return result;
        }

        public async Task<IActionResult> TestRes()
        {
            // This function contains direct SQL queries and external service calls.
            // It needs careful conversion to use EF Core or proper ADO.NET with async operations.
            // For now, a simplified version is provided.

            try
            {
                // Simplified for conversion, actual logic needs proper implementation
                // Example of how to interact with a service (placeholder)
                // var WebSvc = new ECC_Handler_SVC.InwardHandlingSVCSoapClient("InwardHandlingSVCSoap");
                // var obj_ = await WebSvc.HandelResponseONUSAsync(inw.ChqSequance, inw.ClrCenter, des, spicalnote, inw.Serial, userid);

                _logger.LogInformation("TestRes function executed.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in TestRes: {Message}", ex.Message);
            }

            return Ok(); // Or return a specific view/result
        }

        // GET: INWORDController
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Indextest(string id)
        {
            return View();
        }

        private static List<TreeNode> FillRecursive(List<Category> flatObjects, int? parentId = null)
        {
            try
            {
                return flatObjects.Where(x => x.Parent_ID.Equals(parentId))
                                  .Select(item => new TreeNode
                                  {
                                      SubMenu_Name_EN = item.SubMenu_Name_EN,
                                      SubMenu_ID = item.SubMenu_ID,
                                      Related_Page_ID = item.Related_Page_ID,
                                      Children = FillRecursive(flatObjects, item.SubMenu_ID)
                                  }).ToList();
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in FillRecursive: {ex.Message}");
                return new List<TreeNode>();
            }
        }

        public async Task<IActionResult> InwardFinanicalWFDetailsPMADIS_NEW(string id)
        {
            HttpContext.Session.SetString("ErrorMessage", "");

            if (string.IsNullOrEmpty(GetUserName()))
            {
                return RedirectToAction("Login", "Login");
            }

            string methodName = "InwardFinanicalWFDetailsPMADIS";
            int userId = GetUserID();

            try
            {
                var appPage = await _context.App_Pages.SingleOrDefaultAsync(t => t.Page_Name_EN == methodName);
                if (appPage == null) return NotFound(); // Or handle appropriately

                int pageId = appPage.Page_Id;
                int applicationId = appPage.Application_ID;

                // Assuming getuser_group_permision is a helper function or service call
                // await getuser_group_permision(pageId, applicationId, userId);

                ViewBag.Title = appPage.ENG_DESC;
                // ViewBag.Tree = GetAllCategoriesForTree(); // Needs implementation

                var wf = await _context.INWARD_WF_Tbl.SingleOrDefaultAsync(z => z.Serial == id && z.Final_Status != "Accept");
                if (wf == null) return RedirectToAction("InsufficientFunds", "INWARD");

                var incObj = await _context.Inward_Trans.SingleOrDefaultAsync(y => y.Serial == id);
                if (incObj == null) return RedirectToAction("InsufficientFunds", "INWARD");

                // Simplified logic for VIP and GUAR_CUSTOMER, actual service calls needed
                ViewBag.is_vip = (incObj.ClrCenter == "PMA" && incObj.VIP == true && GetBranchID() != "2") ? "YES" : "NO";
                ViewBag.GUAR_CUSTOMER = "Not Available"; // Placeholder

                // Account info service call placeholder
                // var EccAccInfo_WebSvc = new ECC_CAP_Services.SAFA_T24_ECC_SVCSoapClient("SAFA_T24_ECC_SVCSoap");
                // var Accobj = await EccAccInfo_WebSvc.ACCOUNT_INFOAsync(incObj.AltAccount, 1);
                // inChq.BookedBalance = Accobj.BookedBalance;
                // inChq.ClearBalance = Accobj.ClearBalance;
                // inChq.AccountStatus = Accobj.AccountStatus;

                var user = await _context.Users_Tbl.SingleOrDefaultAsync(c => c.User_Name == GetUserName());
                string group = user?.Group_ID;

                ViewBag.Reject = "False";
                ViewBag.recomdationbtn = "True";

                if (group == "AdminAuthorized" || GetBranchID() == "2") // Assuming GroupType.Group_Status.AdminAuthorized is a string
                {
                    ViewBag.Approve = "True";
                    ViewBag.recomdationbtn = "False";
                }
                else
                {
                    // Userlevel logic needs to be converted from stored procedure to EF Core or raw SQL
                    // var Userlevel = await _context.USER_Limits_Auth_Amount(userId, Tbl_id, "d", wf.Amount_JD).ToListAsync();
                    ViewBag.recomdationbtn = "True";

                    // AuthTrans_User_TBL logic
                    var authTransUser = await _context.AuthTrans_User_TBL.SingleOrDefaultAsync(t => t.Auth_user_ID == userId && t.Trans_id == "5" && t.group_ID == user.Group_ID);
                    if (authTransUser != null) { /* ... */ }
                }

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in InwardFinanicalWFDetailsPMADIS_NEW: {Message}", ex.Message);
                HttpContext.Session.SetString("ErrorMessage", "An error occurred: " + ex.Message);
                return View("Error"); // Or a specific error view
            }
        }

        public async Task<IActionResult> InwardFinanicalWFDetailsPMADIS_Auth(string id)
        {
            HttpContext.Session.SetString("ErrorMessage", "");

            if (string.IsNullOrEmpty(GetUserName()))
            {
                return RedirectToAction("Login", "Login");
            }

            string methodName = "InwardFinanicalWFDetailsPMADIS";
            int userId = GetUserID();

            try
            {
                var appPage = await _context.App_Pages.SingleOrDefaultAsync(t => t.Page_Name_EN == methodName);
                if (appPage == null) return NotFound();

                int pageId = appPage.Page_Id;
                int applicationId = appPage.Application_ID;

                // await getuser_group_permision(pageId, applicationId, userId);

                ViewBag.Title = appPage.ENG_DESC;
                // ViewBag.Tree = GetAllCategoriesForTree();

                var wf = await _context.INWARD_WF_Tbl.SingleOrDefaultAsync(z => z.Serial == id && z.Final_Status != "Accept");
                if (wf == null) return RedirectToAction("InsufficientFunds", "INWARD");

                var incObj = await _context.Inward_Trans.SingleOrDefaultAsync(y => y.Serial == id);
                if (incObj == null) return RedirectToAction("InsufficientFunds", "INWARD");

                ViewBag.is_vip = (incObj.ClrCenter == "PMA" && incObj.VIP == true && GetBranchID() != "2") ? "YES" : "NO";
                ViewBag.GUAR_CUSTOMER = "Not Available";

                var user = await _context.Users_Tbl.SingleOrDefaultAsync(c => c.User_Name == GetUserName());
                string group = user?.Group_ID;

                ViewBag.Reject = "False";
                ViewBag.recomdationbtn = "True";

                if (group == "AdminAuthorized" || GetBranchID() == "2")
                {
                    ViewBag.Approve = "True";
                    ViewBag.recomdationbtn = "False";
                }
                else
                {
                    ViewBag.recomdationbtn = "True";
                    var authTransUser = await _context.AuthTrans_User_TBL.SingleOrDefaultAsync(t => t.Auth_user_ID == userId && t.Trans_id == "5" && t.group_ID == user.Group_ID);
                    if (authTransUser != null) { /* ... */ }
                }

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in InwardFinanicalWFDetailsPMADIS_Auth: {Message}", ex.Message);
                HttpContext.Session.SetString("ErrorMessage", "An error occurred: " + ex.Message);
                return View("Error");
            }
        }

        public async Task<IActionResult> ReversePostingPMARAM(string id)
        {
            if (string.IsNullOrEmpty(GetUserName()))
            {
                return RedirectToAction("Login", "Login");
            }

            try
            {
                var wf = await _context.INWARD_WF_Tbl.SingleOrDefaultAsync(z => z.Serial == id);
                if (wf == null) return NotFound();

                var incObj = await _context.Inward_Trans.SingleOrDefaultAsync(y => y.Serial == id);
                if (incObj == null) return NotFound();

                // External service call placeholder
                // var WebSvc = new ECC_Handler_SVC.InwardHandlingSVCSoapClient("InwardHandlingSVCSoap");
                // var obj_ = await WebSvc.HandelResponseONUSAsync(incObj.ChqSequance, incObj.ClrCenter, "", "", incObj.Serial, GetUserID().ToString());

                // Update Inward_Trans and INWARD_WF_Tbl
                incObj.posted = 10; // Assuming 10 means reversed
                wf.Final_Status = "Reverse";
                wf.WF_Level_Desc = "Reverse";
                wf.WF_Level_Date = DateTime.Now;
                wf.WF_Level_User = GetUserName();

                await _context.SaveChangesAsync();

                _logger.LogInformation("ReversePostingPMARAM for ID {Id} successful.", id);
                return Json(new { status = "Success", message = "Posting reversed successfully." });
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update error in ReversePostingPMARAM: {Message}", ex.Message);
                return Json(new { status = "Error", message = "Database error occurred." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ReversePostingPMARAM: {Message}", ex.Message);
                return Json(new { status = "Error", message = "An unexpected error occurred." });
            }
        }

        public async Task<IActionResult> Get_Inward_Trans_Details(string id)
        {
            if (string.IsNullOrEmpty(GetUserName()))
            {
                return RedirectToAction("Login", "Login");
            }

            try
            {
                var inwardTrans = await _context.Inward_Trans.SingleOrDefaultAsync(t => t.Serial == id);
                if (inwardTrans == null)
                {
                    return NotFound();
                }

                // Convert to ViewModel if necessary, or return as JSON
                return Json(inwardTrans);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Get_Inward_Trans_Details: {Message}", ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        public async Task<IActionResult> GetAllCategoriesForTree()
        {
            if (string.IsNullOrEmpty(GetUserName()))
            {
                return RedirectToAction("Login", "Login");
            }

            try
            {
                var categories = await _context.Categories.ToListAsync(); // Assuming 'Categories' is a DbSet
                var treeNodes = FillRecursive(categories.Select(c => new Category { SubMenu_ID = c.SubMenu_ID, Parent_ID = c.Parent_ID, SubMenu_Name_EN = c.SubMenu_Name_EN, Related_Page_ID = c.Related_Page_ID }).ToList());
                return Json(treeNodes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllCategoriesForTree: {Message}", ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        public async Task<IActionResult> GetUserGroupPermission(int pageId, int applicationId, int userId)
        {
            if (string.IsNullOrEmpty(GetUserName()))
            {
                return RedirectToAction("Login", "Login");
            }

            try
            {
                // This function needs to be fully converted based on its VB.NET original logic.
                // It likely involves querying permissions tables and setting session variables.
                // Placeholder for now.
                _logger.LogInformation("GetUserGroupPermission called for PageId: {PageId}, AppId: {AppId}, UserId: {UserId}", pageId, applicationId, userId);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetUserGroupPermission: {Message}", ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        public async Task<IActionResult> Get_Inward_Trans_Details_OnUs(string id)
        {
            if (string.IsNullOrEmpty(GetUserName()))
            {
                return RedirectToAction("Login", "Login");
            }

            try
            {
                var inwardTrans = await _context.Inward_Trans.SingleOrDefaultAsync(t => t.Serial == id);
                if (inwardTrans == null)
                {
                    return NotFound();
                }

                // Convert to ViewModel if necessary, or return as JSON
                return Json(inwardTrans);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Get_Inward_Trans_Details_OnUs: {Message}", ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        public async Task<IActionResult> Get_Inward_Trans_Details_OnUs_Json(string id)
        {
            if (string.IsNullOrEmpty(GetUserName()))
            {
                return RedirectToAction("Login", "Login");
            }

            try
            {
                var inwardTrans = await _context.Inward_Trans.SingleOrDefaultAsync(t => t.Serial == id);
                if (inwardTrans == null)
                {
                    return NotFound();
                }

                // Convert to ViewModel if necessary, or return as JSON
                return Json(inwardTrans);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Get_Inward_Trans_Details_OnUs_Json: {Message}", ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}



        public async Task<bool> GetPermission(string id, string page, string groupId)
        {
            List<Users_Permissions> usersPermissionPage = new List<Users_Permissions>();
            List<Groups_Permissions> groupPermissionPage = new List<Groups_Permissions>();
            try
            {
                groupPermissionPage = await _context.Groups_Permissions.Where(x => x.Group_Id == groupId && x.Page_Id == page && (x.Add == true || x.Delete == true || x.Access == true || x.Reverse == true || x.Update == true || x.Post == true) && x.Application_ID == 1 && x.Access == true).ToListAsync();

                if (groupPermissionPage.Count == 0)
                {
                    if (page == "0")
                    {
                        usersPermissionPage = await _context.Users_Permissions.Where(x => x.UserID == id && x.PageID == page && x.Application_ID == 1).ToListAsync();
                    }
                    else
                    {
                        usersPermissionPage = await _context.Users_Permissions.Where(x => x.UserID == id && x.PageID == page && x.Value == true && x.Application_ID == 1 && x.ActionID == 6).ToListAsync();
                    }

                    if (usersPermissionPage.Count == 0)
                    {
                        return false;
                    }
                    else
                    {
                        // Assuming GroupType.Group_Status.AdminAuthorized and SystemAdmin are string constants
                        if (int.TryParse(page, out int pageInt))
                        {
                            if (pageInt >= 1300 && pageInt <= 1400 && groupId == "AdminAuthorized")
                            {
                                return true;
                            }

                            if (pageInt >= 1 && pageInt <= 100 && groupId == "SystemAdmin")
                            {
                                return true;
                            }

                            if (!(pageInt >= 1 && pageInt <= 100) && !(pageInt >= 1300 && pageInt <= 1400))
                            {
                                return true;
                            }
                        }
                    }
                }
                else
                {
                    if (page == "0")
                    {
                        usersPermissionPage = await _context.Users_Permissions.Where(x => x.UserID == id && x.PageID == page && x.Application_ID == 1).ToListAsync();
                        return true;
                    }
                    else
                    {
                        usersPermissionPage = await _context.Users_Permissions.Where(x => x.UserID == id && x.PageID == page && x.Application_ID == 1 && x.ActionID == 6).ToListAsync();
                        if (usersPermissionPage.Count == 0)
                        {
                            return true;
                        }

                        usersPermissionPage = await _context.Users_Permissions.Where(x => x.UserID == id && x.PageID == page && x.Value == true && x.Application_ID == 1).ToListAsync();
                        if (usersPermissionPage.Count > 0)
                        {
                            if (int.TryParse(page, out int pageInt))
                            {
                                if (pageInt >= 1300 && pageInt <= 1400 && groupId == "AdminAuthorized")
                                {
                                    return true;
                                }
                                if (pageInt >= 1 && pageInt <= 100 && groupId == "SystemAdmin")
                                {
                                    return true;
                                }

                                if (!(pageInt >= 1 && pageInt <= 100) && !(pageInt >= 1300 && pageInt <= 1400))
                                {
                                    return true;
                                }
                            }
                        }

                        usersPermissionPage = await _context.Users_Permissions.Where(x => x.UserID == id && x.PageID == page && x.Value == false && x.Application_ID == 1 && x.ActionID == 6).ToListAsync();
                        if (usersPermissionPage.Count > 0)
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetPermission: {Message}", ex.Message);
            }
            return false;
        }



        public bool Ge_t(string x)
        {
            int _step = 90000;
            _step += 400;
            try
            {
                var page = _context.Menu_Items_Tbl.Where(i => i.Parent_ID == x).ToList();
                _step += 10;
                if (page.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Ge_t: {Message}", ex.Message);
                // _LogSystem.WriteLogg(_Logg_Message, _ApplicationID, GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, GetUserName(), GetUserName(), "", "", "");
                // _LogSystem.WriteTraceLogg(_Logg_Message, _ApplicationID, GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, GetUserName(), GetUserName(), "", "", "");
                return false; // Or handle as appropriate
            }
        }

