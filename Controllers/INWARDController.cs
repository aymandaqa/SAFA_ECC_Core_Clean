using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using SAFA_ECC_Core.Models;
using SAFA_ECC_Core.ViewModels.InwardViewModels;

namespace SAFA_ECC_Core.Controllers
{
    public class INWARDController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<INWARDController> _logger;
        private readonly string _applicationID = "1";
        private readonly string _connectionString;

        public INWARDController(ApplicationDbContext context, ILogger<INWARDController> logger)
        {
            _context = context;
            _logger = logger;
            _connectionString = ""; // TODO: Get from configuration
        }

        // Helper methods
        private string GetUserName()
        {
            return HttpContext.Session.GetString("UserName") ?? "";
        }

        private string GetBranchID()
        {
            return HttpContext.Session.GetString("BranchID") ?? "";
        }

        private string GetComID()
        {
            return HttpContext.Session.GetString("ComID") ?? "";
        }

        private int GetUserID()
        {
            return HttpContext.Session.GetInt32("ID") ?? 0;
        }

        // GET: INWARD
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Indextest(string id)
        {
            return View();
        }

        // Helper method for tree structure
        private static List<TreeNode> FillRecursive(List<Category> flatObjects, int? parentId = null)
        {
            try
            {
                return flatObjects
                    .Where(x => x.Parent_ID.Equals(parentId))
                    .Select(item => new TreeNode
                    {
                        SubMenu_Name_EN = item.SubMenu_Name_EN,
                        SubMenu_ID = item.SubMenu_ID,
                        Related_Page_ID = item.Related_Page_ID,
                        Children = FillRecursive(flatObjects, item.SubMenu_ID)
                    }).ToList();
            }
            catch (Exception)
            {
                return new List<TreeNode>();
            }
        }

        // Get customer death date
        public async Task<string> GetCustomerDeathDate(string customerId)
        {
            string result = "";
            string _result = "";
            string _dateDeathString = "";
            string _deathNotDateString = "";

            try
            {
                var globalParam = await _context.Global_Parameter_TBL
                    .SingleOrDefaultAsync(i => i.Parameter_Name == "ACC_INFO_SVC");

                if (globalParam != null)
                {
                    string _val = globalParam.Parameter_Value;
                    _val = _val.Replace("@CUSTOMER_ID@", customerId);

                    try
                    {
                        // TODO: Implement OFS message execution
                        // _result = ofsObj.Execute_Ofs_Enqury(_val);
                        
                        // TODO: Implement customer class filling
                        // _customer = customerofs.Fill_Customer_Class(_result);
                        
                        // For now, return empty result
                        _result = "";
                    }
                    catch (Exception ex)
                    {
                        string loggMessage = $"Error Get Date of Death From OFS MSG err: {ex.Message}";
                        _logger.LogError(loggMessage);
                    }
                }

                if (string.IsNullOrEmpty(result))
                {
                    // TODO: Implement database connection and query
                    // result = conn.Get_One_Data($"select [DBO].[GET_CUSTOMER_DEATH_DATE] ('{customerId}')");
                }

                return result;
            }
            catch (Exception ex)
            {
                string loggMessage = $"Error Get Date of Death err: {ex.Message}";
                _logger.LogError(loggMessage);
                return result;
            }
        }

        // Test response method
        public async Task<IActionResult> TestRes()
        {
            try
            {
                var onusTbl = new OnUs_Tbl();
                string response = "Insufficient Funds";
                string specialNote = "";
                string userId = "9";
                var fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 13);

                string sql = @"select serial, SUBSTRING(ErrorDescription, 3, 1000) as ErrorDescription 
                              from onus_tbl 
                              where cast(TransDate as date) = '20250216' 
                              and ChqSequance not in (
                                  select ChqSequance from INWARD_WF_Tbl 
                                  WHERE CAST(input_date AS DATE) = '20250216'
                              )";

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand(sql, connection))
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                string description = reader["ErrorDescription"].ToString();
                                string serial = reader["Serial"].ToString();

                                var onusRecord = await _context.OnUs_Tbl
                                    .SingleOrDefaultAsync(i => i.Serial == serial);

