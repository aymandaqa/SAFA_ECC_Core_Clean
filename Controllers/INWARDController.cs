using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SAFA_ECC_Core_Clean.Data;
using SAFA_ECC_Core_Clean.Models;
using SAFA_ECC_Core_Clean.ViewModels.InwardViewModels;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.IO;

namespace SAFA_ECC_Core_Clean.Controllers
{
    public class INWARDController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<INWARDController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public INWARDController(ApplicationDbContext context, ILogger<INWARDController> logger, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        // Helper methods for session (to be replaced with proper identity management)
        private string GetUserName() => HttpContext.Session.GetString("UserName") ?? "Unknown";
        private string GetBranchID() => HttpContext.Session.GetString("BranchID") ?? "0";
        private string GetComID() => HttpContext.Session.GetString("ComID") ?? "0";
        private int GetUserID() => HttpContext.Session.GetInt32("ID") ?? 0;

        // Dummy method for getuser_group_permision - needs actual implementation
        private void getuser_group_permision(int pageId, int applicationId, int userId)
        {
            // Implement actual permission check here
            _logger.LogInformation($"Permission check for UserID: {userId}, PageID: {pageId}, ApplicationID: {applicationId}");
            // For now, assume access
            // HttpContext.Session.SetString("AccessPage", "Access");
        }

        // Dummy method for GetAllCategoriesForTree - needs actual implementation
        private List<TreeNode> GetAllCategoriesForTree()
        {
            // Implement actual tree data retrieval
            return new List<TreeNode>();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Indextest(string id)
        {
            return View();
        }

        public async Task<IActionResult> InwardFinanicalWFDetailsPMADIS(string id)
        {
            HttpContext.Session.SetString("ErrorMessage", "");

            if (string.IsNullOrEmpty(GetUserName()))
            {
                return RedirectToAction("Login", "Login");
            }

            string methodName = "InwardFinanicalWFDetailsPMADIS";
            int userId = GetUserID();

            var appPage = await _context.App_Pages.SingleOrDefaultAsync(t => t.Page_Name_EN == methodName);
            if (appPage == null)
            {
                _logger.LogError($"App_Page not found for method: {methodName}");
                return RedirectToAction("Error", "Home"); // Or handle appropriately
            }

            int pageId = appPage.Page_Id;
            int applicationId = appPage.Application_ID;

            getuser_group_permision(pageId, applicationId, userId);

            ViewBag.Title = appPage.ENG_DESC;
            ViewBag.Tree = GetAllCategoriesForTree();

            var viewModel = new InwardFinanicalWFDetailsPMADISViewModel();
            string branch = GetBranchID();
            string com = GetComID();

            try
            {
                _logger.LogInformation($"Show Cheque InwardFinanicalWFDetailsPMADIS it from Inward_Trans table for user: {GetUserName()}");

                var wf = await _context.INWARD_WF_Tbl.SingleOrDefaultAsync(z => z.Serial == id && z.Final_Status != "Accept");
                if (wf == null)
                {
                    _logger.LogWarning($"INWARD_WF_Tbl not found or already accepted for Serial: {id}");
                    return RedirectToAction("Error", "Home"); // Or handle appropriately
                }

                var incObj = await _context.Inward_Trans.SingleOrDefaultAsync(y => y.Serial == id);
                if (incObj == null)
                {
                    return RedirectToAction("InsufficientFunds", "INWARD");
                }

                if (incObj.ClrCenter == "PMA" && incObj.VIP == true && branch != "2")
                {
                    ViewBag.is_vip = "YES";
                    viewModel.IsVIP = "YES";
                }

                var guarCustomers = await _context.T24_CAB_OVRDRWN_GUAR
                                                .Where(i => i.GUAR_CUSTOMER == incObj.ISSAccount)
                                                .ToListAsync();

                if (guarCustomers.Any())
                {
                    // Assuming EccAccInfo_WebSvc is a service client for T24_ECC_SVCSoapClient
                    // This part needs actual service integration
                    // For now, mocking the data
                    string guarCustomerInfo = "";
                    foreach (var item in guarCustomers)
                    {
                        // Mocking service call
                        // var GUAR_CUSTOMER_Accobj = await EccAccInfo_WebSvc.ACCOUNT_INFO(item.ACCOUNT_NUMBER, 1);
                        // guarCustomerInfo += $"{item.ACCOUNT_NUMBER}*{GUAR_CUSTOMER_Accobj.ClearBalance}*{GUAR_CUSTOMER_Accobj.AccountCurrency}|";
                        guarCustomerInfo += $"{item.ACCOUNT_NUMBER}*MockBalance*MockCurrency|";
                    }
                    ViewBag.GUAR_CUSTOMER = guarCustomerInfo;
                    viewModel.GUAR_CUSTOMER = guarCustomerInfo;
                }
                else
                {
                    ViewBag.GUAR_CUSTOMER = "Not Available";
                    viewModel.GUAR_CUSTOMER = "Not Available";
                }

                // Mocking service call for Accobj
                // var Accobj = await EccAccInfo_WebSvc.ACCOUNT_INFO(incObj.AltAccount, 1);
                viewModel.BookedBalance = "MockBookedBalance";
                viewModel.ClearBalance = "MockClearBalance";
                viewModel.AccountStatus = "MockAccountStatus";

                string userName = GetUserName();
                var user = await _context.Users_Tbl.SingleOrDefaultAsync(c => c.User_Name == userName);
                string group = user?.Group_ID.ToString() ?? "";

                ViewBag.Reject = "False";
                viewModel.Reject = "False";
                ViewBag.recomdationbtn = "True";
                viewModel.Recom = "True";

                if (group == GroupType.Group_Status.AdminAuthorized.ToString() || branch == "2")
                {
                    ViewBag.Approve = "True";
                    viewModel.Approve = "True";
                    ViewBag.recomdationbtn = "False";
                    viewModel.Recom = "False";
                }
                else
                {
                    var transaction = await _context.Transaction_TBL.SingleOrDefaultAsync(z => z.Transaction_Name == wf.Clr_Center);
                    if (transaction != null)
                    {
                        // This part needs actual stored procedure or function mapping
                        // var userLevels = await _context.USER_Limits_Auth_Amount(userId, transaction.Transaction_ID, "d", wf.Amount_JD).ToListAsync();
                        // For now, assuming default behavior
                    }

                    ViewBag.recomdationbtn = "True";
                    viewModel.Recom = "True";

                    string user_name_wf = wf.Level1_status ?? "";
                    string user_name_site = GetUserName();

                    var authTransUser = await _context.AuthTrans_User_TBL
                        .SingleOrDefaultAsync(t => t.Auth_user_ID == userId && t.Trans_id == (incObj.ClrCenter == "PMA" ? "5" : "6") && t.group_ID == user.Group_ID);

                    if (authTransUser != null)
                    {
                        if (wf.Level1_status == null || wf.Level1_status.Contains(user_name_site))
                        {
                            ViewBag.Approve = "True";
                            viewModel.Approve = "True";
                        }
                        else
                        {
                            ViewBag.Approve = "False";
                            viewModel.Approve = "False";
                        }
                    }
                    else
                    {
                        ViewBag.Approve = "False";
                        viewModel.Approve = "False";
                    }
                }

                viewModel.Serial = incObj.Serial;
                viewModel.TransDate = incObj.TransDate;
                viewModel.ChqSequance = incObj.ChqSequance;
                viewModel.Amount = incObj.Amount;
                viewModel.ClrCenter = incObj.ClrCenter;
                viewModel.DrwAccNo = incObj.DrwAccNo;
                viewModel.DrwChqNo = incObj.DrwChqNo;
                viewModel.DrwBank = incObj.DrwBank;
                viewModel.ISSAccount = incObj.ISSAccount;
                viewModel.AltAccount = incObj.AltAccount;
                viewModel.CustomerName = incObj.CustomerName;
                viewModel.CustomerName2 = incObj.CustomerName2;
                viewModel.ErrorDescription = incObj.ErrorDescription;
                viewModel.ReturnCode = incObj.ReturnCode;
                viewModel.T24Response = incObj.T24Response;
                viewModel.VerifyStatus = incObj.verifyStatus;
                viewModel.FirstLevelStatus = incObj.FirstLevelStatus;
                viewModel.FirstLevelUser = incObj.FirstLevelUser;
                viewModel.FirstLevelDate = incObj.FirstLevelDate;
                viewModel.SecoundLevelStatus = incObj.SecoundLevelStatus;
                viewModel.SecoundLevelUser = incObj.SecoundLevelUser;
                viewModel.SecoundLevelDate = incObj.SecoundLevelDate;
                viewModel.History = incObj.History;
                viewModel.VIP = incObj.VIP;
                viewModel.BranchID = incObj.BranchID;
                viewModel.FinalRetCode = incObj.FinalRetCode;
                viewModel.Rejected = incObj.Rejected;
                viewModel.VerifiedTechnically = incObj.VerifiedTechnically;
                viewModel.ReturnDate = incObj.ReturnDate;
                viewModel.Posted = incObj.Posted;
                viewModel.WFLevel = wf.WF_LEVEL;

                // Populate other ViewModel properties from incObj and wf

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in InwardFinanicalWFDetailsPMADIS for Serial: {id}");
                HttpContext.Session.SetString("ErrorMessage", ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> InwardFinanicalWFDetailsPMADIS_Auth(InwardFinanicalWFDetailsPMADIS_AuthViewModel model)
        {
            if (string.IsNullOrEmpty(GetUserName()))
            {
                return RedirectToAction("Login", "Login");
            }

            string methodName = "InwardFinanicalWFDetailsPMADIS_Auth";
            int userId = GetUserID();

            var appPage = await _context.App_Pages.SingleOrDefaultAsync(t => t.Page_Name_EN == methodName);
            if (appPage == null)
            {
                _logger.LogError($"App_Page not found for method: {methodName}");
                return Json(new { success = false, message = "Configuration error." });
            }

            int pageId = appPage.Page_Id;
            int applicationId = appPage.Application_ID;

            getuser_group_permision(pageId, applicationId, userId);

            try
            {
                _logger.LogInformation($"InwardFinanicalWFDetailsPMADIS_Auth called for Serial: {model.Serial} with Status: {model.Status} and ReturnCode: {model.ReturnCode}");

                var inward = await _context.Inward_Trans.SingleOrDefaultAsync(i => i.Serial == model.Serial);
                if (inward == null)
                {
                    return Json(new { success = false, message = "Inward transaction not found." });
                }

                var wf = await _context.INWARD_WF_Tbl.SingleOrDefaultAsync(w => w.Serial == model.Serial && w.Final_Status != "Accept");
                if (wf == null)
                {
                    return Json(new { success = false, message = "Workflow entry not found or already accepted." });
                }

                var wfHistory = new WFHistory
                {
                    ChqSequance = inward.ChqSequance,
                    Serial = inward.Serial,
                    TransDate = inward.TransDate,
                    ClrCenter = inward.ClrCenter,
                    DrwChqNo = inward.DrwChqNo,
                    DrwAccNo = inward.DrwAccNo,
                    Amount = inward.Amount
                };

                if (wf.WF_LEVEL == 1)
                {
                    if (model.Status == 0) // Reject
                    {
                        inward.ReturnCode = model.ReturnCode;
                        inward.verifyStatus = 0;
                        inward.FirstLevelDate = DateTime.Now;
                        inward.FirstLevelStatus = "Reject";
                        inward.FirstLevelUser = GetUserName();
                        inward.History += $"VerifiedTechnically rejected , By : {GetUserName()} At: {DateTime.Now}";
                        wfHistory.ID_WFStatus = WFHistory.WF_Status.InitialTechnicalRejection;
                        wfHistory.Status = $"InitialTechnicalRejection , By :{GetUserName()} With Return code ={model.ReturnCode} At: {DateTime.Now}";
                    }
                    else // Accept
                    {
                        inward.History += $"VerifiedTechnically Accepted , By : {GetUserName()} At: {DateTime.Now}";
                        inward.verifyStatus = 1;
                        inward.FirstLevelDate = DateTime.Now;
                        inward.FirstLevelStatus += " | Accept";
                        inward.FirstLevelUser += " |" + GetUserName();
                        inward.FinalRetCode = ""; // Original VB.NET had this empty
                        wfHistory.ID_WFStatus = WFHistory.WF_Status.TechnicallyAccepted;
                        wfHistory.Status = $"VerifiedTechnically Accepted By :{GetUserName()} At: {DateTime.Now}";
                    }
                    _context.WFHistories.Add(wfHistory);
                    await _context.SaveChangesAsync();
                }
                else if (wf.WF_LEVEL == 2)
                {
                    if (model.Status == 0) // Reject
                    {
                        inward.ReturnCode = model.ReturnCode;
                        inward.Rejected = 1;
                        inward.verifyStatus = 0;
                        inward.VerifiedTechnically = 1;
                        inward.History += $"VerifiedTechnically Rejected Finally , By : {GetUserName()} At: {DateTime.Now}";
                        inward.SecoundLevelDate = DateTime.Now;
                        inward.SecoundLevelStatus += " | Reject";
                        inward.SecoundLevelUser += " | " + GetUserName();
                        wfHistory.ID_WFStatus = WFHistory.WF_Status.FinalTechnicalRejection;
                        wfHistory.Status = $"FinalTechnicalRejection , By : {GetUserName()} With Return code ={model.ReturnCode} At: {DateTime.Now}";
                    }
                    else // Accept
                    {
                        inward.History += $"VerifiedTechnically Accepted Finally , By : {GetUserName()} At: {DateTime.Now}";
                        inward.verifyStatus = 1;
                        inward.SecoundLevelDate = DateTime.Now;
                        inward.SecoundLevelStatus += " | Accept";
                        inward.SecoundLevelUser += " | " + GetUserName();
                        inward.FinalRetCode = ""; // Original VB.NET had this empty
                        wfHistory.ID_WFStatus = WFHistory.WF_Status.TechnicallyAccepted;
                        wfHistory.Status = $"VerifiedTechnically Accepted By :{GetUserName()} At: {DateTime.Now}";
                    }
                    _context.WFHistories.Add(wfHistory);
                    await _context.SaveChangesAsync();

                    // SMS Logic (needs actual implementation)
                    // if (SMS_STATUS == "LIVE") { ... }
                }

                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Operation completed successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in InwardFinanicalWFDetailsPMADIS_Auth for Serial: {model.Serial}");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ReversePostingPMARAM(string id, string MQ_MSG)
        {
            if (string.IsNullOrEmpty(GetUserName()))
            {
                return RedirectToAction("Login", "Login");
            }

            string u = "";
            var _json = new JsonResult(new { retsrep = u });
            int _step = 90000;

            try
            {
                _logger.LogInformation($"ReversePostingPMARAM called for Serial: {id} with MQ_MSG: {MQ_MSG}");

                var inward = await _context.Inward_Trans.SingleOrDefaultAsync(i => i.Serial == id);
                if (inward == null)
                {
                    _logger.LogWarning($"Inward_Trans not found for Serial: {id}");
                    return Json(new { retsrep = "Error: Inward transaction not found." });
                }

                _step += 1;

                // Assuming ECC_Handler_SVC.InwardHandlingSVCSoapClient and HandelResponseONUS are external service calls
                // This part needs actual service integration
                // For now, mocking the response
                // var WebSvc = new ECC_Handler_SVC.InwardHandlingSVCSoapClient("InwardHandlingSVCSoap");
                // var obj_ = WebSvc.HandelResponseONUS(inward.ChqSequance, inward.ClrCenter, "ReversePosting", "", inward.Serial, GetUserID().ToString());

                // Mocking successful response for now
                bool serviceCallSuccess = true; 

                if (serviceCallSuccess)
                {
                    inward.Posted = 0;
                    inward.History += $"ReversePostingPMARAM  Done By {GetUserName()} FROM VIP CHQ AT {DateTime.Now}";
                    await _context.SaveChangesAsync();

                    var wfHistory = new WFHistory
                    {
                        ChqSequance = inward.ChqSequance,
                        Serial = inward.Serial,
                        TransDate = inward.TransDate,
                        ID_WFStatus = WFHistory.WF_Status.NeedManualFix, // Assuming this status for reverse posting
                        Status = $"ReversePostingPMARAM Done By {GetUserName()} AT {DateTime.Now}",
                        ClrCenter = inward.ClrCenter,
                        DrwChqNo = inward.DrwChqNo,
                        DrwAccNo = inward.DrwAccNo,
                        Amount = inward.Amount
                    };
                    _context.WFHistories.Add(wfHistory);
                    await _context.SaveChangesAsync();

                    u = "ReversePostingPMARAM Done";
                }
                else
                {
                    inward.ErrorDescription = "ReversePostingPMARAM Falid";
                    inward.T24Response = "ReversePostingPMARAM Falid";
                    inward.ReturnDate = DateTime.Now;
                    inward.History += $"ReversePostingPMARAM  Falid  By{GetUserName()} FROM VIP CHQ AT {DateTime.Now}";
                    await _context.SaveChangesAsync();
                    u = "ReversePostingPMARAM Falid";
                }
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, $"Database update error in ReversePostingPMARAM for Serial: {id}");
                // Handle specific database validation errors if needed
                u = "Error: Database update failed.";
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage = ex.InnerException.Message;
                }
                _logger.LogError(ex, $"Error in ReversePostingPMARAM for Serial: {id}. Details: {errorMessage}");
                u = $"Error: {errorMessage}";
            }

            _json = Json(new { retsrep = u });
            return _json;
        }
    }

    // Enums and helper classes (from VB.NET) - these should ideally be in separate files/namespaces
    public class GroupType
    {
        public enum Group_Status
        {
            Inquiry = 1,
            Inputter = 2,
            Authorized = 4,
            SystemAdmin = 13,
            ItGroup = 14,
            AdminAuthorized = 17
        }
    }

    public class WFHistory
    {
        public enum WF_Status
        {
            UnderTechnicalVerification = 1,
            TechnicallyAccepted = 2,
            InitialTechnicalRejection = 3,
            FinalTechnicalRejection = 4,
            [End] = 5,
            NeedManualFix = 6,
            ManuallyFixed = 7,
            NotFixed = 8,
            ProticionAccountFlag = 9,
            ChequeAlreadyStopped = 10,
            UnderCreditApproval = 11,
            CreditApproved = 12,
            CreditDeclined = 13,
            CustomerDisesed = 14,
            CustomerDisesedDeclined = 15,
            CustomerDisesedApproved = 16
        }

        public string ChqSequance { get; set; }
        public string Serial { get; set; }
        public DateTime? TransDate { get; set; }
        public WF_Status ID_WFStatus { get; set; }
        public string Status { get; set; }
        public string ClrCenter { get; set; }
        public string DrwChqNo { get; set; }
        public string DrwAccNo { get; set; }
        public decimal? Amount { get; set; }
    }

    public class TreeNode
    {
        public string SubMenu_Name_EN { get; set; }
        public int SubMenu_ID { get; set; }
        public int Related_Page_ID { get; set; }
        public List<TreeNode> Children { get; set; }
    }

    public class Category
    {
        public int SubMenu_ID { get; set; }
        public int? Parent_ID { get; set; }
        public string SubMenu_Name_EN { get; set; }
        public int Related_Page_ID { get; set; }
    }

    public class GET_RETURN_CHQ
    {
        public string clrcenter { get; set; } = "";
        public int count { get; set; }
        public DateTime valuedate { get; set; }
        public string branch { get; set; } = "";
        public string currancy { get; set; } = "";
    }

    public class INChqs
    {
        public string GUAR_CUSTOMER { get; set; }
        public string BookedBalance { get; set; }
        public string ClearBalance { get; set; }
        public string AccountStatus { get; set; }
        public string Reject { get; set; }
        public string Recom { get; set; }
        public string Approve { get; set; }
    }

}

