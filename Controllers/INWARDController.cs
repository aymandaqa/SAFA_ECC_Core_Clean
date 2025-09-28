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
        private readonly IInwardService _inwardService;
        private readonly string _applicationId = "1";
        private readonly string _connectionString;

        public INWARDController(ILogger<INWARDController> logger, ApplicationDbContext context, IInwardService inwardService)
        {
            _logger = logger;
            _context = context;
            _inwardService = inwardService;
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
            var result = await _inwardService.InwardFinanicalWFDetailsONUS_NEW(id);
            if (result.Value is IDictionary<string, object> data && data.TryGetValue("redirectTo", out var redirectTo))
            {
                return Redirect(redirectTo.ToString());
            }
            // Assuming the service returns a JsonResult that can be directly returned by the controller
            return Json(result.Value);
        }

        public async Task<IActionResult> InwardFinanicalWFDetailsPMADIS_Auth(string id)
        {
            var result = await _inwardService.InwardFinanicalWFDetailsONUS_Auth(id);
            if (result.Value is IDictionary<string, object> data && data.TryGetValue("redirectTo", out var redirectTo))
            {
                return Redirect(redirectTo.ToString());
            }
            // Assuming the service returns a JsonResult that can be directly returned by the controller
            return Json(result.Value);
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
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetPermission: {Message}", ex.Message);
                return false;
            }
        }

        public async Task<IActionResult> Pendding_ONUS_Request()
        {
            if (string.IsNullOrEmpty(GetUserName()))
            {
                return RedirectToAction("Login", "Login");
            }

            HttpContext.Session.SetString("ErrorMessage", "");
            int _step = 80000;

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
        private string GetMethodName([System.Runtime.CompilerServices.CallerMemberName] string memberName = null)
        {
            return memberName;
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

                _logger.LogInformation("Show Cheque fixed error it from PMA DIS table");

                incObj = await _context.Inward_Trans.SingleOrDefaultAsync(y => y.Serial == id && (y.Posted == (int)AllEnums.Cheque_Status.New || y.Posted == (int)AllEnums.Cheque_Status.Posted || y.Posted == (int)AllEnums.Cheque_Status.Rejected));
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
                img = await _context.INWARD_IMAGES.FirstOrDefaultAsync(y => y.Serial == incObj.Serial);

                if (img == null)
                {
                    try
                    {
                        var inwardimg = await _context.Cheque_Images_Link_Tbl.SingleOrDefaultAsync(v => v.Serial == incObj.Serial && v.Cheque_ype == "Inward");
                        if (inwardimg == null)
                        {
                            var postimg = await _context.Cheque_Images_Link_Tbl.SingleOrDefaultAsync(v => v.Serial == incObj.PDC_Serial && v.Cheque_ype == "PDC");
                            if (postimg != null)
                            {
                                inwardimg = new Cheque_Images_Link_Tbl
                                {
                                    Serial = incObj.Serial,
                                    Cheque_ype = "Inward",
                                    ImageSerial = postimg.ImageSerial,
                                    ChqSequance = incObj.ChqSequance,
                                    TransDate = DateTime.Now
                                };
                                _context.Cheque_Images_Link_Tbl.Add(inwardimg);
                                await _context.SaveChangesAsync();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error creating Inward image link: {Message}", ex.Message);
                    }
                    img = await _context.INWARD_IMAGES.FirstOrDefaultAsync(y => y.Serial == incObj.Serial);
                }

                incObj.Amount = Math.Round(incObj.Amount, 2, MidpointRounding.AwayFromZero);
                inChq.inward = incObj;
                inChq.Imgs = img;

                return View(inChq);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in InwardFixederrorDetailsPMADIS: {Message}", ex.Message);
                HttpContext.Session.SetString("ErrorMessage", "An error occurred: " + ex.Message);
                return View("Error");
            }
        }

        public async Task<IActionResult> InwardFixederrorDetailsPMA(string id)
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

                _logger.LogInformation("Show Cheque fixed error it from PMA table");

                incObj = await _context.Inward_Trans.SingleOrDefaultAsync(y => y.Serial == id && (y.Posted == (int)AllEnums.Cheque_Status.New || y.Posted == (int)AllEnums.Cheque_Status.Posted || y.Posted == (int)AllEnums.Cheque_Status.Rejected));
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
                img = await _context.INWARD_IMAGES.FirstOrDefaultAsync(y => y.Serial == incObj.Serial);

                if (img == null)
                {
                    try
                    {
                        var inwardimg = await _context.Cheque_Images_Link_Tbl.SingleOrDefaultAsync(v => v.Serial == incObj.Serial && v.Cheque_ype == "Inward");
                        if (inwardimg == null)
                        {
                            var postimg = await _context.Cheque_Images_Link_Tbl.SingleOrDefaultAsync(v => v.Serial == incObj.PDC_Serial && v.Cheque_ype == "PDC");
                            if (postimg != null)
                            {
                                inwardimg = new Cheque_Images_Link_Tbl
                                {
                                    Serial = incObj.Serial,
                                    Cheque_ype = "Inward",
                                    ImageSerial = postimg.ImageSerial,
                                    ChqSequance = incObj.ChqSequance,
                                    TransDate = DateTime.Now
                                };
                                _context.Cheque_Images_Link_Tbl.Add(inwardimg);
                                await _context.SaveChangesAsync();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error creating Inward image link: {Message}", ex.Message);
                    }
                    img = await _context.INWARD_IMAGES.FirstOrDefaultAsync(y => y.Serial == incObj.Serial);
                }

                incObj.Amount = Math.Round(incObj.Amount, 2, MidpointRounding.AwayFromZero);
                inChq.inward = incObj;
                inChq.Imgs = img;

                return View(inChq);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in InwardFixederrorDetailsPMA: {Message}", ex.Message);
                HttpContext.Session.SetString("ErrorMessage", "An error occurred: " + ex.Message);
                return View("Error");
            }
        }

        public async Task<IActionResult> InwardFixederrorDetailsPMA_Auth(string id)
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

                _logger.LogInformation("Show Cheque fixed error it from PMA table");

                incObj = await _context.Inward_Trans.SingleOrDefaultAsync(y => y.Serial == id && (y.Posted == (int)AllEnums.Cheque_Status.New || y.Posted == (int)AllEnums.Cheque_Status.Posted || y.Posted == (int)AllEnums.Cheque_Status.Rejected));
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
                img = await _context.INWARD_IMAGES.FirstOrDefaultAsync(y => y.Serial == incObj.Serial);

                if (img == null)
                {
                    try
                    {
                        var inwardimg = await _context.Cheque_Images_Link_Tbl.SingleOrDefaultAsync(v => v.Serial == incObj.Serial && v.Cheque_ype == "Inward");
                        if (inwardimg == null)
                        {
                            var postimg = await _context.Cheque_Images_Link_Tbl.SingleOrDefaultAsync(v => v.Serial == incObj.PDC_Serial && v.Cheque_ype == "PDC");
                            if (postimg != null)
                            {
                                inwardimg = new Cheque_Images_Link_Tbl
                                {
                                    Serial = incObj.Serial,
                                    Cheque_ype = "Inward",
                                    ImageSerial = postimg.ImageSerial,
                                    ChqSequance = incObj.ChqSequance,
                                    TransDate = DateTime.Now
                                };
                                _context.Cheque_Images_Link_Tbl.Add(inwardimg);
                                await _context.SaveChangesAsync();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error creating Inward image link: {Message}", ex.Message);
                    }
                    img = await _context.INWARD_IMAGES.FirstOrDefaultAsync(y => y.Serial == incObj.Serial);
                }

                incObj.Amount = Math.Round(incObj.Amount, 2, MidpointRounding.AwayFromZero);
                inChq.inward = incObj;
                inChq.Imgs = img;

                return View(inChq);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in InwardFixederrorDetailsPMA_Auth: {Message}", ex.Message);
                HttpContext.Session.SetString("ErrorMessage", "An error occurred: " + ex.Message);
                return View("Error");
            }
        }

        public async Task<IActionResult> InwardFinanicalWFDetailsONUS_Auth(string id)
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

                var inChq = new InwardFinanicalWFDetailsONUSViewModel(); // Assuming InwardFinanicalWFDetailsONUSViewModel is a ViewModel
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
                _logger.LogError(ex, "Error in InwardFinanicalWFDetailsONUS_Auth: {Message}", ex.Message);
                HttpContext.Session.SetString("ErrorMessage", "An error occurred: " + ex.Message);
                return View("Error");
            }
        }

        public async Task<IActionResult> InwardFinanicalWFDetailsPMA_NEW(string id)
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

                var inChq = new InwardFinanicalWFDetailsPMAViewModel(); // Assuming InwardFinanicalWFDetailsPMAViewModel is a ViewModel
                var img = new INWARD_IMAGES();
                var incObj = new Inward_Trans();
                List<CURRENCY_TBL> currencyList = new List<CURRENCY_TBL>();

                _logger.LogInformation("Show Cheque fixed error it from PMA table");

                incObj = await _context.Inward_Trans.SingleOrDefaultAsync(y => y.Serial == id && (y.Posted == (int)AllEnums.Cheque_Status.New || y.Posted == (int)AllEnums.Cheque_Status.Posted || y.Posted == (int)AllEnums.Cheque_Status.Rejected));
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
                img = await _context.INWARD_IMAGES.FirstOrDefaultAsync(y => y.Serial == incObj.Serial);

                if (img == null)
                {
                    try
                    {
                        var inwardimg = await _context.Cheque_Images_Link_Tbl.SingleOrDefaultAsync(v => v.Serial == incObj.Serial && v.Cheque_ype == "Inward");
                        if (inwardimg == null)
                        {
                            var postimg = await _context.Cheque_Images_Link_Tbl.SingleOrDefaultAsync(v => v.Serial == incObj.PDC_Serial && v.Cheque_ype == "PDC");
                            if (postimg != null)
                            {
                                inwardimg = new Cheque_Images_Link_Tbl
                                {
                                    Serial = incObj.Serial,
                                    Cheque_ype = "Inward",
                                    ImageSerial = postimg.ImageSerial,
                                    ChqSequance = incObj.ChqSequance,
                                    TransDate = DateTime.Now
                                };
                                _context.Cheque_Images_Link_Tbl.Add(inwardimg);
                                await _context.SaveChangesAsync();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error creating Inward image link: {Message}", ex.Message);
                    }
                    img = await _context.INWARD_IMAGES.FirstOrDefaultAsync(y => y.Serial == incObj.Serial);
                }

                incObj.Amount = Math.Round(incObj.Amount, 2, MidpointRounding.AwayFromZero);
                inChq.inward = incObj;
                inChq.Imgs = img;

                return View(inChq);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in InwardFinanicalWFDetailsPMA_NEW: {Message}", ex.Message);
                HttpContext.Session.SetString("ErrorMessage", "An error occurred: " + ex.Message);
                return View("Error");
            }
        }

        public async Task<IActionResult> InwardFinanicalWFDetailsPMA_Auth_NEW(string id)
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

                var inChq = new InwardFinanicalWFDetailsPMAViewModel(); // Assuming InwardFinanicalWFDetailsPMAViewModel is a ViewModel
                var img = new INWARD_IMAGES();
                var incObj = new Inward_Trans();
                List<CURRENCY_TBL> currencyList = new List<CURRENCY_TBL>();

                _logger.LogInformation("Show Cheque fixed error it from PMA table");

                incObj = await _context.Inward_Trans.SingleOrDefaultAsync(y => y.Serial == id && (y.Posted == (int)AllEnums.Cheque_Status.New || y.Posted == (int)AllEnums.Cheque_Status.Posted || y.Posted == (int)AllEnums.Cheque_Status.Rejected));
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
                img = await _context.INWARD_IMAGES.FirstOrDefaultAsync(y => y.Serial == incObj.Serial);

                if (img == null)
                {
                    try
                    {
                        var inwardimg = await _context.Cheque_Images_Link_Tbl.SingleOrDefaultAsync(v => v.Serial == incObj.Serial && v.Cheque_ype == "Inward");
                        if (inwardimg == null)
                        {
                            var postimg = await _context.Cheque_Images_Link_Tbl.SingleOrDefaultAsync(v => v.Serial == incObj.PDC_Serial && v.Cheque_ype == "PDC");
                            if (postimg != null)
                            {
                                inwardimg = new Cheque_Images_Link_Tbl
                                {
                                    Serial = incObj.Serial,
                                    Cheque_ype = "Inward",
                                    ImageSerial = postimg.ImageSerial,
                                    ChqSequance = incObj.ChqSequance,
                                    TransDate = DateTime.Now
                                };
                                _context.Cheque_Images_Link_Tbl.Add(inwardimg);
                                await _context.SaveChangesAsync();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error creating Inward image link: {Message}", ex.Message);
                    }
                    img = await _context.INWARD_IMAGES.FirstOrDefaultAsync(y => y.Serial == incObj.Serial);
                }

                incObj.Amount = Math.Round(incObj.Amount, 2, MidpointRounding.AwayFromZero);
                inChq.inward = incObj;
                inChq.Imgs = img;

                return View(inChq);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in InwardFinanicalWFDetailsPMA_Auth_NEW: {Message}", ex.Message);
                HttpContext.Session.SetString("ErrorMessage", "An error occurred: " + ex.Message);
                return View("Error");
            }
        }
    }
}