                                if (onusRecord != null)
                                {
                                    // TODO: Implement web service call
                                    // obj = WebSvc.HandelResponseONUS(onusRecord.ChqSequance, onusRecord.ClrCenter, description, specialNote, onusRecord.Serial, userId);
                                }
                            }
                        }
                    }
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in TestRes: {ex.Message}");
                return Json(new { success = false, error = ex.Message });
            }
        }

        // Inward Financial WF Details PMADIS NEW
        public async Task<IActionResult> InwardFinanicalWFDetailsPMADIS_NEW(string id)
        {
            HttpContext.Session.SetString("ErrorMessage", "");

            int step = 90000;
            step += 5700;

            if (string.IsNullOrEmpty(GetUserName()))
            {
                return RedirectToAction("Login", "Login");
            }

            string methodName = "InwardFinanicalWFDetailsPMADIS";
            step += 1;

            try
            {
                int userId = GetUserID();
                int pageId = 0;
                int applicationId = 0;
                string title = "";

                var appPage = await _context.App_Pages
                    .SingleOrDefaultAsync(t => t.Page_Name_EN == methodName);
                
                if (appPage != null)
                {
                    pageId = appPage.Page_Id;
                    applicationId = appPage.Application_ID;
                    title = appPage.ENG_DESC;
                }

                step += 1;

                // TODO: Implement getuser_group_permision
                // getuser_group_permision(pageId, applicationId, userId);

                ViewBag.Title = title;
                // TODO: Implement GetAllCategoriesForTree
                // ViewBag.Tree = GetAllCategoriesForTree();

                step += 1;

                string branch = GetBranchID();
                var inChq = new INChqs();
                var wf = await _context.INWARD_WF_Tbl
                    .SingleOrDefaultAsync(z => z.Serial == id && z.Final_Status != "Accept");

                step += 1;

                string com = GetComID();
                var incObj = await _context.Inward_Trans
                    .SingleOrDefaultAsync(y => y.Serial == id);

                if (incObj == null)
                {
                    return RedirectToAction("InsufficientFunds", "INWARD");
                }

                if (incObj.ClrCenter == "PMA")
                {
                    if (incObj.VIP == true && branch != "2")
                    {
                        ViewBag.is_vip = "YES";
                    }
                }

                step += 1;

                var guarCustomers = await _context.T24_CAB_OVRDRWN_GUAR
                    .Where(i => i.GUAR_CUSTOMER == incObj.ISSAccount)
                    .ToListAsync();

                step += 1;

                ViewBag.GUAR_CUSTOMER = "";
                if (guarCustomers.Count == 0)
                {
                    ViewBag.GUAR_CUSTOMER = "Not Available";
                    inChq.GUAR_CUSTOMER = "Not Available";
                }
                else
                {
                    foreach (var item in guarCustomers)
                    {
                        // TODO: Implement ECC account info service
                        // var guarCustomerAccObj = EccAccInfoWebSvc.ACCOUNT_INFO(item.ACCOUNT_NUMBER, 1);
                        // ViewBag.GUAR_CUSTOMER += $"{item.ACCOUNT_NUMBER}*{guarCustomerAccObj.ClearBalance}*{guarCustomerAccObj.AccountCurrency}|";
                        // inChq.GUAR_CUSTOMER += $"{item.ACCOUNT_NUMBER}*{guarCustomerAccObj.ClearBalance}*{guarCustomerAccObj.AccountCurrency}|";
                    }
                }

                // TODO: Implement account info service
                // var accObj = EccAccInfoWebSvc.ACCOUNT_INFO(incObj.AltAccount, 1);
                // inChq.BookedBalance = accObj.BookedBalance;
                // inChq.ClearBalance = accObj.ClearBalance;
                // inChq.AccountStatus = accObj.AccountStatus;

                step += 1;

                string userName = GetUserName();
                var user = await _context.Users_Tbl
                    .SingleOrDefaultAsync(c => c.User_Name == userName);

                ViewBag.Reject = "False";
                inChq.Reject = "False";

                ViewBag.recomdationbtn = "True";
                inChq.Recom = "True";

                if (user != null)
                {
                    // TODO: Implement group type checking
                    // if (user.Group_ID == GroupType.Group_Status.AdminAuthorized || branch == "2")
                    // {
                    //     ViewBag.Approve = "True";
                    //     inChq.Approve = "True";
                    //     ViewBag.recomdationbtn = "False";
                    //     inChq.Recom = "False";
                    // }
                }

                // TODO: Implement remaining business logic

                return View(inChq);
            }
            catch (Exception ex)
            {
                string loggMessage = $"Error in InwardFinanicalWFDetailsPMADIS_NEW: {ex.Message}";
                _logger.LogError(loggMessage);
                return RedirectToAction("Error", "Home");
            }
        }

        // Reverse Posting PMARAM
        public async Task<IActionResult> ReversePostingPMARAM(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(GetUserName()))
                {
                    return RedirectToAction("Login", "Login");
                }

                string loggMessage = "Reverse Posting PMARAM started";
                _logger.LogInformation(loggMessage);

                var wfRecord = await _context.INWARD_WF_Tbl
                    .SingleOrDefaultAsync(z => z.Serial == id);

                if (wfRecord == null)
                {
                    return Json(new { success = false, message = "Record not found" });
                }

                var inwardRecord = await _context.Inward_Trans
                    .SingleOrDefaultAsync(y => y.Serial == id);

                if (inwardRecord == null)
                {
                    return Json(new { success = false, message = "Inward record not found" });
                }

                // TODO: Implement web service call for reverse posting
                // var webSvc = new ECC_Handler_SVC.InwardHandlingSVCSoapClient("InwardHandlingSVCSoap");
                // var response = webSvc.ReversePosting(parameters);

                await _context.SaveChangesAsync();

                loggMessage = "Reverse Posting PMARAM completed successfully";
                _logger.LogInformation(loggMessage);

                return Json(new { success = true, message = "Reverse posting completed successfully" });
            }
            catch (Exception ex)
            {
                string loggMessage = $"Error in ReversePostingPMARAM: {ex.Message}";
                _logger.LogError(loggMessage);
                return Json(new { success = false, message = ex.Message });
            }
        }

        // TODO: Add remaining methods from VB.NET controller
        // - InwardFinanicalWFDetailsPMADIS
        // - InwardFinanicalWFDetailsPMADIS_Auth
        // - Other action methods as needed
    }

    // Supporting classes
    public class TreeNode
    {
        public string SubMenu_Name_EN { get; set; }
        public int SubMenu_ID { get; set; }
        public int Related_Page_ID { get; set; }
        public List<TreeNode> Children { get; set; } = new List<TreeNode>();
    }

    public class Category
    {
        public string SubMenu_Name_EN { get; set; }
        public int SubMenu_ID { get; set; }
        public int Related_Page_ID { get; set; }
        public int? Parent_ID { get; set; }
    }

    public class INChqs
    {
        public string GUAR_CUSTOMER { get; set; }
        public decimal BookedBalance { get; set; }
        public decimal ClearBalance { get; set; }
        public string AccountStatus { get; set; }
        public string Reject { get; set; }
        public string Recom { get; set; }
        public string Approve { get; set; }
    }
}
