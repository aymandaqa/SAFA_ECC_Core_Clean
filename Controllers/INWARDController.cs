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



        public string GetList(string x)
        {
            int _step = 90000;
            _step += 600;
            try
            {
                if (HttpContext.Session.GetString("treeitem") == null)
                {
                    HttpContext.Session.SetString("treeitem", "");
                }

                string treeItem = HttpContext.Session.GetString("treeitem");
                string[] array_list = treeItem.Split(':');

                if (!array_list.Contains(x))
                {
                    string y = treeItem + x;
                    HttpContext.Session.SetString("treeitem", y + ":");
                }
                else
                {
                    HttpContext.Session.SetString("treeitem", treeItem.Replace(x, ""));
                }
                return HttpContext.Session.GetString("treeitem");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetList: {Message}", ex.Message);
                // _LogSystem.WriteLogg("Error in GetList", _ApplicationID, GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, GetUserName(), GetUserName(), "", "", "");
                // _LogSystem.WriteTraceLogg("Error in GetList", _ApplicationID, GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, GetUserName(), GetUserName(), "", "", "");
                return ""; // Or handle error appropriately
            }
        }



        public async Task<IActionResult> GetUserGroupPermission(string pageid, string applicationid, string userid)
        {
            int _step = 10000;
            _step += 1700;
            HttpContext.Session.SetString("permission_user_Group", ""); // Equivalent to Nothing
            HttpContext.Session.SetString("AccessPage", "");
            string _groupid = HttpContext.Session.GetString("groupid");

            try
            {
                List<USER_PAGE_PERMISSIONS_Result> group_permission = new List<USER_PAGE_PERMISSIONS_Result>();
                // Assuming SAFA_ECCEntities is replaced by _context
                string pagename = "";
                Menu_Items_Tbl Menu_Items = await _context.Menu_Items_Tbl.SingleOrDefaultAsync(c => c.Related_Page_ID == pageid);

                if (Menu_Items != null)
                {
                    pagename = Menu_Items.SubMenu_Name_EN;
                }

                HttpContext.Session.SetString("pagename", pagename);

                // This part needs to be converted from stored procedure call to EF Core query or raw SQL
                // For now, it's a placeholder.
                // group_permission = await _context.USER_PAGE_PERMISSIONS(applicationid, userid, pageid).ToListAsync();

                _step += 10;
                if (group_permission.Count > 0)
                {
                    Users_Tbl user = await _context.Users_Tbl.SingleOrDefaultAsync(x => x.User_ID == userid);
                    _step += 10;
                    if (user != null)
                    {
                        string page = "";
                        App_Pages _PAGE = await _context.App_Pages.SingleOrDefaultAsync(x => x.Page_Id == pageid);

                        if (_PAGE != null)
                        {
                            page = _PAGE.Other_Details;
                        }

                        foreach (var item in group_permission)
                        {
                            if (item.ACCESS == true)
                            {
                                // Assuming GroupType.Group_Status.AdminAuthorized and SystemAdmin are constants or enum values
                                // Need to convert pageid to int for comparison
                                int pageIdInt = int.Parse(pageid);

                                if (pageIdInt >= 1300 && pageIdInt <= 1400 && _groupid == "AdminAuthorized") // Placeholder for GroupType.Group_Status.AdminAuthorized
                                {
                                    HttpContext.Session.SetString("AccessPage", "Access");
                                }
                                else if (pageIdInt >= 1 && pageIdInt <= 100 && _groupid == "SystemAdmin") // Placeholder for GroupType.Group_Status.SystemAdmin
                                {
                                    HttpContext.Session.SetString("AccessPage", "Access");
                                }
                                else if (!(pageIdInt >= 1 && pageIdInt <= 100) && !(pageIdInt >= 1300 && pageIdInt <= 1400))
                                {
                                    HttpContext.Session.SetString("AccessPage", "Access");
                                }
                                else
                                {
                                    HttpContext.Session.SetString("AccessPage", "NoAccess");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in getuser_group_permision: {Message}", ex.Message);
                // _LogSystem.WriteLogg(_Logg_Message, _ApplicationID, GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, GetUserName(), GetUserName(), "", "", "");
                // _LogSystem.WriteError(_Logg_Message, _step, _ApplicationID, GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, GetUserName(), GetUserName(), "", "", "");
            }
            return Ok(); // Or appropriate IActionResult
        }




        public async Task<IActionResult> Pendding_ONUS_Request()
        {
            int _step = 90000;
            _step += 800;
            HttpContext.Session.SetString("ErrorMessage", "");

            _logger.LogInformation("start Pendding_ONUS_Request /INWARDController");
            // _LogSystem.WriteTraceLogg(_Logg_Message, _ApplicationID, GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, GetUserName(), GetUserName(), "", "", "");

            // ViewBag.Tree = GetAllCategoriesForTree(); // This needs to be an async call if GetAllCategoriesForTree is async

            if (string.IsNullOrEmpty(GetUserName()))
            {
                return RedirectToAction("Login", "Login");
            }

            try
            {
                _logger.LogInformation("Pendding_ONUS_Request /INWARDController / INWARDController");
                // _LogSystem.WriteTraceLogg(_Logg_Message, _ApplicationID, GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, GetUserName(), GetUserName(), "", "", "");

                string methodName = "Pendding_ONUS_Request"; // Assuming GetMethodName() returns the current action name
                _step += 1;
                HttpContext.Session.SetString("methodName", methodName);

                int pageid;
                int applicationid;
                int userid = GetUserID();

                var appPage = await _context.App_Pages.SingleOrDefaultAsync(t => t.Page_Name_EN == methodName);
                if (appPage == null) return NotFound();

                pageid = appPage.Page_Id;
                _step += 1;

                HttpContext.Session.SetInt32("page_id", pageid);

                string title = appPage.ENG_DESC;
                _step += 1;

                ViewBag.Title = title;
                applicationid = appPage.Application_ID;
                _step += 1;

                // Assuming getuser_group_permision is now GetUserGroupPermission and is async
                await GetUserGroupPermission(pageid, applicationid, userid);

                if (HttpContext.Session.GetString("AccessPage") == "NoAccess")
                {
                    return RedirectToAction("block", "Login");
                }
                _step += 1;

                // Logic for Auth_request and TBL needs to be converted
                // Dim Auth_request As New List(Of Auth_Tran_Details_TBL)
                // Dim TBL As New List(Of Auth_Tran_Details_TBL)

                // Placeholder for actual data retrieval
                var authRequests = await _context.Auth_Tran_Details_TBL
                                                 .Where(a => a.Status == "Pending" && a.Trans_id == "1") // Assuming Trans_id for ONUS Request is "1"
                                                 .ToListAsync();

                return View(authRequests);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Pendding_ONUS_Request: {Message}", ex.Message);
                HttpContext.Session.SetString("ErrorMessage", "An error occurred: " + ex.Message);
                return View("Error");
            }
        }




        [HttpPost]
        public async Task<IActionResult> InwordDateVerfication(string AccNo)
        {
            if (string.IsNullOrEmpty(GetUserName()))
            {
                return RedirectToAction("Login", "Login");
            }
            HttpContext.Session.SetString("ErrorMessage", "");
            int _step = 90000;
            _step += 1;

            // Session.Add("access", Nothing); // This seems to be a VB.NET specific session management, needs review

            string methodName = GetMethodName(); // Assuming GetMethodName() is implemented elsewhere
            _step += 1;
            HttpContext.Session.Remove("locked");
            HttpContext.Session.Remove("loked_user");

            if (string.IsNullOrEmpty(HttpContext.Session.GetString("methodName")))
            {
                HttpContext.Session.SetString("methodName", methodName);
            }

            // Group_Types_Tbl group_name = new Group_Types_Tbl(); // Not used
            int applicationId;
            int userId = GetUserID();
            // SAFA_ECCEntities _CAB = new SAFA_ECCEntities(); // Replaced by _context
            int pageId;
            try
            {
                // This try-catch block seems to be inverted in the original VB.NET code.
                // The catch block contains logic that should be in the try block.
                // Assuming the intent is to get pageId and applicationId, then check permissions.
                var appPage = await _context.App_Pages.SingleOrDefaultAsync(t => t.Page_Name_EN == methodName);
                if (appPage == null) return NotFound(); // Or handle appropriately

                pageId = appPage.Page_Id;
                _step += 1;
                applicationId = appPage.Application_ID;
                _step += 1;

                // Assuming getuser_group_permision is a helper function or service call
                // await getuser_group_permision(pageId.ToString(), applicationId.ToString(), userId.ToString());
                // For now, calling the C# version directly
                await GetUserGroupPermission(pageId, applicationId, userId);

                if (HttpContext.Session.GetString("AccessPage") == "NoAccess")
                {
                    return RedirectToAction("block", "Login");
                }
                _step += 1;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in InwordDateVerfication (AccNo): {Message}", ex.Message);
                HttpContext.Session.SetString("ErrorMessage", "An error occurred: " + ex.Message);
                return View("Error"); // Or a specific error view
            }

            return View(); // Placeholder, actual view needs to be determined
        }

        public async Task<IActionResult> InwordDateVerfication()
        {
            HttpContext.Session.SetString("ErrorMessage", "");
            if (string.IsNullOrEmpty(GetUserName()))
            {
                return RedirectToAction("Login", "Login");
            }
            // Session.Add("access", Nothing); // Needs review

            // The rest of the logic for this overload needs to be extracted and converted.
            // For now, returning a placeholder view.
            return View();
        }

        // Helper to get method name (similar to VB.NET's GetMethodName)
        private string GetMethodName()
        {
            return ControllerContext.RouteData.Values["action"].ToString();
        }



        public async Task<IActionResult> InwardFixederrorDetailsONUS(string id)
        {
            HttpContext.Session.SetString("ErrorMessage", "");

            if (string.IsNullOrEmpty(GetUserName()))
            {
                return RedirectToAction("Login", "Login");
            }

            string methodName = GetMethodName();
            List<Return_Codes_Tbl> retDescList = new List<Return_Codes_Tbl>();
            string clrCenter = "";

            int userId = GetUserID();

            try
            {
                var appPage = await _context.App_Pages.SingleOrDefaultAsync(t => t.Page_Name_EN == methodName);
                if (appPage == null) return NotFound();

                int pageId = appPage.Page_Id;
                int applicationId = appPage.Application_ID;

                // Assuming getuser_group_permision is a helper function or service call
                // await GetUserGroupPermission(pageId, applicationId, userId);

                ViewBag.Title = appPage.ENG_DESC;
                // ViewBag.Tree = GetAllCategoriesForTree(); // Needs implementation

                clrCenter = "DISCOUNT";
                retDescList = await _context.Return_Codes_Tbl.Where(i => i.ClrCenter != clrCenter).ToListAsync();
                ViewBag.Ret_Desc = retDescList;

                var inChq = new OnusChqs(); // Assuming OnusChqs is a ViewModel
                var img = new OnUs_Imgs();
                var incObj = new OnUs_Tbl();
                List<CURRENCY_TBL> currencyList = new List<CURRENCY_TBL>();

                _logger.LogInformation("Show Cheque fixed error it from ONUS table");

                incObj = await _context.OnUs_Tbl.SingleOrDefaultAsync(y => y.Serial == id && (y.Posted == (int)AllEnums.Cheque_Status.New || y.Posted == (int)AllEnums.Cheque_Status.Posted || y.Posted == (int)AllEnums.Cheque_Status.Rejected));
                currencyList = await _context.CURRENCY_TBL.ToListAsync();

                if (incObj == null)
                {
                    return RedirectToAction("FixedError", "INWARD");
                }

                foreach (var currency in currencyList)
                {
                    if (incObj.Currency == "1" || incObj.Currency == "2" || incObj.Currency == "3" || incObj.Currency == "5")
                    {
                        if (Convert.ToInt32(incObj.Currency) == currency.ID)
                        {
                            incObj.Currency = currency.SYMBOL_ISO;
                            break;
                        }
                    }
                }

                ViewBag.data = currencyList;
                img = await _context.OnUs_Imgs.FirstOrDefaultAsync(y => y.Serial == incObj.Serial);

                if (img == null)
                {
                    try
                    {
                        var onusimg = await _context.Cheque_Images_Link_Tbl.SingleOrDefaultAsync(v => v.Serial == incObj.Serial && v.Cheque_ype == "OnUs");
                        if (onusimg == null)
                        {
                            var postimg = await _context.Cheque_Images_Link_Tbl.SingleOrDefaultAsync(v => v.Serial == incObj.PDC_Serial && v.Cheque_ype == "PDC");
                            if (postimg != null)
                            {
                                onusimg = new Cheque_Images_Link_Tbl
                                {
                                    Serial = incObj.Serial,
                                    Cheque_ype = "OnUs",
                                    ImageSerial = postimg.ImageSerial,
                                    ChqSequance = incObj.ChqSequance,
                                    TransDate = DateTime.Now
                                };
                                _context.Cheque_Images_Link_Tbl.Add(onusimg);
                                await _context.SaveChangesAsync();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error creating OnUs image link: {Message}", ex.Message);
                    }
                    img = await _context.OnUs_Imgs.FirstOrDefaultAsync(y => y.Serial == incObj.Serial);
                }

                incObj.Amount = Math.Round(incObj.Amount, 2, MidpointRounding.AwayFromZero);
                inChq.onus = incObj;
                inChq.Imgs = img;

                return View(inChq);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in InwardFixederrorDetailsONUS: {Message}", ex.Message);
                HttpContext.Session.SetString("ErrorMessage", "An error occurred: " + ex.Message);
                return View("Error");
            }
        }

        private string GetMethodName([System.Runtime.CompilerServices.CallerMemberName] string memberName = null)
        {
            return memberName;
        }


        public async Task<IActionResult> InwardFixederrorDetailsPMADIS(string id)
        {
            if (string.IsNullOrEmpty(GetUserName()))
            {
                return RedirectToAction("Login", "Login");
            }

            HttpContext.Session.SetString("ErrorMessage", "");

            string methodName = GetMethodName();
            int userId = GetUserID();

            try
            {
                var appPage = await _context.App_Pages.SingleOrDefaultAsync(t => t.Page_Name_EN == methodName);
                if (appPage == null) return NotFound();

                int pageId = appPage.Page_Id;
                int applicationId = appPage.Application_ID;

                // await GetUserGroupPermission(pageId, applicationId, userId);

                ViewBag.Title = appPage.ENG_DESC;
                // ViewBag.Tree = GetAllCategoriesForTree();

                string clrCenter = "";
                List<Return_Codes_Tbl> retDescList = new List<Return_Codes_Tbl>();

                var inChq = new INChqs(); // Assuming INChqs is a ViewModel
                var img = new INWARD_IMAGES();
                var incObj = new Inward_Trans();
                List<CURRENCY_TBL> currencyList = new List<CURRENCY_TBL>();

                _logger.LogInformation("Show Cheque fixed error it from Inward_Trans table");

                incObj = await _context.Inward_Trans.SingleOrDefaultAsync(y => y.Serial == id);

                if (incObj.ClrCenter == "PMA")
                {
                    clrCenter = "PMA";
                    retDescList = await _context.Return_Codes_Tbl.Where(i => i.ClrCenter != clrCenter).ToListAsync();
                }
                else
                {
                    clrCenter = "DISCOUNT";
                    retDescList = await _context.Return_Codes_Tbl.Where(i => i.ClrCenter == clrCenter).ToListAsync();
                }
                ViewBag.Ret_Desc = retDescList;

                currencyList = await _context.CURRENCY_TBL.ToListAsync();

                if (incObj == null)
                {
                    return RedirectToAction("FixedError", "INWARD");
                }

                // Further logic for image and inChq population would go here

                return View(inChq);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in InwardFixederrorDetailsPMADIS: {Message}", ex.Message);
                HttpContext.Session.SetString("ErrorMessage", "An error occurred: " + ex.Message);
                return View("Error");
            }
        }



        public async Task<IActionResult> Inward_DateVerficationDetails(int id)
        {
            int _step = 91100;
            HttpContext.Session.SetString("ErrorMessage", "");

            if (string.IsNullOrEmpty(GetUserName()))
            {
                return RedirectToAction("Login", "Login");
            }

            string branchCode = GetBranchID();
            ViewBag.Branch_codee = branchCode;
            List<string> chqList = new List<string>();

            try
            {
                if (!id.ToString().Contains(":"))
                {
                    // This part needs proper implementation based on the original VB.NET logic
                    // It seems to be handling a list of cheque sequences or a single ID
                    // Placeholder for now
                    _logger.LogInformation("Inward_DateVerficationDetails called with ID: {Id}", id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Inward_DateVerficationDetails: {Message}", ex.Message);
                HttpContext.Session.SetString("ErrorMessage", "An error occurred: " + ex.Message);
                return View("Error");
            }

            return View(); // Or return a specific view with data
        }





        public async Task<string> GetDocType(int id)
        {
            string result = "";
            try
            {
                _logger.LogInformation("Start getDocType");
                result = (await _context.Legal_Document_Type_Tbl.SingleOrDefaultAsync(x => x.ID == id))?.Legal_Doc_Name_EN;
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when getDocType: {Message}", ex.Message);
                // _LogSystem.WriteLogg and WriteError equivalents are already handled by _logger
            }
            return result;
        }




        public async Task<IActionResult> PMADataVerfication()
        {
            HttpContext.Session.SetString("ErrorMessage", "");

            if (string.IsNullOrEmpty(GetUserName()))
            {
                return RedirectToAction("Login", "Login");
            }

            string methodName = GetMethodName(); // Assuming GetMethodName() is implemented elsewhere or can be inferred
            int userId = GetUserID();

            try
            {
                var appPage = await _context.App_Pages.SingleOrDefaultAsync(t => t.Page_Name_EN == methodName);
                if (appPage == null) return NotFound();

                int pageId = appPage.Page_Id;
                int applicationId = appPage.Application_ID;

                HttpContext.Session.SetString("page_id", pageId.ToString());

                // Assuming getuser_group_permision is a helper function or service call
                // await getuser_group_permision(pageId, applicationId, userId);

                if (HttpContext.Session.GetString("AccessPage") == "NoAccess")
                {
                    return RedirectToAction("block", "Login");
                }

                ViewBag.Tree = GetAllCategoriesForTree();

                var cheqStatusList = await _context.CHEQUE_STATUS_ENU.ToListAsync();
                var branchList = await _context.Companies_Tbl.Where(o => o.Company_Type != "4").ToListAsync();
                foreach (var item in branchList)
                {
                    // Assuming Company_Code is string and needs parsing
                    if (item.Company_Code.Length > 5)
                    {
                        item.Company_Code = item.Company_Code.Substring(5);
                    }
                }

                var clearingList = await _context.ClearingCenters.Where(x => x.Id == "INHOUSE" || x.Id == "PMA").ToListAsync();
                var currencyList = await _context.CURRENCY_TBL.ToListAsync();

                ViewBag.CURRENCY = currencyList;
                ViewBag.CHEQUE_STATUS = cheqStatusList;
                ViewBag.Branches = branchList;
                ViewBag.Clearinglst = clearingList;

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in PMADataVerfication: {Message}", ex.Message);
                HttpContext.Session.SetString("ErrorMessage", "An error occurred: " + ex.Message);
                return View("Error");
            }
        }

        private string GetMethodName()
        {
            // Helper to get the current method name, similar to VB.NET's System.Reflection.MethodBase.GetCurrentMethod().Name
            return ControllerContext.ActionDescriptor.ActionName;
        }



        public async Task<IActionResult> PMADATAVerficationDetails_OnUS(int id)
        {
            HttpContext.Session.SetString("ErrorMessage", "");

            if (string.IsNullOrEmpty(GetUserName()))
            {
                return RedirectToAction("Login", "Login");
            }

            string methodName = GetMethodName();
            int userId = GetUserID();

            try
            {
                var appPage = await _context.App_Pages.SingleOrDefaultAsync(t => t.Page_Name_EN == methodName);
                if (appPage == null) return NotFound();

                int pageId = appPage.Page_Id;
                int applicationId = appPage.Application_ID;

                // Assuming getuser_group_permision is a helper function or service call
                // await getuser_group_permision(pageId, applicationId, userId);

                string title = (await _context.App_Pages.SingleOrDefaultAsync(c => c.Page_Id == pageId))?.ENG_DESC;
                ViewBag.Title = title;
                ViewBag.Tree = GetAllCategoriesForTree();

                List<string> chqList = new List<string>();
                int nextChq = 0;

                if (!id.ToString().Contains(":"))
                {
                    string searchListNote = "INHOUSE" + HttpContext.Session.GetString("ID");
                    var serialList = await _context.Serial_List.FirstOrDefaultAsync(d => d.Note == searchListNote);

                    if (serialList != null && !string.IsNullOrEmpty(serialList.Serials))
                    {
                        string[] arrayList = serialList.Serials.Split(';');
                        if (arrayList.Length > 1)
                        {
                            if (id == 0)
                            {
                                return Content("<body><script type='text/javascript'>window.close();</script></body>");
                            }

                            foreach (var item in arrayList)
                            {
                                if (!string.IsNullOrEmpty(item))
                                {
                                    chqList.Add(item.Trim());
                                }
                            }

                            try
                            {
                                int indexOf = chqList.IndexOf(id.ToString());
                                if (indexOf != -1 && indexOf + 1 < chqList.Count)
                                {
                                    nextChq = Convert.ToInt32(chqList[indexOf + 1]);
                                }
                            }
                            catch (Exception)
                            {
                                // Log exception if needed
                            }
                        }
                    }
                }

                ViewBag.NextChq = nextChq;
                ViewBag.CHQList = chqList;

                HttpContext.Session.SetString("IMGERROR", "");

                OnusChqs inChq = new OnusChqs();
                OnUs_Imgs img = null;
                OnUs_Tbl incObj = null;
                List<CURRENCY_TBL> currency = null;

                incObj = await _context.OnUs_Tbl.SingleOrDefaultAsync(y => y.Serial == id);
                currency = await _context.CURRENCY_TBL.ToListAsync();

                if (incObj == null)
                {
                    return Content("<body><script type='text/javascript'>window.close();</script></body>");
                }

                string retCodeStr = incObj.ReturnCode;
                Return_Codes_Tbl retCode = null;

                if (!string.IsNullOrEmpty(retCodeStr))
                {
                    retCodeStr = retCodeStr.Trim();
                    if (string.IsNullOrEmpty(retCodeStr))
                    {
                        retCodeStr = incObj.ReturnCodeFinancail?.Trim();
                        if (!string.IsNullOrEmpty(retCodeStr))
                        {
                            retCode = await _context.Return_Codes_Tbl.SingleOrDefaultAsync(z => z.ReturnCode == retCodeStr && z.ClrCenter == "PMA");
                        }
                    }
                    else
                    {
                        retCode = await _context.Return_Codes_Tbl.SingleOrDefaultAsync(z => z.ReturnCode == retCodeStr && z.ClrCenter == "PMA");
                    }
                }
                ViewBag.RCDescription_AR = retCode;

                foreach (var curr in currency)
                {
                    if (incObj.Currency == "1" || incObj.Currency == "2" || incObj.Currency == "3" || incObj.Currency == "5")
                    {
                        if (Convert.ToInt32(incObj.Currency) == curr.ID)
                        {
                            incObj.Currency = curr.SYMBOL_ISO;
                            break;
                        }
                    }
                }

                ViewBag.data = currency;
                img = await _context.OnUs_Imgs.FirstOrDefaultAsync(y => y.Serial == incObj.Serial);

                if (img == null)
                {
                    try
                    {
                        Cheque_Images_Link_Tbl onusImgLink = await _context.Cheque_Images_Link_Tbl.SingleOrDefaultAsync(v => v.Serial == incObj.Serial && v.Cheque_ype == "OnUs");
                        if (onusImgLink == null)
                        {
                            Cheque_Images_Link_Tbl postImgLink = await _context.Cheque_Images_Link_Tbl.SingleOrDefaultAsync(v => v.Serial == incObj.PDC_Serial && v.Cheque_ype == "PDC");
                            if (postImgLink != null)
                            {
                                onusImgLink = new Cheque_Images_Link_Tbl
                                {
                                    Serial = incObj.Serial,
                                    Cheque_ype = "OnUs",
                                    ImageSerial = postImgLink.ImageSerial,
                                    ChqSequance = incObj.ChqSequance,
                                    TransDate = DateTime.Now
                                };
                                _context.Cheque_Images_Link_Tbl.Add(onusImgLink);
                                await _context.SaveChangesAsync();
                            }
                        }
                    }
                    catch (Exception ex) { _logger.LogError(ex, "Error handling missing image for OnUs cheque."); }
                }

                incObj.Amount = Decimal.Round(incObj.Amount, 2, MidpointRounding.AwayFromZero);
                inChq.onus = incObj;
                inChq.Imgs = img;

                List<Return_Codes_Tbl> retDescList = await _context.Return_Codes_Tbl.Where(i => i.ClrCenter == "PMA").ToListAsync();
                ViewBag.Ret_Desc = retDescList;

                if (inChq.onus.Was_PDC == true)
                {
                    ViewBag.WAS_PDC = "WAS_PDC";
                }

                return View(inChq);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in PMADATAVerficationDetails_OnUS: {Message}", ex.Message);
                HttpContext.Session.SetString("ErrorMessage", "An error occurred: " + ex.Message);
                return View("Error");
            }
        }



        public async Task<IActionResult> PMA_DATAVerficationDetails(int id)
        {
            HttpContext.Session.SetString("ErrorMessage", "");

            if (string.IsNullOrEmpty(GetUserName()))
            {
                return RedirectToAction("Login", "Login");
            }

            string branch = GetBranchID();
            int _step = 90000 + 1500;

            ViewBag.Branch_codee = branch;
            string methodName = GetMethodName();
            _step += 1;

            int userId = GetUserID();

            try
            {
                var appPage = await _context.App_Pages.SingleOrDefaultAsync(t => t.Page_Name_EN == methodName);
                if (appPage == null) return NotFound();

                int pageId = appPage.Page_Id;
                int applicationId = appPage.Application_ID;

                // Assuming getuser_group_permision is a helper function or service call
                // await GetUserGroupPermission(pageId, applicationId, userId);

                if (HttpContext.Session.GetString("AccessPage") == "NoAccess")
                {
                    return RedirectToAction("block", "Login");
                }
                _step += 1;

                ViewBag.Title = appPage.ENG_DESC;
                _step += 1;
                ViewBag.Tree = GetAllCategoriesForTree();
                _step += 1;

                List<string> CHQList = new List<string>();
                int NextChq = 0;

                if (!id.ToString().Contains(":"))
                {
                    string searchListNote = "PMA" + GetUserID();
                    var serialList = await _context.Serial_List.FirstOrDefaultAsync(d => d.Note == searchListNote);

                    if (serialList != null)
                    {
                        string List = serialList.Serials;
                        string[] array_list = List.Split(';');

                        if (array_list.Length != 1)
                        {
                            if (id == 0)
                            {
                                return Content("<body><script type='text/javascript'>window.close();</script></body> ");
                            }

                            foreach (var item in array_list)
                            {
                                if (!string.IsNullOrEmpty(item)) CHQList.Add(item.Trim());
                            }

                            try
                            {
                                int index_of = CHQList.IndexOf(id.ToString());
                                if (index_of >= 0 && index_of + 1 < CHQList.Count)
                                {
                                    NextChq = Convert.ToInt32(CHQList[index_of + 1]);
                                }
                                ViewBag.NextChq = NextChq;
                                ViewBag.CHQList = CHQList;
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, "Error processing CHQList in PMA_DATAVerficationDetails: {Message}", ex.Message);
                                ViewBag.NextChq = 0;
                                ViewBag.CHQList = 0;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in PMA_DATAVerficationDetails initial setup: {Message}", ex.Message);
                ViewBag.NextChq = 0;
                ViewBag.CHQList = 0;
            }

            INChqs inChq = new INChqs();
            INWARD_IMAGES Img = new INWARD_IMAGES();
            Inward_Trans incObj = new Inward_Trans();
            List<CURRENCY_TBL> Currency = new List<CURRENCY_TBL>();

            try
            {
                _logger.LogInformation("Show Cheque to verify it from Inward_Trans table ");

                incObj = await _context.Inward_Trans.SingleOrDefaultAsync(y => y.Serial == id.ToString());
                _step += 1;

                if (incObj.ClrCenter == "PMA")
                {
                    if (incObj.VIP == true && branch != "2")
                    {
                        ViewBag.is_vip = "Yes";
                    }
                }

                Currency = await _context.CURRENCY_TBL.ToListAsync();
                _step += 1;

                if (incObj == null)
                {
                    return Content("<body><script type='text/javascript'>window.close();</script></body> ");
                }
                _step += 1;

                Return_Codes_Tbl Ret_Code = new Return_Codes_Tbl();
                string Ret = "";

                // Assuming getfinalonuscode is a helper function or service call
                // Ret = getfinalonuscode(incObj.ReturnCode, incObj.ReturnCodeFinancail, incObj.ClrCenter);

                if (!string.IsNullOrEmpty(incObj.ReturnCode) || incObj.ReturnCode != null)
                {
                    Ret = incObj.ReturnCode.Trim();

                    if (Ret == "")
                    {
                        if (!string.IsNullOrEmpty(incObj.ReturnCodeFinancail))
                        {
                            Ret = incObj.ReturnCodeFinancail.Trim();
                            Ret_Code = await _context.Return_Codes_Tbl.SingleOrDefaultAsync(z => z.ReturnCode == Ret && z.ClrCenter == "PMA");
                        }
                    }
                    else
                    {
                        Ret_Code = await _context.Return_Codes_Tbl.SingleOrDefaultAsync(z => z.ReturnCode == Ret && z.ClrCenter == "PMA");
                    }
                    ViewBag.RCDescription_AR = Ret_Code;
                }

                foreach (var cur in Currency)
                {
                    if (incObj.Currency == "1" || incObj.Currency == "2" || incObj.Currency == "3" || incObj.Currency == "5")
                    {
                        if (Convert.ToInt32(incObj.Currency) == cur.ID)
                        {
                            incObj.Currency = cur.SYMBOL_ISO;
                            break;
                        }
                    }
                }
                _step += 1;

                ViewBag.data = Currency;
                _step += 1;

                Img = await _context.INWARD_IMAGES.FirstOrDefaultAsync(y => y.Serial == incObj.Serial);
                _step += 1;

                incObj.Amount = Math.Round(incObj.Amount, (incObj.Currency == "JOD" ? 3 : 2), MidpointRounding.AwayFromZero);
                _step += 1;

                inChq.inw = incObj;
                _step += 1;
                inChq.Imgs = Img;

                List<Return_Codes_Tbl> Ret_Desc_list = new List<Return_Codes_Tbl>();
                string clr_center = incObj.ClrCenter;
                Ret_Desc_list = await _context.Return_Codes_Tbl.Where(i => i.ClrCenter == clr_center).ToListAsync();
                _step += 1;
                ViewBag.Ret_Desc = Ret_Desc_list;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in PMA_DATAVerficationDetails: {Message}", ex.Message);
                HttpContext.Session.SetString("ErrorMessage", "An error occurred: " + ex.Message);
                return View("Error");
            }

            if (string.IsNullOrEmpty(GetUserName()))
            {
                return RedirectToAction("Login", "Login");
            }
            return View(inChq);
        }

        private string GetMethodName()
        {
            // This method needs to be implemented to get the current action method name
            // For now, returning a placeholder
            return ControllerContext.RouteData.Values["action"].ToString();
        }

        // Helper class for Next_Chq, if not already defined
        public class Next_Chq
        {
            public List<string> CHQ_List { get; set; } = new List<string>();
            public int serial { get; set; }
        }

        // Helper class for onusChqs, if not already defined
        public class onusChqs
        {
            public OnUs_Tbl onus { get; set; }
            public OnUs_Imgs Imgs { get; set; }
        }

        // Helper class for INChqs, if not already defined
        public class INChqs
        {
            public Inward_Trans inw { get; set; }
            public INWARD_IMAGES Imgs { get; set; }
        }

        // Helper class for Category, if not already defined
        public class Category
        {
            public int SubMenu_ID { get; set; }
            public int? Parent_ID { get; set; }
            public string SubMenu_Name_EN { get; set; }
            public int Related_Page_ID { get; set; }
        }

        // Helper class for TreeNode, if not already defined
        public class TreeNode
        {
            public string SubMenu_Name_EN { get; set; }
            public int SubMenu_ID { get; set; }
            public int Related_Page_ID { get; set; }
            public List<TreeNode> Children { get; set; }
        }

        // Helper class for OnUs_Tbl, if not already defined
        public class OnUs_Tbl
        {
            public string Serial { get; set; }
            public string ReturnCode { get; set; }
            public string ReturnCodeFinancail { get; set; }
            public string ClrCenter { get; set; }
            public bool VIP { get; set; }
            public string Currency { get; set; }
            public decimal Amount { get; set; }
            public bool Was_PDC { get; set; }
            public string PDC_Serial { get; set; }
            public string ChqSequance { get; set; }
            public string AltAccount { get; set; }
        }

        // Helper class for OnUs_Imgs, if not already defined
        public class OnUs_Imgs
        {
            public string Serial { get; set; }
            public byte[] FrontImg { get; set; }
            public byte[] RearImg { get; set; }
            public byte[] UVImage { get; set; }
        }

        // Helper class for Return_Codes_Tbl, if not already defined
        public class Return_Codes_Tbl
        {
            public string ReturnCode { get; set; }
            public string ClrCenter { get; set; }
            public string RCDescription_AR { get; set; }
        }

        // Helper class for Serial_List, if not already defined
        public class Serial_List
        {
            public string Note { get; set; }
            public string Serials { get; set; }
        }

        // Helper class for INWARD_IMAGES, if not already defined
        public class INWARD_IMAGES
        {
            public string Serial { get; set; }
        }

        // Helper class for Inward_Trans, if not already defined
        public class Inward_Trans
        {
            public string Serial { get; set; }
            public string ClrCenter { get; set; }
            public bool VIP { get; set; }
            public string Currency { get; set; }
            public decimal Amount { get; set; }
            public int posted { get; set; }
            public string ReturnCode { get; set; }
            public string ReturnCodeFinancail { get; set; }
            public string ChqSequance { get; set; }
            public string AltAccount { get; set; }
        }

        // Helper class for INWARD_WF_Tbl, if not already defined
        public class INWARD_WF_Tbl
        {
            public string Serial { get; set; }
            public string Final_Status { get; set; }
            public string WF_Level_Desc { get; set; }
            public DateTime WF_Level_Date { get; set; }
            public string WF_Level_User { get; set; }
            public decimal Amount_JD { get; set; }
        }

        // Helper class for App_Pages, if not already defined
        public class App_Pages
        {
            public int Page_Id { get; set; }
            public string Page_Name_EN { get; set; }
            public int Application_ID { get; set; }
            public string ENG_DESC { get; set; }
        }

        // Helper class for Users_Tbl, if not already defined
        public class Users_Tbl
        {
            public string User_Name { get; set; }
            public string Group_ID { get; set; }
        }

        // Helper class for AuthTrans_User_TBL, if not already defined
        public class AuthTrans_User_TBL
        {
            public int Auth_user_ID { get; set; }
            public string Trans_id { get; set; }
            public string group_ID { get; set; }
        }

        // Helper class for Legal_Document_Type_Tbl, if not already defined
        public class Legal_Document_Type_Tbl
        {
            public int ID { get; set; }
            public string Legal_Doc_Name_EN { get; set; }
        }

        // Helper class for Cheque_Images_Link_Tbl, if not already defined
        public class Cheque_Images_Link_Tbl
        {
            public string Serial { get; set; }
            public string Cheque_ype { get; set; }
            public string ImageSerial { get; set; }
            public string ChqSequance { get; set; }
            public DateTime TransDate { get; set; }
        }

        // Helper class for ApplicationDbContext, if not already defined
        public class ApplicationDbContext : DbContext
        {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
            public DbSet<INWARD_WF_Tbl> INWARD_WF_Tbl { get; set; }
            public DbSet<Inward_Trans> Inward_Trans { get; set; }
            public DbSet<App_Pages> App_Pages { get; set; }
            public DbSet<Users_Tbl> Users_Tbl { get; set; }
            public DbSet<AuthTrans_User_TBL> AuthTrans_User_TBL { get; set; }
            public DbSet<Category> Categories { get; set; }
            public DbSet<Return_Codes_Tbl> Return_Codes_Tbl { get; set; }
            public DbSet<CURRENCY_TBL> CURRENCY_TBL { get; set; }
            public DbSet<OnUs_Tbl> OnUs_Tbl { get; set; }
            public DbSet<OnUs_Imgs> OnUs_Imgs { get; set; }
            public DbSet<Serial_List> Serial_List { get; set; }
            public DbSet<INWARD_IMAGES> INWARD_IMAGES { get; set; }
            public DbSet<Legal_Document_Type_Tbl> Legal_Document_Type_Tbl { get; set; }
            public DbSet<Cheque_Images_Link_Tbl> Cheque_Images_Link_Tbl { get; set; }
            public DbSet<Companies_Tbl> Companies_Tbl { get; set; }
            public DbSet<ClearingCenter> ClearingCenters { get; set; }
            public DbSet<CHEQUE_STATUS_ENU> CHEQUE_STATUS_ENU { get; set; }
        }

        // Helper class for Companies_Tbl, if not already defined
        public class Companies_Tbl
        {
            public string Company_Type { get; set; }
            public string Company_Code { get; set; }
        }

        // Helper class for ClearingCenter, if not already defined
        public class ClearingCenter
        {
            public string Id { get; set; }
        }

        // Helper class for CHEQUE_STATUS_ENU, if not already defined
        public class CHEQUE_STATUS_ENU
        {
            // Define properties as per your database schema
        }

    }
}



        public string Decrypt_CAB_Account_No(string accountNo, string chqNo)
        {
            try
            {
                _logger.LogInformation("Decrypt CAB_Account Number using SQL Function -Decrypt_CAB_Acc_No-");

                // This part needs proper ADO.NET implementation or EF Core raw SQL query
                // For now, a placeholder for direct SQL execution
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand($"SELECT dbo.Decrypt_CAB_Acc_No('{accountNo}','{chqNo}')", conn))
                    {
                        var result = cmd.ExecuteScalar();
                        return result?.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when Decrypt CAB_Account Number using SQL Function -Decrypt_CAB_Acc_No-: {Message}", ex.Message);
                // _LogSystem.WriteError(...);
                return null;
            }
        }

