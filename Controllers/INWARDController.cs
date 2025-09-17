using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SAFA_ECC_Core_Clean.Data;
using SAFA_ECC_Core_Clean.Models;
using SAFA_ECC_Core_Clean.ViewModels.InwardViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SAFA_ECC_Core_Clean.Controllers
{
    [Authorize]
    public class INWARDController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<INWARDController> _logger;
        // Assuming these are external services or helper classes, mock them for now
        // private readonly All_CLASSES.AllStoredProcesures _LogSystem;
        // private readonly ECC_CAP_Services.SAFA_T24_ECC_SVCSoapClient _EccAccInfo_WebSvc;
        // private readonly ECC_Handler_SVC.InwardHandlingSVCSoapClient _WebSvc;

        public INWARDController(ApplicationDbContext context, ILogger<INWARDController> logger)
        {
            _context = context;
            _logger = logger;
            // Initialize mock services if needed
            // _LogSystem = new All_CLASSES.AllStoredProcesures(true, true, true, "", "", "", "");
            // _EccAccInfo_WebSvc = new ECC_CAP_Services.SAFA_T24_ECC_SVCSoapClient("SAFA_T24_ECC_SVCSoap");
            // _WebSvc = new ECC_Handler_SVC.InwardHandlingSVCSoapClient("InwardHandlingSVCSoap");
        }

        private string GetUserName() => User.FindFirstValue(ClaimTypes.Name);
        private string GetBranchID() => User.FindFirstValue("BranchID"); // Assuming custom claim
        private string GetComID() => User.FindFirstValue("ComID"); // Assuming custom claim
        private int GetUserID() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)); // Assuming User ID is stored in NameIdentifier

        // Helper for permission check (simplified for now)
        private async Task<bool> HasPermission(string actionName)
        {
            // Implement actual permission logic based on your application's requirements
            // For now, assume all authenticated users have access
            return User.Identity.IsAuthenticated;
        }

        // GET: INWARD/Index
        public IActionResult Index()
        {
            return View();
        }

        // GET: INWARD/Indextest
        public IActionResult Indextest(string id)
        {
            return View();
        }

        // GET: INWARD/Reject_CHQ
        public async Task<IActionResult> Reject_CHQ(string chequeId)
        {
            if (!await HasPermission("Reject_CHQ"))
                return Forbid();

            var model = new RejectChequeViewModel();
            // Mock data retrieval for demonstration
            if (!string.IsNullOrEmpty(chequeId))
            {
                // In a real app, fetch cheque details from DB/service
                model.ChequeId = chequeId;
                model.ChequeNumber = "123456";
                model.Amount = 1500.00m;
                model.RejectionReason = "";
            }
            return View(model);
        }

        // POST: INWARD/Reject_CHQ
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject_CHQ(RejectChequeViewModel model)
        {
            if (!await HasPermission("Reject_CHQ"))
                return Forbid();

            if (ModelState.IsValid)
            {
                // Simulate saving rejection details
                _logger.LogInformation($"Cheque {model.ChequeId} rejected with reason: {model.RejectionReason}");
                TempData["SuccessMessage"] = "تم رفض الشيك بنجاح.";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = "حدث خطأ أثناء رفض الشيك.";
            return View(model);
        }

        // GET: INWARD/ReturnOnUsStoppedChequeDetails
        public async Task<IActionResult> ReturnOnUsStoppedChequeDetails(string chequeId)
        {
            if (!await HasPermission("ReturnOnUsStoppedChequeDetails"))
                return Forbid();

            var model = new ReturnOnUsStoppedChequeDetailsViewModel();
            if (!string.IsNullOrEmpty(chequeId))
            {
                // Mock data retrieval
                model.ChequeId = chequeId;
                model.AccountNumber = "987654321";
                model.ChequeNumber = "654321";
                model.Amount = 2500.00m;
                model.BeneficiaryName = "Ahmed Ali";
                model.StopReason = "Lost Cheque";
            }
            return View(model);
        }

        // Function InwardFinanicalWFDetailsPMADIS
        public async Task<IActionResult> InwardFinanicalWFDetailsPMADIS(string id)
        {
            if (string.IsNullOrEmpty(GetUserName()))
            {
                return RedirectToAction("Login", "Login");
            }

            // Session.Item("ErrorMessage") = ""; // Handled by TempData or similar in ASP.NET Core
            // Dim _json As New JsonResult; _json.Data = ""; // Not directly applicable in MVC Core Action

            string methodName = "InwardFinanicalWFDetailsPMADIS";
            int userId = GetUserID();
            string userName = GetUserName();
            string branchId = GetBranchID();
            string comId = GetComID();

            try
            {
                _logger.LogInformation($"Show Cheque InwardFinanicalWFDetailsPMADIS from Inward_Trans table for ID: {id}");

                var wf = await _context.INWARD_WF_Tbl.SingleOrDefaultAsync(z => z.Serial == id && z.Final_Status != "Accept");
                var incObj = await _context.Inward_Trans.SingleOrDefaultAsync(y => y.Serial == id);

                if (incObj == null)
                {
                    return RedirectToAction("InsufficientFunds", "INWARD");
                }

                var model = new InwardFinanicalWFDetailsPMADISViewModel
                {
                    Serial = incObj.Serial,
                    ClrCenter = incObj.ClrCenter,
                    VIP = incObj.VIP,
                    Amount = incObj.Amount,
                    ISSAccount = incObj.ISSAccount,
                    AltAccount = incObj.AltAccount,
                    // Populate other properties from incObj and wf
                };

                if (incObj.ClrCenter == "PMA" && incObj.VIP == true && branchId != "2")
                {
                    model.IsVIP = "YES";
                }

                // Mocking external service calls
                // var EccAccInfo_WebSvc = new ECC_CAP_Services.SAFA_T24_ECC_SVCSoapClient("SAFA_T24_ECC_SVCSoap");
                // var Accobj = EccAccInfo_WebSvc.ACCOUNT_INFO(incObj.AltAccount, 1);
                // model.BookedBalance = Accobj.BookedBalance;
                // model.ClearBalance = Accobj.ClearBalance;
                // model.AccountStatus = Accobj.AccountStatus;

                // Mocking GUAR_CUSTOMER logic
                // var GUAR_CUSTOMER = await _context.T24_CAB_OVRDRWN_GUAR.Where(i => i.GUAR_CUSTOMER == incObj.ISSAccount).ToListAsync();
                // if (GUAR_CUSTOMER.Count == 0)
                // { model.GUAR_CUSTOMER_Info = "Not Available"; }
                // else
                // { /* Populate GUAR_CUSTOMER_Info */ }

                var userGroup = await _context.Users_Tbl.Where(u => u.UserName == userName).Select(u => u.Group_ID).FirstOrDefaultAsync();

                model.Reject = "False";
                model.Recom = "True";

                // Assuming GroupType.Group_Status.AdminAuthorized is an enum or constant
                // For now, hardcode or define constants for GroupType values
                const int AdminAuthorizedGroup = 4; // Example value

                if (userGroup == AdminAuthorizedGroup || branchId == "2")
                {
                    model.Approve = "True";
                    model.Recom = "False";
                }
                else
                {
                    // Mocking Userlevel and AuthTrans_User_TBL logic
                    // var Tbl_id = await _context.Transaction_TBL.Where(z => z.Transaction_Name == wf.Clr_Center).Select(z => z.Transaction_ID).FirstOrDefaultAsync();
                    // var Userlevel = await _context.USER_Limits_Auth_Amount(userId, Tbl_id, "d", wf.Amount_JD).ToListAsync();
                    // model.Recom = "True";

                    // var x = await _context.AuthTrans_User_TBL.SingleOrDefaultAsync(t => t.Auth_user_ID == userId && t.Trans_id == "5" && t.group_ID == userGroup);
                    // model.IsAuthorized = (x != null && x.Value == true);
                }

                // More complex logic for different levels and statuses would go here

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in InwardFinanicalWFDetailsPMADIS for ID: {id}. Error: {ex.Message}");
                // Handle error, maybe redirect to an error page or show a message
                return RedirectToAction("Error", "Home");
            }
        }

        // Function InwardFinanicalWFDetailsPMADIS_Auth
        public async Task<JsonResult> InwardFinanicalWFDetailsPMADIS_Auth(string id, string Status, string app, string page)
        {
            var jsonResult = new JsonResult(new { success = false, message = "" });
            int groupId = 0;
            int appId = 0;
            int pageId = 0;
            if (!int.TryParse(id, out groupId) || !int.TryParse(app, out appId) || !int.TryParse(page, out pageId))
            {
                jsonResult = new JsonResult(new { success = false, message = "Invalid input parameters." });
                return jsonResult;
            }
            try
            {
                _logger.LogInformation($"Start with InwardFinanicalWFDetailsPMADIS_Auth for GroupID: {groupId}, Status: {Status}");
                var groupPermissionAuth = await _context.Groups_Permissions_Auth
                    .SingleOrDefaultAsync(x => x.Application_ID == appId && x.Group_Id == groupId && x.Page_Id == pageId);
                if (groupPermissionAuth != null)
                {
                    var groupPermission = await _context.Groups_Permissions
                        .SingleOrDefaultAsync(x => x.Application_ID == appId && x.Group_Id == groupId && x.Page_Id == pageId);
                    if (Status == "1") // Accept
                    {
                        if (groupPermission == null)
                        {
                            groupPermission = new Groups_Permissions
                            {
                                Group_Id = groupPermissionAuth.Group_Id,
                                Application_ID = groupPermissionAuth.Application_ID,
                                Page_Id = groupPermissionAuth.Page_Id,
                                Add = groupPermissionAuth.Add,
                                Reverse = groupPermissionAuth.Reverse,
                                Post = groupPermissionAuth.Post,
                                Delete = groupPermissionAuth.Delete,
                                Update = groupPermissionAuth.Update,
                                Access = groupPermissionAuth.Access,
                                // Assuming 'Reverse' in VB was meant for a boolean flag, mapping to 'Reverse' in C# model
                                Reverse = groupPermissionAuth.Reverse 
                            };
                            _context.Groups_Permissions.Add(groupPermission);
                        }
                        else
                        {
                            groupPermission.Add = groupPermissionAuth.Add;
                            groupPermission.Reverse = groupPermissionAuth.Reverse;
                            groupPermission.Post = groupPermissionAuth.Post;
                            groupPermission.Delete = groupPermissionAuth.Delete;
                            groupPermission.Update = groupPermissionAuth.Update;
                            groupPermission.Access = groupPermissionAuth.Access;
                            groupPermission.Reverse = groupPermissionAuth.Reverse;
                        }
                        groupPermissionAuth.status = "Accept";
                    }
                    else // Reject
                    {
                        groupPermissionAuth.status = "Reject";
                    }
                    await _context.SaveChangesAsync();
                    // Log History
                    var history = new Groups_Permissions_History
                    {
                        Group_Id = groupPermissionAuth.Group_Id,
                        Application_ID = groupPermissionAuth.Application_ID,
                        Page_Id = groupPermissionAuth.Page_Id,
                        Add = groupPermissionAuth.Add,
                        Reverse = groupPermissionAuth.Reverse,
                        Post = groupPermissionAuth.Post,
                        Delete = groupPermissionAuth.Delete,
                        Update = groupPermissionAuth.Update,
                        Access = groupPermissionAuth.Access,
                        Reverse = groupPermissionAuth.Reverse,
                        UserName = GetUserName(),
                        Updatedate = DateTime.Now
                    };
                    _context.Groups_Permissions_History.Add(history);
                    await _context.SaveChangesAsync();
                    jsonResult = new JsonResult(new { success = true, message = "Operation successful." });
                }
                else
                {
                    jsonResult = new JsonResult(new { success = false, message = "Group permission authorization record not found." });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in InwardFinanicalWFDetailsPMADIS_Auth for GroupID: {groupId}. Error: {ex.Message}");
                jsonResult = new JsonResult(new { success = false, message = "An error occurred during the operation." });
            }
            return jsonResult;
        }
    }
}

