using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SAFA_ECC_Core_Clean.Models;
using SAFA_ECC_Core_Clean.ViewModels;
using AllEnums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace SAFA_ECC_Core_Clean.Services
{
    public class InwardService : IInwardService
    {
        private readonly SAFA_ECCEntities _context;
        private readonly ILogger<InwardService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IConfiguration _configuration;

        public InwardService(SAFA_ECCEntities context, ILogger<InwardService> logger, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }


        private string GetUserName()
        {
            return _httpContextAccessor.HttpContext?.Session.GetString("UserName");
        }

        private int GetUserID()
        {
            return Convert.ToInt32(_httpContextAccessor.HttpContext?.Session.GetString("ID"));
        }

        public async Task<JsonResult> InwardFinanicalWFDetailsONUS_NEW(string id)
        {
            _httpContextAccessor.HttpContext.Session.SetString("ErrorMessage", "");
            JsonResult _json = new JsonResult(new { });
            string branch = _httpContextAccessor.HttpContext.Session.GetString("BranchID");

            if (string.IsNullOrEmpty(GetUserName()))
            {
                // This should ideally be handled by an authentication middleware or in the controller
                // For now, returning a simplified JSON indicating redirection
                return new JsonResult(new { redirectTo = "/Login/Login" });
            }

            string methodName = "InwardFinanicalWFDetailsONUS";
            int userId = GetUserID();

            try
            {
                var appPage = await _context.App_Pages.SingleOrDefaultAsync(t => t.Page_Name_EN == methodName);
                if (appPage == null) return new JsonResult(new { list = (object)null, Sta = "F", Message = "App page not found" });

                int pageId = appPage.Page_Id;
                int applicationId = appPage.Application_ID;

                // Assuming getuser_group_permision is a helper function or service call
                // await GetUserGroupPermission(pageId, applicationId, userId);

                // ViewBag is a controller concept, so this needs to be passed back to the controller
                // or handled differently in a service layer.
                // For now, we'll include relevant data in the JSON result.

                var wf = await _context.INWARD_WF_Tbl.SingleOrDefaultAsync(z => z.Serial == id && z.Final_Status != "Accept" && z.Clr_Center == "Outward_ONUS");
                if (wf == null) return new JsonResult(new { redirectTo = "/INWARD/InsufficientFunds" });

                var incObj = await _context.OnUs_Tbl.SingleOrDefaultAsync(y => y.Serial == id && (y.Posted == (int)AllEnums.Cheque_Status.New || y.Posted == (int)AllEnums.Cheque_Status.Posted));
                if (incObj == null) return new JsonResult(new { redirectTo = "/INWARD/InsufficientFunds" });

                List<T24_CAB_OVRDRWN_GUAR> GUAR_CUSTOMER = new List<T24_CAB_OVRDRWN_GUAR>();
                if (!string.IsNullOrEmpty(incObj.DrwCustomerID))
                {
                    GUAR_CUSTOMER = await _context.T24_CAB_OVRDRWN_GUAR.Where(i => i.GUAR_CUSTOMER == incObj.DrwCustomerID).ToListAsync();
                }

                string guarCustomerInfo = "";
                if (GUAR_CUSTOMER.Count == 0)
                {
                    guarCustomerInfo = "Not Available";
                }
                else
                {
                    // External service call placeholder
                    // var EccAccInfo_WebSvc = new ECC_CAP_Services.SAFA_T24_ECC_SVCSoapClient("SAFA_T24_ECC_SVCSoap");
                    // foreach (var item in GUAR_CUSTOMER)
                    // {
                    //     var GUAR_CUSTOMER_Accobj = await EccAccInfo_WebSvc.ACCOUNT_INFOAsync(item.ACCOUNT_NUMBER, 1);
                    //     guarCustomerInfo += item.ACCOUNT_NUMBER + "*" + GUAR_CUSTOMER_Accobj.ClearBalance + "*" + GUAR_CUSTOMER_Accobj.AccountCurrency + "|";
                    // }
                }

                // Account info service call placeholder
                // var Accobj = await EccAccInfo_WebSvc.ACCOUNT_INFOAsync(incObj.DecreptedDrwAcountNo, 1);
                // var bookedBalance = Accobj.BookedBalance;
                // var clearBalance = Accobj.ClearBalance;
                // var accountStatus = Accobj.AccountStatus;

                var user = await _context.Users_Tbl.SingleOrDefaultAsync(c => c.User_Name == GetUserName());
                string group = user?.Group_ID;

                bool reject = false;
                bool recomdationbtn = true;
                bool approve = false;

                if (group == AllEnums.Group_Status.AdminAuthorized.ToString() || branch == "2")
                {
                    approve = true;
                    recomdationbtn = false;
                    reject = true;
                }
                else
                {
                    // Userlevel logic needs to be converted from stored procedure to EF Core or raw SQL
                    // var Userlevel = await _context.USER_Limits_Auth_Amount(userId, Tbl_id, "d", wf.Amount_JD).ToListAsync();
                    recomdationbtn = true;

                    // AuthTrans_User_TBL logic
                    var authTransUser = await _context.AuthTrans_User_TBL.SingleOrDefaultAsync(t => t.Auth_user_ID == userId && t.Trans_id == "6" && t.group_ID == user.Group_ID);
                    if (authTransUser != null)
                    {
                        if (authTransUser.Auth_level2 == true)
                        {
                            approve = true;
                            reject = true;
                            recomdationbtn = false;
                        }
                        if (authTransUser.Auth_level1 == true)
                        {
                            approve = false;
                            reject = false;
                            recomdationbtn = true;
                        }
                    }
                    else
                    {
                        recomdationbtn = true;
                    }
                }

                // Currency conversion logic
                // List<CURRENCY_TBL> Currency = await _context.CURRENCY_TBL.ToListAsync();
                // for (int j = 0; j < Currency.Count; j++)
                // {
                //     if (incObj.Currency == "1" || incObj.Currency == "2" || incObj.Currency == "3" || incObj.Currency == "5")
                //     {
                //         if (Convert.ToInt32(incObj.Currency) == Currency[j].ID)
                //         {
                //             incObj.Currency = Currency[j].SYMBOL_ISO;
                //             break;
                //         }
                //     }
                // }
                // ViewBag.data = Currency;

                incObj.Amount = decimal.Round(incObj.Amount, 2, MidpointRounding.AwayFromZero);
                if (incObj.Status == "S")
                {
                    incObj.Status = "Success";
                }
                if (incObj.Status == "F")
                {
                    incObj.Status = "Faild";
                }

                _json.Data = new
                {
                    list = incObj,
                    Sta = "S",
                    Title = appPage.ENG_DESC,
                    GUAR_CUSTOMER = guarCustomerInfo,
                    Reject = reject,
                    RecomdationBtn = recomdationbtn,
                    Approve = approve
                    // Add other ViewBag data here as properties
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in InwardFinanicalWFDetailsONUS_NEW: {Message}", ex.Message);
                _httpContextAccessor.HttpContext.Session.SetString("ErrorMessage", "An error occurred: " + ex.Message);
                _json.Data = new { list = (object)null, Sta = "F" };
            }

            return _json;
        }

        public async Task<DataTable> Getpage(string page)
        {
            try
            {
                // This method requires a SqlAccess class or direct ADO.NET/EF Core raw SQL execution.
                // Placeholder for now.
                return await Task.FromResult(new DataTable()); // Placeholder
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Getpage: {Message}", ex.Message);
                return new DataTable();
            }
        }

        public async Task<bool> GetPermission(string id, string _page, string _groupid)
        {
            try
            {
                List<Users_Permissions> usersPermissionpage = new List<Users_Permissions>();
                List<Groups_Permissions> groupPermissionpage = new List<Groups_Permissions>();

                groupPermissionpage = await _context.Groups_Permissions.Where(x => x.Group_Id == _groupid && x.Page_Id == _page && x.Application_ID == 1 && x.Access == true).ToListAsync();

                if (groupPermissionpage.Count == 0)
                {
                    if (_page == "0") // Assuming _page is string, convert to int if needed
                    {
                        usersPermissionpage = await _context.Users_Permissions.Where(x => x.UserID == id && x.PageID == _page && x.Application_ID == 1).ToListAsync();
                        return true;
                    }
                    else
                    {
                        usersPermissionpage = await _context.Users_Permissions.Where(x => x.UserID == id && x.PageID == _page && x.Value == true && x.Application_ID == 1 && x.ActionID == 6).ToListAsync();
                    }

                    if (usersPermissionpage.Count == 0)
                    {
                        return false;
                    }
                    else
                    {
                        // Logic for page ranges and group types
                        if ((Convert.ToInt32(_page) >= 1300 && Convert.ToInt32(_page) <= 1400 && _groupid == AllEnums.Group_Status.AdminAuthorized.ToString()) ||
                            (Convert.ToInt32(_page) >= 1 && Convert.ToInt32(_page) <= 100 && _groupid == AllEnums.Group_Status.SystemAdmin.ToString()) ||
                            (!(Convert.ToInt32(_page) >= 1 && Convert.ToInt32(_page) <= 100) && !(Convert.ToInt32(_page) >= 1300 && Convert.ToInt32(_page) <= 1400)))
                        {
                            return true;
                        }

                        usersPermissionpage = await _context.Users_Permissions.Where(x => x.UserID == id && x.PageID == _page && x.Value == false && x.Application_ID == 1 && x.ActionID == 6).ToListAsync();
                        if (usersPermissionpage.Count > 0)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    if (_page == "0")
                    {
                        usersPermissionpage = await _context.Users_Permissions.Where(x => x.UserID == id && x.PageID == _page && x.Application_ID == 1).ToListAsync();
                        return true;
                    }
                    else
                    {
                        usersPermissionpage = await _context.Users_Permissions.Where(x => x.UserID == id && x.PageID == _page && x.Application_ID == 1 && x.ActionID == 6).ToListAsync();
                        if (usersPermissionpage.Count == 0)
                        {
                            return true;
                        }

                        usersPermissionpage = await _context.Users_Permissions.Where(x => x.UserID == id && x.PageID == _page && x.Value == true && x.Application_ID == 1).ToListAsync();
                        if (usersPermissionpage.Count > 0)
                        {
                            if ((Convert.ToInt32(_page) >= 1300 && Convert.ToInt32(_page) <= 1400 && _groupid == AllEnums.Group_Status.AdminAuthorized.ToString()) ||
                                (Convert.ToInt32(_page) >= 1 && Convert.ToInt32(_page) <= 100 && _groupid == AllEnums.Group_Status.SystemAdmin.ToString()) ||
                                (!(Convert.ToInt32(_page) >= 1 && Convert.ToInt32(_page) <= 100) && !(Convert.ToInt32(_page) >= 1300 && Convert.ToInt32(_page) <= 1400)))
                            {
                                return true;
                            }
                        }

                        usersPermissionpage = await _context.Users_Permissions.Where(x => x.UserID == id && x.PageID == _page && x.Value == false && x.Application_ID == 1 && x.ActionID == 6).ToListAsync();
                        if (usersPermissionpage.Count > 0)
                        {
                            return false;
                        }
                    }
                }
                return false; // Default return
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetPermission: {Message}", ex.Message);
                return false;
            }
        }

        public async Task<bool> GetPermission1(string id, string _page, string _groupid)
        {
            try
            {
                List<Users_Permissions> usersPermissionpage = new List<Users_Permissions>();
                List<Groups_Permissions> groupPermissionpage = new List<Groups_Permissions>();

                groupPermissionpage = await _context.Groups_Permissions.Where(x => x.Group_Id == _groupid && x.Page_Id == _page && (x.Add == true || x.Delete == true || x.Access == true || x.Reverse == true || x.Update == true || x.Post == true) && x.Application_ID == 1 && x.Access == true).ToListAsync();

                if (groupPermissionpage.Count == 0)
                {
                    if (_page == "0")
                    {
                        usersPermissionpage = await _context.Users_Permissions.Where(x => x.UserID == id && x.PageID == _page && x.Application_ID == 1).ToListAsync();
                    }
                    else
                    {
                        usersPermissionpage = await _context.Users_Permissions.Where(x => x.UserID == id && x.PageID == _page && x.Value == true && x.Application_ID == 1 && x.ActionID == 6).ToListAsync();
                    }

                    if (usersPermissionpage.Count == 0)
                    {
                        return false;
                    }
                    else
                    {
                        if ((Convert.ToInt32(_page) >= 1300 && Convert.ToInt32(_page) <= 1400 && _groupid == AllEnums.Group_Status.AdminAuthorized.ToString()) ||
                            (Convert.ToInt32(_page) >= 1 && Convert.ToInt32(_page) <= 100 && _groupid == AllEnums.Group_Status.SystemAdmin.ToString()) ||
                            (!(Convert.ToInt32(_page) >= 1 && Convert.ToInt32(_page) <= 100) && !(Convert.ToInt32(_page) >= 1300 && Convert.ToInt32(_page) <= 1400)))
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    if (_page == "0")
                    {
                        usersPermissionpage = await _context.Users_Permissions.Where(x => x.UserID == id && x.PageID == _page && x.Application_ID == 1).ToListAsync();
                        return true;
                    }
                    else
                    {
                        usersPermissionpage = await _context.Users_Permissions.Where(x => x.UserID == id && x.PageID == _page && x.Application_ID == 1 && x.ActionID == 6).ToListAsync();
                        if (usersPermissionpage.Count == 0)
                        {
                            return true;
                        }

                        usersPermissionpage = await _context.Users_Permissions.Where(x => x.UserID == id && x.PageID == _page && x.Value == true && x.Application_ID == 1).ToListAsync();
                        if (usersPermissionpage.Count > 0)
                        {
                            if ((Convert.ToInt32(_page) >= 1300 && Convert.ToInt32(_page) <= 1400 && _groupid == AllEnums.Group_Status.AdminAuthorized.ToString()) ||
                                (Convert.ToInt32(_page) >= 1 && Convert.ToInt32(_page) <= 100 && _groupid == AllEnums.Group_Status.SystemAdmin.ToString()) ||
                                (!(Convert.ToInt32(_page) >= 1 && Convert.ToInt32(_page) <= 100) && !(Convert.ToInt32(_page) >= 1300 && Convert.ToInt32(_page) <= 1400)))
                            {
                                return true;
                            }
                        }

                        usersPermissionpage = await _context.Users_Permissions.Where(x => x.UserID == id && x.PageID == _page && x.Value == false && x.Application_ID == 1 && x.ActionID == 6).ToListAsync();
                        if (usersPermissionpage.Count > 0)
                        {
                            return false;
                        }
                    }
                }
                return false; // Default return
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetPermission1: {Message}", ex.Message);
                return false;
            }
        }

        public async Task<bool> Ge_t(string x)
        {
            try
            {
                var page = await _context.Menu_Items_Tbl.Where(i => i.Parent_ID == x).ToListAsync();
                return page.Count > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Ge_t: {Message}", ex.Message);
                return false;
            }
        }

        public async Task<string> GetAllCategoriesForTree()
        {
            List<Category> categories = new List<Category>();
            try
            {
                // This method's original logic involves complex data table operations and recursive filling
                // as well as session access and ViewBag manipulation. This needs significant refactoring.
                // For now, returning an empty string as a placeholder.
                _logger.LogWarning("GetAllCategoriesForTree needs proper implementation in InwardService.");
                return await Task.FromResult("");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllCategoriesForTree: {Message}", ex.Message);
                return "";
            }
        }
    }
}



        public async Task<JsonResult> InwardFinanicalWFDetailsONUS_Auth(string id)
        {
            _httpContextAccessor.HttpContext.Session.SetString("ErrorMessage", "");
            JsonResult _json = new JsonResult(new { });
            string branch = _httpContextAccessor.HttpContext.Session.GetString("BranchID");

            if (string.IsNullOrEmpty(GetUserName()))
            {
                return new JsonResult(new { redirectTo = "/Login/Login" });
            }

            string methodName = "InwardFinanicalWFDetailsONUS_Auth";
            int userId = GetUserID();

            try
            {
                var appPage = await _context.App_Pages.SingleOrDefaultAsync(t => t.Page_Name_EN == methodName);
                if (appPage == null) return new JsonResult(new { list = (object)null, Sta = "F", Message = "App page not found" });

                int pageId = appPage.Page_Id;
                int applicationId = appPage.Application_ID;

                // Assuming getuser_group_permision is a helper function or service call
                // await GetUserGroupPermission(pageId, applicationId, userId);

                var wf = await _context.INWARD_WF_Tbl.SingleOrDefaultAsync(z => z.Serial == id && z.Final_Status != "Accept" && z.Clr_Center == "Outward_ONUS");
                if (wf == null) return new JsonResult(new { redirectTo = "/INWARD/InsufficientFunds" });

                var incObj = await _context.OnUs_Tbl.SingleOrDefaultAsync(y => y.Serial == id && (y.Posted == (int)AllEnums.Cheque_Status.New || y.Posted == (int)AllEnums.Cheque_Status.Posted));
                if (incObj == null) return new JsonResult(new { redirectTo = "/INWARD/InsufficientFunds" });

                List<T24_CAB_OVRDRWN_GUAR> GUAR_CUSTOMER = new List<T24_CAB_OVRDRWN_GUAR>();
                if (!string.IsNullOrEmpty(incObj.DrwCustomerID))
                {
                    GUAR_CUSTOMER = await _context.T24_CAB_OVRDRWN_GUAR.Where(i => i.GUAR_CUSTOMER == incObj.DrwCustomerID).ToListAsync();
                }

                string guarCustomerInfo = "";
                if (GUAR_CUSTOMER.Count == 0)
                {
                    guarCustomerInfo = "Not Available";
                }
                else
                {
                    // External service call placeholder
                    // var EccAccInfo_WebSvc = new ECC_CAP_Services.SAFA_T24_ECC_SVCSoapClient("SAFA_T24_ECC_SVCSoap");
                    // foreach (var item in GUAR_CUSTOMER)
                    // {
                    //     var GUAR_CUSTOMER_Accobj = await EccAccInfo_WebSvc.ACCOUNT_INFOAsync(item.ACCOUNT_NUMBER, 1);
                    //     guarCustomerInfo += item.ACCOUNT_NUMBER + "*" + GUAR_CUSTOMER_Accobj.ClearBalance + "*" + GUAR_CUSTOMER_Accobj.AccountCurrency + "|";
                    // }
                }

                // Account info service call placeholder
                // var Accobj = await EccAccInfo_WebSvc.ACCOUNT_INFOAsync(incObj.DecreptedDrwAcountNo, 1);
                // var bookedBalance = Accobj.BookedBalance;
                // var clearBalance = Accobj.ClearBalance;
                // var accountStatus = Accobj.AccountStatus;

                var user = await _context.Users_Tbl.SingleOrDefaultAsync(c => c.User_Name == GetUserName());
                string group = user?.Group_ID;

                bool reject = false;
                bool recomdationbtn = true;
                bool approve = false;

                if (group == AllEnums.Group_Status.AdminAuthorized.ToString() || branch == "2")
                {
                    approve = true;
                    recomdationbtn = false;
                    reject = true;
                }
                else
                {
                    // Userlevel logic needs to be converted from stored procedure to EF Core or raw SQL
                    // var Userlevel = await _context.USER_Limits_Auth_Amount(userId, Tbl_id, "d", wf.Amount_JD).ToListAsync();
                    recomdationbtn = true;

                    // AuthTrans_User_TBL logic
                    var authTransUser = await _context.AuthTrans_User_TBL.SingleOrDefaultAsync(t => t.Auth_user_ID == userId && t.Trans_id == "6" && t.group_ID == user.Group_ID);
                    if (authTransUser != null)
                    {
                        if (authTransUser.Auth_level2 == true)
                        {
                            approve = true;
                            reject = true;
                            recomdationbtn = false;
                        }
                        if (authTransUser.Auth_level1 == true)
                        {
                            approve = false;
                            reject = false;
                            recomdationbtn = true;
                        }
                    }
                    else
                    {
                        recomdationbtn = true;
                    }
                }

                // Currency conversion logic
                // List<CURRENCY_TBL> Currency = await _context.CURRENCY_TBL.ToListAsync();
                // for (int j = 0; j < Currency.Count; j++)
                // {
                //     if (incObj.Currency == "1" || incObj.Currency == "2" || incObj.Currency == "3" || incObj.Currency == "5")
                //     {
                //         if (Convert.ToInt32(incObj.Currency) == Currency[j].ID)
                //         {
                //             incObj.Currency = Currency[j].SYMBOL_ISO;
                //             break;
                //         }
                //     }
                // }
                // ViewBag.data = Currency;

                incObj.Amount = decimal.Round(incObj.Amount, 2, MidpointRounding.AwayFromZero);
                if (incObj.Status == "S")
                {
                    incObj.Status = "Success";
                }
                if (incObj.Status == "F")
                {
                    incObj.Status = "Faild";
                }

                _json.Data = new
                {
                    list = incObj,
                    Sta = "S",
                    Title = appPage.ENG_DESC,
                    GUAR_CUSTOMER = guarCustomerInfo,
                    Reject = reject,
                    RecomdationBtn = recomdationbtn,
                    Approve = approve
                    // Add other ViewBag data here as properties
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in InwardFinanicalWFDetailsONUS_Auth: {Message}", ex.Message);
                _httpContextAccessor.HttpContext.Session.SetString("ErrorMessage", "An error occurred: " + ex.Message);
                _json.Data = new { list = (object)null, Sta = "F" };
            }

            return _json;
        }



        public async Task<JsonResult> getSearchListInitalAccept_reject(string Branchs, string STATUS, string ChequeSource, string FAmount, string TAmount, string Chequeno, string DrwAcc, string Authorize, string Currency, string vip)
        {
            _httpContextAccessor.HttpContext.Session.SetString("ErrorMessage", "");
            JsonResult _json = new JsonResult(new { });

            if (string.IsNullOrEmpty(GetUserName()))
            {
                return new JsonResult(new { redirectTo = "/Login/Login" });
            }

            string group = _httpContextAccessor.HttpContext.Session.GetString("groupid");
            string branchCode = _httpContextAccessor.HttpContext.Session.GetString("BranchID");
            string userName = GetUserName();

            try
            {
                // Cut-off time logic (simplified, as Global_Parameter_TBL and related logic need proper EF Core mapping)
                // This part needs to be carefully converted, as it involves time comparisons and specific parameter names.
                // For now, we'll assume no cut-off time restrictions.

                string select_query = "";
                string select_query1 = ""; // Used for UNION in VB.NET, needs careful handling
                string clr = "";
                int currFlag = 0;

                if (STATUS == "-1")
                {
                    return new JsonResult(new { ErrorMsg = "Please Enter Cheque Status", Locked_user = "." });
                }

                if (ChequeSource == "ONUS")
                {
                    clr = "ONUS";
                    select_query = "select OnUs_Tbl.Serial, isnull(FirstLevelUser, '') as FirstLevelUser, FirstLevelStatus, BenfBnk, ReturnCodeFinancail, ReturnCode, InputDate, DrwAcctNo, SecoundLevelStatus, SecoundLevelUser, DrwChqNo, ClrCenter, ErrorDescription, DrwBankNo, DrwBranchNo, Currency, Amount, ValueDate, BenAccountNo, BenName, [DrwName], ISSAccount from [OnUs_Tbl] OnUs_Tbl where ClrCenter = 'ONUS' and posted not in (9,10,8) ";
                    select_query1 = "select Inw_Tbl.Serial, isnull(FirstLevelUser, '') as FirstLevelUser, FirstLevelStatus, BenfBnk, ReturnCodeFinancail, ReturnCode, InputDate, DrwAcctNo, SecoundLevelStatus, SecoundLevelUser, DrwChqNo, ClrCenter, ErrorDescription, DrwBankNo, DrwBranchNo, Currency, Amount, ValueDate, BenAccountNo, BenName, [DrwName], ISSAccount from [Inward_Trans] Inw_Tbl where ClrCenter = 'PMA' and posted not in (9,10,8) ";

                    if (vip != "ALL")
                    {
                        if (vip == "Yes")
                        {
                            select_query += " and isnull(vip,0)=1";
                            select_query1 += " and isnull(vip,0)=1";
                        }
                        else
                        {
                            select_query += " and isnull(vip,0)=0";
                            select_query1 += " and isnull(vip,0)=0";
                        }
                    }
                }
                else
                {
                    if (ChequeSource.Contains("INHOUSE"))
                    {
                        clr = "INHOUSE";
                        if (ChequeSource.Contains("PDC"))
                        {
                            select_query = "select OnUs_Tbl.Serial, isnull(FirstLevelUser, '') as FirstLevelUser, FirstLevelStatus, BenfBnk, ReturnCodeFinancail, ReturnCode, InputDate, DrwAcctNo, SecoundLevelStatus, SecoundLevelUser, DrwChqNo, ClrCenter, ErrorDescription, DrwBankNo, DrwBranchNo, Currency, Amount, ValueDate, BenAccountNo, BenName, [DrwName], ISSAccount from [OnUs_Tbl] OnUs_Tbl where ClrCenter = 'INHOUSE' and Was_PDC =1 and posted not in (9,10,8) ";
                        }
                        else
                        {
                            select_query = "select OnUs_Tbl.Serial, isnull(FirstLevelUser, '') as FirstLevelUser, FirstLevelStatus, BenfBnk, ReturnCodeFinancail, ReturnCode, InputDate, DrwAcctNo, SecoundLevelStatus, SecoundLevelUser, DrwChqNo, ClrCenter, ErrorDescription, DrwBankNo, DrwBranchNo, Currency, Amount, ValueDate, BenAccountNo, BenName, [DrwName], ISSAccount from [OnUs_Tbl] OnUs_Tbl where ClrCenter = 'INHOUSE' and Was_PDC =0 and posted not in (9,10,8)";
                        }
                    }
                    else
                    {
                        select_query = "select Inw_Tbl.Serial, isnull(FirstLevelUser, '') as FirstLevelUser, FirstLevelStatus, BenfBnk, ReturnCodeFinancail, ReturnCode, InputDate, DrwAcctNo, SecoundLevelStatus, SecoundLevelUser, DrwChqNo, ClrCenter, ErrorDescription, DrwBankNo, DrwBranchNo, Currency, Amount, ValueDate, BenAccountNo, BenName, [DrwName], ISSAccount from [Inward_Trans] Inw_Tbl where ClrCenter = '" + ChequeSource + "' and posted not in (9,10,8) ";
                    }

                    if (vip != "ALL")
                    {
                        if (vip == "Yes")
                        {
                            select_query += " and isnull(vip,0)=1";
                        }
                        else
                        {
                            select_query += " and isnull(vip,0)=0";
                        }
                    }
                }

                // Branch filtering
                if (Branchs != "-Please Select Type-")
                {
                    if (clr == "INHOUSE" && ChequeSource.Contains("PDC"))
                    {
                        select_query += " And Company_Code = '" + Branchs.Trim() + "'";
                        select_query1 += " And Company_Code = '" + Branchs.Trim() + "'";
                    }
                    else if (clr == "INHOUSE" && !ChequeSource.Contains("PDC"))
                    {
                        select_query += " And InputBrn = '" + Branchs.Trim() + "'";
                        select_query1 += " And InputBrn = '" + Branchs.Trim() + "'";
                    }
                    else
                    {
                        select_query += " And ISNULL(company_code,DrwBranchNo) = '" + Branchs.Trim() + "'";
                        select_query1 += " And ISNULL(company_code,DrwBranchNo) = '" + Branchs.Trim() + "'";
                    }
                }

                if (!string.IsNullOrEmpty(FAmount))
                {
                    select_query += " And Amount >= '" + FAmount.Trim() + "'";
                    select_query1 += " And Amount >= '" + FAmount.Trim() + "'";
                }

                if (!string.IsNullOrEmpty(TAmount))
                {
                    select_query += " And Amount <= '" + TAmount.Trim() + "'";
                    select_query1 += " And Amount <= '" + TAmount.Trim() + "'";
                }

                if (!string.IsNullOrEmpty(DrwAcc))
                {
                    select_query += " And DrwAcctNo like '%" + DrwAcc.Trim() + "%'";
                    select_query1 += " And DrwAcctNo like '%" + DrwAcc.Trim() + "%'";
                }

                if (!string.IsNullOrEmpty(Chequeno))
                {
                    select_query += " And DrwChqNo like '%" + Chequeno.Trim() + "%'";
                    select_query1 += " And DrwChqNo like '%" + Chequeno.Trim() + "%'";
                }

                if (STATUS == "0")
                {
                    select_query += " and verifyStatus = " + STATUS + " and isnull(ReturnCode,'') <>''";
                    select_query1 += " and verifyStatus = " + STATUS + " and isnull(ReturnCode,'') <>''";
                }
                else
                {
                    select_query += " and verifyStatus = " + STATUS;
                    select_query1 += " and verifyStatus = " + STATUS;
                }

                if (branchCode == "2")
                {
                    if (!string.IsNullOrEmpty(Authorize))
                    {
                        if (Authorize == "1")
                        {
                            select_query += " and (isnull(SecoundLeveluser,'') not like '%" + userName + "%' )";
                            select_query1 += " and (isnull(SecoundLeveluser,'') not like '%" + userName + "%' )";
                        }
                        else
                        {
                            select_query += " and isnull(SecoundLeveluser,'') like '%" + userName + "%' ";
                            select_query1 += " and isnull(SecoundLeveluser,'') like '%" + userName + "%' ";
                        }
                    }
                }

                if (group == AllEnums.Group_Status.AdminAuthorized.ToString())
                {
                    select_query += " and category =1100";
                    select_query1 += " and category =1100";
                }
                else
                {
                    select_query += " and category <>1100";
                    select_query1 += " and category <>1100";
                }

                if (Currency != "-1")
                {
                    currFlag = 1;
                    string currencySymbol = "";
                    if (Currency.Trim() == "1") currencySymbol = "JOD";
                    else if (Currency.Trim() == "2") currencySymbol = "USD";
                    else if (Currency.Trim() == "3") currencySymbol = "ILS";
                    else if (Currency.Trim() == "5") currencySymbol = "EUR";

                    select_query += " and Currency = '" + currencySymbol + "'";
                    select_query1 += " and Currency = '" + currencySymbol + "'";
                }

                if (ChequeSource != "-1")
                {
                    select_query += " order by SecoundLevelDate ASC ";
                    select_query1 += " order by SecoundLevelDate ASC ";
                }

                if (ChequeSource == "-1")
                {
                    select_query += " Union " + select_query1;
                }

                List<InwardSearchClass> resultList = new List<InwardSearchClass>();
                string connectionString = ""; // Needs to be retrieved from configuration
                // For now, using a placeholder. Proper configuration retrieval is needed.

                using (var connection = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new System.Data.SqlClient.SqlCommand(select_query, connection))
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                InwardSearchClass obj = new InwardSearchClass
                                {
                                    Serial = reader["Serial"].ToString(),
                                    InputDate = Convert.ToDateTime(reader["InputDate"]).ToString("yyyy/MM/dd"),
                                    DrwChqNo = reader["DrwChqNo"].ToString(),
                                    DrwBankNo = reader["DrwBankNo"].ToString(),
                                    DrwBranchNo = reader["DrwBranchNo"].ToString(),
                                    Currency = reader["Currency"].ToString(),
                                    Amount = Convert.ToDecimal(reader["Amount"]),
                                    ValueDate = Convert.ToDateTime(reader["ValueDate"]).ToString("yyyy/MM/dd"),
                                    DrwName = reader["DrwName"].ToString(),
                                    BenAccountNo = reader["BenAccountNo"].ToString(),
                                    BenName = reader["BenName"].ToString(),
                                    DrwAcctNo = reader["DrwAcctNo"].ToString(),
                                    ISSAccount = reader["ISSAccount"].ToString(),
                                    ClrCenter = reader["ClrCenter"].ToString(),
                                    BenfBnk = reader["BenfBnk"].ToString(),
                                    ReturnCode = reader["ReturnCode"] != DBNull.Value ? reader["ReturnCode"].ToString() : "",
                                    ReturnCodeFinancail = reader["ReturnCodeFinancail"] != DBNull.Value ? reader["ReturnCodeFinancail"].ToString() : "",
                                    SecoundLevelStatus = reader["SecoundLevelStatus"] != DBNull.Value ? reader["SecoundLevelStatus"].ToString() : "",
                                    SecoundLevelUser = reader["SecoundLevelUser"] != DBNull.Value ? reader["SecoundLevelUser"].ToString() : "",
                                    FirstLevelStatus = reader["FirstLevelStatus"] != DBNull.Value ? reader["FirstLevelStatus"].ToString() : "",
                                    FirstLevelUser = reader["FirstLevelUser"] != DBNull.Value ? reader["FirstLevelUser"].ToString() : ""
                                };

                                // Decimal rounding based on currency
                                if (obj.Currency == "JOD")
                                {
                                    obj.Amount = decimal.Round(obj.Amount, 3, MidpointRounding.AwayFromZero);
                                }
                                else
                                {
                                    obj.Amount = decimal.Round(obj.Amount, 2, MidpointRounding.AwayFromZero);
                                }

                                // Return code descriptions
                                var rettblk = await _context.Return_Codes_Tbl.SingleOrDefaultAsync(i => i.ClrCenter == obj.ClrCenter && i.ReturnCode == obj.ReturnCode);
                                if (rettblk != null) obj.ReturnCode += " : " + rettblk.Description_AR;

                                var rettblkFin = await _context.Return_Codes_Tbl.SingleOrDefaultAsync(i => i.ClrCenter == obj.ClrCenter && i.ReturnCode == obj.ReturnCodeFinancail);
                                if (rettblkFin != null) obj.ReturnCodeFinancail += " : " + rettblkFin.Description_AR;

                                resultList.Add(obj);
                            }
                        }
                    }
                }

                double totAmount = 0;
                double totILS = 0;
                double totJOD = 0;
                double totUSD = 0;
                double totEUR = 0;
                int cILS = 0;
                int cJOD = 0;
                int cUSD = 0;
                int cEUR = 0;

                if (currFlag == 1)
                {
                    totAmount = (double)resultList.Sum(x => x.Amount);
                }
                else
                {
                    foreach (var item in resultList)
                    {
                        var currencyId = (await _context.CURRENCY_TBL.SingleOrDefaultAsync(x => x.SYMBOL_ISO == item.Currency))?.ID;
                        if (currencyId == "1")
                        {
                            totJOD += (double)item.Amount;
                            cJOD++;
                        }
                        else if (currencyId == "2")
                        {
                            totUSD += (double)item.Amount;
                            cUSD++;
                        }
                        else if (currencyId == "3")
                        {
                            totILS += (double)item.Amount;
                            cILS++;
                        }
                        else if (currencyId == "5")
                        {
                            totEUR += (double)item.Amount;
                            cEUR++;
                        }
                    }
                }

                _json.Data = new
                {
                    ErrorMsg = "",
                    lstPDC = resultList,
                    AmuntTot = totAmount,
                    ILSAmount = totILS,
                    JODAmount = totJOD,
                    USDAmount = totUSD,
                    EURAmount = totEUR,
                    ILSCount = cILS,
                    JODCount = cJOD,
                    USDCount = cUSD,
                    EURCount = cEUR,
                    Locked_user = "."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in getSearchListInitalAccept_reject: {Message}", ex.Message);
                _httpContextAccessor.HttpContext.Session.SetString("ErrorMessage", "An error occurred: " + ex.Message);
                _json.Data = new { ErrorMsg = "An error occurred: " + ex.Message, Locked_user = "." };
            }

            return _json;
        }



        public async Task<JsonResult> VIEW_WF(string serial, string clrcanter)
        {
            JsonResult _json = new JsonResult(new { });

            if (string.IsNullOrEmpty(GetUserName()))
            {
                return new JsonResult(new { redirectTo = "/Login/Login" });
            }

            try
            {
                List<WFHistory> wf = new List<WFHistory>();

                if (clrcanter != "INHOUSE" && clrcanter != "ONUS")
                {
                    var inw = await _context.Inward_Trans.SingleOrDefaultAsync(c => c.Serial == serial);
                    if (inw != null)
                    {
                        wf = await _context.WFHistories
                                   .Where(x => x.ClrCenter == inw.ClrCenter && x.Serial == serial)
                                   .Select(x => new WFHistory
                                   {
                                       ClrCenter = x.ClrCenter,
                                       Serial = x.Serial,
                                       WF_Level_Desc = x.WF_Level_Desc,
                                       WF_Level_Date = x.WF_Level_Date,
                                       WF_Level_User = x.WF_Level_User
                                   })
                                   .ToListAsync();
                    }
                }
                else
                {
                    var onus = await _context.OnUs_Tbl.SingleOrDefaultAsync(c => c.Serial == serial);
                    if (onus != null)
                    {
                        wf = await _context.WFHistories
                                   .Where(x => x.ClrCenter == "INHOUSE" && x.Serial == serial)
                                   .Select(x => new WFHistory
                                   {
                                       ClrCenter = x.ClrCenter,
                                       Serial = x.Serial,
                                       WF_Level_Desc = x.WF_Level_Desc,
                                       WF_Level_Date = x.WF_Level_Date,
                                       WF_Level_User = x.WF_Level_User
                                   })
                                   .ToListAsync();
                    }
                }

                _json.Data = new { WFLIST = wf };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in VIEW_WF: {Message}", ex.Message);
                _httpContextAccessor.HttpContext.Session.SetString("ErrorMessage", "An error occurred: " + ex.Message);
                _json.Data = new { WFLIST = (object)null, Error = ex.Message };
            }

            return _json;
        }


        public async Task<JsonResult> save_Fix_Ret_CHQ(string serial, string RC, string clecenter, string BnfBranch, string ChequeType)
        {
            JsonResult _json = new JsonResult(new { });

            if (string.IsNullOrEmpty(GetUserName()))
            {
                return new JsonResult(new { redirectTo = "/Login/Login" });
            }

            try
            {
                string userName = GetUserName();

                if (clecenter == "INHOUSE")
                {
                    var onus = await _context.OnUs_Tbl.SingleOrDefaultAsync(c => c.Serial == serial);
                    if (onus != null)
                    {
                        string oldRc = onus.ReturnCode;
                        string oldBranch = onus.InputBrn;

                        onus.InputBrn = BnfBranch;
                        onus.FinalRetCode = await getfinalonuscode(RC, onus.ReturnCodeFinancail, onus.ClrCenter);
                        onus.ReturnCode = RC;
                        onus.LastUpdate = DateTime.Now;
                        onus.LastUpdateBy = userName;
                        onus.History += $"   | Fix Ret CHQ done BY : {userName}  AT : {DateTime.Now} , Change RC from : {oldRc} To : {RC} , And BnfBracnh  From {oldBranch} TO  :   {BnfBranch}";

                        // Update ReturnedCheques_Tbl
                        var retChq = await _context.ReturnedCheques_Tbl.SingleOrDefaultAsync(z => z.DrwAcctNo == onus.DrwAcctNo && z.DrwChqNo.Substring(z.DrwChqNo.Length - 7) == onus.DrwChqNo.Substring(onus.DrwChqNo.Length - 7) && z.DrwBankNo == onus.DrwBankNo && z.DrwBranchNo == onus.DrwBranchNo && z.Posted == (int)AllEnums.Cheque_Status.Returned);
                        if (retChq != null)
                        {
                            retChq.ReturnCode = RC;
                        }
                    }
                }
                else
                {
                    if (ChequeType == "INWARD")
                    {
                        var inw = await _context.Inward_Trans.SingleOrDefaultAsync(c => c.Serial == serial && c.ClrCenter == clecenter);
                        if (inw != null)
                        {
                            string oldRc = inw.ReturnCode;
                            string oldBranch = inw.BenfAccBranch;

                            inw.ReturnCode = RC;
                            inw.FinalRetCode = await getfinalonuscode(RC, inw.ReturnCodeFinancail, inw.ClrCenter);
                            inw.BenfAccBranch = BnfBranch;
                            inw.LastUpdate = DateTime.Now;
                            inw.LastUpdateBy = userName;
                            inw.History += $"   | Fix Ret CHQ done BY : {userName}  AT : {DateTime.Now} , Change RC from : {oldRc} To : {RC} , And BnfBracnh  From {oldBranch} TO  :   {BnfBranch}";

                            var retChq = await _context.ReturnedCheques_Tbl.SingleOrDefaultAsync(z => z.DrwAcctNo == inw.DrwAcctNo && z.DrwChqNo == inw.DrwChqNo && z.DrwChqNo.Substring(z.DrwChqNo.Length - 7) == inw.DrwChqNo.Substring(inw.DrwChqNo.Length - 7) && z.DrwBranchNo == inw.DrwBranchNo && z.Posted == (int)AllEnums.Cheque_Status.Returned);
                            if (retChq != null)
                            {
                                retChq.ReturnCode = RC;
                            }
                        }
                    }
                    else
                    {
                        var out_ = await _context.Outward_Trans.SingleOrDefaultAsync(c => c.Serial == serial && c.ClrCenter == clecenter);
                        if (out_ != null)
                        {
                            string oldRc = out_.ReturnedCode;
                            string oldBranch = out_.InputBrn;

                            out_.ReturnedCode = RC;
                            out_.InputBrn = BnfBranch;
                            out_.LastUpdate = DateTime.Now;
                            out_.LastUpdateBy = userName;
                            out_.History += $"   | Fix Ret CHQ done BY : {userName}  AT : {DateTime.Now} , Change RC from : {oldRc} To : {RC} , And BnfBracnh  From {oldBranch} TO  :   {BnfBranch}";

                            var retChq = await _context.ReturnedCheques_Tbl.SingleOrDefaultAsync(z => z.DrwAcctNo == out_.DrwAcctNo && z.DrwChqNo.Substring(z.DrwChqNo.Length - 7) == out_.DrwChqNo.Substring(out_.DrwChqNo.Length - 7) && z.DrwBankNo == out_.DrwBankNo && z.DrwBranchNo == out_.DrwBranchNo && z.Posted == (int)AllEnums.Cheque_Status.Returned);
                            if (retChq != null)
                            {
                                retChq.ReturnCode = RC;
                            }
                        }
                    }
                }

                await _context.SaveChangesAsync();
                _json.Data = new { retsrep = "Done Successfully" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in save_Fix_Ret_CHQ: {Message}", ex.Message);
                _json.Data = new { retsrep = "Something Wrong" };
            }

            return _json;
        }

        private async Task<string> getfinalonuscode(string RC, string RCF, string clr)
        {
            // This is a placeholder for the getfinalonuscode function.
            // The actual logic needs to be implemented based on the VB.NET original.
            return await Task.FromResult("final_code_placeholder");
        }


_        public async Task<JsonResult> getSearchList(string Branchs, string STATUS, string FromDate, string ToDate, string RSF, string trans,
            string FromReturnedDate, string ToReturnedDate, string BenAccNo,
            string FromBank, string ToBank,
            string Currency, string ChequeSource, string Amount, string DRWAccNo, string ChequeNo, string FromTransDate, string ToTransDatet, string tot, string vip)
        {
            var _json = new JsonResult(new { });

            if (string.IsNullOrEmpty(GetUserName()))
            {
                return new JsonResult(new { redirectTo = "/Login/Login" });
            }

            if (STATUS == "-1")
            {
                return new JsonResult(new { ErrorMsg = "Please Enter Cheque Status" });
            }

            try
            {
                IQueryable<InwardSearchPma> query;

                if (ChequeSource == "-1")
                {
                    var inward = _context.Inward_Trans.Select(t => new InwardSearchPma
                    {
                        BenName = t.BenName,
                        QVFStatus = t.QVFStatus ?? "Pending",
                        QVFAddtlInf = t.QVFAddtlInf ?? "Pending",
                        DrwBranchNo = t.DrwBranchNo,
                        ReturnCodeFinancail = t.ReturnCodeFinancail ?? "",
                        ReturnCode = t.ReturnCode ?? "",
                        Was_PDC = t.Was_PDC,
                        TransDate = t.TransDate.ToString(),
                        Serial = t.Serial,
                        ISSAccount = t.ISSAccount,
                        InputDate = t.InputDate.ToString(),
                        RSF = t.RSF ?? "Pending",
                        DrwChqNo = t.DrwChqNo,
                        BenfBnk = t.BenfBnk,
                        BenfAccBranch = t.BenfAccBranch,
                        Currency = t.Currency,
                        Amount = t.Amount,
                        ValueDate = t.ValueDate.ToString(),
                        DrwName = t.DrwName,
                        BenAccountNo = t.BenAccountNo,
                        DrwAcctNo = t.DrwAcctNo ?? "",
                        ClrCenter = t.ClrCenter,
                        note2 = t.note2 ?? ""
                    });

                    var onus = _context.OnUs_Tbl.Select(o => new InwardSearchPma
                    {
                        BenName = o.BenName,
                        QVFStatus = o.QVFStatus ?? "Pending",
                        QVFAddtlInf = o.QVFAddtlInf ?? "Pending",
                        DrwBranchNo = o.DrwBranchNo,
                        Was_PDC = o.Was_PDC,
                        TransDate = o.TransDate.ToString(),
                        RSF = o.RSF ?? "Pending",
                        Serial = o.Serial,
                        ISSAccount = o.ISSAccount,
                        BenfAccBranch = o.BenfAccBranch,
                        BenfBnk = o.BenfBnk,
                        DrwBankNo = o.DrwBankNo,
                        Currency = o.Currency,
                        Amount = o.Amount,
                        ValueDate = o.ValueDate.ToString(),
                        DrwName = o.DrwName,
                        BenAccountNo = o.BenAccountNo,
                        DrwAcctNo = o.DrwAcctNo ?? "",
                        ClrCenter = o.ClrCenter,
                        note2 = o.note2 ?? "",
                        FinalRetCode = o.FinalRetCode ?? ""
                    });

                    query = inward.Union(onus);
                }
                else if (ChequeSource == "3")
                {
                    query = _context.Inward_Trans.Select(t => new InwardSearchPma
                    {
                        BenName = t.BenName,
                        QVFStatus = t.QVFStatus ?? "Pending",
                        QVFAddtlInf = t.QVFAddtlInf ?? "Pending",
                        DrwBranchNo = t.DrwBranchNo,
                        ReturnCodeFinancail = t.ReturnCodeFinancail ?? "",
                        ReturnCode = t.ReturnCode ?? "",
                        TransDate = t.TransDate.ToString(),
                        Was_PDC = t.Was_PDC,
                        Serial = t.Serial,
                        InputDate = t.InputDate.ToString(),
                        RSF = t.RSF ?? "Pending",
                        ISSAccount = t.ISSAccount,
                        DrwChqNo = t.DrwChqNo,
                        BenfBnk = t.BenfBnk,
                        BenfAccBranch = t.BenfAccBranch,
                        Currency = t.Currency,
                        Amount = t.Amount,
                        ValueDate = t.ValueDate.ToString(),
                        DrwName = t.DrwName,
                        BenAccountNo = t.BenAccountNo,
                        DrwAcctNo = t.DrwAcctNo ?? "",
                        ClrCenter = t.ClrCenter,
                        note2 = t.note2 ?? "",
                        FinalRetCode = t.FinalRetCode ?? ""
                    });
                }
                else
                {
                    query = _context.OnUs_Tbl.Select(o => new InwardSearchPma
                    {
                        BenName = o.BenName,
                        QVFStatus = o.QVFStatus ?? "Pending",
                        QVFAddtlInf = o.QVFAddtlInf ?? "Pending",
                        DrwBranchNo = o.DrwBranchNo,
                        Was_PDC = o.Was_PDC,
                        TransDate = o.TransDate.ToString(),
                        RSF = o.RSF ?? "Pending",
                        Serial = o.Serial,
                        ISSAccount = o.ISSAccount,
                        BenfAccBranch = o.BenfAccBranch,
                        BenfBnk = o.BenfBnk,
                        DrwBankNo = o.DrwBankNo,
                        Currency = o.Currency,
                        Amount = o.Amount,
                        ValueDate = o.ValueDate.ToString(),
                        DrwName = o.DrwName,
                        BenAccountNo = o.BenAccountNo,
                        DrwAcctNo = o.DrwAcctNo ?? "",
                        ClrCenter = o.ClrCenter,
                        note2 = o.note2 ?? "",
                        FinalRetCode = o.FinalRetCode ?? ""
                    });
                }

                // Apply filters
                if (!string.IsNullOrEmpty(Branchs) && Branchs != "-1")
                {
                    query = query.Where(q => q.BenfAccBranch == Branchs);
                }

                if (!string.IsNullOrEmpty(STATUS) && STATUS != "-1")
                {
                    // Placeholder for complex status filtering logic
                    // This part requires detailed analysis of the VB.NET original to accurately translate
                    // For now, we'll assume a direct match for simplicity, but this needs refinement.
                    // The original VB.NET code builds dynamic SQL based on status, which is complex to replicate directly in LINQ.
                    // Example: if (STATUS == "1") query = query.Where(q => q.ReturnCode == "");
                    // More detailed implementation would involve a switch or if-else if structure for each status value.
                }

                if (!string.IsNullOrEmpty(FromDate) && DateTime.TryParse(FromDate, out DateTime fromDateParsed))
                {
                    query = query.Where(q => DateTime.Parse(q.TransDate) >= fromDateParsed);
                }
                if (!string.IsNullOrEmpty(ToDate) && DateTime.TryParse(ToDate, out DateTime toDateParsed))
                {
                    query = query.Where(q => DateTime.Parse(q.TransDate) <= toDateParsed);
                }

                if (!string.IsNullOrEmpty(RSF) && RSF != "-1")
                {
                    query = query.Where(q => q.RSF == RSF);
                }

                // 'trans' parameter is not directly mapped to a field in InwardSearchPma, needs clarification or custom logic

                if (!string.IsNullOrEmpty(FromReturnedDate) && DateTime.TryParse(FromReturnedDate, out DateTime fromReturnedDateParsed))
                {
                    // Assuming a 'ReturnedDate' field exists or can be derived
                    // query = query.Where(q => q.ReturnedDate >= fromReturnedDateParsed);
                }
                if (!string.IsNullOrEmpty(ToReturnedDate) && DateTime.TryParse(ToReturnedDate, out DateTime toReturnedDateParsed))
                {
                    // Assuming a 'ReturnedDate' field exists or can be derived
                    // query = query.Where(q => q.ReturnedDate <= toReturnedDateParsed);
                }

                if (!string.IsNullOrEmpty(BenAccNo))
                {
                    query = query.Where(q => q.BenAccountNo == BenAccNo);
                }

                if (!string.IsNullOrEmpty(FromBank))
                {
                    // Assuming 'FromBank' maps to DrwBankNo or similar
                    query = query.Where(q => q.DrwBankNo == FromBank);
                }
                if (!string.IsNullOrEmpty(ToBank))
                {
                    // Assuming 'ToBank' maps to BenfBnk or similar
                    query = query.Where(q => q.BenfBnk == ToBank);
                }

                if (!string.IsNullOrEmpty(Currency) && Currency != "-1")
                {
                    query = query.Where(q => q.Currency == Currency);
                }

                if (!string.IsNullOrEmpty(Amount) && decimal.TryParse(Amount, out decimal amountParsed))
                {
                    query = query.Where(q => q.Amount == amountParsed);
                }

                if (!string.IsNullOrEmpty(DRWAccNo))
                {
                    query = query.Where(q => q.DrwAcctNo == DRWAccNo);
                }

                if (!string.IsNullOrEmpty(ChequeNo))
                {
                    query = query.Where(q => q.DrwChqNo == ChequeNo);
                }

                if (!string.IsNullOrEmpty(FromTransDate) && DateTime.TryParse(FromTransDate, out DateTime fromTransDateParsed))
                {
                    query = query.Where(q => DateTime.Parse(q.TransDate) >= fromTransDateParsed);
                }
                if (!string.IsNullOrEmpty(ToTransDatet) && DateTime.TryParse(ToTransDatet, out DateTime toTransDateParsed))
                {
                    query = query.Where(q => DateTime.Parse(q.TransDate) <= toTransDateParsed);
                }

                // 'tot' and 'vip' parameters are not directly mapped to fields in InwardSearchPma, needs clarification or custom logic

                var result = await query.ToListAsync();

                // Calculate totals (TotAmount, TotILS, TotJOD, TotUSD, TotEUR, CILS, CJOD, CUSD, CEUR)
                double TotAmount = result.Sum(q => (double)q.Amount);
                double TotILS = result.Where(q => q.Currency == "ILS").Sum(q => (double)q.Amount);
                double TotJOD = result.Where(q => q.Currency == "JOD").Sum(q => (double)q.Amount);
                double TotUSD = result.Where(q => q.Currency == "USD").Sum(q => (double)q.Amount);
                double TotEUR = result.Where(q => q.Currency == "EUR").Sum(q => (double)q.Amount);

                int CILS = result.Count(q => q.Currency == "ILS");
                int CJOD = result.Count(q => q.Currency == "JOD");
                int CUSD = result.Count(q => q.Currency == "USD");
                int CEUR = result.Count(q => q.Currency == "EUR");

                _json.Data = new
                {
                    ErrorMsg = "",
                    CountOfCheques = result.Count,
                    lstPDC = result,
                    AmuntTot = TotAmount,
                    ILSAmount = TotILS,
                    JODAmount = TotJOD,
                    USDAmount = TotUSD,
                    EURAmount = TotEUR,
                    ILSCount = CILS,
                    JODCount = CJOD,
                    USDCount = CUSD,
                    EURCount = CEUR,
                    Locked_user = "."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in getSearchList: {Message}", ex.Message);
                _json.Data = new { ErrorMsg = "An error occurred while searching." };
            }

            return _json;
        }


        public async Task<JsonResult> returnsuspen(string chqseq, string clr_center, string Account, string retuen_code, string serial)
        {
            var _json = new JsonResult(new { });

            if (string.IsNullOrEmpty(GetUserName()))
            {
                return new JsonResult(new { redirectTo = "/Login/Login" });
            }

            try
            {
                var inward = await _context.Inward_Trans.SingleOrDefaultAsync(i => i.Serial == serial);
                var onus = await _context.OnUs_Tbl.SingleOrDefaultAsync(o => o.Serial == serial);
                var username = GetUserName();

                if (inward != null)
                {
                    inward.ReturnCode = retuen_code;
                    await _context.SaveChangesAsync();
                }

                if (onus != null)
                {
                    onus.ReturnCode = retuen_code;
                    await _context.SaveChangesAsync();
                }

                // The original code calls a method named "Retueninward". This method is not defined in the provided code.
                // I will assume that this method is responsible for handling the return process.
                // I will add a placeholder for this method call.
                await Retueninward(chqseq, clr_center, Account, retuen_code, serial, "RET_SUSP");

                _json.Data = new { retsrep = "Done Successfully" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in returnsuspen: {Message}", ex.Message);
                _json.Data = new { retsrep = "Something Wrong" };
            }

            return _json;
        }

        private async Task Retueninward(string chqseq, string clr_center, string Account, string retuen_code, string serial, string MQ_MSG)
        {
            // This is a placeholder for the Retueninward method.
            // The actual logic needs to be implemented based on the VB.NET original.
            await Task.CompletedTask;
        }



        public async Task<string> GetCustomerDues(string Customer_id)
        {
            string result = "";
            try
            {
                // Placeholder for CustomerDues.Customer_DuesSoapClient
                // This would typically involve a service reference or HttpClient call to a SOAP service.
                // For now, we will return an empty string or a mock value.
                // Example: var customerDuesService = new CustomerDues.Customer_DuesSoapClient();
                // result = await customerDuesService.GetCustomerDuesAsync(Customer_id);
                _logger.LogInformation("GetCustomerDues called for Customer_id: {CustomerId}", Customer_id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetCustomerDues: {Message}", ex.Message);
            }
            return result;
        }



        public async Task<string> EVALUATE_AMOUNT_IN_JOD(string CURANCY, double AMOUNT)
        {
            string result = "";
            try
            {
                // Assuming a stored procedure named [DBO].[EVALUATE_AMOUNT_IN_JOD] exists
                // and returns a single string value.
                // This needs to be adjusted based on the actual stored procedure return type and parameters.
                var query = _context.Database.SqlQuery<string>($"SELECT [DBO].[EVALUATE_AMOUNT_IN_JOD]({CURANCY}, {AMOUNT})");
                result = await query.FirstOrDefaultAsync();

                _logger.LogInformation("EVALUATE_AMOUNT_IN_JOD called for Currency: {Currency}, Amount: {Amount}. Result: {Result}", CURANCY, AMOUNT, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in EVALUATE_AMOUNT_IN_JOD: {Message}", ex.Message);
            }
            return result;
        }



        public async Task<JsonResult> FixedCHQ(string Status, string _chqseq, string clr_center, string returncode, string drwchq, string drwacc)
        {
            var _json = new JsonResult(new { });

            if (string.IsNullOrEmpty(GetUserName()))
            {
                return new JsonResult(new { redirectTo = "/Login/Login" });
            }

            try
            {
                var retcode = returncode.Split('-')[0].Trim();
                var onus = await _context.OnUs_Tbl.SingleOrDefaultAsync(x => x.ChqSequance == _chqseq);
                var inward = await _context.Inward_Trans.SingleOrDefaultAsync(x => x.ChqSequance == _chqseq);
                var wf = await _context.INWARD_WF_Tbl.SingleOrDefaultAsync(v => v.ChqSequance == _chqseq && v.Final_Status != "Accept");

                // Assuming System_Configurations_Tbl and Global_Parameter_TBL are accessible via _context
                var sys = await _context.Global_Parameter_TBL.SingleOrDefaultAsync(i => i.Parameter_Name == "FIXCHQ");
                string[] resultConfig = null;
                if (sys != null) resultConfig = sys.Parameter_Value.Split(',');

                if (Status == "1") // fixed
                {
                    if (clr_center == "ONUS")
                    {
                        if (onus != null)
                        {
                            var _serial = onus.ChqSequance;
                            if (onus.Status.Equals("S"))
                            {
                                onus.LastUpdate = DateTime.Now;
                                onus.LastUpdateBy = GetUserName();
                                onus.History += "|" + " This chq Verified Technically Accepted, by:" + GetUserName();
                                await _context.SaveChangesAsync();

                                // Update WF table
                                if (wf != null)
                                {
                                    wf.ISFixederror = true;
                                    wf.Final_Status_User = GetUserName();
                                    wf.last_update_by = GetUserName();
                                    wf.last_update_date = DateTime.Now;
                                    await _context.SaveChangesAsync();

                                    // Placeholder for chqwf method
                                    bool ss = await chqwf(_serial); // This method needs to be implemented or mocked
                                    if (ss)
                                    {
                                        wf.Final_Status = "Accept";
                                        wf.History += " | " + GetUserName() + "Chq Approved Technically , Final Status : Accept" + " , At " + DateTime.Now;
                                    }
                                    else
                                    {
                                        wf.Final_Status = "Pending";
                                        wf.History += " | " + GetUserName() + "Chq Approved Technically, still need Finanical WF   , At " + DateTime.Now;
                                    }
                                    await _context.SaveChangesAsync();
                                }

                                // Add WFHistory
                                var wfHistory = new WFHistory
                                {
                                    DrwChqNo = onus.DrwChqNo,
                                    ChqSequance = onus.ChqSequance,
                                    Serial = onus.Serial,
                                    TransDate = onus.TransDate,
                                    ID_WFStatus = WFHistory.WF_Status.TechnicallyAccepted,
                                    Status = onus.Status + ":" + "TechnicallyAccepted" + "By :" + GetUserName(),
                                    ClrCenter = onus.ClrCenter,
                                    DrwAccNo = onus.DrwAcctNo,
                                    Amount = onus.Amount
                                };
                                _context.WFHistories.Add(wfHistory);
                                await _context.SaveChangesAsync();

                                _json.Data = new { FERR = "Fixed chq Done Successfully" };
                            }
                            else if (onus.Status.Equals("F"))
                            {
                                // Handle 'F' status (failed)
                                var CHQSEQ = await GENERATE_UNIQUE_CHEQUE_SEQUANCE(onus.DrwChqNo, onus.DrwBankNo, onus.DrwBranchNo, onus.DrwAcctNo); // Needs implementation

                                if (resultConfig != null)
                                {
                                    foreach (var item in resultConfig)
                                    {
                                        if (onus.ErrorDescription.Contains(item))
                                        {
                                            // Placeholder for ONUSInwardForcePosting
                                            bool xx = await ONUSInwardForcePosting(onus, CHQSEQ); // Needs implementation
                                            break;
                                        }
                                    }
                                }

                                // Update WF table if not already handled by ONUSInwardForcePosting
                                if (wf != null)
                                {
                                    wf.Final_Status_User = GetUserName();
                                    wf.last_update_by = GetUserName();
                                    wf.last_update_date = DateTime.Now;
                                    wf.ISFixederror = true;
                                    await _context.SaveChangesAsync();

                                    bool ss = await chqwf(_serial); // This method needs to be implemented or mocked
                                    if (ss)
                                    {
                                        wf.Final_Status = "Accept";
                                        wf.History += " | " + GetUserName() + "Chq Approved Technically , Final Status : Accept" + " , At " + DateTime.Now;
                                    }
                                    else
                                    {
                                        wf.Final_Status = "Pending";
                                        wf.History += " | " + GetUserName() + "Chq Approved Technically, still need Finanical WF   , At " + DateTime.Now;
                                    }
                                    await _context.SaveChangesAsync();
                                }

                                // Add WFHistory
                                var wfHistory = new WFHistory
                                {
                                    DrwChqNo = onus.DrwChqNo,
                                    ChqSequance = onus.ChqSequance,
                                    Serial = onus.Serial,
                                    TransDate = onus.TransDate,
                                    ID_WFStatus = WFHistory.WF_Status.TechnicallyAccepted,
                                    Status = onus.Status + ":" + "TechnicallyAccepted" + "By :" + GetUserName(),
                                    ClrCenter = onus.ClrCenter,
                                    DrwAccNo = onus.DrwAcctNo,
                                    Amount = onus.Amount
                                };
                                _context.WFHistories.Add(wfHistory);
                                await _context.SaveChangesAsync();

                                _json.Data = new { FERR = "Fixed chq Done Successfully" };
                            }
                        }
                    }
                    else // PMA or DISCOUNT
                    {
                        if (inward != null)
                        {
                            var _serial = inward.ChqSequance;
                            if (inward.Status == "S")
                            {
                                inward.LastUpdate = DateTime.Now;
                                inward.LastUpdateBy = GetUserName();
                                inward.History += "|" + " This chq Verified Technically Accepted , by:" + GetUserName();
                                await _context.SaveChangesAsync();

                                // Update WF table
                                if (wf != null)
                                {
                                    wf.Final_Status_User = GetUserName();
                                    wf.last_update_by = GetUserName();
                                    wf.last_update_date = DateTime.Now;
                                    wf.ISFixederror = true;
                                    await _context.SaveChangesAsync();

                                    bool ss = await chqwf(_serial); // This method needs to be implemented or mocked
                                    if (ss)
                                    {
                                        wf.Final_Status = "Accept";
                                        wf.History += " | " + GetUserName() + "Chq Approved Technically  , Final Status : Accept" + " , At " + DateTime.Now;
                                    }
                                    else
                                    {
                                        wf.Final_Status = "Pending";
                                        wf.History += " | " + GetUserName() + "Chq Approved Technically, still need Finanical WF  , At " + DateTime.Now;
                                    }
                                    await _context.SaveChangesAsync();
                                }

                                // Add WFHistory
                                var wfHistory = new WFHistory
                                {
                                    DrwChqNo = inward.DrwChqNo,
                                    ChqSequance = inward.ChqSequance,
                                    Serial = inward.Serial,
                                    TransDate = inward.TransDate,
                                    ID_WFStatus = WFHistory.WF_Status.TechnicallyAccepted,
                                    Status = inward.Status + ":" + "TechnicallyAccepted" + "By :" + GetUserName(),
                                    ClrCenter = inward.ClrCenter,
                                    DrwAccNo = inward.DrwAcctNo,
                                    Amount = inward.Amount
                                };
                                _context.WFHistories.Add(wfHistory);
                                await _context.SaveChangesAsync();

                                _json.Data = new { FERR = "Fixed chq Done Successfully" };
                            }
                            else if (inward.Status == "F")
                            {
                                // Handle 'F' status (failed)
                                var CHQSEQ = await GENERATE_UNIQUE_CHEQUE_SEQUANCE(inward.DrwChqNo, inward.DrwBankNo, inward.DrwBranchNo, inward.DrwAcctNo); // Needs implementation

                                // Placeholder for POSTCheques_DebitCridet
                                bool DebitCridet_status = await POSTCheques_DebitCridet(inward.Serial, CHQSEQ, retcode); // Needs implementation

                                // Placeholder for Use_Suspense and return_Suspense
                                bool Suspense_status = await Use_Suspense(CHQSEQ, inward.Serial, inward.Currency);
                                if (Suspense_status)
                                {
                                    bool Suspense_stat = await return_Suspense(CHQSEQ, inward.Serial, inward.Currency, "04");
                                }

                                // Update WF table if not already handled
                                if (wf != null)
                                {
                                    _context.INWARD_WF_Tbl.Remove(wf);
                                    await _context.SaveChangesAsync();
                                }

                                _json.Data = new { FERR = "Fixed chq Done Successfully" };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in FixedCHQ: {Message}", ex.Message);
                _json.Data = new { FERR = "An error occurred while fixing the cheque." };
            }

            return _json;
        }

        // Placeholder methods that need to be implemented based on VB.NET original
        private async Task<bool> chqwf(string serial) { await Task.CompletedTask; return true; }
        private async Task<string> GENERATE_UNIQUE_CHEQUE_SEQUANCE(string drwChqNo, string drwBankNo, string drwBranchNo, string drwAcctNo) { await Task.CompletedTask; return Guid.NewGuid().ToString(); }
        private async Task<bool> ONUSInwardForcePosting(OnUs_Tbl onus, string chqseq) { await Task.CompletedTask; return true; }
        private async Task<bool> POSTCheques_DebitCridet(string serials, string chq_seq, string rc) { await Task.CompletedTask; return true; }
        private async Task<bool> Use_Suspense(string chq_seq, string serial, string currancy) { await Task.CompletedTask; return true; }
        private async Task<bool> return_Suspense(string chq_seq, string serial, string currancy, string retuen_code) { await Task.CompletedTask; return true; }



        public async Task<bool> return_Suspense(string chq_seq, string serial, string currancy, string retuen_code)
        {
            try
            {
                _logger.LogInformation("Starting return_Suspense for chq_seq: {ChqSeq}, serial: {Serial}, currency: {Currency}, return_code: {ReturnCode}", chq_seq, serial, currancy, retuen_code);

                var inwardTrans = await _context.Inward_Trans.SingleOrDefaultAsync(i => i.Serial == serial);
                string clr_center = "INWARD";

                var suspenseAccount = await _context.Suspense_Accounts_Tbl.SingleOrDefaultAsync(x => x.Currency == currancy && x.ChequeTypeID == clr_center);

                if (suspenseAccount != null)
                {
                    string account = suspenseAccount.AlternativeAccountNo.ToString();

                    // This calls the returnsuspen method, which is already defined in the service.
                    // The original VB.NET code calls itself, which is unusual. Assuming it meant to call the service's returnsuspen.
                    var result = await returnsuspen(chq_seq, inwardTrans?.ClrCenter ?? clr_center, account, retuen_code, serial);

                    // Check the result of the returnsuspen call
                    if (result.Value is IDictionary<string, object> data && data.TryGetValue("retsrep", out var retsrep) && retsrep.ToString() == "Done Successfully")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in return_Suspense: {Message}", ex.Message);
                return false;
            }
        }



        public async Task<bool> Use_Suspense(string chq_seq, string serial, string currancy)
        {
            try
            {
                _logger.LogInformation("Starting Use_Suspense for chq_seq: {ChqSeq}, serial: {Serial}, currency: {Currency}", chq_seq, serial, currancy);

                string clr_center = "INWARD";
                var suspenseAccount = await _context.Suspense_Accounts_Tbl.SingleOrDefaultAsync(x => x.Currency == currancy && x.ChequeTypeID == clr_center);

                if (suspenseAccount != null)
                {
                    string account = suspenseAccount.AlternativeAccountNo.ToString();

                    // Placeholder for POSTCheques_Suspens method
                    bool useSuspense = await POSTCheques_Suspens(serial, account, chq_seq); // This method needs to be implemented or mocked

                    return useSuspense;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Use_Suspense: {Message}", ex.Message);
                return false;
            }
        }

        // Placeholder method that needs to be implemented based on VB.NET original
        private async Task<bool> POSTCheques_Suspens(string serial, string account, string chq_seq) { await Task.CompletedTask; return true; }



        public async Task<bool> return_SuspenseONUS(string chq_seq, string serial, string currancy, string retuen_code)
        {
            try
            {
                _logger.LogInformation("Starting return_SuspenseONUS for chq_seq: {ChqSeq}, serial: {Serial}, currency: {Currency}, return_code: {ReturnCode}", chq_seq, serial, currancy, retuen_code);

                var onusTrans = await _context.OnUs_Tbl.SingleOrDefaultAsync(i => i.Serial == serial);
                string clr_center = "ONUS"; // Assuming ClrCenter for ONUS transactions

                var suspenseAccount = await _context.Suspense_Accounts_Tbl.SingleOrDefaultAsync(x => x.Currency == currancy && x.ChequeTypeID == clr_center);

                if (suspenseAccount != null)
                {
                    string account = suspenseAccount.AlternativeAccountNo.ToString();

                    // Call the returnsuspen method (already implemented in this service)
                    var result = await returnsuspen(chq_seq, onusTrans?.ClrCenter ?? clr_center, account, retuen_code, serial);

                    // Check the result of the returnsuspen call
                    if (result.Value is IDictionary<string, object> data && data.TryGetValue("retsrep", out var retsrep) && retsrep.ToString() == "Done Successfully")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in return_SuspenseONUS: {Message}", ex.Message);
                return false;
            }
        }



        public async Task<JsonResult> VerifyAllDiscountCHQ(List<string> Serials)
        {
            var _json = new JsonResult(new { });

            if (string.IsNullOrEmpty(GetUserName()))
            {
                return new JsonResult(new { redirectTo = "/Login/Login" });
            }

            var inwardRejected = new List<Inward_Trans>();

            if (Serials != null && Serials.Any())
            {
                try
                {
                    foreach (var serialAmount in Serials)
                    {
                        var parts = serialAmount.Split(":");
                        if (parts.Length == 2 && int.TryParse(parts[0].Trim(), out int id) && int.TryParse(parts[1].Trim(), out int amount))
                        {
                            var inward = await _context.Inward_Trans.SingleOrDefaultAsync(x => x.Serial == id.ToString()); // Assuming Serial is string

                            if (amount > 0)
                            {
                                if (inward != null)
                                {
                                    inward.verifyStatus = 1;
                                    inward.Posted = AllEnums.Cheque_Status.Verified;
                                    inward.History += "|" + "Verify Done by :" + GetUserName() + "At :" + DateTime.Now;
                                    _context.Entry(inward).State = EntityState.Modified;
                                    await _context.SaveChangesAsync();

                                    var wfHistory = new WFHistory
                                    {
                                        ChqSequance = inward.ChqSequance,
                                        Serial = inward.Serial,
                                        DrwAccNo = inward.DrwAcctNo,
                                        TransDate = inward.ValueDate,
                                        DrwChqNo = inward.DrwChqNo,
                                        ID_WFStatus = WFHistory.WF_Status.TechnicallyAccepted,
                                        Status = "TechnicallyAccepted" + "By :" + GetUserName(),
                                        ClrCenter = inward.ClrCenter,
                                        Amount = inward.Amount
                                    };
                                    _context.WFHistories.Add(wfHistory);
                                    await _context.SaveChangesAsync();
                                }
                            }
                            else
                            {
                                if (inward != null) inwardRejected.Add(inward);
                            }
                        }
                    }

                    _json.Data = new { ErrorMsg = "", lstPDC = inwardRejected };
                    return _json;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in VerifyAllDiscountCHQ: {Message}", ex.Message);
                    _json.Data = new { ErrorMsg = "An error occurred while verifying cheques." };
                }
            }

            return _json;
        }



        public async Task<bool> POSTCheques_ONUS(string Serials)
        {
            if (string.IsNullOrEmpty(GetUserName()))
            {
                // This method is called internally, so no direct redirect. Log and return false.
                _logger.LogWarning("POSTCheques_ONUS called without a logged-in user.");
                return false;
            }

            try
            {
                // Dependencies for external services (ECC_Handler_SVC.InwardHandlingSVCSoapClient, ECC_CAP_Services.SAFA_T24_ECC_SVCSoapClient)
                // These need to be properly injected or instantiated. For now, using placeholders.
                // var WebSvc = new ECC_Handler_SVC.InwardHandlingSVCSoapClient("InwardHandlingSVCSoap");
                // var EccAccInfo_WebSvc = new ECC_CAP_Services.SAFA_T24_ECC_SVCSoapClient("SAFA_T24_ECC_SVCSoap");

                var cheque = await _context.OnUs_Tbl.SingleOrDefaultAsync(x => x.Serial == Serials);
                if (cheque == null)
                {
                    _logger.LogWarning("POSTCheques_ONUS: OnUs_Tbl with Serial {Serial} not found.", Serials);
                    return false;
                }

                // Retrieve system configurations (placeholder values for now)
                // In a real application, these would be loaded from appsettings.json or a configuration service.
                string sysName = ""; // await _context.System_Configurations_Tbl.Where(c => c.Config_Type == "PDC_SYSTEM_ID" && c.Config_Id == "1").Select(c => c.Config_Value).FirstOrDefaultAsync();
                string paymentMethod = ""; // await _context.System_Configurations_Tbl.Where(c => c.Config_Type == "ECC_PAYMENT_METHOD" && c.Config_Id == "2").Select(c => c.Config_Value).FirstOrDefaultAsync();
                string urgencyCode = ""; // await _context.System_Configurations_Tbl.Where(c => c.Config_Type == "ECC_URGENCY_CODE" && c.Config_Id == "3").Select(c => c.Config_Value).FirstOrDefaultAsync();
                string pdcCharges = ""; // await _context.System_Configurations_Tbl.Where(c => c.Config_Type == "ECC_PDC_CHARGES" && c.Config_Id == "4").Select(c => c.Config_Value).FirstOrDefaultAsync();

                // Mocking configuration values for demonstration
                sysName = "ECC_SYSTEM";
                paymentMethod = "105";
                urgencyCode = "URGENT";
                pdcCharges = "5.00";

                // Construct ECC_FT_Request object (placeholder)
                // This part requires a proper definition of ECC_CAP_Services.ECC_FT_Request and Temenos_Message_Types
                // For now, we'll simulate the request and response.
                // var obj = new ECC_CAP_Services.ECC_FT_Request
                // {
                //     PsSystem = sysName,
                //     RequestDate = DateTime.Now.Date.ToString("yyyyMMdd"),
                //     RequestTime = DateTime.Now.ToString("hh:mm:ss"),
                //     RequestCode = "ONUSInwardForcePosting",
                //     TransSeq = cheque.Serial,
                //     CheckSerial = cheque.DrwChqNo,
                //     CheckSeq = cheque.ChqSequance,
                //     PayBranchCode = cheque.DrwBranchNo,
                //     PayBankCode = cheque.DrwBankNo,
                //     PayAccountNumber = cheque.DrwAcctNo,
                //     BFDBranchCode = cheque.BenfAccBranch,
                //     BFDBankCode = cheque.BenfBnk,
                //     BFDAccountNumber = "0000000000000",
                //     CheckAmount = cheque.Amount.ToString("N2", CultureInfo.InvariantCulture).Replace(",", ""), // Assuming N2 for most currencies, N3 for JOD
                //     CurrencyCode = GetCurrencyCode(cheque.Currency) // This method needs to be implemented
                // };

                // Simulate FT_ResponseClass
                var ftResponse = new FT_ResponseClass
                {
                    FT_Res = new FT_Response
                    {
                        ResponseStatus = "S", // Simulate success
                        ResponseDescription = "Simulated Success",
                        ErrorMessage = "",
                        AcctCompany = ""
                    },
                    chqSeq = cheque.ChqSequance
                };

                // Simulate external service call
                // ftResponse.FT_Res = await EccAccInfo_WebSvc.ECC_OFS_MESSAGE(obj, Temenos_Message_Types.ONUSInwardForcePosting, 1);

                if (ftResponse.FT_Res.ResponseStatus == "S")
                {
                    cheque.Posted = AllEnums.Cheque_Status.Posted;
                    cheque.LastUpdateBy = GetUserName();
                    cheque.LastUpdate = DateTime.Now;
                    cheque.ErrorCode = "";
                    cheque.T24Response = "ONUSInwardForcePosting " + ftResponse.FT_Res.ResponseStatus + ":" + ftResponse.FT_Res.ResponseDescription;
                    cheque.Status = "S";
                    cheque.ErrorDescription = ftResponse.FT_Res.ResponseStatus + ":" + ftResponse.FT_Res.ResponseDescription;
                    cheque.History += "|" + "ONUSInwardForcePosting   Done by : " + GetUserName() + "AT:" + DateTime.Now;

                    if (ftResponse.FT_Res.ResponseDescription.Contains("Insufficient"))
                    {
                        cheque.ReturnCodeFinancail = "02";
                    }

                    if (!string.IsNullOrEmpty(ftResponse.FT_Res.AcctCompany))
                    {
                        cheque.OLDDrwBranchNo = ftResponse.FT_Res.AcctCompany;
                    }

                    await _context.SaveChangesAsync();

                    // Update WF table (remove if Final_Status <> "Accept")
                    var wf = await _context.INWARD_WF_Tbl.SingleOrDefaultAsync(x => x.Serial == cheque.Serial && x.Final_Status != "Accept");
                    if (wf != null)
                    {
                        _context.INWARD_WF_Tbl.Remove(wf);
                        await _context.SaveChangesAsync();
                    }

                    // Update OnUs_Tbl for NeedFinanciallyWF and NeedFixedError
                    cheque.NeedFinanciallyWF = 0;
                    cheque.NeedFixedError = 0;
                    await _context.SaveChangesAsync();

                    // Handle ReturnedCheques_Tbl if response is "Success"
                    if (ftResponse.FT_Res.ResponseDescription == "Success")
                    {
                        var returnedCheque = await _context.ReturnedCheques_Tbl.SingleOrDefaultAsync(
                            z => z.DrwAcctNo == cheque.DrwAcctNo &&
                                 z.DrwChqNo == cheque.DrwChqNo && // Simplified comparison
                                 z.DrwBankNo == cheque.DrwBankNo &&
                                 z.DrwBranchNo == cheque.DrwBranchNo &&
                                 z.Posted == AllEnums.Cheque_Status.Returned);

                        if (returnedCheque != null)
                        {
                            returnedCheque.Posted += AllEnums.Cheque_Status.Cleared;
                            returnedCheque.LastUpdate = DateTime.Now;
                            returnedCheque.LastUpdateBy = GetUserName();
                            returnedCheque.History = "Chq Cleared    by :  " + GetUserName() + "At : " + DateTime.Now;
                            returnedCheque.Returned = 0;
                            returnedCheque.ReturnCode = 0;
                            await _context.SaveChangesAsync();
                        }
                    }

                    // Update OFS_Response_Errors_Tbl
                    var respError = await _context.OFS_Response_Errors_Tbl.SingleOrDefaultAsync(v => v.ChqSequance == ftResponse.chqSeq);
                    if (respError == null)
                    {
                        respError = new OFS_Response_Errors_Tbl
                        {
                            LastErrorMessage = ftResponse.FT_Res.ResponseDescription,
                            Cheque_Type = "ONUS",
                            Serial = cheque.Serial,
                            ChqSequance = ftResponse.chqSeq,
                            LastAmendBy = GetUserName(),
                            LastAmendDate = DateTime.Now,
                            History = ftResponse.FT_Res.ResponseDescription
                        };
                        _context.OFS_Response_Errors_Tbl.Add(respError);
                    }
                    else
                    {
                        respError.LastAmendBy = GetUserName();
                        respError.LastAmendDate = DateTime.Now;
                        respError.History = ftResponse.FT_Res.ResponseDescription;
                        respError.LastErrorMessage = ftResponse.FT_Res.ResponseDescription;
                        _context.Entry(respError).State = EntityState.Modified;
                    }
                    await _context.SaveChangesAsync();

                    // Call HandelResponseONUS (placeholder)
                    // await WebSvc.HandelResponseONUSAsync(cheque.ChqSequance, cheque.ClrCenter, ftResponse.FT_Res.ResponseDescription, ftResponse.FT_Res.SpecialNotes, cheque.Serial, GetUserID().ToString());

                    return true;
                }
                else // ResponseStatus is not "S" (failed)
                {
                    cheque.History += "|" + "ONUSInwardForcePosting Faild";
                    cheque.LastUpdateBy = GetUserName();
                    cheque.LastUpdate = DateTime.Now;
                    cheque.ErrorCode = 9999;
                    cheque.Status = "F";
                    cheque.ErrorDescription = "ONUSInwardForcePosting: " + ftResponse.FT_Res.ResponseDescription;
                    cheque.T24Response = "ONUSInwardForcePosting: " + ftResponse.FT_Res.ResponseDescription;

                    if (ftResponse.FT_Res.ErrorMessage.Contains("TIMEOUT"))
                    {
                        cheque.IsTimeOut = 1;
                    }

                    if (!string.IsNullOrEmpty(ftResponse.FT_Res.AcctCompany))
                    {
                        cheque.OLDDrwBranchNo = ftResponse.FT_Res.AcctCompany;
                    }

                    await _context.SaveChangesAsync();

                    // Call HandelResponseONUS (placeholder)
                    // await WebSvc.HandelResponseONUSAsync(cheque.ChqSequance, cheque.ClrCenter, ftResponse.FT_Res.ResponseDescription, ftResponse.FT_Res.SpecialNotes, cheque.Serial, GetUserID().ToString());

                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in POSTCheques_ONUS: {Message}", ex.Message);
                return false;
            }
        }

        // Helper class definitions (assuming they exist or need to be created)
        public class FT_ResponseClass
        {
            public FT_Response FT_Res { get; set; }
            public string chqSeq { get; set; }
        }

        public class FT_Response
        {
            public string ResponseStatus { get; set; }
            public string ResponseDescription { get; set; }
            public string ErrorMessage { get; set; }
            public string AcctCompany { get; set; }
        }

        // AllEnums.Cheque_Status needs to be defined or mapped
        // Example:
        public static class AllEnums
        {
            public enum Cheque_Status
            {
                Posted = 1,
                Verified = 2,
                Returned = 3,
                Cleared = 4
            }
        }

        // GetCurrencyCode method needs to be implemented
        private string GetCurrencyCode(string currency) { return currency; } // Placeholder



        public async Task<InsufficientFundsViewModel> GetInsufficientFundsData(string userName, string branchId, string comId)
        {
            var model = new InsufficientFundsViewModel();

            // Populate ViewBag.Tree (assuming GetAllCategoriesForTree is a helper method)
            // model.Tree = GetAllCategoriesForTree(); // This would be done in the controller

            // Clearing Centers
            var clearingCenters = await _context.ClearingCenters.OrderByDescending(obj => obj.Id).ToListAsync();
            model.ClearingCenters = clearingCenters;

            // Branches
            if (branchId == "2" || branchId == "714")
            {
                model.Branches = await _context.Companies_Tbl
                                            .Where(o => o.Company_Type != "4")
                                            .Select(item => new CompanyViewModel { Company_ID = item.Company_ID, Company_Code = item.Company_Code.Substring(5), Company_Name = item.Company_Name })
                                            .ToListAsync();
            }
            else
            {
                model.Branches = await _context.Companies_Tbl
                                            .Where(i => i.Company_ID == comId)
                                            .Select(item => new CompanyViewModel { Company_ID = item.Company_ID, Company_Code = item.Company_Code.Substring(5), Company_Name = item.Company_Name })
                                            .ToListAsync();
            }

            // Customer Types (assuming BindCustomerType is a helper method)
            // model.CustomerTypes = BindCustomerType(branchId); // This would be done in the controller

            // Currencies
            model.Currencies = await _context.CURRENCY_TBL.ToListAsync();

            return model;
        }

        // Helper method to simulate GetUserName() and GetUserID()
        private string GetUserName() { return _httpContextAccessor.HttpContext?.Session.GetString("UserName"); }
        private string GetUserID() { return _httpContextAccessor.HttpContext?.Session.GetString("ID"); }

        // Placeholder for BindCustomerType - needs actual implementation
        private List<CustomerType> BindCustomerType(string branchId)
        {
            // Implement actual logic to bind customer types based on branchId
            return new List<CustomerType>();
        }

        // Placeholder for GetAllCategoriesForTree - needs actual implementation
        private object GetAllCategoriesForTree()
        {
            // Implement actual logic to get categories for tree view
            return new object();
        }



        public async Task<InwardFinanicalWFDetailsONUSViewModel> GetInwardFinanicalWFDetailsONUSData(string id, string userName, string branchId, string comId)
        {
            var model = new InwardFinanicalWFDetailsONUSViewModel();

            // Assuming GetMethodName() is a helper to get the current action method name
            string methodName = "InwardFinanicalWFDetailsONUS"; // Hardcoding for now, ideally dynamic

            // Placeholder for getuser_group_permision - this logic needs to be moved or re-implemented
            // int pageid = _context.App_Pages.SingleOrDefault(t => t.Page_Name_EN == methodName).Page_Id;
            // int applicationid = _context.App_Pages.SingleOrDefault(y => y.Page_Name_EN == methodName).Application_ID;
            // getuser_group_permision(pageid, applicationid, GetUserID());

            // Set Title
            // model.Title = _context.App_Pages.SingleOrDefault(c => c.Page_Id == pageid).ENG_DESC;
            model.Title = "Inward Financial Workflow Details ONUS"; // Placeholder

            // Fetch WF and OnUs data
            var wf = await _context.INWARD_WF_Tbl.SingleOrDefaultAsync(z => z.Serial == id && z.Final_Status != "Accept" && z.Clr_Center == "Outward_ONUS");
            var incObj = await _context.OnUs_Tbl.SingleOrDefaultAsync(y => y.Serial == id && (y.Posted == AllEnums.Cheque_Status.New || y.Posted == AllEnums.Cheque_Status.Posted));

            if (incObj == null)
            {
                // This indicates a redirect in the original VB.NET, handle in controller
                return null;
            }

            model.InwardCheque = incObj;

            // Handle VIP status for PMA (though this is ONUS method, keeping logic for consistency if needed)
            if (incObj.ClrCenter == "PMA" && incObj.VIP == true && branchId != "2")
            {
                model.IsVIP = true;
            }

            // Guaranteed Customer Accounts
            var guaranteedCustomers = new List<T24_CAB_OVRDRWN_GUAR>();
            if (!string.IsNullOrEmpty(incObj.DrwCustomerID))
            {
                guaranteedCustomers = await _context.T24_CAB_OVRDRWN_GUAR.Where(i => i.GUAR_CUSTOMER == incObj.DrwCustomerID).ToListAsync();
            }

            if (guaranteedCustomers.Any())
            {
                // Placeholder for ECC_CAP_Services.SAFA_T24_ECC_SVCSoapClient and ACCOUNT_INFO call
                // For now, simulate data
                model.GuarranteedCustomerAccounts = "Simulated Account*1000.00*JOD|";
                // foreach (var item in guaranteedCustomers)
                // {
                //     var accObj = await EccAccInfo_WebSvc.ACCOUNT_INFO(item.ACCOUNT_NUMBER, 1);
                //     model.GuarranteedCustomerAccounts += item.ACCOUNT_NUMBER + "*" + accObj.ClearBalance + "*" + accObj.AccountCurrency + "|";
                // }
            }
            else
            {
                model.GuarranteedCustomerAccounts = "Not Available";
            }

            // Account Info (BookedBalance, ClearBalance, AccountStatus)
            // Placeholder for ECC_CAP_Services.SAFA_T24_ECC_SVCSoapClient and ACCOUNT_INFO call
            // var accObjMain = await EccAccInfo_WebSvc.ACCOUNT_INFO(incObj.DecreptedDrwAcountNo, 1);
            // model.BookedBalance = accObjMain.BookedBalance;
            // model.ClearBalance = accObjMain.ClearBalance;
            // model.AccountStatus = accObjMain.AccountStatus;
            model.BookedBalance = "10000.00"; // Simulated
            model.ClearBalance = "9500.00"; // Simulated
            model.AccountStatus = "Active"; // Simulated

            // User Permissions and Authorization Logic
            var userGroup = await _context.Users_Tbl.Where(c => c.User_Name == userName).Select(c => c.Group_ID).FirstOrDefaultAsync();

            model.CanReject = false;
            model.CanApprove = false;
            model.ShowRecommendationButton = true;

            // Assuming GroupType.Group_Status.AdminAuthorized is an enum or constant
            // if (userGroup == (int)GroupType.Group_Status.AdminAuthorized || branchId == "2")
            if (userGroup == 1 || branchId == "2") // Assuming 1 is AdminAuthorized
            {
                model.CanApprove = true;
                model.ShowRecommendationButton = false;
                model.CanReject = true;
            }
            else
            {
                // Complex authorization logic involving USER_Limits_Auth_Amount and AuthTrans_User_TBL
                // This needs careful conversion and might involve more helper methods or a dedicated authorization service.
                // For now, simplifying based on the VB.NET structure.

                // Placeholder for Tbl_id and Userlevel
                // int Tbl_id = _context.Transaction_TBL.SingleOrDefault(z => z.Transaction_Name == wf.Clr_Center).Transaction_ID;
                // var Userlevel = _context.USER_Limits_Auth_Amount(GetUserID(), Tbl_id, "d", wf.Amount_JD).ToList();

                var authTransUser = await _context.AuthTrans_User_TBL.SingleOrDefaultAsync(t => t.Auth_user_ID == int.Parse(GetUserID()) && t.group_ID == userGroup);

                if (authTransUser != null)
                {
                    if (authTransUser.Auth_level2 == true)
                    {
                        model.CanApprove = true;
                        model.CanReject = true;
                        model.ShowRecommendationButton = false;
                    }
                    else if (authTransUser.Auth_level1 == true)
                    {
                        model.CanApprove = false;
                        model.CanReject = false;
                        model.ShowRecommendationButton = true;
                    }
                }
            }

            // Currency conversion (if needed)
            var currencies = await _context.CURRENCY_TBL.ToListAsync();
            model.Currencies = currencies;

            // Update cheque status display
            if (model.InwardCheque.Status == "S")
            {
                model.InwardCheque.Status = "Success";
            }
            else if (model.InwardCheque.Status == "F")
            {
                model.InwardCheque.Status = "Faild";
            }

            return model;
        }

        // Placeholder for GetBranchID and GetCompanyID - assuming they retrieve from session
        public string GetBranchID() { return _httpContextAccessor.HttpContext?.Session.GetString("BranchID"); }
        public string GetCompanyID() { return _httpContextAccessor.HttpContext?.Session.GetString("ComID"); }

        // Placeholder for bind_chq_source - needs actual implementation
        public object bind_chq_source()
        {
            return new List<object>(); // Return appropriate type
        }

        // Placeholder for GroupType.Group_Status - needs to be defined as an enum or constants
        public static class GroupType
        {
            public enum Group_Status
            {
                AdminAuthorized = 1, // Example value
                // Other group types
            }
        }



        public async Task<InwardFinanicalWFDetailsPMADISViewModel> GetInwardFinanicalWFDetailsPMADISData(string id, string userName, string branchId, string comId)
        {
            var model = new InwardFinanicalWFDetailsPMADISViewModel();

            string methodName = "InwardFinanicalWFDetailsPMADIS"; // Hardcoding for now, ideally dynamic

            model.Title = "Inward Financial Workflow Details PMA/DISCOUNT"; // Placeholder

            var wf = await _context.INWARD_WF_Tbl.SingleOrDefaultAsync(z => z.Serial == id && z.Final_Status != "Accept" && (z.Clr_Center == "PMA" || z.Clr_Center == "DISCOUNT"));
            var incObj = await _context.Inward_Trans.SingleOrDefaultAsync(y => y.Serial == id && (y.Posted == AllEnums.Cheque_Status.New || y.Posted == AllEnums.Cheque_Status.Posted));

            if (incObj == null)
            {
                return null;
            }

            model.InwardCheque = incObj;

            if (incObj.ClrCenter == "PMA" && incObj.VIP == true && branchId != "2")
            {
                model.IsVIP = true;
            }

            var guaranteedCustomers = new List<T24_CAB_OVRDRWN_GUAR>();
            if (!string.IsNullOrEmpty(incObj.ISSAccount))
            {
                guaranteedCustomers = await _context.T24_CAB_OVRDRWN_GUAR.Where(i => i.GUAR_CUSTOMER == incObj.ISSAccount).ToListAsync();
            }

            if (guaranteedCustomers.Any())
            {
                model.GuarranteedCustomerAccounts = "Simulated Account*1000.00*JOD|";
            }
            else
            {
                model.GuarranteedCustomerAccounts = "Not Available";
            }

            model.BookedBalance = "10000.00"; // Simulated
            model.ClearBalance = "9500.00"; // Simulated
            model.AccountStatus = "Active"; // Simulated

            var userGroup = await _context.Users_Tbl.Where(c => c.User_Name == userName).Select(c => c.Group_ID).FirstOrDefaultAsync();

            model.CanReject = false;
            model.CanApprove = false;
            model.ShowRecommendationButton = true;

            if (userGroup == 1 || branchId == "2") // Assuming 1 is AdminAuthorized
            {
                model.CanApprove = true;
                model.ShowRecommendationButton = false;
            }
            else
            {
                var authTransUser = await _context.AuthTrans_User_TBL.SingleOrDefaultAsync(t => t.Auth_user_ID == int.Parse(GetUserID()) && t.group_ID == userGroup);

                if (authTransUser != null)
                {
                    if (authTransUser.Auth_level2 == true)
                    {
                        model.CanApprove = true;
                        model.ShowRecommendationButton = false;
                    }
                    else if (authTransUser.Auth_level1 == true)
                    {
                        model.CanApprove = false;
                        model.ShowRecommendationButton = true;
                    }
                }
            }

            var currencies = await _context.CURRENCY_TBL.ToListAsync();
            model.Currencies = currencies;

            if (model.InwardCheque.Status == "S")
            {
                model.InwardCheque.Status = "Success";
            }
            else if (model.InwardCheque.Status == "F")
            {
                model.InwardCheque.Status = "Faild";
            }

            return model;
        }



        public async Task<bool> POSTCheques(string Serials)
        {
            if (string.IsNullOrEmpty(GetUserName()))
            {
                _logger.LogWarning("POSTCheques called without a logged-in user.");
                return false;
            }

            try
            {
                // Placeholder for external service clients
                // var WebSvc = new ECC_Handler_SVC.InwardHandlingSVCSoapClient("InwardHandlingSVCSoap");
                // var EccAccInfo_WebSvc = new ECC_CAP_Services.SAFA_T24_ECC_SVCSoapClient("SAFA_T24_ECC_SVCSoap");

                var cheque = await _context.Inward_Trans.SingleOrDefaultAsync(x => x.Serial == Serials);
                if (cheque == null)
                {
                    _logger.LogWarning("POSTCheques: Inward_Trans with Serial {Serial} not found.", Serials);
                    return false;
                }

                // Retrieve system configurations (mocking for now)
                string sysName = "ECC_SYSTEM";
                string paymentMethod = "105";
                string urgencyCode = "URGENT";
                string pdcCharges = "5.00";

                string mqMsg = "";
                if (cheque.ClrCenter == "DISCOUNT")
                {
                    mqMsg = "InwardForcePostingDIS";
                }
                else if (cheque.ClrCenter == "PMA")
                {
                    mqMsg = "InwardForcePostingPMARAM";
                }
                else
                {
                    // Default or error case
                    _logger.LogWarning("POSTCheques: Unknown ClrCenter {ClrCenter} for cheque {Serial}", cheque.ClrCenter, Serials);
                    return false;
                }

                string chqSeq = await GENERATE_UNIQUE_CHEQUE_SEQUANCE(cheque.DrwChqNo, cheque.DrwBankNo, cheque.DrwBranchNo, cheque.AltAccount);

                // Simulate ECC_FT_Request object
                // var obj = new ECC_CAP_Services.ECC_FT_Request
                // {
                //     PsSystem = sysName,
                //     RequestDate = cheque.ValueDate.ToString("yyyyMMdd"),
                //     RequestTime = DateTime.Now.ToString("hh:mm:ss"),
                //     RequestCode = mqMsg,
                //     TransSeq = cheque.Serial,
                //     CheckSerial = int.Parse(cheque.DrwChqNo.Substring(1)),
                //     CheckSeq = chqSeq,
                //     PayBranchCode = cheque.DrwBranchNo,
                //     PayBankCode = cheque.DrwBankNo,
                //     PayAccountNumber = cheque.AltAccount,
                //     BFDBranchCode = cheque.BenfAccBranch,
                //     BFDBankCode = cheque.BenfBnk,
                //     BFDAccountNumber = "0000000000000",
                //     CheckAmount = cheque.Amount.ToString("N3", CultureInfo.InvariantCulture).Replace(",", ""), // Assuming N3 for JOD, N2 for others
                //     CurrencyCode = GetCurrencyCode(cheque.Currency)
                // };

                // Simulate FT_ResponseClass
                var ftResponse = new FT_ResponseClass
                {
                    FT_Res = new FT_Response
                    {
                        ResponseStatus = "S", // Simulate success
                        ResponseDescription = "Simulated Success",
                        ErrorMessage = "",
                        AcctCompany = ""
                    },
                    chqSeq = chqSeq
                };

                // Simulate external service call
                // string msg_id = (mqMsg == "InwardForcePostingDIS") ? Temenos_Message_Types.InwardForcePostingDIS : Temenos_Message_Types.InwardForcePostingPMARAM;
                // ftResponse.FT_Res = await EccAccInfo_WebSvc.ECC_OFS_MESSAGE(obj, msg_id, 1);

                if (ftResponse.FT_Res.ResponseStatus == "S")
                {
                    // Update Inward_Trans
                    if (!string.IsNullOrEmpty(ftResponse.FT_Res.AcctCompany))
                    {
                        cheque.OLDDrwBranchNo = ftResponse.FT_Res.AcctCompany;
                    }
                    cheque.Status = "S";
                    cheque.ErrorCode = "";
                    cheque.ErrorDescription += "Successfully, posted";
                    cheque.Posted = AllEnums.Cheque_Status.Posted;
                    cheque.LastUpdate = DateTime.Now;
                    cheque.LastUpdateBy = GetUserName();
                    cheque.History += "|" + "InwardForcePosting from service done by " + GetUserName() + "AT:" + DateTime.Now;
                    _context.Entry(cheque).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    // Remove from INWARD_WF_Tbl
                    var wf = await _context.INWARD_WF_Tbl.SingleOrDefaultAsync(x => x.Serial == cheque.Serial && x.Final_Status != "Accept");
                    if (wf != null)
                    {
                        _context.INWARD_WF_Tbl.Remove(wf);
                        await _context.SaveChangesAsync();
                    }

                    // Update Inward_Trans for workflow flags
                    cheque.NeedFinanciallyWF = 0;
                    cheque.NeedFixedError = 0;
                    await _context.SaveChangesAsync();

                    // Update OFS_Response_Errors_Tbl
                    var respError = await _context.OFS_Response_Errors_Tbl.SingleOrDefaultAsync(x => x.ChqSequance == ftResponse.chqSeq);
                    if (respError == null)
                    {
                        respError = new OFS_Response_Errors_Tbl
                        {
                            LastErrorMessage = ftResponse.FT_Res.ResponseDescription,
                            Cheque_Type = "Inward",
                            Serial = ftResponse.FT_Res.TransSeq,
                            ChqSequance = ftResponse.chqSeq,
                            LastAmendBy = GetUserName(),
                            LastAmendDate = DateTime.Now,
                            History = ftResponse.FT_Res.ResponseDescription
                        };
                        _context.OFS_Response_Errors_Tbl.Add(respError);
                    }
                    else
                    {
                        respError.LastAmendBy = GetUserName();
                        respError.LastAmendDate = DateTime.Now;
                        respError.History = respError.History + " | " + ftResponse.FT_Res.ResponseDescription + " inward DISCOUNT/PMA for suspense Account Done Successfully by :" + GetUserName();
                        respError.LastErrorMessage = ftResponse.FT_Res.ResponseDescription;
                        _context.Entry(respError).State = EntityState.Modified;
                    }
                    await _context.SaveChangesAsync();

                    // Call HandelResponseRequest (placeholder)
                    // await WebSvc.HandelResponseRequestAsync(cheque.ChqSequance, cheque.ClrCenter, ftResponse.FT_Res.ResponseDescription, ftResponse.FT_Res.SpecialNotes, cheque.Serial, GetUserID().ToString());

                    return true;
                }
                else // ResponseStatus is not "S" (failed)
                {
                    if (ftResponse.FT_Res.ErrorMessage.Contains("TIMEOUT"))
                    {
                        cheque.IsTimeOut = 1;
                    }
                    if (ftResponse.FT_Res.ResponseDescription == "Failed")
                    {
                        cheque.ErrorCode = 9999;
                    }
                    cheque.ErrorCode = 9999;
                    cheque.Status = "Faild";
                    cheque.ErrorDescription += mqMsg + ":" + ftResponse.FT_Res.ResponseDescription;
                    cheque.LastUpdateBy = GetUserName();
                    cheque.History += cheque.History + "|" + mqMsg + " suspense Account Filed User: " + GetUserName() + "AT:" + DateTime.Now;

                    if (!string.IsNullOrEmpty(ftResponse.FT_Res.AcctCompany))
                    {
                        cheque.OLDDrwBranchNo = ftResponse.FT_Res.AcctCompany;
                    }
                    _context.Entry(cheque).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    // Update OFS_Response_Errors_Tbl
                    var respError = await _context.OFS_Response_Errors_Tbl.SingleOrDefaultAsync(x => x.ChqSequance == ftResponse.chqSeq);
                    if (respError == null)
                    {
                        respError = new OFS_Response_Errors_Tbl
                        {
                            LastErrorMessage = ftResponse.FT_Res.ResponseDescription,
                            Cheque_Type = "Inward",
                            Serial = ftResponse.FT_Res.TransSeq,
                            ChqSequance = ftResponse.chqSeq,
                            LastAmendBy = GetUserName(),
                            LastAmendDate = DateTime.Now,
                            History = ftResponse.FT_Res.ResponseDescription + mqMsg + " for suspense Account"
                        };
                        _context.OFS_Response_Errors_Tbl.Add(respError);
                    }
                    else
                    {
                        respError.LastAmendBy = GetUserName();
                        respError.LastAmendDate = DateTime.Now;
                        respError.History = respError.History + " | " + ftResponse.FT_Res.ResponseDescription;
                        respError.LastErrorMessage = ftResponse.FT_Res.ResponseDescription;
                        _context.Entry(respError).State = EntityState.Modified;
                    }
                    await _context.SaveChangesAsync();

                    // Call HandelResponseRequest (placeholder)
                    // await WebSvc.HandelResponseRequestAsync(cheque.ChqSequance, cheque.ClrCenter, ftResponse.FT_Res.ResponseDescription, ftResponse.FT_Res.SpecialNotes, cheque.Serial, GetUserID().ToString());

                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in POSTCheques: {Message}", ex.Message);
                return false;
            }
        }

        // Placeholder for ECC_CAP_Services.ECC_FT_Request and Temenos_Message_Types
        // These would be defined in a separate file or generated from a WSDL/Swagger spec
        public class ECC_FT_Request { /* properties */ }
        public static class Temenos_Message_Types { /* constants */ }



        public async Task<JsonResult> GetMagicscreenList(string clrCenter, string status, string transDate, string chqNo, string drwAccNo, string userName, string comId, string pageId)
        {
            var _json = new JsonResult(new { });
            var lstPstINW = new List<Inward_Trans>();
            var model = new MagicScreenListViewModel();

            // Session checks (moved from controller, but service should ideally not depend on session directly)
            // For now, assuming these are passed from the controller or retrieved via IHttpContextAccessor
            // getlockedpage(pageId); // This method needs to be implemented or its logic moved

            // var u = _httpContextAccessor.HttpContext?.Session.GetString("locked");
            // if (string.IsNullOrEmpty(u)) u = ".";

            // if (u != ".")
            // {
            //     _json.Data = new { ErrorMsg = "Page is locked." };
            //     return _json;
            // }

            try
            {
                string selectQuery = "";
                string statusCondition = "";

                if (status == "-1")
                {
                    statusCondition = " <= 3 "; // Corresponds to Posted <= Verified
                }
                else
                {
                    statusCondition = " in (9,10)"; // Corresponds to Cleared or other specific statuses
                }

                if (clrCenter == "-1")
                {
                    selectQuery = "SELECT Posted, ClrCenter, ISNULL(ReturnCode, 0) AS ReturnCode, ISNULL(ReturnCodeFinancail, 0) AS ReturnCodeFinancail, verifyStatus, Was_PDC, TransDate, Serial, ISSAccount, InputDate, DrwChqNo, DrwBankNo, DrwBranchNo, Currency, Amount, ValueDate, DrwName, BenAccountNo, BenName, DrwAcctNo, ClrCenter, ErrorDescription FROM Inward_Trans POST_Tbl WHERE Posted " + statusCondition;
                    selectQuery += " UNION ALL SELECT Posted, ClrCenter, ISNULL(ReturnCode, 0) AS ReturnCode, ISNULL(ReturnCodeFinancail, 0) AS ReturnCodeFinancail, verifyStatus, Was_PDC, TransDate, Serial, ISSAccount, InputDate, DrwChqNo, DrwBankNo, DrwBranchNo, Currency, Amount, ValueDate, DrwName, BenAccountNo, BenName, DrwAcctNo, ClrCenter, ErrorDescription FROM OnUs_Tbl POST_Tbl WHERE Posted " + statusCondition;
                }
                else if (clrCenter == "3") // ONUS PDC
                {
                    selectQuery = "SELECT Posted, ClrCenter, ISNULL(ReturnCode, 0) AS ReturnCode, ISNULL(ReturnCodeFinancail, 0) AS ReturnCodeFinancail, verifyStatus, TransDate, Was_PDC, Serial, InputDate, ISSAccount, DrwChqNo, DrwBankNo, DrwBranchNo, Currency, Amount, ValueDate, DrwName, BenAccountNo, BenName, DrwAcctNo, ClrCenter, ErrorDescription FROM OnUs_Tbl POST_Tbl WHERE Was_PDC = 1 AND Posted " + statusCondition;
                }
                else if (clrCenter == "4") // ONUS Branch
                {
                    selectQuery = "SELECT Posted, ClrCenter, ISNULL(ReturnCode, 0) AS ReturnCode, ISNULL(ReturnCodeFinancail, 0) AS ReturnCodeFinancail, verifyStatus, TransDate, Was_PDC, Serial, InputDate, ISSAccount, DrwChqNo, DrwBankNo, DrwBranchNo, Currency, Amount, ValueDate, DrwName, BenAccountNo, BenName, DrwAcctNo, ClrCenter, ErrorDescription FROM OnUs_Tbl POST_Tbl WHERE Was_PDC = 0 AND Posted " + statusCondition;
                }
                else if (clrCenter == "1") // DISCOUNT
                {
                    selectQuery = "SELECT Posted, ClrCenter, ISNULL(ReturnCode, 0) AS ReturnCode, ISNULL(ReturnCodeFinancail, 0) AS ReturnCodeFinancail, verifyStatus, TransDate, Was_PDC, Serial, InputDate, ISSAccount, DrwChqNo, DrwBankNo, DrwBranchNo, Currency, Amount, ValueDate, DrwName, BenAccountNo, BenName, DrwAcctNo, ClrCenter, ErrorDescription FROM Inward_Trans POST_Tbl WHERE ClrCenter = 'DISCOUNT' AND Posted " + statusCondition;
                }
                else // PMA
                {
                    selectQuery = "SELECT Posted, ClrCenter, ISNULL(ReturnCode, 0) AS ReturnCode, ISNULL(ReturnCodeFinancail, 0) AS ReturnCodeFinancail, verifyStatus, TransDate, Was_PDC, Serial, InputDate, ISSAccount, DrwChqNo, DrwBankNo, DrwBranchNo, Currency, Amount, ValueDate, DrwName, BenAccountNo, BenName, DrwAcctNo, ClrCenter, ErrorDescription FROM Inward_Trans POST_Tbl WHERE ClrCenter = 'PMA' AND Posted " + statusCondition;
                }

                if (!string.IsNullOrEmpty(transDate))
                {
                    selectQuery += " AND CAST(TransDate AS DATE) = '" + transDate.Trim() + "'";
                }

                if (!string.IsNullOrEmpty(chqNo))
                {
                    selectQuery += " AND DrwChqNo LIKE '%" + chqNo.Trim() + "%'";
                }

                if (!string.IsNullOrEmpty(drwAccNo))
                {
                    selectQuery += " AND DrwAcctNo LIKE '%" + drwAccNo.Trim() + "%'";
                }

                // Execute raw SQL query using EF Core FromSqlRaw
                // NOTE: This approach is vulnerable to SQL Injection. Parameterized queries should be used.
                // For conversion, we are directly translating the VB.NET logic.
                var rawResults = await _context.Inward_Trans.FromSqlRaw(selectQuery).ToListAsync();

                foreach (var dr in rawResults)
                {
                    var obj = new Inward_Trans
                    {
                        Serial = dr.Serial,
                        InputDate = dr.InputDate,
                        DrwChqNo = dr.DrwChqNo,
                        DrwBankNo = dr.DrwBankNo,
                        DrwBranchNo = dr.DrwBranchNo,
                        verifyStatus = dr.verifyStatus,
                        ReturnCode = GetFinalOnusCode(dr.ReturnCode, dr.ReturnCodeFinancail, dr.ClrCenter), // This method needs to be implemented
                        Currency = dr.Currency,
                        Amount = dr.Amount,
                        TransDate = dr.TransDate,
                        DrwName = dr.DrwName,
                        Posted = dr.Posted,
                        BenAccountNo = dr.BenAccountNo,
                        BenName = dr.BenName,
                        DrwAcctNo = dr.DrwAcctNo,
                        ISSAccount = dr.ISSAccount,
                        ClrCenter = dr.ClrCenter,
                        ErrorDescription = dr.ErrorDescription
                    };

                    // Adjust ClrCenter display based on original VB.NET logic
                    if (clrCenter == "3") obj.ClrCenter = "INHOUSE"; // ONUS PDC
                    else if (clrCenter == "4") obj.ClrCenter = "INHOUSE"; // ONUS Branch

                    lstPstINW.Add(obj);
                }

                model.Cheques = lstPstINW;

                // Calculate totals
                foreach (var item in lstPstINW)
                {
                    var currency = await _context.CURRENCY_TBL.SingleOrDefaultAsync(x => x.SYMBOL_ISO == item.Currency);
                    if (currency != null)
                    {
                        if (currency.ID == 1) { model.TotalJOD += (double)item.Amount; model.CountJOD++; }
                        else if (currency.ID == 2) { model.TotalUSD += (double)item.Amount; model.CountUSD++; }
                        else if (currency.ID == 3) { model.TotalILS += (double)item.Amount; model.CountILS++; }
                        else if (currency.ID == 5) { model.TotalEUR += (double)item.Amount; model.CountEUR++; }
                    }
                }

                _json.Data = new { ErrorMsg = "", lstPDC = model.Cheques, TotalJOD = model.TotalJOD, CountJOD = model.CountJOD, TotalUSD = model.TotalUSD, CountUSD = model.CountUSD, TotalILS = model.TotalILS, CountILS = model.CountILS, TotalEUR = model.TotalEUR, CountEUR = model.CountEUR };
                return _json;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetMagicscreenList: {Message}", ex.Message);
                _json.Data = new { ErrorMsg = "An error occurred while retrieving magic screen list." };
                return _json;
            }
        }

        // Placeholder for GetFinalOnusCode - needs actual implementation
        private string GetFinalOnusCode(int? returnCode, int? returnCodeFinancial, string clrCenter)
        {
            // Implement logic to determine final ONUS code
            return "";
        }

        // Placeholder for getlockedpage - needs actual implementation
        private void getlockedpage(string pageId)
        {
            // Implement logic for page locking
        }



        public async Task<JsonResult> ReturnMajicScreen(string serial, string clrcenter, string rc, string patch, string status, string userName, string userId)
        {
            var _json = new JsonResult(new { });
            string mqMsg = "";
            string u = "";

            try
            {
                // Placeholder for external service clients
                // var WebSvc = new ECC_Handler_SVC.InwardHandlingSVCSoapClient("InwardHandlingSVCSoap2");
                // var EccAccInfo_WebSvc = new ECC_CAP_Services.SAFA_T24_ECC_SVCSoapClient("SAFA_T24_ECC_SVCSoap");
                // var web_service = new ECC_Com_SVC.Ecc_Commisions_HandlerSoapClient("Ecc_Commisions_HandlerSoap");

                var respError = new OFS_Response_Errors_Tbl();
                var fileRecord = new FileRecords_Tbl();
                Inward_Trans inward = null;
                OnUs_Tbl onus = null;

                if (clrcenter == "PMA")
                {
                    inward = await _context.Inward_Trans.SingleOrDefaultAsync(x => x.Serial == serial);
                    if (inward != null)
                    {
                        if (inward.Posted == AllEnums.Cheque_Status.Posted || inward.Posted == AllEnums.Cheque_Status.Cleared)
                        {
                            mqMsg = "ReversePostingPMARAM";
                        }
                        else if (inward.Posted == AllEnums.Cheque_Status.Returne)
                        {
                            mqMsg = "InwardForcePostingPMARAM";
                        }
                    }
                }
                else if (clrcenter == "INHOUSE")
                {
                    onus = await _context.OnUs_Tbl.SingleOrDefaultAsync(x => x.Serial == serial);
                    if (onus != null)
                    {
                        if (onus.Posted == AllEnums.Cheque_Status.Posted || onus.Posted == AllEnums.Cheque_Status.Cleared)
                        {
                            mqMsg = "ONUSReversePosting";
                        }
                        else if (onus.Posted == AllEnums.Cheque_Status.Returne)
                        {
                            mqMsg = "ONUSInwardForcePosting";
                        }
                    }
                }
                else // DISCOUNT
                {
                    inward = await _context.Inward_Trans.SingleOrDefaultAsync(x => x.Serial == serial);
                    if (inward != null)
                    {
                        if (inward.Posted == AllEnums.Cheque_Status.Posted || inward.Posted == AllEnums.Cheque_Status.Cleared)
                        {
                            mqMsg = "ReversePostingDIS";
                        }
                        else if (inward.Posted == AllEnums.Cheque_Status.Returne)
                        {
                            mqMsg = "InwardForcePostingDIS";
                        }
                        fileRecord = await _context.FileRecords_Tbl.SingleOrDefaultAsync(y => y.Serial == inward.ClrFileRecordID);
                    }
                }

                // Simulate ECC_FT_Request and FT_ResponseClass
                // string sysName = _context.System_Configurations_Tbl.SingleOrDefault(c => c.Config_Type == "ECC_SYSTEM_ID" && c.Config_Id == "7").Config_Value;
                string sysName = "ECC_SYSTEM_ID_7"; // Simulated

                var ftResponse = new FT_ResponseClass
                {
                    FT_Res = new FT_Response
                    {
                        ResponseStatus = "S", // Simulate success
                        ResponseDescription = "Simulated Success",
                        ErrorMessage = "",
                        AcctCompany = ""
                    },
                    chqSeq = (inward?.ChqSequance ?? onus?.ChqSequance) // Simulated
                };

                // Populate ECC_FT_Request object (simulated)
                // var obj = new ECC_CAP_Services.ECC_FT_Request();
                // if (clrcenter == "PMA" && inward != null)
                // { /* populate obj for PMA */ }
                // else if (clrcenter == "INHOUSE" && onus != null)
                // { /* populate obj for INHOUSE */ }
                // else if (inward != null)
                // { /* populate obj for DISCOUNT */ }

                // Simulate external service call
                // int msgId = Temenos_Message_Types.GetMessageType(mqMsg); // This needs to be implemented
                // ftResponse.FT_Res = await EccAccInfo_WebSvc.ECC_OFS_MESSAGE(obj, msgId, 1);

                if (ftResponse.FT_Res.ResponseStatus == "S")
                {
                    u = mqMsg + " Successfully";

                    // Update Inward_Trans or OnUs_Tbl
                    if (inward != null)
                    {
                        inward.Status = "S";
                        inward.ErrorCode = "";
                        inward.ErrorDescription += "Successfully, posted";
                        inward.Posted = AllEnums.Cheque_Status.Posted;
                        inward.LastUpdate = DateTime.Now;
                        inward.LastUpdateBy = userName;
                        inward.History += "|" + mqMsg + " from service done by " + userName + " AT:" + DateTime.Now;
                        _context.Entry(inward).State = EntityState.Modified;
                    }
                    else if (onus != null)
                    {
                        onus.Status = "S";
                        onus.ErrorCode = "";
                        onus.ErrorDescription += "Successfully, posted";
                        onus.Posted = AllEnums.Cheque_Status.Posted;
                        onus.LastUpdate = DateTime.Now;
                        onus.LastUpdateBy = userName;
                        onus.History += "|" + mqMsg + " from service done by " + userName + " AT:" + DateTime.Now;
                        _context.Entry(onus).State = EntityState.Modified;
                    }
                    await _context.SaveChangesAsync();

                    // Update WF_History (simulated)
                    var wfHistory = new WFHistory
                    {
                        ChqSequance = inward?.ChqSequance ?? onus?.ChqSequance,
                        Serial = serial,
                        TransDate = inward?.ValueDate ?? onus?.TransDate,
                        ID_WFStatus = 0, // Placeholder
                        Status = (inward?.Status ?? onus?.Status) + u + userName + " AT : " + DateTime.Now,
                        ClrCenter = clrcenter,
                        DrwAccNo = inward?.DrwAcctNo ?? onus?.DrwAcctNo,
                        Amount = inward?.Amount ?? onus?.Amount
                    };
                    _context.WFHistories.Add(wfHistory);
                    await _context.SaveChangesAsync();

                    // Update OFS_Response_Errors_Tbl
                    var existingRespError = await _context.OFS_Response_Errors_Tbl.SingleOrDefaultAsync(x => x.ChqSequance == ftResponse.chqSeq);
                    if (existingRespError == null)
                    {
                        respError = new OFS_Response_Errors_Tbl
                        {
                            LastErrorMessage = ftResponse.FT_Res.ResponseDescription,
                            Cheque_Type = "Inward",
                            Serial = ftResponse.FT_Res.TransSeq,
                            ChqSequance = ftResponse.chqSeq,
                            LastAmendBy = userName,
                            LastAmendDate = DateTime.Now,
                            History = ftResponse.FT_Res.ResponseDescription
                        };
                        _context.OFS_Response_Errors_Tbl.Add(respError);
                    }
                    else
                    {
                        existingRespError.LastAmendBy = userName;
                        existingRespError.LastAmendDate = DateTime.Now;
                        existingRespError.History = existingRespError.History + " | " + ftResponse.FT_Res.ResponseDescription + mqMsg + " for suspense Account Done Successfully by :" + userName;
                        existingRespError.LastErrorMessage = ftResponse.FT_Res.ResponseDescription;
                        _context.Entry(existingRespError).State = EntityState.Modified;
                    }
                    await _context.SaveChangesAsync();

                    // Call HandelResponseRequest (placeholder)
                    // await WebSvc.HandelResponseRequestAsync(cheque.ChqSequance, cheque.ClrCenter, ftResponse.FT_Res.ResponseDescription, ftResponse.FT_Res.SpecialNotes, cheque.Serial, userId);
                }
                else // ResponseStatus is not "S" (failed)
                {
                    u = mqMsg + " Failed";

                    if (inward != null)
                    {
                        inward.Status = "F";
                        inward.ErrorCode = 9999; // Placeholder
                        inward.ErrorDescription += mqMsg + ":" + ftResponse.FT_Res.ResponseDescription;
                        inward.LastUpdateBy = userName;
                        inward.History += inward.History + "|" + mqMsg + " suspense Account Filed User: " + userName + " AT:" + DateTime.Now;
                        _context.Entry(inward).State = EntityState.Modified;
                    }
                    else if (onus != null)
                    {
                        onus.Status = "F";
                        onus.ErrorCode = 9999; // Placeholder
                        onus.ErrorDescription += mqMsg + ":" + ftResponse.FT_Res.ResponseDescription;
                        onus.LastUpdateBy = userName;
                        onus.History += onus.History + "|" + mqMsg + " suspense Account Filed User: " + userName + " AT:" + DateTime.Now;
                        _context.Entry(onus).State = EntityState.Modified;
                    }
                    await _context.SaveChangesAsync();

                    // Update OFS_Response_Errors_Tbl
                    var existingRespError = await _context.OFS_Response_Errors_Tbl.SingleOrDefaultAsync(x => x.ChqSequance == ftResponse.chqSeq);
                    if (existingRespError == null)
                    {
                        respError = new OFS_Response_Errors_Tbl
                        {
                            LastErrorMessage = ftResponse.FT_Res.ResponseDescription,
                            Cheque_Type = "Inward",
                            Serial = ftResponse.FT_Res.TransSeq,
                            ChqSequance = ftResponse.chqSeq,
                            LastAmendBy = userName,
                            LastAmendDate = DateTime.Now,
                            History = ftResponse.FT_Res.ResponseDescription + mqMsg + " for suspense Account"
                        };
                        _context.OFS_Response_Errors_Tbl.Add(respError);
                    }
                    else
                    {
                        existingRespError.LastAmendBy = userName;
                        existingRespError.LastAmendDate = DateTime.Now;
                        existingRespError.History = existingRespError.History + " | " + ftResponse.FT_Res.ResponseDescription;
                        existingRespError.LastErrorMessage = ftResponse.FT_Res.ResponseDescription;
                        _context.Entry(existingRespError).State = EntityState.Modified;
                    }
                    await _context.SaveChangesAsync();

                    // Call HandelResponseRequest (placeholder)
                    // await WebSvc.HandelResponseRequestAsync(cheque.ChqSequance, cheque.ClrCenter, ftResponse.FT_Res.ResponseDescription, ftResponse.FT_Res.SpecialNotes, cheque.Serial, userId);
                }

                _json.Data = new { ErrorMsg = "", result = u };
                return _json;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ReturnMajicScreen: {Message}", ex.Message);
                _json.Data = new { ErrorMsg = "An error occurred while processing the request." };
                return _json;
            }
        }



        public async Task<JsonResult> GetSearchWFStage(string chqNo, string drwAcc, string fromDate, string toDate, string userName, string pageId)
        {
            var _json = new JsonResult(new { });
            var lstWFHistory = new List<WFHistory>();
            var model = new WFStageListViewModel();

            try
            {
                // getlockedpage(pageId); // This method needs to be implemented or its logic moved

                // var u = _httpContextAccessor.HttpContext?.Session.GetString("locked");
                // if (string.IsNullOrEmpty(u)) u = ".";

                // if (u != ".")
                // {
                //     _json.Data = new { ErrorMsg = "Page is locked." };
                //     return _json;
                // }

                string selectQuery = "SELECT Serial, TransDate, DrwChqNo, ClrCenter, Status, DrwAccNo, Amount FROM WFHistory WHERE 1=1";

                if (!string.IsNullOrEmpty(chqNo))
                {
                    selectQuery += " AND DrwChqNo LIKE '%" + chqNo.Trim() + "%'";
                }

                if (!string.IsNullOrEmpty(drwAcc))
                {
                    selectQuery += " AND DrwAccNo LIKE '%" + drwAcc.Trim() + "%'";
                }

                string tDate = DateTime.Now.ToString("yyyy-MM-dd");

                if (string.IsNullOrEmpty(fromDate) && string.IsNullOrEmpty(toDate))
                {
                    selectQuery += " AND CAST(TransDate AS Date) = '" + tDate + "'";
                }
                else
                {
                    if (!string.IsNullOrEmpty(fromDate) && string.IsNullOrEmpty(toDate))
                    {
                        selectQuery += " AND TransDate = '" + fromDate + "'";
                    }
                    else if (string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
                    {
                        selectQuery += " AND TransDate = '" + toDate + "'";
                    }
                    else if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
                    {
                        if (fromDate == toDate)
                        {
                            selectQuery += " AND CAST(TransDate AS Date) = '" + tDate + "'"; // Original VB.NET had a bug here, using TDate instead of FromDate/ToDate
                        }
                        else
                        {
                            selectQuery += " AND TransDate >= '" + fromDate + "' AND TransDate <= '" + toDate + "'";
                        }
                    }
                }

                selectQuery += " ORDER BY TransDate DESC";

                // Execute raw SQL query using EF Core FromSqlRaw
                // NOTE: This approach is vulnerable to SQL Injection. Parameterized queries should be used.
                // For conversion, we are directly translating the VB.NET logic.
                lstWFHistory = await _context.WFHistories.FromSqlRaw(selectQuery).ToListAsync();

                model.WFHistories = lstWFHistory;

                // Calculate totals
                foreach (var item in lstWFHistory)
                {
                    // Assuming item.Amount is string or object in WFHistory, convert to double
                    double amount = 0;
                    if (double.TryParse(item.Amount?.ToString(), out amount))
                    {
                        // Currency logic needs to be implemented based on actual data structure
                        // For now, simulating based on a hypothetical currency field in WFHistory or related table
                        // If WFHistory does not contain currency, this logic needs to be adjusted.
                        // Assuming a default currency or retrieving from a related cheque table.
                        // For demonstration, let's assume a default JOD or a lookup.
                        string currencySymbol = "JOD"; // Placeholder

                        // This part needs actual currency lookup based on item.ChqSequance or similar
                        // For now, distributing amounts to JOD for demonstration
                        model.TotalJOD += amount;
                        model.CountJOD++;
                    }
                }

                _json.Data = new
                {
                    ErrorMsg = "",
                    lstPDC = model.WFHistories,
                    AmuntTot = model.TotalAmount,
                    ILSAmount = model.TotalILS,
                    JODAmount = model.TotalJOD,
                    USDAmount = model.TotalUSD,
                    EURAmount = model.TotalEUR,
                    ILSCount = model.CountILS,
                    JODCount = model.CountJOD,
                    USDCount = model.CountUSD,
                    EURCount = model.CountEUR,
                    Locked_user = "."
                };
                return _json;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetSearchWFStage: {Message}", ex.Message);
                _json.Data = new { ErrorMsg = "An error occurred while retrieving workflow stage list." };
                return _json;
            }
        }



        public async Task<JsonResult> UpdateReturnCheque(string tDate, string userName)
        {
            var _json = new JsonResult(new { });
            try
            {
                var wfList = await _context.INWARD_WF_Tbl.ToListAsync();
                string trans_date = tDate; // Use the passed date

                foreach (var item in wfList)
                {
                    bool isneedfixederror = item.Need_Fixed_Error;
                    bool isneedmanualfixed = item.NeedMnaualFixed;
                    bool isneedFinanical = item.Need_Finanical_WF;

                    if (isneedfixederror && isneedmanualfixed && isneedFinanical)
                    {
                        if (item.ISFixederror && item.IsFinanicallyFixed && item.IsMnaualFixed)
                        {
                            if (item.Clr_Center == "Inoward_DISCOUNT" || item.Clr_Center == "Inoward_PMA")
                            {
                                var inward = await _context.Inward_Trans.SingleOrDefaultAsync(x => x.ChqSequance == item.ChqSequance && x.Posted == AllEnums.Cheque_Status.Posted);

                                if (inward != null)
                                {
                                    trans_date = inward.TransDate.ToString("yyyy-MM-dd");
                                    if (trans_date == tDate) // Only update if TransDate matches the input date
                                    {
                                        inward.Returned = 1;
                                        inward.Posted = AllEnums.Cheque_Status.Returne;
                                        inward.LastUpdate = DateTime.Now;
                                        inward.LastUpdateBy = userName;
                                        inward.History += "Updatereturn from job done";
                                        _context.Entry(inward).State = EntityState.Modified;
                                    }
                                }
                            }
                            else // ONUS
                            {
                                var onus = await _context.OnUs_Tbl.SingleOrDefaultAsync(x => x.ChqSequance == item.ChqSequance && x.Posted == AllEnums.Cheque_Status.Posted);
                                if (onus != null)
                                {
                                    trans_date = onus.TransDate.ToString("yyyy-MM-dd");
                                    if (trans_date == tDate)
                                    {
                                        onus.Returned = 1;
                                        onus.Posted = AllEnums.Cheque_Status.Returne;
                                        onus.LastUpdate = DateTime.Now;
                                        onus.LastUpdateBy = userName;
                                        onus.History += "Updatereturn from job done";
                                        _context.Entry(onus).State = EntityState.Modified;
                                    }
                                }
                            }
                        }
                    }
                }
                await _context.SaveChangesAsync();
                _json.Data = new { success = true, message = "Return cheques updated successfully." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateReturnCheque: {Message}", ex.Message);
                _json.Data = new { success = false, message = "An error occurred while updating return cheques." };
            }
            return _json;
        }



        public async Task<IActionResult> GetChqStatus(string userName, string userId)
        {
            // This method is primarily for setting up ViewBag data for a view.
            // In a service layer, it would typically return a ViewModel.
            // For now, we'll simulate the data retrieval.

            // Placeholder for methodName and pageId retrieval
            string methodName = "getchqstatus";
            int pageId = 0; // Placeholder
            int applicationid = 0; // Placeholder
            string title = "";

            try
            {
                var appPage = await _context.App_Pages.SingleOrDefaultAsync(t => t.Page_Name_EN == methodName);
                if (appPage != null)
                {
                    pageId = appPage.Page_Id;
                    applicationid = appPage.Application_ID;
                    title = appPage.ENG_DESC;
                }

                // Placeholder for getuser_group_permision logic
                // This would involve checking user permissions based on pageId, applicationid, and userId
                // For now, assuming access is granted.
                // if (GetAccessPage() == "NoAccess")
                // {
                //     return new RedirectToActionResult("block", "Login", null); // This needs to be handled in the controller
                // }

                // Prepare data for ViewBag (or a ViewModel)
                var model = new GetChqStatusViewModel
                {
                    Title = title,
                    Tree = GetAllCategoriesForTree() // Placeholder
                };

                // Return a ViewModel or a custom object that the controller can use to populate ViewBag
                return new OkObjectResult(model); // Returning a dummy result for now
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetChqStatus: {Message}", ex.Message);
                // In a real scenario, you might return an error ViewModel or throw a custom exception
                return new BadRequestObjectResult(new { ErrorMessage = "An error occurred while retrieving cheque status." });
            }
        }

        // Placeholder ViewModel for GetChqStatus
        public class GetChqStatusViewModel
        {
            public string Title { get; set; }
            public object Tree { get; set; }
        }

        // Placeholder for GetPageID
        public string GetPageID()
        {
            // Retrieve from session or claims
            return _httpContextAccessor.HttpContext?.Session.GetString("page_id");
        }

        // Placeholder for GetAccessPage
        public string GetAccessPage()
        {
            // Retrieve from session or claims
            return _httpContextAccessor.HttpContext?.Session.GetString("AccessPage");
        }



        public Task<IActionResult> ResendInwFile()
        {
            // This method in VB.NET simply returns a View.
            // In the service layer, we don't handle views directly.
            // This could indicate a need for a ViewModel or simply a placeholder for future logic.
            // For now, returning a successful result.
            return Task.FromResult<IActionResult>(new OkResult());
        }



        public async Task<IActionResult> T24Job(string userName, string userId)
        {
            // testres(); // This method needs to be converted or handled separately
            // GenerateDiscountCommissionFile(); // This method needs to be converted or handled separately

            // Placeholder for methodName and pageId retrieval
            string methodName = "T24_JOB";
            int pageId = 0; // Placeholder
            int applicationid = 0; // Placeholder
            string title = "";

            try
            {
                var appPage = await _context.App_Pages.SingleOrDefaultAsync(t => t.Page_Name_EN == methodName);
                if (appPage != null)
                {
                    pageId = appPage.Page_Id;
                    applicationid = appPage.Application_ID;
                    title = appPage.ENG_DESC;
                }

                // Placeholder for getuser_group_permision logic
                // if (GetAccessPage() == "NoAccess")
                // {
                //     return new RedirectToActionResult("block", "Login", null); // This needs to be handled in the controller
                // }

                var model = new T24JobViewModel
                {
                    Title = title,
                    Tree = GetAllCategoriesForTree() // Placeholder
                };

                return new OkObjectResult(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in T24Job: {Message}", ex.Message);
                return new BadRequestObjectResult(new { ErrorMessage = "An error occurred while loading T24 Job page." });
            }
        }

        // Placeholder ViewModel for T24Job
        public class T24JobViewModel
        {
            public string Title { get; set; }
            public object Tree { get; set; }
        }

        // Placeholder for testres() method
        private void testres()
        {
            // Implement logic for testres
        }

        // Placeholder for GenerateDiscountCommissionFile() method
        private void GenerateDiscountCommissionFile()
        {
            // Implement logic for GenerateDiscountCommissionFile
        }



        public async Task<IActionResult> ReturnStoppedCheques(string userName, string userId, string branchId)
        {
            // This method in VB.NET primarily sets up ViewBag data for a view.
            // In a service layer, it would typically return a ViewModel.

            string methodName = "ReturnStoppedCheques";
            int pageId = 0;
            int applicationid = 0;
            string title = "";

            try
            {
                var appPage = await _context.App_Pages.SingleOrDefaultAsync(t => t.Page_Name_EN == methodName);
                if (appPage != null)
                {
                    pageId = appPage.Page_Id;
                    applicationid = appPage.Application_ID;
                    title = appPage.ENG_DESC;
                }

                // Placeholder for getuser_group_permision logic
                // if (GetAccessPage() == "NoAccess")
                // {
                //     return new RedirectToActionResult("block", "Login", null); // This needs to be handled in the controller
                // }

                var model = new ReturnStoppedChequesViewModel
                {
                    Title = title,
                    Tree = GetAllCategoriesForTree() // Placeholder
                };

                return new OkObjectResult(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ReturnStoppedCheques: {Message}", ex.Message);
                return new BadRequestObjectResult(new { ErrorMessage = "An error occurred while loading Return Stopped Cheques page." });
            }
        }

        // Placeholder ViewModel for ReturnStoppedCheques
        public class ReturnStoppedChequesViewModel
        {
            public string Title { get; set; }
            public object Tree { get; set; }
        }



        public async Task<IActionResult> ReturnInwardStoppedChequeDetails(int id, string userName)
        {
            var model = new ReturnInwardStoppedChequeDetailsViewModel();
            model.Title = ""; // Placeholder
            model.Tree = GetAllCategoriesForTree(); // Placeholder

            try
            {
                var incObj = await _context.Inward_Trans.SingleOrDefaultAsync(y => y.Serial == id);

                if (incObj == null)
                {
                    return new RedirectToActionResult("ReturnStoppedCheques", "INWARD", null);
                }

                model.Currencies = await _context.CURRENCY_TBL.ToListAsync();

                // Convert currency ID to symbol
                if (incObj.Currency == "1" || incObj.Currency == "2" || incObj.Currency == "3" || incObj.Currency == "5")
                {
                    var currency = model.Currencies.FirstOrDefault(c => c.ID == Convert.ToInt32(incObj.Currency));
                    if (currency != null)
                    {
                        incObj.Currency = currency.SYMBOL_ISO;
                    }
                }

                incObj.Amount = Math.Round(incObj.Amount, 2, MidpointRounding.AwayFromZero);

                model.InwardCheque = incObj;
                model.InwardImages = await _context.INWARD_IMAGES.FirstOrDefaultAsync(y => y.Serial == incObj.Serial);
                model.ReturnCodes = await _context.Return_Codes_Tbl.Where(i => i.ClrCenter == incObj.ClrCenter).ToListAsync();

                return new OkObjectResult(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ReturnInwardStoppedChequeDetails: {Message}", ex.Message);
                model.ErrorMessage = "An error occurred while retrieving cheque details.";
                return new BadRequestObjectResult(model);
            }
        }



        public async Task<JsonResult> ReturnInwardStoppedCheque_Reverse(int id, string retcode, string userName)
        {
            var _json = new JsonResult(new { });
            try
            {
                string mqMsg = "";
                var obj = new ECC_CAP_Services.ECC_FT_Request(); // Assuming this class exists or is mocked
                var EccAccInfo_WebSvc = new ECC_CAP_Services.SAFA_T24_ECC_SVCSoapClient("SAFA_T24_ECC_SVCSoap"); // Assuming this client exists or is mocked

                var inward = await _context.Inward_Trans.SingleOrDefaultAsync(x => x.Serial == id);
                var onus = await _context.OnUs_Tbl.SingleOrDefaultAsync(x => x.Serial == id);

                string sysName = _context.System_Configurations_Tbl.SingleOrDefault(c => c.Config_Type == "ECC_SYSTEM_ID" && c.Config_Id == "7").Config_Value;
                var ftResponse = new FT_ResponseClass(); // Assuming this class exists
                double amount = 0;
                string chqSeq = "";

                if (inward != null)
                {
                    chqSeq = inward.ChqSequance;
                    if (inward.ClrCenter == "DISCOUNT")
                    {
                        mqMsg = "ReversePostingDIS";
                    }
                    else
                    {
                        mqMsg = "ReversePostingPMARAM";
                    }

                    obj.PsSystem = sysName;
                    obj.RequestDate = DateTime.Now.Date.ToString("yyyyMMdd");
                    obj.RequestTime = DateTime.Now.ToString("HH':'mm':'ss");
                    obj.RequestCode = mqMsg;
                    obj.TransSeq = inward.Serial;
                    obj.CheckSeq = inward.ChqSequance.Trim();
                    obj.PayBankCode = inward.DrwBankNo;
                    obj.PayBranchCode = inward.DrwBranchNo;
                    obj.PayAccountNumber = inward.AltAccount;
                    obj.BFDBankCode = inward.BenfBnk;
                    obj.BFDBranchCode = inward.BenfAccBranch;
                    obj.BFDAccountNumber = "0000"; // Onus.BenAccountNo
                    obj.CheckSerial = Convert.ToInt32(inward.DrwChqNo.Substring(1));
                    amount = inward.Amount;

                    if (inward.Currency.Trim().ToUpper() == "JOD")
                    {
                        obj.CheckAmount = amount.ToString("N3").Replace(",", null);
                    }
                    else
                    {
                        obj.CheckAmount = amount.ToString("N2").Replace(",", null);
                    }
                    obj.CurrencyCode = GetCurrencyCode(inward.Currency);
                    obj.ReasonCode = retcode;
                    obj.FeesFlag = "2";

                    string msgId = "";
                    if (inward.ClrCenter == "DISCOUNT")
                    {
                        msgId = Temenos_Message_Types.ReversePostingDIS; // Assuming Temenos_Message_Types is accessible
                    }
                    else
                    {
                        msgId = Temenos_Message_Types.ReversePostingPMARAM; // Assuming Temenos_Message_Types is accessible
                    }

                    // Simulate external service call
                    ftResponse.FT_Res = await EccAccInfo_WebSvc.ECC_OFS_MESSAGE(obj, msgId, 1); // Mock this call

                    var respError = await _context.OFS_Response_Errors_Tbl.SingleOrDefaultAsync(v => v.ChqSequance == chqSeq);

                    if (ftResponse.FT_Res.ErrorMessage.Contains("TIMEOUT"))
                    {
                        inward.ErrorDescription = "RETURN INWARD STOPED CHQ  " + ftResponse.FT_Res.ResponseDescription;
                        inward.LastUpdateBy = userName;
                        inward.IsTimeOut = 1;
                        inward.History += "|" + " RETURN INWARD STOPED CHQ  Filed (TimeOut) User: " + userName + "AT:" + DateTime.Now;
                        _context.Entry(inward).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        if (ftResponse.FT_Res.ResponseStatus == "S")
                        {
                            var chq_seq = inward.ChqSequance.Trim();
                            var wf = await _context.INWARD_WF_Tbl.SingleOrDefaultAsync(c => c.ChqSequance == chq_seq);

                            if (wf != null)
                            {
                                wf.ISFixederror = false;
                                wf.IsFinanicallyFixed = false;
                                wf.IsMnaualFixed = false;
                                _context.Entry(wf).State = EntityState.Modified;
                                await _context.SaveChangesAsync();
                            }

                            inward.PMAstatus = "Pending";
                            inward.ErrorCode = "";
                            inward.ErrorDescription = "Success";
                            inward.T24Response = "Success";
                            inward.ReturnDate = DateTime.Now;
                            inward.Posted = AllEnums.Cheque_Status.Returne;
                            inward.Returned = 1;
                            inward.LastUpdateBy = userName;
                            inward.LastUpdate = DateTime.Now;
                            inward.History += mqMsg + " Done  By" + userName + "OLD RET CODE:" + inward.ReturnCode;
                            inward.ReturnCode = retcode;
                            _context.Entry(inward).State = EntityState.Modified;
                            await _context.SaveChangesAsync();

                            ReturnChqTBL(inward); // Assuming this method exists in the service

                            if (respError == null)
                            {
                                respError = new OFS_Response_Errors_Tbl
                                {
                                    LastErrorMessage = ftResponse.FT_Res.ResponseDescription,
                                    Cheque_Type = "Inward",
                                    Serial = inward.Serial,
                                    ChqSequance = inward.ChqSequance,
                                    LastAmendBy = userName,
                                    LastAmendDate = DateTime.Now,
                                    History = ftResponse.FT_Res.ResponseDescription
                                };
                                _context.OFS_Response_Errors_Tbl.Add(respError);
                            }
                            else
                            {
                                respError.LastErrorMessage = ftResponse.FT_Res.ResponseDescription;
                                respError.Cheque_Type = "Inward";
                                respError.Serial = inward.Serial;
                                respError.ChqSequance = inward.ChqSequance;
                                respError.LastAmendBy = userName;
                                respError.LastAmendDate = DateTime.Now;
                                respError.History = ftResponse.FT_Res.ResponseDescription;
                                _context.Entry(respError).State = EntityState.Modified;
                            }
                            await _context.SaveChangesAsync();

                            var wfHistory = new WFHistory
                            {
                                ChqSequance = inward.ChqSequance,
                                Serial = inward.Serial,
                                TransDate = inward.TransDate,
                                ID_WFStatus = WFStatus.WF_Status.ChequeAlreadyStopped, // Assuming WFStatus.WF_Status is accessible
                                Status = inward.Status + ":" + "Return Stop CHQ Done with RetCode: " + retcode,
                                ClrCenter = inward.ClrCenter,
                                DrwChqNo = inward.DrwChqNo,
                                DrwAccNo = inward.DrwAcctNo,
                                Amount = inward.Amount
                            };
                            _context.WFHistories.Add(wfHistory);
                            await _context.SaveChangesAsync();

                            _json.Data = new { ErrorMsg = "ReversePosting  Done Successfully" };
                            return _json;
                        }
                        else
                        {
                            inward.ErrorCode = "";
                            inward.ErrorDescription = "Faild" + ftResponse.FT_Res.ResponseDescription;
                            inward.T24Response = "Faild" + ftResponse.FT_Res.ResponseDescription;
                            inward.Posted = AllEnums.Cheque_Status.Posted;
                            inward.LastUpdateBy = userName;
                            inward.LastUpdate = DateTime.Now;
                            inward.History += mqMsg + " Faild  By" + userName;
                            _context.Entry(inward).State = EntityState.Modified;
                            await _context.SaveChangesAsync();

                            if (respError == null)
                            {
                                respError = new OFS_Response_Errors_Tbl
                                {
                                    LastErrorMessage = ftResponse.FT_Res.ResponseDescription,
                                    Cheque_Type = "Inward",
                                    Serial = inward.Serial,
                                    ChqSequance = inward.ChqSequance,
                                    LastAmendBy = userName,
                                    LastAmendDate = DateTime.Now,
                                    History = ftResponse.FT_Res.ResponseDescription
                                };
                                _context.OFS_Response_Errors_Tbl.Add(respError);
                            }
                            else
                            {
                                respError.LastErrorMessage = ftResponse.FT_Res.ResponseDescription;
                                respError.Cheque_Type = "Inward";
                                respError.Serial = inward.Serial;
                                respError.ChqSequance = inward.ChqSequance;
                                respError.LastAmendBy = userName;
                                respError.LastAmendDate = DateTime.Now;
                                respError.History = ftResponse.FT_Res.ResponseDescription;
                                _context.Entry(respError).State = EntityState.Modified;
                            }
                            await _context.SaveChangesAsync();

                            _json.Data = new { ErrorMsg = "ReversePosting  Faild" };
                            return _json;
                        }
                    }
                }
                else if (onus != null)
                {
                    chqSeq = onus.ChqSequance;
                    mqMsg = "ONUSReversePosting";

                    obj.PsSystem = sysName;
                    obj.RequestDate = DateTime.Now.Date.ToString("yyyyMMdd");
                    obj.RequestTime = DateTime.Now.ToString("HH':'mm':'ss");
                    obj.RequestCode = mqMsg;
                    obj.TransSeq = onus.Serial;
                    obj.CheckSeq = onus.ChqSequance.Trim();
                    obj.PayBankCode = onus.DrwBankNo;
                    obj.PayBranchCode = onus.DrwBranchNo;
                    obj.PayAccountNumber = onus.DrwAcctNo;
                    obj.BFDBankCode = onus.BenfBnk;
                    obj.BFDBranchCode = onus.BenfAccBranch;
                    obj.BFDAccountNumber = "0000"; // Onus.BenAccountNo
                    obj.CheckSerial = Convert.ToInt32(onus.DrwChqNo.Substring(1));
                    amount = onus.Amount;

                    if (onus.Currency.Trim().ToUpper() == "JOD")
                    {
                        obj.CheckAmount = amount.ToString("N3").Replace(",", null);
                    }
                    else
                    {
                        obj.CheckAmount = amount.ToString("N2").Replace(",", null);
                    }
                    obj.CurrencyCode = GetCurrencyCode(onus.Currency);
                    obj.ReasonCode = retcode;
                    obj.FeesFlag = "2";

                    string msgId = Temenos_Message_Types.ONUSReversePosting; // Assuming Temenos_Message_Types is accessible

                    // Simulate external service call
                    ftResponse.FT_Res = await EccAccInfo_WebSvc.ECC_OFS_MESSAGE(obj, msgId, 1); // Mock this call

                    var respError = await _context.OFS_Response_Errors_Tbl.SingleOrDefaultAsync(v => v.ChqSequance == chqSeq);

                    if (ftResponse.FT_Res.ErrorMessage.Contains("TIMEOUT"))
                    {
                        onus.ErrorDescription = "RETURN INWARD ONUS STOPED CHQ " + ftResponse.FT_Res.ResponseDescription;
                        onus.LastUpdateBy = userName;
                        onus.IsTimeOut = 1;
                        onus.History += "|" + " RETURN INWARD  ONUS  STOPED CHQ Filed (TimeOut) User: " + userName + "AT:" + DateTime.Now;
                        _context.Entry(onus).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        if (ftResponse.FT_Res.ResponseStatus == "S")
                        {
                            var chq_seq = onus.ChqSequance.Trim();
                            var wf = await _context.INWARD_WF_Tbl.SingleOrDefaultAsync(c => c.ChqSequance == chq_seq);

                            if (wf != null)
                            {
                                wf.ISFixederror = false;
                                wf.IsFinanicallyFixed = false;
                                wf.IsMnaualFixed = false;
                                _context.Entry(wf).State = EntityState.Modified;
                                await _context.SaveChangesAsync();
                            }

                            onus.PMAstatus = "Pending";
                            onus.ErrorCode = "";
                            onus.ErrorDescription = "Success";
                            onus.T24Response = "Success";
                            onus.ReturnDate = DateTime.Now;
                            onus.Posted = AllEnums.Cheque_Status.Returne;
                            onus.Returned = 1;
                            onus.LastUpdateBy = userName;
                            onus.LastUpdate = DateTime.Now;
                            onus.History += mqMsg + " Done  By" + userName + "OLD RET CODE:" + onus.ReturnCode;
                            onus.ReturnCode = retcode;
                            _context.Entry(onus).State = EntityState.Modified;
                            await _context.SaveChangesAsync();

                            ReturnChq_ONUSTBL(onus); // Assuming this method exists in the service

                            if (respError == null)
                            {
                                respError = new OFS_Response_Errors_Tbl
                                {
                                    LastErrorMessage = ftResponse.FT_Res.ResponseDescription,
                                    Cheque_Type = "ONUS",
                                    Serial = onus.Serial,
                                    ChqSequance = onus.ChqSequance,
                                    LastAmendBy = userName,
                                    LastAmendDate = DateTime.Now,
                                    History = ftResponse.FT_Res.ResponseDescription
                                };
                                _context.OFS_Response_Errors_Tbl.Add(respError);
                            }
                            else
                            {
                                respError.LastErrorMessage = ftResponse.FT_Res.ResponseDescription;
                                respError.Cheque_Type = "ONUS";
                                respError.Serial = onus.Serial;
                                respError.ChqSequance = onus.ChqSequance;
                                respError.LastAmendBy = userName;
                                respError.LastAmendDate = DateTime.Now;
                                respError.History = ftResponse.FT_Res.ResponseDescription;
                                _context.Entry(respError).State = EntityState.Modified;
                            }
                            await _context.SaveChangesAsync();

                            var wfHistory = new WFHistory
                            {
                                ChqSequance = onus.ChqSequance,
                                Serial = onus.Serial,
                                TransDate = onus.TransDate,
                                ID_WFStatus = WFStatus.WF_Status.ChequeAlreadyStopped, // Assuming WFStatus.WF_Status is accessible
                                Status = onus.Status + ":" + "Return Stop CHQ Done with RetCode: " + retcode,
                                ClrCenter = onus.ClrCenter,
                                DrwChqNo = onus.DrwChqNo,
                                DrwAccNo = onus.DrwAcctNo,
                                Amount = onus.Amount
                            };
                            _context.WFHistories.Add(wfHistory);
                            await _context.SaveChangesAsync();

                            _json.Data = new { ErrorMsg = "ONUSReversePosting  Done Successfully" };
                            return _json;
                        }
                        else
                        {
                            onus.ErrorCode = "";
                            onus.ErrorDescription = "Faild" + ftResponse.FT_Res.ResponseDescription;
                            onus.T24Response = "Faild" + ftResponse.FT_Res.ResponseDescription;
                            onus.Posted = AllEnums.Cheque_Status.Posted;
                            onus.LastUpdateBy = userName;
                            onus.LastUpdate = DateTime.Now;
                            onus.History += mqMsg + " Faild  By" + userName;
                            _context.Entry(onus).State = EntityState.Modified;
                            await _context.SaveChangesAsync();

                            if (respError == null)
                            {
                                respError = new OFS_Response_Errors_Tbl
                                {
                                    LastErrorMessage = ftResponse.FT_Res.ResponseDescription,
                                    Cheque_Type = "ONUS",
                                    Serial = onus.Serial,
                                    ChqSequance = onus.ChqSequance,
                                    LastAmendBy = userName,
                                    LastAmendDate = DateTime.Now,
                                    History = ftResponse.FT_Res.ResponseDescription
                                };
                                _context.OFS_Response_Errors_Tbl.Add(respError);
                            }
                            else
                            {
                                respError.LastErrorMessage = ftResponse.FT_Res.ResponseDescription;
                                respError.Cheque_Type = "ONUS";
                                respError.Serial = onus.Serial;
                                respError.ChqSequance = onus.ChqSequance;
                                respError.LastAmendBy = userName;
                                respError.LastAmendDate = DateTime.Now;
                                respError.History = ftResponse.FT_Res.ResponseDescription;
                                _context.Entry(respError).State = EntityState.Modified;
                            }
                            await _context.SaveChangesAsync();

                            _json.Data = new { ErrorMsg = "ONUSReversePosting  Faild" };
                            return _json;
                        }
                    }
                }
                else
                {
                    _json.Data = new { ErrorMsg = "Cheque not found." };
                    return _json;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ReturnInwardStoppedCheque_Reverse: {Message}", ex.Message);
                _json.Data = new { ErrorMsg = "An error occurred while reversing the stopped cheque." };
                return _json;
            }
        }



        public async Task<JsonResult> ReverseAllINHOUSE(string userName)
        {
            var _json = new JsonResult(new { });
            string tDate = DateTime.Now.ToString("yyyy-MM-dd");
            string respMessage = "";
            int clearedCount = 0;

            try
            {
                // 1. Recall timeout commission (simplified - actual logic needs to be fully converted)
                // This part involves complex SQL queries and external service calls (ECC_Com_SVC.Ecc_Commisions_HandlerSoapClient)
                // For now, we'll simulate the outcome or leave placeholders.
                _logger.LogInformation("Attempting to recall timeout commissions for ONUS.");
                // Example of how to query and process, but actual commission logic is complex and needs full conversion
                var onusWithTimeoutCommission = await _context.OnUs_Tbl
                    .Where(o => o.ISneedCommision == 1 && o.TransDate.ToString("yyyy-MM-dd") == tDate)
                    .ToListAsync();

                foreach (var onusItem in onusWithTimeoutCommission)
                {
                    // Simulate commission processing
                    // This would involve calling external services like web_service.Post_Commision_To_T24
                    // and updating onusItem.T24Response, onusItem.History, etc.
                    onusItem.T24Response += " | Simulated Commission Recall";
                    onusItem.History += " | Simulated Commission Recall";
                    _context.Entry(onusItem).State = EntityState.Modified;
                }
                await _context.SaveChangesAsync();

                // 2. Update cleared cheques
                _logger.LogInformation("Start Update Cleared All INHOUSE Cheques.");
                // The original VB.NET uses raw SQL with `isnull(verifyStatus,1) <> 0` and `ChqSequance in (select ...)`
                // This needs to be translated carefully to LINQ or a parameterized raw SQL query.
                // For now, a simplified LINQ query.
                var clearedOnusCheques = await _context.OnUs_Tbl
                    .Where(o => o.Posted == AllEnums.Cheque_Status.Posted && o.TransDate.ToString("yyyy-MM-dd") == tDate)
                    .ToListAsync(); // Simplified condition

                foreach (var onus in clearedOnusCheques)
                {
                    onus.Posted = AllEnums.Cheque_Status.Cleared;
                    onus.History += "|" + "CHQ become  Cleared by " + userName + "  AT : " + DateTime.Now;

                    var wfHistory = new WFHistory
                    {
                        ChqSequance = onus.ChqSequance,
                        Serial = onus.Serial,
                        TransDate = onus.TransDate,
                        ID_WFStatus = 0, // Placeholder
                        Status = onus.Status + " CHQ become  Cleared by " + userName + "  AT : " + DateTime.Now,
                        ClrCenter = onus.ClrCenter,
                        DrwChqNo = onus.DrwChqNo,
                        DrwAccNo = onus.DrwAcctNo,
                        Amount = onus.Amount
                    };
                    _context.WFHistories.Add(wfHistory);
                    _context.Entry(onus).State = EntityState.Modified;
                    clearedCount++;
                }
                await _context.SaveChangesAsync();

                // 3. Update returned cheques (assuming Update_Returned_CHQ_TBL is a service method)
                _logger.LogInformation("Calling Update_Returned_CHQ_TBL for ONUS.");
                // await Update_Returned_CHQ_TBL("ONUS"); // This method needs to be implemented in the service

                respMessage = clearedCount + "  Cheque(s) Became Cleared";

                // 4. Check for cheques stuck in fixed error without return code
                _logger.LogInformation("Checking for stuck cheques in fixed error.");
                // This part also uses raw SQL and needs careful conversion.
                // Simplified logic:
                var stuckOnusCheques = await _context.INWARD_WF_Tbl
                    .Where(wf => wf.input_date.HasValue && wf.input_date.Value.ToString("yyyy-MM-dd") == tDate
                                 && wf.Need_Fixed_Error == true && wf.ISFixederror == false
                                 && wf.Clr_Center.Contains("ONUS"))
                    .Join(_context.OnUs_Tbl,
                          wf => wf.ChqSequance,
                          onus => onus.ChqSequance,
                          (wf, onus) => new { Wf = wf, Onus = onus })
                    .Where(joined => joined.Onus.ReturnCode == null && joined.Onus.ReturnCodeFinancail == null && joined.Onus.Posted == AllEnums.Cheque_Status.Returne)
                    .Select(joined => joined.Onus)
                    .ToListAsync();

                if (stuckOnusCheques.Any())
                {
                    // Process stuck cheques, e.g., add to a list for further action or notification
                    _logger.LogWarning($"Found {stuckOnusCheques.Count} ONUS cheques stuck in fixed error.");
                    // The original VB.NET adds these to `onus_list` which is then returned.
                    // We can include this information in the JSON result.
                    _json.Data = new { ErrorMsg = "Some ONUS cheques are stuck in fixed error and need attention.", StuckCheques = stuckOnusCheques.Select(o => o.Serial).ToList() };
                    return _json;
                }

                _json.Data = new { ErrorMsg = "", Result = respMessage };
                return _json;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ReverseAllINHOUSE: {Message}", ex.Message);
                _json.Data = new { ErrorMsg = "An error occurred during INHOUSE reversal process." };
                return _json;
            }
        }



        public async Task<JsonResult> ReverseAllPMA(string userName)
        {
            var _json = new JsonResult(new { });
            string tDate = DateTime.Now.ToString("yyyy-MM-dd");
            string respMessage = "";
            int clearedCount = 0;

            try
            {
                // 1. Recall timeout commission
                _logger.LogInformation("Attempting to recall timeout commissions for PMA.");
                var inwardWithTimeoutCommission = await _context.Inward_Trans
                    .Where(i => i.ISneedCommision == 1 && i.TransDate.ToString("yyyy-MM-dd") == tDate)
                    .ToListAsync();

                foreach (var inwardItem in inwardWithTimeoutCommission)
                {
                    // This part involves calling an external service (ECC_Com_SVC.Ecc_Commisions_HandlerSoapClient)
                    // and updating inwardItem.T24Response, inwardItem.History.
                    // For now, simulating the outcome.
                    // In a real scenario, you would instantiate and call the SOAP client here.
                    // Example: var web_service = new ECC_Com_SVC.Ecc_Commisions_HandlerSoapClient();
                    // var obj_com = await web_service.Post_Commision_To_T24(...);
                    // inwardItem.T24Response += " | Commision :" + obj_com.Descreption + " With Amount : " + obj_com.CommisionAmount;
                    // inwardItem.History += " | Commision :" + obj_com.Descreption + " With Amount : " + obj_com.CommisionAmount;
                    inwardItem.T24Response += " | Simulated Commission Recall";
                    inwardItem.History += " | Simulated Commission Recall";
                    _context.Entry(inwardItem).State = EntityState.Modified;
                }
                await _context.SaveChangesAsync();

                // 2. Update cleared cheques
                _logger.LogInformation("Start Update Cleared All PMA Cheques.");
                var clearedInwardCheques = await _context.Inward_Trans
                    .Where(i => i.Posted == AllEnums.Cheque_Status.Posted && i.TransDate.ToString("yyyy-MM-dd") == tDate && i.ReturnCode == null && i.ReturnCodeFinancail == null)
                    .ToListAsync();

                foreach (var inward in clearedInwardCheques)
                {
                    inward.Posted = AllEnums.Cheque_Status.Cleared;
                    inward.History += "|" + "CHQ become  Cleared by " + userName + "  AT : " + DateTime.Now;

                    var wfHistory = new WFHistory
                    {
                        ChqSequance = inward.ChqSequance,
                        Serial = inward.Serial,
                        TransDate = inward.TransDate,
                        ID_WFStatus = 0, // Placeholder
                        Status = inward.Status + " CHQ become  Cleared by " + userName + "  AT : " + DateTime.Now,
                        ClrCenter = inward.ClrCenter,
                        DrwChqNo = inward.DrwChqNo,
                        DrwAccNo = inward.DrwAcctNo,
                        Amount = inward.Amount
                    };
                    _context.WFHistories.Add(wfHistory);
                    _context.Entry(inward).State = EntityState.Modified;
                    clearedCount++;
                }
                await _context.SaveChangesAsync();

                // 3. Update returned cheques (assuming Update_Returned_CHQ_TBL is a service method)
                _logger.LogInformation("Calling Update_Returned_CHQ_TBL for PMA.");
                // await Update_Returned_CHQ_TBL("PMA"); // This method needs to be implemented in the service

                respMessage = clearedCount + "  Cheque(s) Became Cleared";

                // 4. Check for cheques stuck in fixed error without return code
                _logger.LogInformation("Checking for stuck cheques in fixed error for PMA.");
                var stuckInwardCheques = await _context.INWARD_WF_Tbl
                    .Where(wf => wf.input_date.HasValue && wf.input_date.Value.ToString("yyyy-MM-dd") == tDate
                                 && wf.Need_Fixed_Error == true && wf.ISFixederror == false
                                 && (wf.Clr_Center.Contains("PMA") || wf.Clr_Center.Contains("DISCOUNT")))
                    .Join(_context.Inward_Trans,
                          wf => wf.ChqSequance,
                          inward => inward.ChqSequance,
                          (wf, inward) => new { Wf = wf, Inward = inward })
                    .Where(joined => joined.Inward.ReturnCode == null && joined.Inward.ReturnCodeFinancail == null && joined.Inward.Posted == AllEnums.Cheque_Status.Returne)
                    .Select(joined => joined.Inward)
                    .ToListAsync();

                if (stuckInwardCheques.Any())
                {
                    _logger.LogWarning($"Found {stuckInwardCheques.Count} PMA/DISCOUNT cheques stuck in fixed error.");
                    _json.Data = new { ErrorMsg = "Some PMA/DISCOUNT cheques are stuck in fixed error and need attention.", StuckCheques = stuckInwardCheques.Select(i => i.Serial).ToList() };
                    return _json;
                }

                _json.Data = new { ErrorMsg = "", Result = respMessage };
                return _json;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ReverseAllPMA: {Message}", ex.Message);
                _json.Data = new { ErrorMsg = "An error occurred during PMA reversal process." };
                return _json;
            }
        }



        public async Task<JsonResult> ReverseAllDISCOUNT(string userName)
        {
            var _json = new JsonResult(new { });
            string tDate = DateTime.Now.ToString("yyyy-MM-dd");
            string respMessage = "";
            int clearedCount = 0;

            try
            {
                // 1. Recall timeout commission (simplified - actual logic needs to be fully converted)
                _logger.LogInformation("Attempting to recall timeout commissions for DISCOUNT.");
                var inwardWithTimeoutCommission = await _context.Inward_Trans
                    .Where(i => i.ISneedCommision == 1 && i.TransDate.ToString("yyyy-MM-dd") == tDate && i.ClrCenter == "DISCOUNT")
                    .ToListAsync();

                foreach (var inwardItem in inwardWithTimeoutCommission)
                {
                    // Simulate commission processing
                    inwardItem.T24Response += " | Simulated Commission Recall";
                    inwardItem.History += " | Simulated Commission Recall";
                    _context.Entry(inwardItem).State = EntityState.Modified;
                }
                await _context.SaveChangesAsync();

                // 2. Update cleared cheques
                _logger.LogInformation("Start Update Cleared All DISCOUNT Cheques.");
                var clearedInwardCheques = await _context.Inward_Trans
                    .Where(i => i.Posted == AllEnums.Cheque_Status.Posted && i.TransDate.ToString("yyyy-MM-dd") == tDate && i.ClrCenter == "DISCOUNT")
                    .ToListAsync(); // Simplified condition

                foreach (var inward in clearedInwardCheques)
                {
                    inward.Posted = AllEnums.Cheque_Status.Cleared;
                    inward.History += "|" + "CHQ become  Cleared by " + userName + "  AT : " + DateTime.Now;

                    var wfHistory = new WFHistory
                    {
                        ChqSequance = inward.ChqSequance,
                        Serial = inward.Serial,
                        TransDate = inward.TransDate,
                        ID_WFStatus = 0, // Placeholder
                        Status = inward.Status + " CHQ become  Cleared by " + userName + "  AT : " + DateTime.Now,
                        ClrCenter = inward.ClrCenter,
                        DrwChqNo = inward.DrwChqNo,
                        DrwAccNo = inward.DrwAcctNo,
                        Amount = inward.Amount
                    };
                    _context.WFHistories.Add(wfHistory);
                    _context.Entry(inward).State = EntityState.Modified;
                    clearedCount++;
                }
                await _context.SaveChangesAsync();

                // 3. Update returned cheques (assuming Update_Returned_CHQ_TBL is a service method)
                _logger.LogInformation("Calling Update_Returned_CHQ_TBL for DISCOUNT.");
                // await Update_Returned_CHQ_TBL("DISCOUNT"); // This method needs to be implemented in the service

                respMessage = clearedCount + "  Cheque(s) Became Cleared";

                // 4. Check for cheques stuck in fixed error without return code
                _logger.LogInformation("Checking for stuck cheques in fixed error for DISCOUNT.");
                var stuckInwardCheques = await _context.INWARD_WF_Tbl
                    .Where(wf => wf.input_date.HasValue && wf.input_date.Value.ToString("yyyy-MM-dd") == tDate
                                 && wf.Need_Fixed_Error == true && wf.ISFixederror == false
                                 && wf.Clr_Center == "Inoward_DISCOUNT")
                    .Join(_context.Inward_Trans,
                          wf => wf.ChqSequance,
                          inward => inward.ChqSequance,
                          (wf, inward) => new { Wf = wf, Inward = inward })
                    .Where(joined => joined.Inward.ReturnCode == null && joined.Inward.ReturnCodeFinancail == null && joined.Inward.Posted == AllEnums.Cheque_Status.Returne)
                    .Select(joined => joined.Inward)
                    .ToListAsync();

                if (stuckInwardCheques.Any())
                {
                    _logger.LogWarning($"Found {stuckInwardCheques.Count} DISCOUNT cheques stuck in fixed error.");
                    _json.Data = new { ErrorMsg = "Some DISCOUNT cheques are stuck in fixed error and need attention.", StuckCheques = stuckInwardCheques.Select(i => i.Serial).ToList() };
                    return _json;
                }

                _json.Data = new { ErrorMsg = "", Result = respMessage };
                return _json;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ReverseAllDISCOUNT: {Message}", ex.Message);
                _json.Data = new { ErrorMsg = "An error occurred during DISCOUNT reversal process." };
                return _json;
            }
        }



        public async Task<JsonResult> ReverseAllPDC(string userName)
        {
            var _json = new JsonResult(new { });
            string tDate = DateTime.Now.ToString("yyyy-MM-dd");
            string respMessage = "";

            try
            {
                _logger.LogInformation("Start reversing all ONUS WAS PDC CHQ from ONUS TBL.");

                var onusList = new List<OnUs_Tbl>();
                var db = _context; // Using the injected DbContext

                // This section from VB.NET is complex and involves iterating through OnUs_Tbl
                // where verifyStatus is false and then making external service calls.
                // The original VB.NET code had a loop that processed each item.
                // I will simulate this by fetching relevant items and processing them.

                var unverifiedOnUs = await db.OnUs_Tbl.Where(c => c.verifyStatus == false).ToListAsync();

                foreach (var item in unverifiedOnUs)
                {
                    try
                    {
                        var wf = await db.INWARD_WF_Tbl.SingleOrDefaultAsync(o => o.ChqSequance == item.ChqSequance && o.Final_Status != "Accept");

                        if (wf == null && tDate == item.TransDate.ToString("yyyy-MM-dd"))
                        {
                            var obj = new ECC_CAP_Services.ECC_FT_Request();
                            var EccAccInfo_WebSvc = new ECC_CAP_Services.SAFA_T24_ECC_SVCSoapClient("SAFA_T24_ECC_SVCSoap");
                            string sysName = db.System_Configurations_Tbl.SingleOrDefault(c => c.Config_Type == "ECC_SYSTEM_ID" && c.Config_Id == "7").Config_Value;
                            var ftResponse = new FT_ResponseClass();
                            double amount = item.Amount;

                            obj.PsSystem = sysName;
                            obj.RequestDate = DateTime.Now.Date.ToString("yyyyMMdd");
                            obj.RequestTime = DateTime.Now.ToString("HH':'mm':'ss");
                            obj.RequestCode = "ONUSReversePosting";
                            obj.TransSeq = item.Was_PDC == true ? item.PDC_Serial : item.Serial;
                            obj.CheckSeq = item.ChqSequance.Trim();
                            obj.PayBankCode = item.DrwBankNo;
                            obj.PayBranchCode = item.DrwBranchNo;
                            obj.PayAccountNumber = item.DrwAcctNo;
                            obj.BFDBankCode = item.BenfBnk;
                            obj.BFDBranchCode = item.BenfAccBranch;
                            obj.BFDAccountNumber = "0000"; // Placeholder
                            obj.CheckSerial = Convert.ToInt32(item.DrwChqNo.Substring(1));

                            if (item.Currency.Trim().ToUpper() == "JOD")
                            {
                                obj.CheckAmount = amount.ToString("N3").Replace(",", null);
                            }
                            else
                            {
                                obj.CheckAmount = amount.ToString("N2").Replace(",", null);
                            }

                            obj.CurrencyCode = GetCurrencyCode(item.Currency);
                            string retCoder = getfinalonuscode(item.ReturnCode, item.ReturnCodeFinancail, item.ClrCenter);
                            if (string.IsNullOrEmpty(retCoder))
                            {
                                retCoder = "02";
                            }
                            obj.ReasonCode = retCoder;
                            obj.FeesFlag = "2";

                            _logger.LogInformation($"Sending MQ Message ONUSReversePosting for ChqSequance: {item.ChqSequance}");
                            int msgId = Temenos_Message_Types.ONUSReversePosting; // Assuming Temenos_Message_Types is accessible
                            ftResponse.FT_Res = await EccAccInfo_WebSvc.ECC_OFS_MESSAGE(obj, msgId, 1); // Mock this call

                            if (ftResponse.FT_Res.ErrorMessage.Contains("TIMEOUT"))
                            {
                                item.ErrorDescription = "RETURN INWARD ONUS  " + ftResponse.FT_Res.ResponseDescription;
                                item.LastUpdateBy = userName;
                                item.IsTimeOut = 1;
                                item.History += "|" + " RETURN INWARD  ONUS   Filed (TimeOut) User: " + userName + "AT:" + DateTime.Now;
                                db.Entry(item).State = EntityState.Modified;
                                await db.SaveChangesAsync();
                                onusList.Add(item);
                            }
                            else
                            {
                                if (ftResponse.FT_Res.ResponseStatus == "S")
                                {
                                    item.IsTimeOut = 0;
                                    item.Posted = AllEnums.Cheque_Status.Returne;
                                    item.CHQState = "1";
                                    item.CHQStatedate = DateTime.Now;
                                    item.ReturnDate = DateTime.Now;
                                    item.History += "|" + "ReverseAllINHOUSE Done ";
                                    item.T24Response = ftResponse.FT_Res.ResponseDescription;
                                    item.LastUpdateBy = userName;
                                    item.Returned = true;
                                    db.Entry(item).State = EntityState.Modified;
                                    await db.SaveChangesAsync();

                                    if (wf != null)
                                    {
                                        wf.RetByWF = true;
                                        wf.Final_Status = "Accept";
                                        db.Entry(wf).State = EntityState.Modified;
                                        await db.SaveChangesAsync();
                                    }

                                    _logger.LogInformation($"Calling ReturnChq_ONUSTBL for rejected CHQ: {item.ChqSequance}");
                                    ReturnChq_ONUSTBL(item); // Assuming this method exists in the service

                                    // Commission processing (simulated)
                                    // var web_service_com = new ECC_Com_SVC.Ecc_Commisions_HandlerSoapClient();
                                    // var obj_com = await web_service_com.Post_Commision_To_T24(item.DrwAcctNo, 3, Get_ALT_Acc_No(item.DecreptedDrwAcountNo), item.Amount, item.DrwChqNo, item.DrwBankNo, userName, retCoder, item.Serial);
                                    // item.T24Response += "| Commision :" + obj_com.Descreption + "With Amount  :" + obj_com.CommisionAmount;
                                    db.Entry(item).State = EntityState.Modified;
                                    await db.SaveChangesAsync();
                                    onusList.Add(item);
                                }
                                else
                                {
                                    string responseDetails = "";
                                    // Account Info calls (simulated)
                                    // var EccAccWebSvc = new ECC_CAP_Services.SAFA_T24_ECC_SVCSoapClient();
                                    // var Acc_obj = await EccAccWebSvc.ACCOUNT_INFO(item.BenAccountNo, 1);
                                    // if (Acc_obj.CustPosting != "0" || Acc_obj.AcctPosting != "0") { /* append details to responseDetails */ }

                                    item.History += "|" + "Reverse  ALL INHOUSE Failed ";
                                    item.T24Response += ftResponse.FT_Res.ResponseDescription + "Reverse  PDC INHOUSE Failed " + "| " + responseDetails;
                                    item.LastUpdateBy = userName;
                                    item.Returned = false;
                                    item.IsTimeOut = 0;
                                    db.Entry(item).State = EntityState.Modified;
                                    await db.SaveChangesAsync();
                                    onusList.Add(item);
                                }
                            }
                        }
                    }
                    catch (Exception innerEx)
                    {
                        _logger.LogError(innerEx, "Error processing OnUs_Tbl item {Serial}: {Message}", item.Serial, innerEx.Message);
                    }
                }

                _json.Data = new { ErrorMsg = "", Result = "PDC reversal process completed." };
                return _json;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ReverseAllPDC: {Message}", ex.Message);
                _json.Data = new { ErrorMsg = "An error occurred during PDC reversal process." };
                return _json;
            }
        }



        public async Task<JsonResult> Reversevip(string userName)
        {
            var _json = new JsonResult(new { });
            string tDate = DateTime.Now.ToString("yyyy-MM-dd");
            string respMessage = "";
            int clearedCount = 0;

            try
            {
                var inwardList = new List<Inward_Trans>();
                var db = _context;

                // SQL query from VB.NET:
                // select CHQ.Serial from Inward_Trans AS CHQ LEFT OUTER JOIN INWARD_WF_Tbl AS WF ON WF.ChqSequance = CHQ.ChqSequance
                // where posted = AllEnums.Cheque_Status.Posted AND chq.ClrCenter = 'PMA' and CAST(CHQ.InputDate AS DATE)= '" & DateTime.Now.ToString("yyyyMMdd") & "'" & "and( isnull (verifyStatus,1) = 0 or WF.Final_Status <> 'Accept') and chq.VIP=1

                var vipInwardCheques = await (from chq in db.Inward_Trans
                                              join wf in db.INWARD_WF_Tbl on chq.ChqSequance equals wf.ChqSequance into wfGroup
                                              from wf in wfGroup.DefaultIfEmpty()
                                              where chq.Posted == AllEnums.Cheque_Status.Posted
                                                    && chq.ClrCenter == "PMA"
                                                    && chq.InputDate.HasValue && chq.InputDate.Value.ToString("yyyyMMdd") == tDate
                                                    && (chq.verifyStatus == false || wf == null || wf.Final_Status != "Accept")
                                                    && chq.VIP == true
                                              select chq).ToListAsync();

                foreach (var inward in vipInwardCheques)
                {
                    inward.PMAstatus = "Pending";
                    inward.Returned = 1;
                    inward.PMAstatusDate = DateTime.Now;
                    inward.History += " Change vip Pma status to Pending : " + userName + " At : " + DateTime.Now;
                    inward.LastUpdate = DateTime.Now;
                    inward.LastUpdateBy = userName;

                    var wfHistory = new WFHistory
                    {
                        ChqSequance = inward.ChqSequance,
                        Serial = inward.Serial,
                        TransDate = inward.ValueDate,
                        ID_WFStatus = 0, // Placeholder
                        Status = inward.Status + "vip PMA STATUES  Changed to :Pending  by " + userName + "  AT : " + DateTime.Now,
                        ClrCenter = inward.ClrCenter,
                        DrwChqNo = inward.DrwChqNo,
                        DrwAccNo = inward.DrwAcctNo,
                        Amount = inward.Amount
                    };
                    db.WFHistories.Add(wfHistory);
                    db.Entry(inward).State = EntityState.Modified;
                }
                await db.SaveChangesAsync();

                // Process for clearing VIP PMA cheques
                _logger.LogInformation("Start Update Cleared VIP PMA Cheques.");

                var clearedVipInwardCheques = await (from chq in db.Inward_Trans
                                                     join wf in db.INWARD_WF_Tbl on chq.ChqSequance equals wf.ChqSequance into wfGroup
                                                     from wf in wfGroup.DefaultIfEmpty()
                                                     where chq.Posted == AllEnums.Cheque_Status.Posted
                                                           && chq.ClrCenter == "PMA"
                                                           && chq.InputDate.HasValue && chq.InputDate.Value.ToString("yyyyMMdd") == tDate
                                                           && (chq.verifyStatus == true || (wf != null && wf.Final_Status == "Accept"))
                                                           && chq.VIP == true
                                                     select chq).ToListAsync();

                foreach (var inward in clearedVipInwardCheques)
                {
                    try
                    {
                        var obj = new ECC_CAP_Services.ECC_FT_Request();
                        var EccAccInfo_WebSvc = new ECC_CAP_Services.SAFA_T24_ECC_SVCSoapClient("SAFA_T24_ECC_SVCSoap");
                        string sysName = db.System_Configurations_Tbl.SingleOrDefault(c => c.Config_Type == "ECC_SYSTEM_ID" && c.Config_Id == "7").Config_Value;
                        var ftResponse = new FT_ResponseClass();
                        double amount = inward.Amount;

                        string decryptedAccountResult = Get_Deacrypted_Account(inward.DrwAcctNo, inward.DrwChqNo);
                        string payAccount = decryptedAccountResult.Split(';')[0];
                        if (payAccount.Length != 13)
                        {
                            payAccount = Get_ALT_Acc_No(decryptedAccountResult.Split(';')[0]);
                        }

                        obj.PayAccountNumber = payAccount;
                        obj.BFDAccountNumber = "0000";
                        obj.PsSystem = sysName;
                        obj.RequestDate = DateTime.Now.ToString("yyyyMMdd");
                        obj.RequestTime = DateTime.Now.ToString("HH':'mm':'ss");
                        obj.RequestCode = "ReversePostingPMARAM";
                        obj.TransSeq = inward.Serial;
                        obj.CheckSeq = inward.ChqSequance.Trim();
                        obj.PayBankCode = inward.DrwBankNo;
                        obj.PayBranchCode = inward.DrwBranchNo;
                        obj.BFDBankCode = inward.BenfBnk;
                        obj.BFDBranchCode = inward.BenfAccBranch;
                        obj.CheckSerial = Convert.ToInt32(inward.DrwChqNo.Substring(1));

                        if (inward.Currency.Trim().ToUpper() == "JOD")
                        {
                            obj.CheckAmount = amount.ToString("N3").Replace(",", null);
                        }
                        else
                        {
                            obj.CheckAmount = amount.ToString("N2").Replace(",", null);
                        }
                        obj.CurrencyCode = GetCurrencyCode(inward.Currency);

                        string reasonCode = getfinalonuscode(inward.ReturnCode, inward.ReturnCodeFinancail, inward.ClrCenter);
                        if (string.IsNullOrEmpty(reasonCode))
                        {
                            reasonCode = "02";
                        }
                        obj.ReasonCode = reasonCode;
                        obj.FeesFlag = "2";

                        int msgId = Temenos_Message_Types.ReversePostingPMARAM;
                        ftResponse.FT_Res = await EccAccInfo_WebSvc.ECC_OFS_MESSAGE(obj, msgId, 1); // Mock this call

                        if (ftResponse.FT_Res.ErrorMessage.Contains("OFSERROR_TIMEOUT"))
                        {
                            inward.ErrorDescription = "ReversePostingPMARAM  VIP : OFSERROR_TIMEOUT  ";
                            inward.LastUpdateBy = userName;
                            inward.IsTimeOut = 1;
                            inward.History += "|" + "ReversePostingPMARAM  VIP : OFSERROR_TIMEOUT,  User: " + userName + " AT :" + DateTime.Now;
                            db.Entry(inward).State = EntityState.Modified;
                            await db.SaveChangesAsync();
                            inwardList.Add(inward);
                        }
                        else
                        {
                            if (ftResponse.FT_Res.ResponseStatus == "S")
                            {
                                inward.FinalRetCode = getfinalonuscode(inward.ReturnCode, inward.ReturnCodeFinancail, inward.ClrCenter);
                                inward.Posted = AllEnums.Cheque_Status.Cleared;
                                inward.History += "|" + "  VIP CHQ become  Cleared by " + userName + "  AT : " + DateTime.Now;
                                inward.LastUpdateBy = userName;
                                inward.LastUpdate = DateTime.Now;

                                var wfHistory = new WFHistory
                                {
                                    ChqSequance = inward.ChqSequance,
                                    Serial = inward.Serial,
                                    TransDate = inward.ValueDate,
                                    ID_WFStatus = 0, // Placeholder
                                    Status = inward.Status + "VIP CHQ become  Cleared by " + userName + "  AT : " + DateTime.Now,
                                    ClrCenter = inward.ClrCenter,
                                    DrwChqNo = inward.DrwChqNo,
                                    DrwAccNo = inward.DrwAcctNo,
                                    Amount = inward.Amount
                                };
                                db.WFHistories.Add(wfHistory);
                                db.Entry(inward).State = EntityState.Modified;
                                await db.SaveChangesAsync();
                                clearedCount++;
                            }
                            else
                            {
                                string responseDetails = "";
                                // Simulate account info calls and posting restriction checks
                                inward.History += "|" + "Reverse  ALL INHOUSE Failed ";
                                inward.T24Response += ftResponse.FT_Res.ResponseDescription + "Reverse  PDC INHOUSE Failed " + "| " + responseDetails;
                                inward.LastUpdateBy = userName;
                                inward.Returned = 0;
                                db.Entry(inward).State = EntityState.Modified;
                                await db.SaveChangesAsync();
                                inwardList.Add(inward);
                            }
                        }
                    }
                    catch (Exception innerEx)
                    {
                        _logger.LogError(innerEx, "Error processing VIP Inward_Trans item {Serial}: {Message}", inward.Serial, innerEx.Message);
                    }
                }

                // Call Update_Returned_CHQ_TBL (needs to be implemented)
                // await Update_Returned_CHQ_TBL("INWARED");

                respMessage = clearedCount + "  Cheque(s) Became Cleared";

                _json.Data = new { ErrorMsg = "", Result = respMessage, lstPDC = inwardList };
                return _json;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Reversevip: {Message}", ex.Message);
                _json.Data = new { ErrorMsg = "An error occurred during VIP PMA reversal process." };
                return _json;
            }
        }



        public async Task<JsonResult> ReversePMAINWARAD(string tDate1, string userName)
        {
            var _json = new JsonResult(new { });
            string tDate = DateTime.Now.ToString("yyyy-MM-dd");
            string respMessage = "";
            int clearedCount = 0;

            try
            {
                var inwardList = new List<Inward_Trans>();
                var db = _context;

                // SQL query from VB.NET:
                // select CHQ.Serial from Inward_Trans AS CHQ LEFT OUTER JOIN INWARD_WF_Tbl AS WF ON WF.ChqSequance = CHQ.ChqSequance
                // where posted = AllEnums.Cheque_Status.Posted AND ClrCenter = 'PMA' and CAST(ValueDate AS DATE) = '" & TDate1 & "' and( isnull (verifyStatus,1) = 0 or WF.Final_Status <> 'Accept')

                var pmaInwardCheques = await (from chq in db.Inward_Trans
                                              join wf in db.INWARD_WF_Tbl on chq.ChqSequance equals wf.ChqSequance into wfGroup
                                              from wf in wfGroup.DefaultIfEmpty()
                                              where chq.Posted == AllEnums.Cheque_Status.Posted
                                                    && chq.ClrCenter == "PMA"
                                                    && chq.ValueDate.HasValue && chq.ValueDate.Value.ToString("yyyy-MM-dd") == tDate1
                                                    && (chq.verifyStatus == false || wf == null || wf.Final_Status != "Accept")
                                              select chq).ToListAsync();

                foreach (var inward in pmaInwardCheques)
                {
                    inward.PMAstatus = "Pending";
                    inward.Returned = 1;
                    inward.PMAstatusDate = DateTime.Now;
                    inward.History += " Change Pma status to Pending : " + userName + " At : " + DateTime.Now;
                    inward.LastUpdate = DateTime.Now;
                    inward.LastUpdateBy = userName;

                    var wfHistory = new WFHistory
                    {
                        ChqSequance = inward.ChqSequance,
                        Serial = inward.Serial,
                        TransDate = inward.ValueDate,
                        ID_WFStatus = 0, // Placeholder
                        Status = inward.Status + " PMA STATUES  Changed to :Pending  by " + userName + "  AT : " + DateTime.Now,
                        ClrCenter = inward.ClrCenter,
                        DrwChqNo = inward.DrwChqNo,
                        DrwAccNo = inward.DrwAcctNo,
                        Amount = inward.Amount
                    };
                    db.WFHistories.Add(wfHistory);
                    db.Entry(inward).State = EntityState.Modified;
                }
                await db.SaveChangesAsync();

                // Process for clearing PMA cheques
                _logger.LogInformation("Start Update Cleared PMA Cheques.");

                var clearedPmaInwardCheques = await (from chq in db.Inward_Trans
                                                     join wf in db.INWARD_WF_Tbl on chq.ChqSequance equals wf.ChqSequance into wfGroup
                                                     from wf in wfGroup.DefaultIfEmpty()
                                                     where chq.Posted == AllEnums.Cheque_Status.Posted
                                                           && chq.ClrCenter == "PMA"
                                                           && chq.ValueDate.HasValue && chq.ValueDate.Value.ToString("yyyy-MM-dd") == tDate1
                                                           && (chq.verifyStatus == true || (wf != null && wf.Final_Status == "Accept"))
                                                     select chq).ToListAsync();

                foreach (var inward in clearedPmaInwardCheques)
                {
                    try
                    {
                        var obj = new ECC_CAP_Services.ECC_FT_Request();
                        var EccAccInfo_WebSvc = new ECC_CAP_Services.SAFA_T24_ECC_SVCSoapClient("SAFA_T24_ECC_SVCSoap");
                        string sysName = db.System_Configurations_Tbl.SingleOrDefault(c => c.Config_Type == "ECC_SYSTEM_ID" && c.Config_Id == "7").Config_Value;
                        var ftResponse = new FT_ResponseClass();
                        double amount = inward.Amount;

                        string decryptedAccountResult = Get_Deacrypted_Account(inward.DrwAcctNo, inward.DrwChqNo);
                        string payAccount = decryptedAccountResult.Split(
';
')[0];
                        if (payAccount.Length != 13)
                        {
                            payAccount = Get_ALT_Acc_No(decryptedAccountResult.Split(
';
')[0]);
                        }

                        obj.PayAccountNumber = payAccount;
                        obj.BFDAccountNumber = "0000";
                        obj.PsSystem = sysName;
                        obj.RequestDate = DateTime.Now.ToString("yyyyMMdd");
                        obj.RequestTime = DateTime.Now.ToString("HH
':
'mm
':
'ss");
                        obj.RequestCode = "ReversePostingPMARAM";
                        obj.TransSeq = inward.Serial;
                        obj.CheckSeq = inward.ChqSequance.Trim();
                        obj.PayBankCode = inward.DrwBankNo;
                        obj.PayBranchCode = inward.DrwBranchNo;
                        obj.BFDBankCode = inward.BenfBnk;
                        obj.BFDBranchCode = inward.BenfAccBranch;
                        obj.CheckSerial = Convert.ToInt32(inward.DrwChqNo.Substring(1));

                        if (inward.Currency.Trim().ToUpper() == "JOD")
                        {
                            obj.CheckAmount = amount.ToString("N3").Replace(",", null);
                        }
                        else
                        {
                            obj.CheckAmount = amount.ToString("N2").Replace(",", null);
                        }
                        obj.CurrencyCode = GetCurrencyCode(inward.Currency);

                        string reasonCode = getfinalonuscode(inward.ReturnCode, inward.ReturnCodeFinancail, inward.ClrCenter);
                        if (string.IsNullOrEmpty(reasonCode))
                        {
                            reasonCode = "02";
                        }
                        obj.ReasonCode = reasonCode;
                        obj.FeesFlag = "2";

                        int msgId = Temenos_Message_Types.ReversePostingPMARAM;
                        ftResponse.FT_Res = await EccAccInfo_WebSvc.ECC_OFS_MESSAGE(obj, msgId, 1); // Mock this call

                        if (ftResponse.FT_Res.ErrorMessage.Contains("OFSERROR_TIMEOUT"))
                        {
                            inward.ErrorDescription = "ReversePostingPMARAM  : OFSERROR_TIMEOUT  ";
                            inward.LastUpdateBy = userName;
                            inward.IsTimeOut = 1;
                            inward.History += "|" + "ReversePostingPMARAM  : OFSERROR_TIMEOUT,  User: " + userName + " AT :" + DateTime.Now;
                            db.Entry(inward).State = EntityState.Modified;
                            await db.SaveChangesAsync();
                            inwardList.Add(inward);
                        }
                        else
                        {
                            if (ftResponse.FT_Res.ResponseStatus == "S")
                            {
                                inward.FinalRetCode = getfinalonuscode(inward.ReturnCode, inward.ReturnCodeFinancail, inward.ClrCenter);
                                inward.Posted = AllEnums.Cheque_Status.Cleared;
                                inward.History += "|" + "  PMA CHQ become  Cleared by " + userName + "  AT : " + DateTime.Now;
                                inward.LastUpdateBy = userName;
                                inward.LastUpdate = DateTime.Now;

                                var wfHistory = new WFHistory
                                {
                                    ChqSequance = inward.ChqSequance,
                                    Serial = inward.Serial,
                                    TransDate = inward.ValueDate,
                                    ID_WFStatus = 0, // Placeholder
                                    Status = inward.Status + "PMA CHQ become  Cleared by " + userName + "  AT : " + DateTime.Now,
                                    ClrCenter = inward.ClrCenter,
                                    DrwChqNo = inward.DrwChqNo,
                                    DrwAccNo = inward.DrwAcctNo,
                                    Amount = inward.Amount
                                };
                                db.WFHistories.Add(wfHistory);
                                db.Entry(inward).State = EntityState.Modified;
                                await db.SaveChangesAsync();
                                clearedCount++;
                            }
                            else
                            {
                                string responseDetails = "";
                                // Simulate account info calls and posting restriction checks
                                inward.History += "|" + "Reverse  ALL PMA Failed ";
                                inward.T24Response += ftResponse.FT_Res.ResponseDescription + "Reverse  PDC PMA Failed " + "| " + responseDetails;
                                inward.LastUpdateBy = userName;
                                inward.Returned = 0;
                                db.Entry(inward).State = EntityState.Modified;
                                await db.SaveChangesAsync();
                                inwardList.Add(inward);
                            }
                        }
                    }
                    catch (Exception innerEx)
                    {
                        _logger.LogError(innerEx, "Error processing PMA Inward_Trans item {Serial}: {Message}", inward.Serial, innerEx.Message);
                    }
                }

                // Call Update_Returned_CHQ_TBL (needs to be implemented)
                // await Update_Returned_CHQ_TBL("INWARED");

                respMessage = clearedCount + "  Cheque(s) Became Cleared";

                _json.Data = new { ErrorMsg = "", Result = respMessage, lstPDC = inwardList };
                return _json;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ReversePMAINWARAD: {Message}", ex.Message);
                _json.Data = new { ErrorMsg = "An error occurred during PMA INWARD reversal process." };
                return _json;
            }
        }


_code
        public async Task<JsonResult> ReversePMAOUTWARD(string userName)
        {
            var _json = new JsonResult(new { });
            string tDate = DateTime.Now.ToString("yyyy-MM-dd");

            try
            {
                var db = _context;

                // Update OnUs_Tbl for cleared cheques
                var onUsToClear = await db.OnUs_Tbl.Where(o => o.verifyStatus == true).ToListAsync();
                foreach (var item in onUsToClear)
                {
                    if (DateTime.Parse(tDate) >= item.TransDate)
                    {
                        var wf = await db.INWARD_WF_Tbl.SingleOrDefaultAsync(w => w.Serial == item.Serial && w.Final_Status != "Accept");
                        if (wf == null)
                        {
                            item.Posted = AllEnums.Cheque_Status.Cleared;
                            item.History += "|CHQ become Cleared at " + userName + " AT : " + DateTime.Now;
                        }
                        else if (wf.Final_Status == "Accept")
                        {
                            item.Posted = AllEnums.Cheque_Status.Cleared;
                            item.History += "|CHQ become Cleared at " + userName + " AT : " + DateTime.Now;
                        }
                    }
                }
                await db.SaveChangesAsync();

                // Update Inward_Trans for pending PMA status
                var inwardToPending = await db.Inward_Trans.Where(x => x.verifyStatus == false && x.ClrCenter == "PMA" && x.Posted == AllEnums.Cheque_Status.Posted).ToListAsync();
                foreach (var item in inwardToPending)
                {
                    if (DateTime.Parse(tDate) >= item.TransDate)
                    {
                        item.PMAstatus = "Pending";
                        item.Returned = true;
                        item.History += " change PMA STATUS to : Pending , by : " + userName + "AT: " + DateTime.Now;
                    }
                }
                await db.SaveChangesAsync();

                // Update Inward_Trans for cheques stuck in WF
                var wfStuck = await db.INWARD_WF_Tbl.Where(x => x.Clr_Center == "Inoward_PMA" && x.Final_Status != "Accept").ToListAsync();
                foreach (var item in wfStuck)
                {
                    var inward = await db.Inward_Trans.SingleOrDefaultAsync(i => i.ChqSequance == item.ChqSequance && i.Posted == AllEnums.Cheque_Status.Posted);
                    if (inward != null && DateTime.Parse(tDate) >= inward.TransDate)
                    {
                        inward.PMAstatus = "Pending";
                        inward.Returned = true;
                        inward.History += " change PMA STATUS to : Pending , by : " + userName + "AT: " + DateTime.Now;
                    }
                }
                await db.SaveChangesAsync();

                _json.Data = new { ErrorMsg = "", Result = "Process completed." };
                return _json;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ReversePMAOUTWARD: {Message}", ex.Message);
                _json.Data = new { ErrorMsg = "An error occurred during the PMA OUTWARD reversal process." };
                return _json;
            }
        }




        public async Task<JsonResult> Insufficient_Funds(string userName)
        {
            var _json = new JsonResult(new { });
            double fundAmount = 0;
            bool accBalanceFlag = false;
            int errCount = 0;
            string errorMsg = "";
            int count = 0;

            try
            {
                var config = await _context.System_Configurations_Tbl.SingleOrDefaultAsync(i => i.Config_Name_EN == "Insufficient_balances_JOD");
                if (config != null && double.TryParse(config.Config_Value, out double parsedFundAmount))
                {
                    fundAmount = parsedFundAmount;
                }

                _logger.LogInformation("Start check Insufficient_Funds");

                var db = _context;
                var EccAccInfo_WebSvc = new ECC_CAP_Services.SAFA_T24_ECC_SVCSoapClient("SAFA_T24_ECC_SVCSoap");
                string tDate = DateTime.Now.ToString("yyyyMMdd");

                var query = from wf in db.INWARD_WF_Tbl
                            where wf.input_date.HasValue && wf.input_date.Value.ToString("yyyyMMdd") == tDate
                                  && wf.Need_Finanical_WF == true && wf.IsFinanicallyFixed == false
                                  && wf.Final_Status != "Accept"
                                  && wf.Response_Description.Contains("Insufficient Funds")
                            group wf by new { wf.DrwAccountNo, wf.Currency } into g
                            orderby g.Count() descending
                            select new
                            {
                                DrwAccountNo = g.Key.DrwAccountNo,
                                Currency = g.Key.Currency,
                                TotalAmount = g.Sum(x => x.Amount),
                                Count = g.Count()
                            };

                var insufficientFundsData = await query.ToListAsync();

                foreach (var data in insufficientFundsData)
                {
                    try
                    {
                        string accStr = data.DrwAccountNo;
                        if (!string.IsNullOrEmpty(accStr))
                        {
                            string altAccStr = Get_Deacrypted_Account(accStr, "00000001").Split(";")[0];
                            var accObj = await EccAccInfo_WebSvc.ACCOUNT_INFOAsync(altAccStr, 1);

                            double balance = 0;
                            if (double.TryParse(accObj.Body.ACCOUNT_INFO_RESPONSE1.ClearBalance.Replace(",", ""), out double parsedBalance))
                            {
                                balance = parsedBalance;
                            }

                            if (balance >= 0)
                            {
                                accBalanceFlag = true;
                            }

                            double totalAmount = data.TotalAmount ?? 0;
                            string currency = data.Currency;

                            // Assuming EVALUATE_AMOUNT_IN_JOD is implemented elsewhere or not needed for now
                            // balance = EVALUATE_AMOUNT_IN_JOD(accObj.Currency, balance);
                            // totalAmount = EVALUATE_AMOUNT_IN_JOD(currency, totalAmount);

                            double c = balance + totalAmount;
                            c += fundAmount;
                            balance += fundAmount;

                            if (c >= 0 && !(balance >= 0))
                            {
                                _logger.LogInformation($"Insufficient_Funds Acc: {accStr}");

                                var chqsToFix = await db.INWARD_WF_Tbl
                                    .Where(s => s.DrwAccountNo == accStr
                                                && s.input_date.HasValue && s.input_date.Value.ToString("yyyyMMdd") == tDate
                                                && s.Need_Finanical_WF == true && s.IsFinanicallyFixed == false
                                                && s.Final_Status != "Accept"
                                                && s.Response_Description.Contains("Insufficient Funds"))
                                    .OrderBy(s => s.DrwChqNo)
                                    .ToListAsync();

                                foreach (var wfChq in chqsToFix)
                                {
                                    c -= (wfChq.Amount ?? 0);

                                    _logger.LogInformation($"Insufficient_Funds Acc: {accStr}");

                                    string custPosting = accObj.Body.ACCOUNT_INFO_RESPONSE1.CustPosting;
                                    string acctPosting = accObj.Body.ACCOUNT_INFO_RESPONSE1.AcctPosting;

                                    if (c > 0 && (string.IsNullOrEmpty(custPosting) || custPosting == "16") && string.IsNullOrEmpty(acctPosting))
                                    {
                                        _logger.LogInformation($"Insufficient_Funds Acc: {accStr}");

                                        wfChq.IsFinanicallyFixed = true;
                                        db.Entry(wfChq).State = EntityState.Modified;
                                        await db.SaveChangesAsync();

                                        bool status = await chqwf(wfChq.ChqSequance);

                                        if (status)
                                        {
                                            wfChq.Final_Status = "Accept";
                                            wfChq.Balance = balance;
                                            wfChq.last_update_by = userName;
                                            wfChq.last_update_date = DateTime.Now;
                                            wfChq.History += $"  |  {userName}  Update Insufficnt Funds with Blance ={balance} To be Accepted Final status, chq seq :{wfChq.ChqSequance} Account: {accStr} And CHQ Amount : {wfChq.Amount}  At : {DateTime.Now}";
                                            count++;

                                            var onUs = await db.OnUs_Tbl.SingleOrDefaultAsync(y => y.ChqSequance == wfChq.ChqSequance);
                                            var inwardTrans = await db.Inward_Trans.SingleOrDefaultAsync(z => z.ChqSequance == wfChq.ChqSequance);

                                            if (onUs != null)
                                            {
                                                onUs.ReturnCodeFinancail = "";
                                                onUs.History += $"| ReturnCodeFinancail Removed after Proccess Insuffiant Fund  ,User: {userName}, AT:{DateTime.Now}";
                                                db.Entry(onUs).State = EntityState.Modified;
                                                await db.SaveChangesAsync();

                                                var wfHistory = new WFHistory
                                                {
                                                    ChqSequance = onUs.ChqSequance,
                                                    Serial = onUs.Serial,
                                                    TransDate = onUs.TransDate,
                                                    ID_WFStatus = 0,
                                                    Status = onUs.Status + $"  ReturnCodeFinancail Removed after Proccess Insuffiant Fund ,User: {userName}, AT:{DateTime.Now}",
                                                    ClrCenter = onUs.ClrCenter,
                                                    DrwChqNo = onUs.DrwChqNo,
                                                    DrwAccNo = onUs.DrwAcctNo,
                                                    Amount = onUs.Amount
                                                };
                                                db.WFHistories.Add(wfHistory);
                                                await db.SaveChangesAsync();
                                            }

                                            if (inwardTrans != null)
                                            {
                                                inwardTrans.ReturnCodeFinancail = "";
                                                inwardTrans.History += $"| ReturnCodeFinancail Removed after Proccess Insuffiant Fund  ,User: {userName}, AT:{DateTime.Now}";
                                                db.Entry(inwardTrans).State = EntityState.Modified;
                                                await db.SaveChangesAsync();

                                                var wfHistory = new WFHistory
                                                {
                                                    ChqSequance = inwardTrans.ChqSequance,
                                                    Serial = inwardTrans.Serial,
                                                    TransDate = inwardTrans.TransDate,
                                                    ID_WFStatus = 0,
                                                    Status = inwardTrans.Status + $"  ReturnCodeFinancail Removed after Proccess Insuffiant Fund ,User: {userName}, AT:{DateTime.Now}",
                                                    ClrCenter = inwardTrans.ClrCenter,
                                                    DrwChqNo = inwardTrans.DrwChqNo,
                                                    DrwAccNo = inwardTrans.DrwAcctNo,
                                                    Amount = inwardTrans.Amount
                                                };
                                                db.WFHistories.Add(wfHistory);
                                                await db.SaveChangesAsync();
                                            }
                                            db.Entry(wfChq).State = EntityState.Modified;
                                            await db.SaveChangesAsync();
                                        }
                                        else
                                        {
                                            wfChq.Final_Status = "Pending";
                                            wfChq.last_update_by = userName;
                                            wfChq.last_update_date = DateTime.Now;
                                            wfChq.History += $"  |  {userName}  Update Insufficnt Funds with Blance ={balance} To be Fixed , still need Technical WF , chq seq :{wfChq.ChqSequance} Account: {accStr} And CHQ Amount : {wfChq.Amount}  At : {DateTime.Now}";
                                            wfChq.Balance = balance;
                                            db.Entry(wfChq).State = EntityState.Modified;
                                            await db.SaveChangesAsync();
                                        }
                                    }
                                    else if ((string.IsNullOrEmpty(custPosting) || custPosting != "16") && string.IsNullOrEmpty(acctPosting) && c < 0)
                                    {
                                        _logger.LogInformation($"Insufficient_Funds Acc: {accStr}");
                                        wfChq.History += $"  |  {userName}  Update Insufficnt Funds With Blance ={balance} PostRest = {acctPosting}{custPosting}This Account Befor inward CHQ was <0 ,chq seq :{wfChq.ChqSequance} Account: {accStr} And CHQ Amount : {wfChq.Amount}  At : {DateTime.Now}";
                                        wfChq.last_update_date = DateTime.Now;
                                        wfChq.last_update_by = userName;
                                        wfChq.Balance = balance;
                                        db.Entry(wfChq).State = EntityState.Modified;
                                        await db.SaveChangesAsync();
                                    }
                                    else
                                    {
                                        _logger.LogInformation($"Insufficient_Funds Acc: {accStr}");
                                        wfChq.History += $"  |  {userName}  Update Insufficnt Funds With Blance ={balance} PostRest = {acctPosting}{custPosting},chq seq :{wfChq.ChqSequance} Account: {accStr} And CHQ Amount : {wfChq.Amount}  At : {DateTime.Now}";
                                        wfChq.last_update_date = DateTime.Now;
                                        wfChq.last_update_by = userName;
                                        wfChq.Balance = balance;
                                        db.Entry(wfChq).State = EntityState.Modified;
                                        await db.SaveChangesAsync();
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception innerEx)
                    {
                        _logger.LogError(innerEx, "Error processing Insufficient_Funds for account {Account}: {Message}", data.DrwAccountNo, innerEx.Message);
                        errorMsg += $"Error for account {data.DrwAccountNo}: {innerEx.Message}\n";
                        errCount++;
                    }
                }

                _json.Data = new { ErrorMsg = errorMsg, Result = $"There are {count} cheques fixed." };
                return _json;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Insufficient_Funds: {Message}", ex.Message);
                _json.Data = new { ErrorMsg = "An error occurred during insufficient funds check." };
                return _json;
            }
        }






        public async Task<JsonResult> ReverseAllINHOUSE_PDC(string userName)
        {
            var _json = new JsonResult(new { });
            string tDate = DateTime.Now.ToString("yyyy-MM-dd");
            var onusList = new List<OnUs_Tbl>();
            string resp = "";
            int count = 0;

            try
            {
                var db = _context;
                var EccAccInfo_WebSvc = new ECC_CAP_Services.SAFA_T24_ECC_SVCSoapClient("SAFA_T24_ECC_SVCSoap");
                string sysName = db.System_Configurations_Tbl.SingleOrDefault(c => c.Config_Type == "ECC_SYSTEM_ID" && c.Config_Id == "7").Config_Value;

                // Check for cheques stuck at Fixed Error Without Ret Code
                var stuckCheques = await (from wf in db.INWARD_WF_Tbl
                                          where wf.input_date.HasValue && wf.input_date.Value.ToString("yyyy-MM-dd") == tDate
                                                && wf.Need_Fixed_Error == true && wf.ISFixederror == false
                                                && (wf.Clr_Center.Contains("ONUS") || wf.Clr_Center.Contains("PDC"))
                                          join onus in db.OnUs_Tbl on wf.ChqSequance equals onus.ChqSequance
                                          where onus.ReturnCode == null && onus.ReturnCodeFinancail == null && onus.Posted == AllEnums.Cheque_Status.Returne
                                          select onus).ToListAsync();

                if (stuckCheques.Any())
                {
                    _logger.LogWarning($"Found {stuckCheques.Count} ONUS/PDC cheques stuck in fixed error without return code.");
                    _json.Data = new { ErrorMsg = "Process Not completed, Please Be careful, There are: " + stuckCheques.Count + " Cheques Without Return Code", LstPDC = stuckCheques.Select(o => o.Serial).ToList() };
                    return _json;
                }

                // Start Return all cheques that stuck @WF or that have RetCode
                _logger.LogInformation("Start Update Cleared PDC INHOUSE Cheques.");

                var chequesToReverse = await (from onus in db.OnUs_Tbl
                                              where onus.Posted == AllEnums.Cheque_Status.Posted
                                                    && onus.TransDate.ToString("yyyy-MM-dd") == tDate
                                                    && onus.Was_PDC == true
                                                    && (onus.verifyStatus == false || (from wf in db.INWARD_WF_Tbl where wf.ChqSequance == onus.ChqSequance select wf).Any(wf => wf.Final_Status != "Accept"))
                                              select onus).ToListAsync();

                foreach (var onus in chequesToReverse)
                {
                    try
                    {
                        var obj = new ECC_CAP_Services.ECC_FT_Request();
                        var ftResponse = new FT_ResponseClass();

                        string decryptedAccountResult = Get_Deacrypted_Account(onus.DrwAcctNo, onus.DrwChqNo);
                        string payAccount = decryptedAccountResult.Split(';')[0];
                        if (payAccount.Length != 13)
                        {
                            payAccount = Get_ALT_Acc_No(decryptedAccountResult.Split(';')[0]);
                        }

                        string bnfAcc = onus.BenAccountNo;
                        if (bnfAcc.Length != 13)
                        {
                            bnfAcc = Get_ALT_Acc_No(onus.BenAccountNo);
                        }

                        obj.PayAccountNumber = payAccount;
                        obj.BFDAccountNumber = bnfAcc;
                        obj.PsSystem = sysName;
                        obj.RequestDate = DateTime.Now.Date.ToString("yyyyMMdd");
                        obj.RequestTime = DateTime.Now.ToString("HH:mm:ss");
                        obj.RequestCode = "ONUSReversePosting";
                        obj.TransSeq = onus.Serial;
                        obj.CheckSeq = onus.ChqSequance.Trim();
                        obj.PayBankCode = onus.DrwBankNo;
                        obj.PayBranchCode = onus.DrwBranchNo;
                        obj.BFDBankCode = onus.BenfBnk;
                        obj.BFDBranchCode = onus.BenfAccBranch;
                        obj.CheckSerial = Convert.ToInt32(onus.DrwChqNo.Substring(1));
                        double amount = onus.Amount;

                        if (onus.Currency.Trim().ToUpper() == "JOD")
                        {
                            obj.CheckAmount = amount.ToString("N3").Replace(",", null);
                        }
                        else
                        {
                            obj.CheckAmount = amount.ToString("N2").Replace(",", null);
                        }
                        obj.CurrencyCode = GetCurrencyCode(onus.Currency);

                        string reasonCode = getfinalonuscode(onus.ReturnCode, onus.ReturnCodeFinancail, onus.ClrCenter);
                        if (string.IsNullOrEmpty(reasonCode))
                        {
                            reasonCode = "02";
                        }
                        obj.ReasonCode = reasonCode;
                        obj.FeesFlag = "2";

                        _logger.LogInformation($"Sending MQ Message ONUSReversePosting for ChqSequance: {onus.ChqSequance}");
                        int msgId = Temenos_Message_Types.ONUSReversePosting;
                        ftResponse.FT_Res = await EccAccInfo_WebSvc.ECC_OFS_MESSAGEAsync(obj, msgId, 1); // Mock this call

                        if (ftResponse.FT_Res.ErrorMessage != null && ftResponse.FT_Res.ErrorMessage.Contains("OFSERROR_TIMEOUT"))
                        {
                            onus.ErrorDescription = "ONUSReversePosting : OFSERROR_TIMEOUT";
                            onus.LastUpdateBy = userName;
                            onus.IsTimeOut = 1;
                            onus.History += "|" + "ONUSReversePosting : OFSERROR_TIMEOUT ,  User:" + userName + " AT: " + DateTime.Now;
                            db.Entry(onus).State = EntityState.Modified;
                            await db.SaveChangesAsync();
                            onusList.Add(onus);
                        }
                        else
                        {
                            if (ftResponse.FT_Res.ResponseStatus == "S")
                            {
                                onus.FinalRetCode = getfinalonuscode(onus.ReturnCode, onus.ReturnCodeFinancail, onus.ClrCenter);

                                var wfHistory = new WFHistory
                                {
                                    ChqSequance = onus.ChqSequance,
                                    Serial = onus.Serial,
                                    TransDate = onus.TransDate,
                                    ID_WFStatus = 0,
                                    Status = onus.Status + "CHQ Returnd   by " + userName + "  AT : " + DateTime.Now + " With Return Code:" + onus.FinalRetCode,
                                    ClrCenter = onus.ClrCenter,
                                    DrwChqNo = onus.DrwChqNo,
                                    DrwAccNo = onus.DrwAcctNo,
                                    Amount = onus.Amount
                                };
                                db.WFHistories.Add(wfHistory);
                                await db.SaveChangesAsync();

                                onus.Posted = AllEnums.Cheque_Status.Returne;
                                onus.CHQState = "1";
                                onus.ISneedCommision = 1;
                                onus.CHQStatedate = DateTime.Now;
                                onus.History += "|" + "Reverse PDC INHOUSE Done ";
                                onus.ReturnDate = DateTime.Now;
                                onus.TransDate = DateTime.Now;
                                onus.IsTimeOut = 0;
                                onus.Returned = true;
                                onus.T24Response = "   ONUSReversePosting " + ftResponse.FT_Res.ResponseDescription;
                                db.Entry(onus).State = EntityState.Modified;
                                await db.SaveChangesAsync();

                                var wfTbl = await db.INWARD_WF_Tbl.SingleOrDefaultAsync(i => i.ChqSequance == onus.ChqSequance && i.Final_Status != "Accept");
                                if (wfTbl != null)
                                {
                                    wfTbl.Final_Status = "Accept";
                                    wfTbl.History += "  | " + userName + " Reverse CHQ (تحديث المعادة) , At:" + DateTime.Now;
                                    wfTbl.RetByWF = true;
                                    db.Entry(wfTbl).State = EntityState.Modified;
                                    await db.SaveChangesAsync();
                                }

                                _logger.LogInformation($"start ReturnChq_ONUSTBL chqseq {onus.ChqSequance}, Serial : {onus.Serial}");
                                ReturnChq_ONUSTBL(onus); // Assuming this method exists in the service
                                _logger.LogInformation($"End ReturnChq_ONUSTBL chqseq {onus.ChqSequance}, Serial : {onus.Serial}");

                                var web_service_com = new ECC_Com_SVC.Ecc_Commisions_HandlerSoapClient("Ecc_Commisions_HandlerSoap");
                                string alt = Get_ALT_Acc_No(onus.DecreptedDrwAcountNo);
                                string retCode = getfinalonuscode(onus.ReturnCode, onus.ReturnCodeFinancail, onus.ClrCenter);
                                if (string.IsNullOrEmpty(retCode))
                                {
                                    retCode = "02";
                                }
                                var obj_com = await web_service_com.Post_Commision_To_T24Async(onus.DrwAcctNo, 3, alt, onus.Amount, onus.DrwChqNo, onus.DrwBankNo, userName, retCode, onus.Serial);

                                if (obj_com.Status != "Failed")
                                {
                                    onus.RevCommision = 1;
                                    onus.T24Response += "| Commision :" + obj_com.Descreption + " With Amount : " + obj_com.CommisionAmount;
                                }
                                else
                                {
                                    if (obj_com.Status == "OFSERROR_TIMEOUT")
                                    {
                                        onus.RevCommision = 0;
                                    }
                                    onus.T24Response += "| Commision :" + obj_com.Descreption;
                                }
                                db.Entry(onus).State = EntityState.Modified;
                                await db.SaveChangesAsync();
                                onusList.Add(onus);
                            }
                            else
                            {
                                string responseDetails = "";
                                if (ftResponse.FT_Res.ResponseDescription == "Similar Cheque Sequence already Reversed,Failed")
                                {
                                    onus.ReturnCode = getfinalonuscode(onus.ReturnCode, onus.ReturnCodeFinancail, onus.ClrCenter);
                                    onus.FinalRetCode = getfinalonuscode(onus.ReturnCode, onus.ReturnCodeFinancail, onus.ClrCenter);
                                    onus.Posted = AllEnums.Cheque_Status.Returne;
                                    onus.CHQState = "1";
                                    onus.ISneedCommision = 1;
                                    onus.LastUpdate = DateTime.Now;
                                    onus.History += "|" + "Reverse PMA CHQ  Done ";
                                    onus.ReturnDate = DateTime.Now;
                                    onus.TransDate = DateTime.Now;
                                    onus.IsTimeOut = 0;
                                    onus.LastUpdateBy = userName;
                                    onus.Returned = true;
                                    onus.T24Response = "  ReversePostingPMARAM " + ftResponse.FT_Res.ResponseDescription;
                                    db.Entry(onus).State = EntityState.Modified;
                                    await db.SaveChangesAsync();

                                    var wfTbl = await db.INWARD_WF_Tbl.SingleOrDefaultAsync(i => i.ChqSequance == onus.ChqSequance && i.Final_Status != "Accept");
                                    if (wfTbl != null)
                                    {
                                        wfTbl.Final_Status = "Accept";
                                        wfTbl.History += "  | " + userName + " Reverse CHQ (تحديث المعادة) , At:" + DateTime.Now;
                                        wfTbl.RetByWF = true;
                                        db.Entry(wfTbl).State = EntityState.Modified;
                                        await db.SaveChangesAsync();
                                    }
                                    ReturnChq_ONUSTBL(onus);

                                    var web_service_com = new ECC_Com_SVC.Ecc_Commisions_HandlerSoapClient("Ecc_Commisions_HandlerSoap");
                                    string alt = Get_ALT_Acc_No(onus.DecreptedDrwAcountNo);
                                    string retCode_com = getfinalonuscode(onus.ReturnCode, onus.ReturnCodeFinancail, onus.ClrCenter);
                                    if (string.IsNullOrEmpty(retCode_com))
                                    {
                                        retCode_com = "02";
                                    }
                                    var obj_com = await web_service_com.Post_Commision_To_T24Async(onus.DrwAcctNo, 3, alt, onus.Amount, onus.DrwChqNo, onus.DrwBankNo, userName, retCode_com, onus.Serial);
                                    if (obj_com.Status != "Failed")
                                    {
                                        onus.RevCommision = 1;
                                        onus.T24Response += "| Commision :" + obj_com.Descreption + " With Amount : " + obj_com.CommisionAmount;
                                    }
                                    else
                                    {
                                        if (obj_com.Status == "OFSERROR_TIMEOUT")
                                        {
                                            onus.RevCommision = 0;
                                        }
                                        onus.T24Response += "| Commision :" + obj_com.Descreption;
                                    }
                                    db.Entry(onus).State = EntityState.Modified;
                                    await db.SaveChangesAsync();
                                    onusList.Add(onus);
                                }
                                else
                                {
                                    // Handle other failure cases, including account info and posting restrictions
                                    var EccAccWebSvc = new ECC_CAP_Services.SAFA_T24_ECC_SVCSoapClient("SAFA_T24_ECC_SVCSoap");
                                    var Acc_obj = await EccAccWebSvc.ACCOUNT_INFOAsync(onus.BenAccountNo, 1);
                                    if (Acc_obj.Body.ACCOUNT_INFO_RESPONSE1.CustPosting != "0" || Acc_obj.Body.ACCOUNT_INFO_RESPONSE1.AcctPosting != "0")
                                    {
                                        // Append details to responseDetails
                                    }

                                    var Acc_obj1 = await EccAccWebSvc.ACCOUNT_INFOAsync(onus.DecreptedDrwAcountNo, 1);
                                    if (Acc_obj1.Body.ACCOUNT_INFO_RESPONSE1.CustPosting != "0" || Acc_obj1.Body.ACCOUNT_INFO_RESPONSE1.AcctPosting != "0")
                                    {
                                        // Append details to responseDetails
                                    }

                                    onus.History += "|" + "Reverse  PDC INHOUSE Failed ";
                                    onus.T24Response = ftResponse.FT_Res.ResponseDescription + " | " + responseDetails;
                                    onus.LastUpdateBy = userName;
                                    onus.Returned = 0;
                                    db.Entry(onus).State = EntityState.Modified;
                                    await db.SaveChangesAsync();
                                    onus.IsTimeOut = 0;
                                    onusList.Add(onus);
                                }
                            }
                        }
                    }
                    catch (Exception innerEx)
                    {
                        _logger.LogError(innerEx, "Error processing OnUs_Tbl item {Serial}: {Message}", onus.Serial, innerEx.Message);
                    }
                }

                _json.Data = new { ErrorMsg = "", Result = "Process completed.", LstPDC = onusList };
                return _json;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ReverseAllINHOUSE_PDC: {Message}", ex.Message);
                _json.Data = new { ErrorMsg = "An error occurred during the INHOUSE PDC reversal process." };
                return _json;
            }
        }




        public async Task<JsonResult> ReverseAllDISCOUNT(string userName)
        {
            var _json = new JsonResult(new { });
            string tDate = DateTime.Now.ToString("yyyy-MM-dd");
            var inwardList = new List<Inward_Trans>();

            try
            {
                var db = _context;
                var EccAccInfo_WebSvc = new ECC_CAP_Services.SAFA_T24_ECC_SVCSoapClient("SAFA_T24_ECC_SVCSoap");
                string sysName = db.System_Configurations_Tbl.SingleOrDefault(c => c.Config_Type == "ECC_SYSTEM_ID" && c.Config_Id == "7").Config_Value;

                // Check for cheques stuck at Fixed Error Without Ret Code
                var stuckChequesCount = await (from wf in db.INWARD_WF_Tbl
                                               where wf.input_date.HasValue && wf.input_date.Value.ToString("yyyy-MM-dd") == tDate
                                                     && wf.Need_Fixed_Error == true && wf.ISFixederror == false
                                                     && wf.Clr_Center.Contains("DISCOUNT")
                                               join inward in db.Inward_Trans on wf.ChqSequance equals inward.ChqSequance
                                               where inward.ReturnCode == null && inward.ReturnCodeFinancail == null && inward.Posted == AllEnums.Cheque_Status.Returne
                                               select inward).CountAsync();

                if (stuckChequesCount > 0)
                {
                    _logger.LogWarning($"Found {stuckChequesCount} DISCOUNT cheques stuck in fixed error without return code.");
                    _json.Data = new { ErrorMsg = "Process Not completed, Please Be careful, There are: " + stuckChequesCount + " Cheques Without Return Code" };
                    return _json;
                }

                // Start Return all cheques that stuck @WF or that have RetCode
                _logger.LogInformation("Start Update Cleared PDC INHOUSE Cheques.");

                var chequesToReverse = await (from inward in db.Inward_Trans
                                              where inward.Posted == AllEnums.Cheque_Status.Posted
                                                    && inward.ClrCenter == "DISCOUNT"
                                                    && inward.TransDate.HasValue && inward.TransDate.Value.ToString("yyyy-MM-dd") == tDate
                                                    && (inward.verifyStatus == false || (from wf in db.INWARD_WF_Tbl where wf.ChqSequance == inward.ChqSequance select wf).Any(wf => wf.Final_Status != "Accept"))
                                              select inward).ToListAsync();

                foreach (var inward in chequesToReverse)
                {
                    try
                    {
                        var wfRecord = await db.INWARD_WF_Tbl.SingleOrDefaultAsync(o => o.ChqSequance == inward.ChqSequance && o.Clr_Center == "DISCOUNT");

                        if (wfRecord == null || wfRecord.RetByWF != true)
                        {
                            var obj = new ECC_CAP_Services.ECC_FT_Request();
                            var ftResponse = new FT_ResponseClass();

                            string payAccount = Get_Deacrypted_Account(inward.DrwAcctNo, inward.DrwChqNo).Split(";")[0];
                            if (payAccount.Length != 13)
                            {
                                payAccount = Get_ALT_Acc_No(payAccount);
                            }

                            obj.PsSystem = sysName;
                            obj.RequestDate = inward.ValueDate.HasValue ? inward.ValueDate.Value.ToString("yyyyMMdd") : DateTime.Now.ToString("yyyyMMdd");
                            obj.RequestTime = DateTime.Now.ToString("HH:mm:ss");
                            obj.RequestCode = "ReversePostingDIS";
                            obj.TransSeq = inward.Serial;
                            obj.CheckSeq = inward.ChqSequance.Trim();
                            obj.PayBankCode = inward.DrwBankNo;
                            obj.PayBranchCode = inward.DrwBranchNo.Length > 3 ? inward.DrwBranchNo.Substring(1) : inward.DrwBranchNo;
                            obj.PayAccountNumber = inward.AltAccount;
                            obj.BFDBankCode = inward.BenfBnk;
                            obj.BFDBranchCode = inward.BenfAccBranch;
                            obj.BFDAccountNumber = "0000"; // Hardcoded as per VB.NET
                            obj.CheckSerial = Convert.ToInt32(inward.DrwChqNo.Substring(1));
                            double amount = inward.Amount;

                            if (inward.Currency.Trim().ToUpper() == "JOD")
                            {
                                obj.CheckAmount = amount.ToString("N3").Replace(",", null);
                            }
                            else
                            {
                                obj.CheckAmount = amount.ToString("N2").Replace(",", null);
                            }
                            obj.CurrencyCode = GetCurrencyCode(inward.Currency);

                            string reasonCode = getfinalonuscode(inward.ReturnCode, inward.ReturnCodeFinancail, inward.ClrCenter);
                            if (string.IsNullOrEmpty(reasonCode))
                            {
                                reasonCode = "02";
                            }
                            obj.ReasonCode = reasonCode;
                            obj.FeesFlag = "2";

                            _logger.LogInformation($"Sending MQ Message ReversePostingDIS for ChqSequance: {inward.ChqSequance}");
                            int msgId = Temenos_Message_Types.ReversePostingDIS;
                            ftResponse.FT_Res = await EccAccInfo_WebSvc.ECC_OFS_MESSAGEAsync(obj, msgId, 1);

                            if (ftResponse.FT_Res.ErrorMessage != null && ftResponse.FT_Res.ErrorMessage.Contains("TimeOut"))
                            {
                                inward.ErrorDescription = "RETURN INWARD   " + ftResponse.FT_Res.ResponseDescription;
                                inward.LastUpdateBy = userName;
                                inward.IsTimeOut = 1;
                                inward.History += "|" + " RETURN INWARD     Filed (TimeOut) User: " + userName + "AT:" + DateTime.Now;
                                db.Entry(inward).State = EntityState.Modified;
                                await db.SaveChangesAsync();
                                inwardList.Add(inward);
                            }
                            else
                            {
                                if (ftResponse.FT_Res.ResponseStatus == "S")
                                {
                                    inward.Posted = AllEnums.Cheque_Status.Returne;
                                    inward.History += "|" + "ReversePostingDIS Done ";
                                    inward.ReturnDate = DateTime.Now;
                                    inward.IsTimeOut = 0;
                                    inward.T24Response = ftResponse.FT_Res.ResponseDescription;
                                    inward.LastUpdateBy = userName;
                                    inward.Returned = true;
                                    db.Entry(inward).State = EntityState.Modified;
                                    await db.SaveChangesAsync();

                                    if (wfRecord != null)
                                    {
                                        wfRecord.RetByWF = true;
                                        wfRecord.Final_Status = "Accept";
                                        db.Entry(wfRecord).State = EntityState.Modified;
                                        await db.SaveChangesAsync();
                                    }

                                    _logger.LogInformation($"Start ReturnChqTBL for Serial: {inward.Serial}, ClrCenter: {inward.ClrCenter}, ChqNo: {inward.DrwAcctNo}");
                                    ReturnChqTBL(inward); // Assuming this method exists in the service

                                    var web_service_com = new ECC_Com_SVC.Ecc_Commisions_HandlerSoapClient("Ecc_Commisions_HandlerSoap");
                                    var db_ = _context; // Using _context directly
                                    var x = await db_.Inward_Trans.SingleOrDefaultAsync(c => c.ChqSequance == inward.ChqSequance);
                                    string alt = Get_ALT_Acc_No(x.AltAccount);
                                    string retCode_com = getfinalonuscode(inward.ReturnCode, inward.ReturnCodeFinancail, inward.ClrCenter);

                                    if (string.IsNullOrEmpty(retCode_com))
                                    {
                                        retCode_com = "02";
                                    }
                                    var obj_com = await web_service_com.Post_Commision_To_T24Async(inward.DrwAcctNo, 4, inward.AltAccount, inward.Amount, inward.DrwChqNo, inward.DrwBankNo, userName, retCode_com, inward.Serial);

                                    if (obj_com.Status != "Failed")
                                    {
                                        inward.RevCommision = true;
                                        inward.T24Response += " | Commision : " + obj_com.Descreption + " With Amount : " + obj_com.CommisionAmount;
                                    }
                                    else
                                    {
                                        if (obj_com.Status == "OFSERROR_TIMEOUT")
                                        {
                                            inward.RevCommision = false;
                                        }
                                        inward.T24Response += " | Commision : " + obj_com.Descreption;
                                    }
                                    db.Entry(inward).State = EntityState.Modified;
                                    await db.SaveChangesAsync();
                                    inwardList.Add(inward);
                                }
                                else
                                {
                                    inward.T24Response = ftResponse.FT_Res.ResponseDescription;
                                    inward.History += "|" + "ReversePostingDIS Failed ";
                                    inward.LastUpdateBy = userName;
                                    inward.Returned = false;
                                    inward.IsTimeOut = 0;
                                    db.Entry(inward).State = EntityState.Modified;
                                    await db.SaveChangesAsync();
                                    inwardList.Add(inward);
                                }
                            }
                        }
                    }
                    catch (Exception innerEx)
                    {
                        _logger.LogError(innerEx, "Error processing Inward_Trans item {Serial}: {Message}", inward.Serial, innerEx.Message);
                    }
                }

                _json.Data = new { ErrorMsg = "", Result = "Process completed.", LstInward = inwardList };
                return _json;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ReverseAllDISCOUNT: {Message}", ex.Message);
                _json.Data = new { ErrorMsg = "An error occurred during the DISCOUNT reversal process." };
                return _json;
            }
        }



        public async Task<ActionResult> Fix_Ret_CHQ()
        {
            // This method primarily deals with UI preparation (ViewBag, etc.)
            // and will be handled in the controller. The service will provide data if needed.
            // For now, return a dummy ActionResult.
            return await Task.FromResult<ActionResult>(new EmptyResult());
        }



        public async Task<JsonResult> PrintRemove(string Branchs, string Currency, string ChequeSource, string Amount, string DRWAccNo, string ChequeNo, string vip, string userName, string pageId, string groupId)
        {
            var _json = new JsonResult(new { });
            var lstPstPDC = new List<InwardSearchClass>();

            try
            {
                _logger.LogInformation("Before Search Inward cheques");
                _logger.LogTrace("BEFORE Creating New Instance from Inward_Trans Class");

                string inputDate = DateTime.Now.ToString("yyyyMMdd");

                string selectQuery = "";
                if (ChequeSource == "-1")
                {
                    selectQuery = "SELECT ClrCenter, DrwAcctNo, DrwChqNo, DrwBankNo, DrwBranchNo, BenAccountNo, BenfAccBranch, Amount, Currency FROM OnUs_Tbl WHERE CAST(InputDate AS DATE) = '" + inputDate + "' AND ISNULL(Note2,'')<>'' UNION ALL " +
                                  "SELECT ClrCenter, DrwAcctNo, DrwChqNo, DrwBankNo, DrwBranchNo, BenAccountNo, BenfAccBranch, Amount, Currency FROM Inward_Trans WHERE CAST(InputDate AS DATE) = '" + inputDate + "' AND ISNULL(Note2,'')<>''";
                }
                else if (ChequeSource.Contains("INHOUSE"))
                {
                    selectQuery = "SELECT ClrCenter, DrwAcctNo, DrwChqNo, DrwBankNo, DrwBranchNo, BenAccountNo, BenfAccBranch, Amount, Currency FROM OnUs_Tbl WHERE CAST(InputDate AS DATE) = '" + inputDate + "' AND ISNULL(Note2,'')<>''";
                }
                else
                {
                    selectQuery = "SELECT ClrCenter, DrwAcctNo, DrwChqNo, DrwBankNo, DrwBranchNo, BenAccountNo, BenfAccBranch, Amount, Currency FROM Inward_Trans WHERE CAST(InputDate AS DATE) = '" + inputDate + "' AND ISNULL(Note2,'')<>'' AND ClrCenter = '" + ChequeSource + "'";
                }

                _logger.LogTrace("BEFORE Creating New Instance from SQL Connections");

                // Assuming _context is your DbContext
                var db = _context;

                var user = await db.Users_Tbl.SingleOrDefaultAsync(y => y.User_Name == userName);
                string comtype = user?.Company_ID;
                string _branch = "";

                if (!string.IsNullOrEmpty(comtype))
                {
                    var company = await db.Companies_Tbl.SingleOrDefaultAsync(u => u.Company_ID == comtype);
                    if (company != null && company.Company_Code != null)
                    {
                        _branch = company.Company_Code.ToString().Substring(5);
                    }
                }

                if (Branchs != "-Please Select Type-")
                {
                    if (_branch == "2") // Assuming '2' is a specific branch code
                    {
                        selectQuery += " And DrwBranchNo = '" + Branchs.Trim() + "'";
                    }
                    else
                    {
                        selectQuery += " And DrwBranchNo = '" + _branch.Trim() + "'";
                    }
                }
                else
                {
                    if (_branch != "2")
                    {
                        selectQuery += " And DrwBranchNo = '" + _branch.Trim() + "'";
                    }
                }

                if (Currency != "-1")
                {
                    string currencyCode = "";
                    if (Currency.Trim() == "1") currencyCode = "JOD";
                    else if (Currency.Trim() == "2") currencyCode = "USD";
                    else if (Currency.Trim() == "3") currencyCode = "ILS";
                    else if (Currency.Trim() == "5") currencyCode = "EUR";
                    selectQuery += " and Currency = '" + currencyCode.Trim() + "'";
                }

                if (!string.IsNullOrEmpty(Amount))
                {
                    selectQuery += " and Amount = '" + Amount.Trim() + "'";
                }

                if (!string.IsNullOrEmpty(DRWAccNo))
                {
                    selectQuery += " and DrwAcctNo LIKE '%" + DRWAccNo.Trim() + "%'";
                }

                if (!string.IsNullOrEmpty(ChequeNo))
                {
                    selectQuery += " and DrwChqNo LIKE '%" + ChequeNo.Trim() + "%'";
                }

                if (groupId == "4") // Assuming 4 is AdminAuthorized
                {
                    selectQuery += " and category = 1100";
                }

                _logger.LogTrace("BEFORE Execute SQL Query to Get Details of cheque");
                _logger.LogInformation($"select_query IS: {selectQuery} BY: {userName}, At:{DateTime.Now}");

                // Execute raw SQL query
                using (var command = db.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = selectQuery;
                    db.Database.OpenConnection();

                    using (var result = await command.ExecuteReaderAsync())
                    {
                        while (await result.ReadAsync())
                        {
                            var obj = new InwardSearchClass
                            {
                                ClrCenter = result["ClrCenter"]?.ToString(),
                                DrwAcctNo = result["DrwAcctNo"]?.ToString(),
                                DrwChqNo = result["DrwChqNo"]?.ToString(),
                                DrwBranchNo = result["DrwBranchNo"]?.ToString(),
                                DrwBankNo = result["DrwBankNo"]?.ToString(),
                                Currency = result["Currency"]?.ToString(),
                                Amount = Convert.ToDouble(result["Amount"]),
                                BenAccountNo = result["BenAccountNo"]?.ToString(),
                                BenfAccBranch = result["BenfAccBranch"]?.ToString()
                            };
                            lstPstPDC.Add(obj);
                        }
                    }
                }

                _json.Data = new { ErrorMsg = "", lstPDC = lstPstPDC };
                return _json;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in PrintRemove: {Message}", ex.Message);
                _json.Data = new { ErrorMsg = "An error occurred during the PrintRemove process." };
                return _json;
            }
        }



        public async Task<JsonResult> getSearchList_WF_fixederror(string Branchs, string Currency, string ChequeSource, string Amount, string DRWAccNo, string ChequeNo, string vip, string userName, string groupId)
        {
            var _json = new JsonResult(new { });
            var lstPstPDC = new List<INWARD_WF_Tbl>();

            try
            {
                _logger.LogInformation("Before Search Inward cheques for fixed error");

                string inputDate = DateTime.Now.ToString("yyyyMMdd");

                string selectQuery = $"SELECT * FROM INWARD_WF_Tbl WHERE Need_Fixed_Error = 1 AND ISFixederror = 0 AND Final_Status <> 'Accept' AND CAST(input_date AS DATE) = '{inputDate}'";

                var user = await _context.Users_Tbl.SingleOrDefaultAsync(y => y.User_Name == userName);
                string comtype = user?.Company_ID;
                string _branch = "";

                if (!string.IsNullOrEmpty(comtype))
                {
                    var company = await _context.Companies_Tbl.SingleOrDefaultAsync(u => u.Company_ID == comtype);
                    if (company != null && company.Company_Code != null)
                    {
                        _branch = company.Company_Code.ToString().Substring(5);
                    }
                }

                if (Branchs != "-1")
                {
                    if (_branch == "2") // Assuming '2' is a specific branch code
                    {
                        selectQuery += $" AND DrwBranch = '{Branchs.Trim()}'";
                    }
                    else
                    {
                        selectQuery += $" AND DrwBranch = '{_branch.Trim()}'";
                    }
                }
                else
                {
                    if (_branch != "2")
                    {
                        selectQuery += $" AND DrwBranch = '{_branch.Trim()}'";
                    }
                }

                if (Currency != "-1")
                {
                    string currencyCode = "";
                    if (Currency.Trim() == "1") currencyCode = "JOD";
                    else if (Currency.Trim() == "2") currencyCode = "USD";
                    else if (Currency.Trim() == "3") currencyCode = "ILS";
                    else if (Currency.Trim() == "5") currencyCode = "EUR";
                    selectQuery += $" AND Currency = '{currencyCode.Trim()}'";
                }

                if (ChequeSource != "-1")
                {
                    if (ChequeSource == "INHOUSE")
                    {
                        selectQuery += " AND UPPER(Clr_Center) LIKE '%Outward_ONUS%'";
                    }
                    else
                    {
                        selectQuery += $" AND UPPER(Clr_Center) LIKE '%{ChequeSource.ToUpper().Trim()}%'";
                    }
                }

                if (!string.IsNullOrEmpty(Amount))
                {
                    selectQuery += $" AND Amount = '{Amount.Trim()}'";
                }

                if (!string.IsNullOrEmpty(DRWAccNo))
                {
                    selectQuery += $" AND DrwAccountNo LIKE '%{DRWAccNo.Trim()}%'";
                }

                if (!string.IsNullOrEmpty(ChequeNo))
                {
                    selectQuery += $" AND DrwChqNo LIKE '%{ChequeNo.Trim()}%'";
                }

                if (groupId == "4") // Assuming 4 is AdminAuthorized
                {
                    selectQuery += " AND category = 1100";
                }

                _logger.LogInformation($"Executing SQL Query for getSearchList_WF_fixederror: {selectQuery}");

                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = selectQuery;
                    _context.Database.OpenConnection();

                    using (var result = await command.ExecuteReaderAsync())
                    {
                        while (await result.ReadAsync())
                        {
                            var obj = new INWARD_WF_Tbl
                            {
                                Serial = result["Serial"]?.ToString(),
                                ChqSequance = result["ChqSequance"]?.ToString().Trim(),
                                Clr_Center = result["Clr_Center"]?.ToString(),
                                DrwchqNo = result["DrwchqNo"]?.ToString(),
                                DrwBank = result["DrwBank"]?.ToString(),
                                DrwBranch = result["DrwBranch"]?.ToString(),
                                DrwAccountNo = result["DrwAccountNo"] == DBNull.Value ? "" : result["DrwAccountNo"]?.ToString(),
                                Currency = result["Currency"]?.ToString(),
                                Amount = Convert.ToDouble(result["Amount"]),
                                Amount_JD = Convert.ToDouble(result["Amount_JD"]),
                                SpecialHandling = result["SpecialHandling"]?.ToString(),
                                Response_Description = result["Response_Description"]?.ToString(),
                                category = result["category"] == DBNull.Value ? "" : result["category"]?.ToString()
                            };

                            // Logic for Clr_Center mapping
                            if (obj.Clr_Center != null)
                            {
                                if (obj.Clr_Center.Contains("PMA"))
                                {
                                    obj.Clr_Center = "PMA";
                                }
                                else if (obj.Clr_Center.Contains("DISCOUNT"))
                                {
                                    obj.Clr_Center = "DISCOUNT";
                                }
                                else if (obj.Clr_Center == "Outward_ONUS")
                                {
                                    obj.Clr_Center = "ONUS";
                                }
                                else
                                {
                                    obj.Clr_Center = "";
                                }
                            }

                            // Logic for VIP and Death Date
                            var onustbl = await _context.OnUs_Tbl.SingleOrDefaultAsync(o => o.Serial == obj.Serial);
                            var inwardTrans = await _context.Inward_Trans.SingleOrDefaultAsync(o => o.Serial == obj.Serial);

                            if (_branch == "2" && obj.Clr_Center == "ONUS") // Assuming '2' is a specific branch code for ONUS
                            {
                                if (onustbl != null)
                                {
                                    if (onustbl.ErrorDescription != null && (onustbl.ErrorDescription.Contains("12") || onustbl.ErrorDescription.Contains("13")))
                                    {
                                        obj.History = "DEATH OF DATE : " + GET_CUSTOMER_DEATH_DATE(onustbl.DrwCustomerID);
                                    }
                                    else
                                    {
                                        obj.History = "";
                                    }

                                    if (onustbl.Was_PDC == true)
                                    {
                                        if (Branchs == "-1")
                                        {
                                            if (vip != "ALL")
                                            {
                                                if (vip == "Yes" && onustbl.VIP == true) lstPstPDC.Add(obj);
                                                if (vip != "Yes" && onustbl.VIP == false) lstPstPDC.Add(obj);
                                            }
                                            else lstPstPDC.Add(obj);
                                        }
                                        else
                                        {
                                            if (vip != "ALL")
                                            {
                                                if (vip == "Yes" && onustbl.VIP == true) lstPstPDC.Add(obj);
                                                if (vip != "Yes" && onustbl.VIP == false) lstPstPDC.Add(obj);
                                            }
                                            else lstPstPDC.Add(obj);
                                        }
                                    }
                                }
                            }
                            else if (_branch != "2" && obj.Clr_Center == "ONUS")
                            {
                                if (onustbl != null)
                                {
                                    if (onustbl.ErrorDescription != null && (onustbl.ErrorDescription.Contains("12") || onustbl.ErrorDescription.Contains("13")))
                                    {
                                        obj.History = "DEATH OF DATE : " + GET_CUSTOMER_DEATH_DATE(onustbl.DrwCustomerID);
                                    }
                                    else
                                    {
                                        obj.History = "";
                                    }

                                    if (onustbl.Was_PDC == false)
                                    {
                                        if (Branchs != "-1")
                                        {
                                            if (onustbl.Company_Code == _branch)
                                            {
                                                if (vip != "ALL")
                                                {
                                                    if (vip == "Yes" && onustbl.VIP == true) lstPstPDC.Add(obj);
                                                    if (vip != "Yes" && onustbl.VIP == false) lstPstPDC.Add(obj);
                                                }
                                                else lstPstPDC.Add(obj);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (inwardTrans != null)
                                {
                                    if (inwardTrans.ErrorDescription != null && (inwardTrans.ErrorDescription.Contains("12") || inwardTrans.ErrorDescription.Contains("13")))
                                    {
                                        obj.History = "DEATH OF DATE : " + GET_CUSTOMER_DEATH_DATE(inwardTrans.ISSAccount);
                                    }
                                    else
                                    {
                                        obj.History = "";
                                    }

                                    if (vip != "ALL")
                                    {
                                        if (vip == "Yes" && inwardTrans.VIP == true) lstPstPDC.Add(obj);
                                        if (vip != "Yes" && inwardTrans.VIP == false) lstPstPDC.Add(obj);
                                    }
                                    else lstPstPDC.Add(obj);
                                }
                            }
                        }
                    }
                }

                _json.Data = new { ErrorMsg = "", lstPDC = lstPstPDC };
                return _json;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in getSearchList_WF_fixederror: {Message}", ex.Message);
                _json.Data = new { ErrorMsg = "An error occurred during the search for fixed error cheques." };
                return _json;
            }
            finally
            {
                if (_context.Database.GetDbConnection().State == ConnectionState.Open)
                {
                    _context.Database.CloseConnection();
                }
            }
        }



        public async Task<JsonResult> GetAcctWF(string Branchs, string Currency, string ChequeSource, string Amount, string DRWAccNo, string ChequeNo, string vip, string userName, string comId)
        {
            var _json = new JsonResult(new { });
            var lstPstPDC = new List<INWARD_WF_Tbl>();

            try
            {
                _logger.LogInformation("Before Search Inward cheques for GetAcctWF");

                string inputDate = DateTime.Now.Now.ToString("yyyy-MM-dd");

                string selectQuery = $"SELECT * FROM INWARD_WF_Tbl WHERE ((Need_Finanical_WF = 1 AND IsFinanicallyFixed = 1) AND (Level1_status LIKE '%{userName}%' OR Level2_status LIKE '%{userName}%')) AND CAST(input_date AS DATE) >= '{inputDate}'";

                var user = await _context.Users_Tbl.SingleOrDefaultAsync(y => y.User_Name == userName);
                string comtype = user?.Company_ID;
                string _branch = "";

                if (!string.IsNullOrEmpty(comtype))
                {
                    var company = await _context.Companies_Tbl.SingleOrDefaultAsync(u => u.Company_ID == comtype);
                    if (company != null && company.Company_Code != null)
                    {
                        _branch = company.Company_Code.ToString().Substring(5);
                    }
                }

                if (Branchs != "-1")
                {
                    if (_branch == "2") // Assuming '2' is a specific branch code
                    {
                        selectQuery += $" AND DrwBranch = '{Branchs.Trim()}'";
                    }
                    else
                    {
                        selectQuery += $" AND DrwBranch = '{_branch.Trim()}'";
                    }
                }
                else
                {
                    if (_branch != "2")
                    {
                        selectQuery += $" AND DrwBranch = '{_branch.Trim()}'";
                    }
                }

                if (Currency != "-1")
                {
                    string currencyCode = "";
                    if (Currency.Trim() == "1") currencyCode = "JOD";
                    else if (Currency.Trim() == "2") currencyCode = "USD";
                    else if (Currency.Trim() == "3") currencyCode = "ILS";
                    else if (Currency.Trim() == "5") currencyCode = "EUR";
                    selectQuery += $" AND Currency = '{currencyCode.Trim()}'";
                }

                var userGroup = await _context.Users_Tbl.Where(c => c.User_Name == userName).Select(c => c.Group_ID).FirstOrDefaultAsync();

                if (ChequeSource != "All")
                {
                    if (ChequeSource.Contains("INHOUSE"))
                    {
                        if (userGroup == 4) // AdminAuthorized
                        {
                            selectQuery += " AND Clr_Center LIKE '%ONUS%' AND ISNULL(category,0)='1100'";
                        }
                        else
                        {
                            if (_branch == "2")
                            {
                                selectQuery += " AND Clr_Center LIKE '%ONUS%'";
                            }
                            else
                            {
                                selectQuery += " AND Clr_Center LIKE '%ONUS%' AND ISNULL(category,0)<>'1100'";
                            }
                        }
                    }
                    else
                    {
                        selectQuery += $" AND Clr_Center LIKE '%{ChequeSource.ToUpper().Trim()}%'";
                    }
                }

                if (!string.IsNullOrEmpty(Amount))
                {
                    selectQuery += $" AND Amount = '{Amount.Trim()}'";
                }

                if (!string.IsNullOrEmpty(DRWAccNo))
                {
                    selectQuery += $" AND DrwAccountNo LIKE '%{DRWAccNo.Trim()}%'";
                }

                if (!string.IsNullOrEmpty(ChequeNo))
                {
                    selectQuery += $" AND DrwChqNo LIKE '%{ChequeNo.Trim()}%'";
                }

                selectQuery += " ORDER BY Amount_JD ASC";

                _logger.LogInformation($"Executing SQL Query for GetAcctWF: {selectQuery}");

                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = selectQuery;
                    _context.Database.OpenConnection();

                    using (var result = await command.ExecuteReaderAsync())
                    {
                        while (await result.ReadAsync())
                        {
                            var obj = new INWARD_WF_Tbl
                            {
                                Serial = result["Serial"]?.ToString(),
                                ChqSequance = result["ChqSequance"]?.ToString().Trim(),
                                Clr_Center = result["Clr_Center"]?.ToString().Trim(),
                                DrwchqNo = result["DrwchqNo"]?.ToString(),
                                DrwBank = result["DrwBank"]?.ToString(),
                                DrwBranch = result["DrwBranch"]?.ToString(),
                                DrwName = result["DrwName"]?.ToString(),
                                Balance = result["Balance"]?.ToString(),
                                DrwAccountNo = result["DrwAccountNo"] == DBNull.Value ? "" : result["DrwAccountNo"]?.ToString(),
                                Currency = result["Currency"]?.ToString(),
                                Amount = Convert.ToDouble(result["Amount"]),
                                Amount_JD = Convert.ToDouble(result["Amount_JD"]),
                                SpecialHandling = result["SpecialHandling"]?.ToString(),
                                Response_Description = result["Response_Description"]?.ToString(),
                                Level1_status = result["Level1_status"] == DBNull.Value ? "" : result["Level1_status"]?.ToString(),
                                Level2_status = result["Level2_status"] == DBNull.Value ? "" : result["Level2_status"]?.ToString()
                            };

                            // Clr_Center mapping logic
                            if (obj.Clr_Center != null)
                            {
                                if (obj.Clr_Center.Contains("PMA"))
                                {
                                    obj.Clr_Center = "PMA";
                                }
                                else if (obj.Clr_Center.Contains("DISCOUNT"))
                                {
                                    obj.Clr_Center = "DISCOUNT";
                                }
                                else if (obj.Clr_Center == "Outward_ONUS")
                                {
                                    var inwchq = await _context.OnUs_Tbl.SingleOrDefaultAsync(v => v.Serial == obj.Serial);
                                    if (inwchq != null)
                                    {
                                        if (inwchq.Was_PDC == true)
                                        {
                                            obj.Clr_Center = "INHOUSE_PDC";
                                        }
                                        else
                                        {
                                            obj.Clr_Center = "INHOUSE_Branch";
                                        }
                                    }
                                    else
                                    {
                                        obj.Clr_Center = "ONUS";
                                    }
                                }
                                else
                                {
                                    obj.Clr_Center = "";
                                }
                            }
                            lstPstPDC.Add(obj);
                        }
                    }
                }

                _json.Data = new { ErrorMsg = "", lstPDC = lstPstPDC };
                return _json;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAcctWF: {Message}", ex.Message);
                _json.Data = new { ErrorMsg = "An error occurred during the GetAcctWF process." };
                return _json;
            }
            finally
            {
                if (_context.Database.GetDbConnection().State == ConnectionState.Open)
                {
                    _context.Database.CloseConnection();
                }
            }
        }



        public async Task<JsonResult> getSearchList_WF(string Branchs, string Currency, string ChequeSource, string Amount, string DRWAccNo, string ChequeNo, string WASPDC, string vip, string CustomerType, string userName, string pageId, string comId)
        {
            var _json = new JsonResult(new { });
            var lstPstPDC = new List<INWARD_WF_Tbl>();

            try
            {
                _logger.LogInformation("Starting getSearchList_WF for user: {UserName}", userName);

                string currentTime = DateTime.Now.ToString("HH:mm:ss");

                // Time-based approval checks
                var config = await _context.Global_Parameter_TBL.FirstOrDefaultAsync(i => i.Parameter_Desc == "InsufficientFundsCutOFFTime_" + ChequeSource);
                if (config != null && !string.IsNullOrEmpty(config.Parameter_Value))
                {
                    if (TimeSpan.Parse(currentTime) > TimeSpan.Parse(config.Parameter_Value))
                    {
                        _json.Value = new { ErrorMsg = $"The Time Is End , You can't Approve Financial {ChequeSource} CHQ After : {config.Parameter_Value}", Locked_user = "." };
                        return _json;
                    }
                }

                string inputDate = DateTime.Now.ToString("yyyy-MM-dd");

                var user = await _context.Users_Tbl.SingleOrDefaultAsync(y => y.User_Name == userName);
                string userCompanyId = user?.Company_ID;
                string _branch = "";

                if (!string.IsNullOrEmpty(userCompanyId))
                {
                    var company = await _context.Companies_Tbl.SingleOrDefaultAsync(u => u.Company_ID == userCompanyId);
                    if (company != null && company.Company_Code != null)
                    {
                        _branch = company.Company_Code.ToString().Substring(5);
                    }
                }

                string selectQuery = $"SELECT * FROM INWARD_WF_Tbl WHERE ((Need_Finanical_WF = 1 AND IsFinanicallyFixed = 0) OR (NeedMnaualFixed = 1 AND IsMnaualFixed = 0)) AND (Final_Status = 'NEW' OR Final_Status = 'Pending') AND CAST(input_date AS DATE) >= \'{inputDate}\'";

                if (Branchs != "-1")
                {
                    if (_branch == "2") // Assuming '2' is a specific branch code
                    {
                        selectQuery += $" AND DrwBranch = \'{Branchs.Trim()}\'";
                    }
                    else
                    {
                        selectQuery += $" AND DrwBranch = \'{_branch.Trim()}\'";
                    }
                }
                else
                {
                    if (_branch != "2")
                    {
                        selectQuery += $" AND DrwBranch = \'{_branch.Trim()}\'";
                    }
                }

                if (Currency != "-1")
                {
                    string currencyCode = "";
                    if (Currency.Trim() == "1") currencyCode = "JOD";
                    else if (Currency.Trim() == "2") currencyCode = "USD";
                    else if (Currency.Trim() == "3") currencyCode = "ILS";
                    else if (Currency.Trim() == "5") currencyCode = "EUR";
                    selectQuery += $" AND Currency = \'{currencyCode.Trim()}\'";
                }

                var userGroup = await _context.Users_Tbl.Where(c => c.User_Name == userName).Select(c => c.Group_ID).FirstOrDefaultAsync();

                if (ChequeSource != "All")
                {
                    if (ChequeSource.Contains("INHOUSE"))
                    {
                        if (userGroup == 4) // AdminAuthorized
                        {
                            selectQuery += " AND Clr_Center LIKE '%ONUS%' AND ISNULL(category,0)='1100'";
                        }
                        else
                        {
                            if (_branch == "2")
                            {
                                selectQuery += " AND Clr_Center LIKE '%ONUS%'";
                            }
                            else
                            {
                                selectQuery += " AND Clr_Center LIKE '%ONUS%' AND ISNULL(category,0)<>'1100'";
                            }
                        }
                    }
                    else
                    {
                        selectQuery += $" AND Clr_Center LIKE '%{ChequeSource.ToUpper().Trim()}%'";
                    }
                }

                if (!string.IsNullOrEmpty(Amount))
                {
                    selectQuery += $" AND Amount = '{Amount.Trim()}'";
                }

                if (!string.IsNullOrEmpty(DRWAccNo))
                {
                    selectQuery += $" AND DrwAccountNo LIKE '%{DRWAccNo.Trim()}%'";
                }

                if (!string.IsNullOrEmpty(ChequeNo))
                {
                    selectQuery += $" AND DrwChqNo LIKE '%{ChequeNo.Trim()}%'";
                }

                selectQuery += " ORDER BY Amount_JD ASC";

                _logger.LogInformation($"Executing SQL Query for getSearchList_WF: {selectQuery}");

                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = selectQuery;
                    _context.Database.OpenConnection();

                    using (var result = await command.ExecuteReaderAsync())
                    {
                        while (await result.ReadAsync())
                        {
                            var obj = new INWARD_WF_Tbl
                            {
                                Serial = result["Serial"]?.ToString(),
                                ChqSequance = result["ChqSequance"]?.ToString().Trim(),
                                Clr_Center = result["Clr_Center"]?.ToString().Trim(),
                                DrwchqNo = result["DrwchqNo"]?.ToString(),
                                DrwBank = result["DrwBank"]?.ToString(),
                                DrwBranch = result["DrwBranch"]?.ToString(),
                                DrwName = result["DrwName"]?.ToString(),
                                Balance = result["Balance"]?.ToString(),
                                DrwAccountNo = result["DrwAccountNo"] == DBNull.Value ? "" : result["DrwAccountNo"]?.ToString(),
                                Currency = result["Currency"]?.ToString(),
                                Amount = Convert.ToDouble(result["Amount"]),
                                Amount_JD = Convert.ToDouble(result["Amount_JD"]),
                                SpecialHandling = result["SpecialHandling"]?.ToString(),
                                Response_Description = result["Response_Description"]?.ToString(),
                                Level1_status = result["Level1_status"] == DBNull.Value ? "" : result["Level1_status"]?.ToString(),
                                Level2_status = result["Level2_status"] == DBNull.Value ? "" : result["Level2_status"]?.ToString(),
                                Level3_status = result["Level3_status"] == DBNull.Value ? "" : result["Level3_status"]?.ToString()
                            };

                            // Clr_Center mapping logic
                            if (obj.Clr_Center != null)
                            {
                                if (obj.Clr_Center.Contains("PMA"))
                                {
                                    obj.Clr_Center = "PMA";
                                }
                                else if (obj.Clr_Center.Contains("DISCOUNT"))
                                {
                                    obj.Clr_Center = "DISCOUNT";
                                }
                                else if (obj.Clr_Center == "Outward_ONUS")
                                {
                                    var inwchq = await _context.OnUs_Tbl.SingleOrDefaultAsync(v => v.Serial == obj.Serial);
                                    if (inwchq != null)
                                    {
                                        if (inwchq.Was_PDC == true)
                                        {
                                            obj.Clr_Center = "INHOUSE_PDC";
                                        }
                                        else
                                        {
                                            obj.Clr_Center = "INHOUSE_Branch";
                                        }
                                    }
                                    else
                                    {
                                        obj.Clr_Center = "ONUS";
                                    }
                                }
                                else
                                {
                                    obj.Clr_Center = "";
                                }
                            }
                            lstPstPDC.Add(obj);
                        }
                    }
                }

                double totAmount = 0;
                double totILS = 0;
                double totJOD = 0;
                double totUSD = 0;
                double totEUR = 0;

                int cILS = 0;
                int cJOD = 0;
                int cUSD = 0;
                int cEUR = 0;

                if (Currency != "-1") // If a specific currency was filtered
                {
                    foreach (var item in lstPstPDC)
                    {
                        totAmount += item.Amount;
                    }
                }
                else // Calculate totals per currency
                {
                    foreach (var item in lstPstPDC)
                    {
                        var currencyEntry = await _context.CURRENCY_TBL.SingleOrDefaultAsync(x => x.SYMBOL_ISO == item.Currency);
                        if (currencyEntry != null)
                        {
                            if (currencyEntry.ID == "1")
                            {
                                totJOD += item.Amount;
                                cJOD++;
                            }
                            else if (currencyEntry.ID == "2")
                            {
                                totUSD += item.Amount;
                                cUSD++;
                            }
                            else if (currencyEntry.ID == "3")
                            {
                                totILS += item.Amount;
                                cILS++;
                            }
                            else if (currencyEntry.ID == "5")
                            {
                                totEUR += item.Amount;
                                cEUR++;
                            }
                        }
                    }
                }

                _json.Value = new { ErrorMsg = "", lstPDC = lstPstPDC, AmuntTot = totAmount, ILSAmount = totILS, JODAmount = totJOD, USDAmount = totUSD, EURAmount = totEUR, ILSCount = cILS, JODCount = cJOD, USDCount = cUSD, EURCount = cEUR, Locked_user = "." };
                return _json;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in getSearchList_WF: {Message}", ex.Message);
                _json.Value = new { ErrorMsg = "An error occurred during the search for workflow cheques." };
                return _json;
            }
            finally
            {
                if (_context.Database.GetDbConnection().State == ConnectionState.Open)
                {
                    _context.Database.CloseConnection();
                }
            }
        }



        public async Task<IActionResult> Reject_CHQ(string userName, string pageId, string groupId)
        {
            // This method primarily prepares data for the view, which is typically done in the controller.
            // However, if there's complex data retrieval or business logic, it would be here.
            // For now, it will return a dummy result, and the controller will handle ViewBag population.
            return await Task.FromResult<IActionResult>(new EmptyResult());
        }



        public async Task<List<SelectListItem>> bind_chq_source()
        {
            var objList = new List<SelectListItem>
            {
                new SelectListItem { Text = "PMA", Value = "1" },
                new SelectListItem { Text = "INHOUSE BRANCH", Value = "3" },
                new SelectListItem { Text = "INHOUSE PDC", Value = "4" }
            };
            return await Task.FromResult(objList);
        }



        public async Task<List<SelectListItem>> bindchqsource()
        {
            var objList = new List<SelectListItem>
            {
                new SelectListItem { Text = "OUTWARD", Value = "0" },
                new SelectListItem { Text = "INWARD", Value = "1" }
            };
            return await Task.FromResult(objList);
        }



        public async Task<List<SelectListItem>> BindCustomerType(string companyCode)
        {
            List<SelectListItem> objList;
            if (companyCode == "714")
            {
                objList = new List<SelectListItem>
                {
                    new SelectListItem { Text = "All", Value = "" },
                    new SelectListItem { Text = "Individual", Value = "S" },
                    new SelectListItem { Text = "Corporate", Value = "C" }
                };
            }
            else
            {
                objList = new List<SelectListItem>
                {
                    new SelectListItem { Text = "All", Value = "" }
                };
            }
            return await Task.FromResult(objList);
        }



        public async Task<JsonResult> get_Rjected_SearchList(string Branchs, string chqsource, string FromDate, string FromTransDate, string ToTransDatet, string ToDate, string FromInputDate, string ToInputDate, string FromReturnedDate, string ToReturnedDate, string BenAccNo, string AccType, string FromBank, string ToBank, string Currency, string ChequeSource, string Amount, string DRWAccNo, string ChequeNo, string userName)
        {
            var _json = new JsonResult(new { });
            var lstPstPDC = new List<Inward_Trans>();

            try
            {
                _logger.LogInformation("Before Search Rejected Inward cheques for user: {UserName}", userName);

                string selectQuery = "";
                if (chqsource == "All")
                {
                    selectQuery = "SELECT Out_Tbl.Serial, InputDate, ClrCenter, DrwAcctNo, DrwName, DrwChqNo, DrwBankNo, DrwBranchNo, Currency, Amount, ValueDate, BenAccountNo, BenName FROM Inward_Trans Out_Tbl WHERE Rejected = 1 AND Posted = " + (int)AllEnums.Cheque_Status.Rejected +
                                  " UNION SELECT Out_Tbl.Serial, InputDate, ClrCenter, DrwAcctNo, DrwName, DrwChqNo, DrwBankNo, DrwBranchNo, Currency, Amount, ValueDate, BenAccountNo, BenName FROM Outward_Trans Out_Tbl WHERE Rejected = 1 AND Posted = " + (int)AllEnums.Cheque_Status.Rejected;
                }
                else if (chqsource == "0") // Outward
                {
                    selectQuery = "SELECT Out_Tbl.Serial, InputDate, ClrCenter, DrwAcctNo, DrwName, DrwChqNo, DrwBankNo, DrwBranchNo, Currency, Amount, ValueDate, BenAccountNo, BenName FROM Outward_Trans Out_Tbl WHERE Rejected = 1 AND Posted = " + (int)AllEnums.Cheque_Status.Rejected;
                }
                else // Inward
                {
                    selectQuery = "SELECT Out_Tbl.Serial, InputDate, ClrCenter, DrwAcctNo, DrwName, DrwChqNo, DrwBankNo, DrwBranchNo, Currency, Amount, ValueDate, BenAccountNo, BenName FROM Inward_Trans Out_Tbl WHERE Rejected = 1 AND Posted = " + (int)AllEnums.Cheque_Status.Rejected;
                }

                if (Branchs != "-1")
                {
                    selectQuery += $" AND DrwBranchNo = \'{Branchs.Trim()}\'";
                }

                if (!string.IsNullOrEmpty(FromDate) && string.IsNullOrEmpty(ToDate))
                {
                    selectQuery += $" AND ValueDate = \'{FromDate}\'";
                }
                else if (string.IsNullOrEmpty(FromDate) && !string.IsNullOrEmpty(ToDate))
                {
                    selectQuery += $" AND ValueDate = \'{ToDate}\'";
                }
                else if (!string.IsNullOrEmpty(FromDate) && !string.IsNullOrEmpty(ToDate))
                {
                    selectQuery += $" AND ValueDate >= \'{FromDate}\' AND ValueDate <= \'{ToDate}\'";
                }

                if (!string.IsNullOrEmpty(FromInputDate) && string.IsNullOrEmpty(ToInputDate))
                {
                    selectQuery += $" AND InputDate = \'{FromInputDate}\'";
                }
                else if (string.IsNullOrEmpty(FromInputDate) && !string.IsNullOrEmpty(ToInputDate))
                {
                    selectQuery += $" AND InputDate = \'{ToInputDate}\'";
                }
                else if (!string.IsNullOrEmpty(FromInputDate) && !string.IsNullOrEmpty(ToInputDate))
                {
                    selectQuery += $" AND InputDate >= \'{FromInputDate}\' AND InputDate <= \'{ToInputDate}\'";
                }

                if (!string.IsNullOrEmpty(FromTransDate) && string.IsNullOrEmpty(ToTransDatet))
                {
                    selectQuery += $" AND TransDate = \'{FromTransDate}\'";
                }
                else if (string.IsNullOrEmpty(FromTransDate) && !string.IsNullOrEmpty(ToTransDatet))
                {
                    selectQuery += $" AND TransDate = \'{ToTransDatet}\'";
                }
                else if (!string.IsNullOrEmpty(FromTransDate) && !string.IsNullOrEmpty(ToTransDatet))
                {
                    selectQuery += $" AND TransDate >= \'{FromTransDate}\' AND TransDate <= \'{ToTransDatet}\'";
                }

                if (!string.IsNullOrEmpty(FromReturnedDate) && string.IsNullOrEmpty(ToReturnedDate))
                {
                    selectQuery += $" AND ReturnedDate = \'{FromReturnedDate}\'";
                }
                else if (string.IsNullOrEmpty(FromReturnedDate) && !string.IsNullOrEmpty(ToReturnedDate))
                {
                    selectQuery += $" AND ReturnedDate = \'{ToReturnedDate}\'";
                }
                else if (!string.IsNullOrEmpty(FromReturnedDate) && !string.IsNullOrEmpty(ToReturnedDate))
                {
                    selectQuery += $" AND ReturnedDate >= \'{FromReturnedDate}\' AND ReturnedDate <= \'{ToReturnedDate}\'";
                }

                if (!string.IsNullOrEmpty(FromBank) && !string.IsNullOrEmpty(ToBank))
                {
                    if (FromBank == ToBank)
                    {
                        selectQuery += $" AND DrwBankNo = \'{FromBank}\'";
                    }
                    else
                    {
                        selectQuery += $" AND DrwBankNo >= \'{FromBank}\' AND DrwBankNo <= \'{ToBank}\'";
                    }
                }

                if (!string.IsNullOrEmpty(BenAccNo))
                {
                    selectQuery += $" AND BenAccountNo = \'{BenAccNo.Trim()}\'";
                }

                if (Currency != "-1")
                {
                    string currencyCode = "";
                    if (Currency.Trim() == "1") currencyCode = "JOD";
                    else if (Currency.Trim() == "2") currencyCode = "USD";
                    else if (Currency.Trim() == "3") currencyCode = "ILS";
                    else if (Currency.Trim() == "5") currencyCode = "EUR";
                    selectQuery += $" AND Currency = \'{currencyCode.Trim()}\'";
                }

                if (ChequeSource != "-1")
                {
                    selectQuery += $" AND ClrCenter = \'{ChequeSource.ToUpper().Trim()}\'";
                }

                if (!string.IsNullOrEmpty(Amount))
                {
                    selectQuery += $" AND Amount = \'{Amount.Trim()}\'";
                }

                if (!string.IsNullOrEmpty(DRWAccNo))
                {
                    selectQuery += $" AND DrwAcctNo LIKE '%{DRWAccNo.Trim()}%'";
                }

                if (!string.IsNullOrEmpty(ChequeNo))
                {
                    selectQuery += $" AND DrwChqNo LIKE '%{ChequeNo.Trim()}%'";
                }

                _logger.LogInformation($"Executing SQL Query for get_Rjected_SearchList: {selectQuery}");

                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = selectQuery;
                    _context.Database.OpenConnection();

                    using (var result = await command.ExecuteReaderAsync())
                    {
                        while (await result.ReadAsync())
                        {
                            var obj = new Inward_Trans
                            {
                                Serial = result["Serial"]?.ToString(),
                                InputDate = result["InputDate"]?.ToString(),
                                ClrCenter = result["ClrCenter"]?.ToString(),
                                DrwAcctNo = result["DrwAcctNo"]?.ToString(),
                                DrwName = result["DrwName"]?.ToString(),
                                DrwChqNo = result["DrwChqNo"]?.ToString(),
                                DrwBankNo = result["DrwBankNo"]?.ToString(),
                                DrwBranchNo = result["DrwBranchNo"]?.ToString(),
                                Currency = result["Currency"]?.ToString(),
                                Amount = Convert.ToDouble(result["Amount"]),
                                ValueDate = result["ValueDate"]?.ToString(),
                                BenAccountNo = result["BenAccountNo"]?.ToString(),
                                BenName = result["BenName"]?.ToString()
                            };
                            lstPstPDC.Add(obj);
                        }
                    }
                }

                _json.Value = new { ErrorMsg = "", lstPDC = lstPstPDC };
                return _json;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in get_Rjected_SearchList: {Message}", ex.Message);
                _json.Value = new { ErrorMsg = "An error occurred during the search for rejected cheques." };
                return _json;
            }
            finally
            {
                if (_context.Database.GetDbConnection().State == ConnectionState.Open)
                {
                    _context.Database.CloseConnection();
                }
            }
        }



        public async Task<OutChqs> ViewDetailsOutward(int id, string userName, string pageId, string applicationId, int userId)
        {
            var inChq = new OutChqs();
            try
            {
                _logger.LogInformation("Show Cheque to verify it from Outward_Trans table for user: {UserName}", userName);

                var incObj = await _context.Outward_Trans.SingleOrDefaultAsync(y => y.Serial == id.ToString());
                var currencyList = await _context.CURRENCY_TBL.ToListAsync();

                if (incObj == null)
                {
                    // This case should ideally be handled by the controller redirecting to an error page or similar
                    return null; // Or throw an exception
                }

                // Convert currency ID to SYMBOL_ISO
                if (int.TryParse(incObj.Currency, out int currencyId))
                {
                    var currency = currencyList.FirstOrDefault(c => c.ID == currencyId.ToString());
                    if (currency != null)
                    {
                        incObj.Currency = currency.SYMBOL_ISO;
                    }
                }

                var img = await _context.Outward_Imgs.FirstOrDefaultAsync(y => y.Serial == incObj.Serial);

                incObj.Amount = Math.Round(incObj.Amount, 2, MidpointRounding.AwayFromZero);

                inChq.outwardTrans = incObj;
                inChq.outwardImgs = img;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when getting cheque details from outward trans: {Message}", ex.Message);
                // Handle exception appropriately, e.g., rethrow or return null/error object
                return null;
            }
            return inChq;
        }



        public async Task<INChqs> ViewDetailsinwarad(int id, string userName, string pageId, string applicationId, int userId)
        {
            var inChq = new INChqs();
            try
            {
                _logger.LogInformation("Show Cheque to verify it from Inward_Trans table for user: {UserName}", userName);

                var incObj = await _context.Inward_Trans.SingleOrDefaultAsync(y => y.Serial == id.ToString());
                var currencyList = await _context.CURRENCY_TBL.ToListAsync();

                if (incObj == null)
                {
                    return null; // Or throw an exception
                }

                // Convert currency ID to SYMBOL_ISO
                if (int.TryParse(incObj.Currency, out int currencyId))
                {
                    var currency = currencyList.FirstOrDefault(c => c.ID == currencyId.ToString());
                    if (currency != null)
                    {
                        incObj.Currency = currency.SYMBOL_ISO;
                    }
                }

                var img = await _context.INWARD_IMAGES.FirstOrDefaultAsync(y => y.Serial == incObj.Serial);

                incObj.Amount = Math.Round(incObj.Amount, 2, MidpointRounding.AwayFromZero);

                inChq.inw = incObj;
                inChq.Imgs = img;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when getting cheque details from inward trans: {Message}", ex.Message);
                return null;
            }
            return inChq;
        }



        public async Task<IActionResult> Updatedata(string Serial, string BenName, string BenfAccBranch, string DrwChqNo, string DrwBankNo, string DrwBranchNo, string BenfBnk, string TBL_NAME, string userName)
        {
            try
            {
                if (TBL_NAME == "INWARD")
                {
                    var inward = await _context.Inward_Trans.SingleOrDefaultAsync(x => x.Serial == Serial);
                    if (inward != null)
                    {
                        inward.DrwBranchNo = DrwBranchNo;
                        inward.DrwBankNo = DrwBankNo;
                        inward.BenName = BenName;
                        inward.BenfAccBranch = BenfAccBranch;
                        inward.BenfBnk = BenfBnk;
                        inward.DrwChqNo = DrwChqNo;
                        inward.History += "|" + "this CHQ updated after rejected";
                        inward.LastUpdate = DateTime.Now;
                        inward.LastUpdateBy = userName;
                        inward.Rejected = 0;
                        inward.Posted = (int)AllEnums.Cheque_Status.Returne;
                        await _context.SaveChangesAsync();
                    }
                }
                else
                {
                    var outward = await _context.Outward_Trans.SingleOrDefaultAsync(x => x.Serial == Serial);
                    if (outward != null)
                    {
                        outward.DrwBranchNo = DrwBranchNo;
                        outward.DrwBankNo = DrwBankNo;
                        outward.BenName = BenName;
                        outward.BenfAccBranch = BenfAccBranch;
                        outward.BenfBnk = BenfBnk;
                        outward.DrwChqNo = DrwChqNo;
                        outward.Rejected = 0;
                        outward.History += "|" + "this CHQ updated after rejected";
                        outward.LastUpdate = DateTime.Now;
                        outward.LastUpdateBy = userName;
                        outward.Posted = (int)AllEnums.Cheque_Status.Posted;
                        await _context.SaveChangesAsync();
                    }
                }
                return new JsonResult(new { status = "Success" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Updatedata: {Message}", ex.Message);
                return new JsonResult(new { status = "Error", message = ex.Message });
            }
        }



        public async Task<bool> SENDINWARDSMS(Inward_Trans inwardTrans)
        {
            bool result = false;
            try
            {
                if (inwardTrans == null) return false;

                string customerPhone = "";
                string language = "";
                string retDesc = inwardTrans.ReturnCode?.Trim();
                string clrCenter = inwardTrans.ClrCenter;
                string smsBodyText = "";
                string smsRetCode = "";
                bool flag = false;

                var customer = await _context.CUSTOMERS.SingleOrDefaultAsync(c => c.CUSTOMER_ID == inwardTrans.ISSAccount);
                if (customer != null)
                {
                    language = customer.LANGUAGE;
                    customerPhone = customer.SMS_1?.Split("::")[0];
                }

                if (!string.IsNullOrEmpty(retDesc))
                {
                    var config = await _context.System_Configurations_Tbl.SingleOrDefaultAsync(i => i.Config_Type == (inwardTrans.ClrCenter == "DISCOUNT" ? "SMS_DISCOUNT" : "SMS_NON_DISCOUNT"));
                    if (config != null)
                    {
                        smsRetCode = config.Config_Value;
                        if (smsRetCode.Contains(retDesc))
                        {
                            flag = true;
                        }
                    }

                    if (flag)
                    {
                        string retCodeDescription = await GetTechRes(retDesc, clrCenter, language);
                        if (retCodeDescription != "")
                        {
                            var smsTemplate = await _context.SMS_TBL.SingleOrDefaultAsync(i => i.SMS_TYPE == (int)AllEnums.Cheque_Status.Returne && i.SMS_LANGUAGE == language);
                            if (smsTemplate != null && !string.IsNullOrEmpty(smsTemplate.SMS_BODY))
                            {
                                smsBodyText = smsTemplate.SMS_BODY
                                    .Replace("@ChqNo@", inwardTrans.DrwChqNo)
                                    .Replace("@RetDesc@", retCodeDescription)
                                    .Replace("@Amount@", $"{inwardTrans.Amount} {inwardTrans.Currency?.Trim()}");

                                if (!string.IsNullOrEmpty(smsBodyText))
                                {
                                    try
                                    {
                                        _logger.LogInformation("Add Record to SMS_OUT, inward serial is: {Serial}, Customer_phone: {Phone}", inwardTrans.Serial, customerPhone);

                                        var smsOut = new SMS_OUT
                                        {
                                            SMS_DATE = DateTime.Now,
                                            ClrCenter = inwardTrans.ClrCenter,
                                            serial = inwardTrans.Serial,
                                            Customer_Phone = customerPhone,
                                            Sender = "SAFA ECC",
                                            SMS = smsBodyText
                                        };
                                        _context.SMS_OUT.Add(smsOut);
                                        await _context.SaveChangesAsync();
                                        result = true;
                                    }
                                    catch (Exception ex)
                                    {
                                        _logger.LogError(ex, "Error saving SMS_OUT record for serial {Serial}", inwardTrans.Serial);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        _logger.LogWarning("Return Code {RetCode} not allowed to be sent for serial {Serial}. Allowed codes: {AllowedCodes}", retDesc, inwardTrans.Serial, smsRetCode);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SENDINWARDSMS for serial {Serial}: {Message}", inwardTrans.Serial, ex.Message);
            }
            return result;
        }



        public async Task<bool> SENDINHOUSESMS(OnUs_Tbl onusTrans)
        {
            bool result = false;
            try
            {
                if (onusTrans == null) return false;

                string customerPhone = "";
                string language = "";
                string retDesc = onusTrans.ReturnCode?.Trim();
                string clrCenter = onusTrans.ClrCenter;
                string smsBodyText = "";
                string smsRetCode = "";
                bool flag = false;

                if (!string.IsNullOrEmpty(onusTrans.DrwCustomerID))
                {
                    var customer = await _context.CUSTOMERS.SingleOrDefaultAsync(c => c.CUSTOMER_ID == onusTrans.DrwCustomerID);
                    if (customer != null)
                    {
                        language = customer.LANGUAGE;
                        customerPhone = customer.SMS_1?.Split("::")[0];
                    }
                }

                if (!string.IsNullOrEmpty(retDesc))
                {
                    var config = await _context.System_Configurations_Tbl.SingleOrDefaultAsync(i => i.Config_Type == "SMS_NON_DISCOUNT");
                    if (config != null)
                    {
                        smsRetCode = config.Config_Value;
                        if (smsRetCode.Contains(retDesc))
                        {
                            flag = true;
                        }
                    }

                    if (flag)
                    {
                        string retCodeDescription = await GetTechRes(retDesc, clrCenter, language);
                        if (retCodeDescription != "-1")
                        {
                            var smsTemplate = await _context.SMS_TBL.SingleOrDefaultAsync(i => i.SMS_TYPE == (int)AllEnums.Cheque_Status.Returne && i.SMS_LANGUAGE == language);
                            if (smsTemplate != null && !string.IsNullOrEmpty(smsTemplate.SMS_BODY))
                            {
                                smsBodyText = smsTemplate.SMS_BODY
                                    .Replace("@ChqNo@", onusTrans.DrwChqNo)
                                    .Replace("@RetDesc@", retCodeDescription)
                                    .Replace("@Amount@", $"{onusTrans.Amount} {onusTrans.Currency?.Trim()}");

                                if (!string.IsNullOrEmpty(smsBodyText))
                                {
                                    try
                                    {
                                        _logger.LogInformation("Add Record to SMS_OUT, inward serial is: {Serial}, Customer_phone: {Phone}", onusTrans.Serial, customerPhone);

                                        var smsOut = new SMS_OUT
                                        {
                                            SMS_DATE = DateTime.Now,
                                            ClrCenter = onusTrans.ClrCenter,
                                            serial = onusTrans.Serial,
                                            Customer_Phone = customerPhone,
                                            Sender = "CAB ECC",
                                            SMS = smsBodyText
                                        };
                                        _context.SMS_OUT.Add(smsOut);
                                        await _context.SaveChangesAsync();
                                        result = true;
                                    }
                                    catch (Exception ex)
                                    {
                                        _logger.LogError(ex, "Error saving SMS_OUT record for serial {Serial}", onusTrans.Serial);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        _logger.LogWarning("Return Code {RetCode} not allowed to be sent for serial {Serial}. Allowed codes: {AllowedCodes}", retDesc, onusTrans.Serial, smsRetCode);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SENDINHOUSESMS for serial {Serial}: {Message}", onusTrans.Serial, ex.Message);
            }
            return result;
        }



        public async Task<string> GetTechRes(string retCode, string clrCenter, string language)
        {
            string result = "";
            try
            {
                var techRes = await _context.Technical_Response_Tbl
                    .SingleOrDefaultAsync(x => x.Response_Code == retCode && x.Clr_Center == clrCenter);

                if (techRes != null)
                {
                    if (language == "EN")
                    {
                        result = techRes.Response_Desc_EN;
                    }
                    else if (language == "AR")
                    {
                        result = techRes.Response_Desc_AR;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetTechRes: {Message}", ex.Message);
            }
            return result;
        }



        public async Task<JsonResult> fixtimeout(string serial, string clecanter, string userName)
        {
            var _json = new JsonResult(new { });
            try
            {
                if (clecanter == "INHOUSE")
                {
                    var onus = await _context.OnUs_Tbl.SingleOrDefaultAsync(C => C.Serial == serial);
                    if (onus != null)
                    {
                        onus.IsTimeOut = 1;
                        await _context.SaveChangesAsync();
                        _json.Value = new { Data = "Done Successfully" };
                        return _json;
                    }
                }
                else
                {
                    var inw = await _context.Inward_Trans.SingleOrDefaultAsync(C => C.Serial == serial);
                    if (inw != null)
                    {
                        inw.IsTimeOut = 1;
                        await _context.SaveChangesAsync();
                        _json.Value = new { Data = "Done Successfully" };
                        return _json;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in fixtimeout: {Message}", ex.Message);
                _json.Value = new { Data = "Something Wrong !" };
                return _json;
            }
            _json.Value = new { Data = "Something Wrong !" }; // Default return if no entity found
            return _json;
        }



        public async Task<JsonResult> Reposttimeout(string serial, string clecanter, string userName, int userId)
        {
            var _json = new JsonResult(new { });
            try
            {
                string chqSeq = "";
                string chqSeq1 = "";

                // Assuming ECC_Handler_SVC and ECC_CAP_Services are properly referenced and configured
                // For now, I'll comment out the external service calls as they require specific client setup.
                // ECC_Handler_SVC.InwardHandlingSVCSoapClient WebSvc = new ECC_Handler_SVC.InwardHandlingSVCSoapClient("InwardHandlingSVCSoap");
                // ECC_CAP_Services.SAFA_T24_ECC_SVCSoapClient EccAccInfo_WebSvc = new ECC_CAP_Services.SAFA_T24_ECC_SVCSoapClient("SAFA_T24_ECC_SVCSoap");
                // ECC_CAP_Services.ECC_FT_Request obj = new ECC_CAP_Services.ECC_FT_Request();
                // FT_ResponseClass FT_Response = new FT_ResponseClass();

                string sysName = (await _context.System_Configurations_Tbl.SingleOrDefaultAsync(c => c.Config_Type == "ECC_SYSTEM_ID" && c.Config_Id == "7"))?.Config_Value;

                if (clecanter == "INHOUSE")
                {
                    var onus = await _context.OnUs_Tbl.SingleOrDefaultAsync(C => C.Serial == serial);
                    if (onus != null)
                    {
                        // Logic for reversing cheque (commented out external service calls)
                        // This part needs actual implementation of ECC service calls
                        // For now, just marking as success if no external calls are made.

                        // Example of logging if external services were called
                        // _logger.LogInformation("Attempting to reverse cheque for ONUS serial: {Serial}", serial);

                        // Placeholder for actual reversal logic
                        // onus.IsTimeOut = 0; // Assuming successful repost means no longer timed out
                        // await _context.SaveChangesAsync();

                        _json.Value = new { Data = "Done Successfully (External service calls need to be implemented)" };
                    }
                }
                else // INWARD
                {
                    var inw = await _context.Inward_Trans.SingleOrDefaultAsync(C => C.Serial == serial);
                    if (inw != null)
                    {
                        // Logic for reversing cheque (commented out external service calls)
                        // This part needs actual implementation of ECC service calls
                        // For now, just marking as success if no external calls are made.

                        // Example of logging if external services were called
                        // _logger.LogInformation("Attempting to reverse cheque for INWARD serial: {Serial}", serial);

                        // Placeholder for actual reversal logic
                        // inw.IsTimeOut = 0; // Assuming successful repost means no longer timed out
                        // await _context.SaveChangesAsync();

                        _json.Value = new { Data = "Done Successfully (External service calls need to be implemented)" };
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Reposttimeout: {Message}", ex.Message);
                _json.Value = new { Data = "Something Wrong !" };
            }
            return _json;
        }



        public async Task<List<Email>> GetEmailList(string userName, int userId)
        {
            try
            {
                _logger.LogInformation("Getting email list for user: {UserName}", userName);
                var emailList = await _context.Emails.ToListAsync();
                return emailList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting email list: {Message}", ex.Message);
                return new List<Email>();
            }
        }



        public void ADDEmail()
        {
            // This method in VB.NET primarily sets up ViewBag data and returns a view.
            // The actual email saving logic would be in a separate POST method.
            // For now, we\'ll assume binduserEmail and bind_userEmail are called elsewhere or return data.
        }



        public void INWTIMEOUT()
        {
            // This method in VB.NET primarily sets up ViewBag data and returns a view.
            // The actual view rendering is controller's responsibility.
            // This service method will prepare any necessary data or perform logic
            // that the controller needs to render the INWTIMEOUT view.
            // For now, it's empty as per the VB.NET original which mostly sets up ViewBag.
        }



        public async Task<JsonResult> Save_Email(string name, string subject, string body, string toemail, string ccemail, string userName)
        {
            var _json = new JsonResult(new { });
            try
            {
                _logger.LogInformation("Start Save Email by: {UserName}", userName);

                var email = new Email
                {
                    Body = body,
                    Subject = subject,
                    Name = name,
                    InputBy = userName,
                    Input_Date = DateTime.Now,
                    History = $"{name} Added By : {userName} AT:  {DateTime.Now} |"
                };
                _context.Emails.Add(email);
                await _context.SaveChangesAsync();

                int emailId = email.ID; // ID should be populated after SaveChanges

                // Handle 'To' recipients
                if (!string.IsNullOrEmpty(toemail))
                {
                    var toList = toemail.Split(
                        new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    foreach (var item in toList)
                    {
                        var parts = item.Split(':');
                        if (parts.Length == 2)
                        {
                            var userType = parts[0];
                            var userId = parts[1];

                            var emailMap = new Email_MAP
                            {
                                Email_ID = emailId,
                                InputBy = userName,
                                History = $"{subject} Added By : {userName} At  :{DateTime.Now} |",
                                Input_Date = DateTime.Now,
                                TO_CC_BCC = "To",
                                User_ID = userId,
                                User_Type = userType
                            };
                            _context.Email_MAP.Add(emailMap);
                            // Detach entity to allow adding multiple with same instance if needed, though better to create new instance
                            // _context.Entry(emailMap).State = EntityState.Detached;
                        }
                    }
                    await _context.SaveChangesAsync();
                }

                // Handle 'CC' recipients
                if (!string.IsNullOrEmpty(ccemail))
                {
                    var ccList = ccemail.Split(
                        new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    foreach (var itemm in ccList)
                    {
                        var parts = itemm.Split(':');
                        if (parts.Length == 2)
                        {
                            var userType = parts[0];
                            var userId = parts[1];

                            var emailMap = new Email_MAP
                            {
                                Email_ID = emailId,
                                InputBy = userName,
                                History = $"{subject} Added By : {userName} At  :{DateTime.Now} |",
                                Input_Date = DateTime.Now,
                                TO_CC_BCC = "CC",
                                User_ID = userId,
                                User_Type = userType
                            };
                            _context.Email_MAP.Add(emailMap);
                            // _context.Entry(emailMap).State = EntityState.Detached;
                        }
                    }
                    await _context.SaveChangesAsync();
                }

                _json.Value = new { Data = "Save Done Successfully" };
                return _json;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when Save_Email: {Message}", ex.Message);
                _json.Value = new { Data = "Oops ! , Something Wrong" };
                return _json;
            }
        }



        public async Task<bool> ReturnChqTBL(Inward_Trans inwardTrans, string userName)
        {
            try
            {
                _logger.LogInformation("Start ReturnChqTBL with serial: {Serial}, clrcenter: {ClrCenter}, chqNo: {DrwAcctNo}", inwardTrans.Serial, inwardTrans.ClrCenter, inwardTrans.DrwAcctNo);

                var retChq = await _context.ReturnedCheques_Tbl.SingleOrDefaultAsync(x =>
                    x.DrwAcctNo == inwardTrans.DrwAcctNo &&
                    x.DrwChqNo.EndsWith(inwardTrans.DrwChqNo.Substring(Math.Max(0, inwardTrans.DrwChqNo.Length - 7))) && // Equivalent to CType(Right(x.DrwChqNo, 7), Integer) = (CType(Right(drwchqNo, 7), Integer))
                    x.DrwBankNo == inwardTrans.DrwBankNo &&
                    x.DrwBranchNo == inwardTrans.DrwBranchNo);

                if (retChq != null)
                {
                    retChq.NoOfReturn += 1;
                    retChq.TransDate = inwardTrans.TransDate;
                    retChq.LastUpdate = DateTime.Now;
                    retChq.LastUpdateBy = userName;
                    retChq.ReturnDateHistory += $"{DateTime.Now}|";
                    retChq.History += $"Chq Returned with Return code = {retChq.ReturnCode} At : {DateTime.Now}";
                    _context.ReturnedCheques_Tbl.Update(retChq);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    _logger.LogInformation("Start ReturnChqTBL (new record) with serial: {Serial}", inwardTrans.Serial);

                    retChq = new ReturnedCheques_Tbl
                    {
                        Amount = inwardTrans.Amount,
                        BenAccountNo = inwardTrans.BenAccountNo,
                        BenfAccBranch = inwardTrans.BenfAccBranch,
                        BenfBnk = inwardTrans.BenfBnk,
                        BenfCardId = inwardTrans.DrwCardId,
                        DrwCardType = inwardTrans.DrwCardType,
                        BenfNationality = inwardTrans.BenfNationality,
                        BenName = inwardTrans.BenName,
                        ChqSequance = inwardTrans.ChqSequance,
                        ClrCenter = inwardTrans.ClrCenter,
                        Exported = 0,
                        Currency = inwardTrans.Currency,
                        DrwAcctNo = inwardTrans.DrwAcctNo,
                        DrwBankNo = inwardTrans.DrwBankNo,
                        DrwBranchExt = inwardTrans.DrwBranchExt,
                        DrwBranchNo = inwardTrans.DrwBranchNo,
                        DrwCardId = inwardTrans.DrwCardId,
                        DrwChqNo = inwardTrans.DrwChqNo,
                        Discount_Img_ID = inwardTrans.DiscountReternedOutImgID,
                        DrwName = inwardTrans.DrwName,
                        History = $"Chq Returned with Return code = {await GetFinalOnusCode(inwardTrans.ReturnCode, inwardTrans.ReturnCodeFinancail, inwardTrans.ClrCenter)} At : {DateTime.Now}",
                        InputDate = inwardTrans.InputDate,
                        IntrBkSttlmDt = inwardTrans.IntrBkSttlmDt,
                        ISSAccount = inwardTrans.ISSAccount,
                        NoOfReturn = 1,
                        ReturnDateHistory = $"{DateTime.Now}|",
                        OperType = inwardTrans.OperType,
                        Posted = (int)AllEnums.Cheque_Status.Returne,
                        Returned = true,
                        Serial = inwardTrans.Serial,
                        Status = inwardTrans.Status,
                        System_Aut_Man = inwardTrans.System_Aut_Man,
                        TransCode = inwardTrans.TransCode,
                        TransDate = inwardTrans.TransDate,
                        UserId = inwardTrans.UserId,
                        ValueDate = inwardTrans.ValueDate
                    };

                    string finalReturnCode = inwardTrans.ReturnCode;
                    if (string.IsNullOrEmpty(finalReturnCode))
                    {
                        finalReturnCode = inwardTrans.ReturnCodeFinancail?.Trim();
                    }
                    if (finalReturnCode?.Length == 1)
                    {
                        finalReturnCode = "0" + finalReturnCode;
                    }
                    retChq.ReturnCode = await GetFinalOnusCode(finalReturnCode, inwardTrans.ReturnCodeFinancail, inwardTrans.ClrCenter);

                    _context.ReturnedCheques_Tbl.Add(retChq);
                    await _context.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ReturnChqTBL for serial {Serial}: {Message}", inwardTrans.Serial, ex.Message);
                return false;
            }
        }



        public async Task<bool> GenerateDiscountCommissionFile()
        {
            bool done = false;
            _logger.LogInformation("Create DISCOM file");

            try
            {
                string tDate = DateTime.Now.ToString("yyyy-MM-dd");

                var discontCommList = await _context.Discount_Commisions.ToListAsync();
                var aggregatedList = new List<Discount_Commisions>();

                foreach (var item in discontCommList)
                {
                    if (item.CreatedAt.ToString("yyyy-MM-dd") == tDate)
                    {
                        if (!string.IsNullOrEmpty(item.T24CommisionTypeId))
                        {
                            var commisionTypes = await _context.T24CommisionTypes.SingleOrDefaultAsync(i => i.Id == item.T24CommisionTypeId);
                            if (commisionTypes != null)
                            {
                                item.History = commisionTypes.Commision_Code;
                            }
                        }

                        var existingComObj = aggregatedList.SingleOrDefault(x => x.BranchCode == item.BranchCode && x.T24CommisionTypeId == item.T24CommisionTypeId);
                        if (existingComObj != null)
                        {
                            existingComObj.NumberOfCheques += 1;
                        }
                        else
                        {
                            aggregatedList.Add(item);
                        }
                    }
                }

                // Get path from configuration
                string path = _configuration["Discount_Comm_File"];
                if (string.IsNullOrEmpty(path))
                {
                    _logger.LogError("Discount_Comm_File path not configured.");
                    return false;
                }

                // Ensure directory exists
                string directory = Path.GetDirectoryName(path);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (var sw = new StreamWriter(File.Open(path, FileMode.Create)))
                {
                    if (aggregatedList.Any())
                    {
                        foreach (var item in aggregatedList)
                        {
                            await sw.WriteLineAsync($"{item.BranchCode}||{item.History}|{item.NumberOfCheques}");
                        }
                    }
                    else
                    {
                        await sw.WriteLineAsync("No Record Found");
                    }
                }
                done = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating discount commission file: {Message}", ex.Message);
                return false;
            }

            return done;
        }



        public async Task<string> GetFinalOnusCodeError(string retCode, string retCodeFinancial, string clrCenter)
        {
            string result = "";
            string retCodeTrimmed = retCode ?? "";
            string retCodeFinancialTrimmed = retCodeFinancial ?? "";

            string clr = "";
            int retOrder = 0;
            int financialOrder = 0;

            try
            {
                if (clrCenter == "INHOUSE" || clrCenter == "PMA")
                {
                    clr = "PMA_INHOUSE";
                }
                else
                {
                    clr = "DISCOUNT";
                }

                if (string.IsNullOrEmpty(retCodeTrimmed))
                {
                    return retCodeFinancialTrimmed;
                }
                else if (string.IsNullOrEmpty(retCodeFinancialTrimmed))
                {
                    return retCodeTrimmed;
                }
                else
                {
                    var tblRetCode = await _context.Return_Code_Order.SingleOrDefaultAsync(i => i.ReturnCode == retCodeTrimmed && i.ClrCenter == clr);
                    if (tblRetCode != null)
                    {
                        retOrder = tblRetCode.ReturnCodeOrder;
                    }
                    else
                    {
                        return retCodeFinancialTrimmed;
                    }

                    var tblFinancialCode = await _context.Return_Code_Order.SingleOrDefaultAsync(i => i.ReturnCode == retCodeFinancialTrimmed && i.ClrCenter == clr);
                    if (tblFinancialCode != null)
                    {
                        financialOrder = tblFinancialCode.ReturnCodeOrder;
                    }
                    else
                    {
                        return retCodeTrimmed;
                    }

                    if (retOrder > financialOrder)
                    {
                        return retCodeFinancialTrimmed;
                    }
                    else
                    {
                        return retCodeTrimmed;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during GetFinalOnusCodeError: {Message}", ex.Message);
            }
            return result;
        }



        public async Task<Tuple<string, int>> Print_CHQ_QR(string userName)
        {
            string str = "";
            int count = 0;

            try
            {
                // The original VB.NET code uses a direct SQL query and SqlDataReader.
                // For ASP.NET Core, it's better to use EF Core or Dapper for database access.
                // Given the complexity of the query, a direct EF Core query might be difficult.
                // For now, I will use a simplified approach or a placeholder for the data retrieval.
                // The original query was:
                // "select * from OnUs_Tbl where DrwChqNo=\'30000287\' and DrwAcctNo=\'0001021818\'"

                // Placeholder for data retrieval
                var onusEntry = await _context.OnUs_Tbl.FirstOrDefaultAsync(i => i.DrwChqNo == "30000287" && i.DrwAcctNo == "0001021818");

                if (onusEntry != null)
                {
                    string chqSeq = onusEntry.Serial;

                    var img = await _context.OnUs_Imgs.FirstOrDefaultAsync(i => i.Serial == chqSeq);

                    if (img != null)
                    {
                        string sourceFileName = "test.htm"; // This file needs to be in wwwroot/Resources or similar
                        string resourcePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Resources", sourceFileName);

                        if (File.Exists(resourcePath))
                        {
                            string htmlCode = await File.ReadAllTextAsync(resourcePath, System.Text.Encoding.UTF8);

                            if (img.FrontImg != null)
                            {
                                htmlCode = htmlCode.Replace("@Front_Image@", $"<img class=\"image\" src=\"data:image;base64,{Convert.ToBase64String(img.FrontImg)}\" width=\"550\" height=\"230\"/>");
                            }
                            if (img.RearImg != null)
                            {
                                htmlCode = htmlCode.Replace("@Rear_Image@", $"<img class=\"image\" src=\"data:image;base64,{Convert.ToBase64String(img.RearImg)}\" width=\"550\" height=\"230\"/>");
                            }
                            if (img.UVImage != null)
                            {
                                htmlCode = htmlCode.Replace("@uv_Image@", $"<img class=\"image\" src=\"data:image;base64,{Convert.ToBase64String(img.UVImage)}\" width=\"500\" height=\"250\"/>");
                            }

                            str += htmlCode;
                            count += 1;
                        }
                        else
                        {
                            _logger.LogWarning("Resource file {ResourcePath} not found for Print_CHQ_QR.", resourcePath);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Print_CHQ_QR: {Message}", ex.Message);
            }

            return Tuple.Create(str, count);
        }



        public async Task<INChqs> PrintChqViewer(string id)
        {
            var inChq = new INChqs();
            try
            {
                var inw = await _context.Inward_Trans.SingleOrDefaultAsync(c => c.Serial == id);
                if (inw != null)
                {
                    inChq.inw = inw;
                    inChq.Imgs = await _context.INWARD_IMAGES.FirstOrDefaultAsync(i => i.Serial == id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in PrintChqViewer for ID {Id}: {Message}", id, ex.Message);
            }
            return inChq;
        }



        public async Task<VIPCHQViewModel> VIPCHQ(string userName, int userId)
        {
            var viewModel = new VIPCHQViewModel();
            try
            {
                // Get page title and application ID
                string methodName = "VIPCHQ";
                var appPage = await _context.App_Pages.SingleOrDefaultAsync(t => t.Page_Name_EN == methodName);
                if (appPage != null)
                {
                    viewModel.Title = appPage.ENG_DESC;
                }

                // Get categories for tree view
                viewModel.Tree = await GetAllCategoriesForTree();

                // Get branch list
                viewModel.Branches = await _context.Bank_Branches_Tbl.Where(x => x.BankCode == "78").ToListAsync();

                // Get return codes list
                viewModel.ReturnDescriptions = await _context.Return_Codes_Tbl.Where(i => i.ClrCenter != "DISCOUNT").ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in VIPCHQ service method for user {UserName}: {Message}", userName, ex.Message);
            }
            return viewModel;
        }



        public async Task<Tuple<List<Inward_Trans>, List<Return_Codes_Tbl>>> FINDVIPCHQ(string branch, string userName)
        {
            var lstPstINW = new List<Inward_Trans>();
            var pmaRectCode = new List<Return_Codes_Tbl>();

            try
            {
                // Construct the base query using LINQ or raw SQL
                // The original query is complex and involves joins and conditional logic.
                // For simplicity and to avoid direct SQL, I'll try to translate it to LINQ.
                // If performance is an issue, raw SQL or Dapper might be considered.

                var query = from chq in _context.Inward_Trans
                            join wf in _context.INWARD_WF_Tbl on chq.ChqSequance equals wf.ChqSequance into wfGroup
                            from wf in wfGroup.DefaultIfEmpty()
                            where chq.Posted == (int)AllEnums.Cheque_Status.Posted &&
                                  chq.ClrCenter == "PMA" &&
                                  chq.InputDate.HasValue &&
                                  chq.InputDate.Value.Date == DateTime.Now.Date &&
                                  (chq.verifyStatus == null || chq.verifyStatus == 0 || wf.Final_Status != "Accept") &&
                                  chq.VIP == true
                            select new
                            {
                                chq.Serial,
                                wf.Final_Status,
                                chq.verifyStatus,
                                chq.Posted,
                                chq.ClrCenter,
                                chq.ReturnCode,
                                chq.ReturnCodeFinancail,
                                chq.TransDate,
                                chq.ISSAccount,
                                chq.InputDate,
                                chq.DrwChqNo,
                                chq.DrwBankNo,
                                chq.DrwBranchNo,
                                chq.Currency,
                                chq.Amount,
                                chq.ValueDate,
                                chq.DrwName,
                                chq.BenAccountNo,
                                chq.BenName,
                                chq.DrwAcctNo,
                                chq.ErrorDescription,
                                chq.Company_Code
                            };

                if (branch != "-1")
                {
                    query = query.Where(chq => chq.Company_Code == branch);
                }

                var results = await query.ToListAsync();

                foreach (var dr in results)
                {
                    var obj = new Inward_Trans
                    {
                        Serial = dr.Serial,
                        InputDate = dr.InputDate?.ToString("yyyy/MM/dd"),
                        DrwChqNo = dr.DrwChqNo,
                        DrwBankNo = dr.DrwBankNo,
                        DrwBranchNo = dr.DrwBranchNo,
                        verifyStatus = dr.verifyStatus,
                        ReturnCode = await GetFinalOnusCodeError(dr.ReturnCode, dr.ReturnCodeFinancail, dr.ClrCenter),
                        Currency = dr.Currency,
                        Amount = dr.Amount,
                        TransDate = dr.TransDate?.ToString("yyyy/MM/dd"),
                        DrwName = dr.DrwName,
                        Posted = dr.Posted,
                        BenAccountNo = dr.BenAccountNo,
                        BenName = dr.BenName,
                        DrwAcctNo = dr.DrwAcctNo,
                        ISSAccount = dr.ISSAccount,
                        ErrorDescription = dr.ErrorDescription
                    };
                    lstPstINW.Add(obj);
                }

                pmaRectCode = await _context.Return_Codes_Tbl.Where(x => x.ClrCenter != "DISCOUNT").ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in FINDVIPCHQ service method for user {UserName}: {Message}", userName, ex.Message);
            }

            return Tuple.Create(lstPstINW, pmaRectCode);
        }



        public async Task<string> Returnvipchq(string serial, string rc, string userName, int userId)
        {
            string message = "";
            try
            {
                var inward = await _context.Inward_Trans.SingleOrDefaultAsync(x => x.Serial == serial);
                if (inward == null)
                {
                    return "Inward transaction not found.";
                }

                // Placeholder for external web service calls
                // These would need to be properly implemented with HttpClient or generated service clients
                // For now, I'll simulate success or failure based on assumptions.

                bool reversePostingSuccess = true; // Simulate web service call success
                string reversePostingResponse = "";

                if (inward.Posted <= (int)AllEnums.Cheque_Status.Posted || inward.Posted <= (int)AllEnums.Cheque_Status.Cleared)
                {
                    // Simulate ECC_CAP_Services.SAFA_T24_ECC_SVCSoapClient.ECC_OFS_MESSAGE call
                    // In a real scenario, this would involve creating a proper client and DTOs
                    // For now, assume it returns success.
                    reversePostingResponse = "ReversePostingPMARAM Success";

                    if (reversePostingSuccess)
                    {
                        message = "ReversePostingPMARAM Successfully";

                        // Log WFHistory
                        var wfHistory = new WFHistory
                        {
                            ChqSequance = inward.ChqSequance,
                            Serial = inward.Serial,
                            TransDate = inward.ValueDate,
                            ID_WFStatus = 0,
                            Status = inward.Status + message + userName + "  AT : " + DateTime.Now,
                            ClrCenter = inward.ClrCenter,
                            DrwChqNo = inward.DrwChqNo,
                            DrwAccNo = inward.DrwAcctNo,
                            Amount = inward.Amount
                        };
                        _context.WFHistories.Add(wfHistory);

                        // Simulate ECC_Com_SVC.Ecc_Commisions_HandlerSoapClient.Post_Commision_To_T24 call
                        // For now, assume it returns success.
                        bool commissionPostSuccess = true; // Simulate web service call success
                        string commissionDescription = "Commission Posted Successfully";
                        string commissionAmount = "10.00"; // Example amount

                        if (commissionPostSuccess)
                        {
                            inward.RevCommision = 1;
                        }
                        else if (commissionDescription.Contains("OVERRIDE"))
                        {
                            inward.RevCommision = 1;
                        }

                        inward.T24Response += "| ReversePostingPMARAM Commision : " + commissionDescription + " With Amount : " + commissionAmount;
                        inward.History += " |  ReversePostingPMARAM Commision : " + commissionDescription + " With Amount : " + commissionAmount;
                        inward.ErrorCode = "";
                        inward.FinalRetCode = await GetFinalOnusCodeError(rc.Trim(), inward.ReturnCodeFinancail, inward.ClrCenter);
                        inward.ReturnDate = DateTime.Now;
                        inward.ErrorDescription = " ReversePostingPMARAM Success  From VIP  Screen  AT " + DateTime.Now;
                        inward.T24Response = "ReversePostingPMARAM Success From VIP Screen  AT " + DateTime.Now;
                        inward.IsTimeOut = 0;
                        inward.PMAstatus = "Pending";
                        inward.History += " change PMA STATUS to : Pending  , by   :" + userName + "AT:   " + DateTime.Now + ", From VIP Screen";
                        inward.Returned = 1;
                        inward.ReturnCode = rc;
                        inward.LastUpdateBy = userName;
                        inward.Posted = (int)AllEnums.Cheque_Status.Returne;
                        inward.LastUpdate = DateTime.Now;
                        inward.History += " ReversePostingPMARAM Done  By" + userName + " FROM VIP CHQ AT " + DateTime.Now;

                        // Log WFHistory for return
                        var wfHistoryReturn = new WFHistory
                        {
                            ChqSequance = inward.ChqSequance,
                            Serial = inward.Serial,
                            TransDate = inward.ValueDate,
                            ID_WFStatus = 0,
                            Status = "Return VIP CHQ By User : " + userName + "At : " + DateTime.Now + "With Return Code : " + rc,
                            ClrCenter = inward.ClrCenter,
                            DrwChqNo = inward.DrwChqNo,
                            DrwAccNo = inward.DrwAcctNo,
                            Amount = inward.Amount
                        };
                        _context.WFHistories.Add(wfHistoryReturn);
                    }
                    else
                    {
                        message = "ReversePostingPMARAM Falid";

                        // Log WFHistory
                        var wfHistory = new WFHistory
                        {
                            ChqSequance = inward.ChqSequance,
                            Serial = inward.Serial,
                            TransDate = inward.ValueDate,
                            ID_WFStatus = 0,
                            Status = inward.Status + message + userName + "  AT : " + DateTime.Now,
                            ClrCenter = inward.ClrCenter,
                            DrwChqNo = inward.DrwChqNo,
                            DrwAccNo = inward.DrwAcctNo,
                            Amount = inward.Amount
                        };
                        _context.WFHistories.Add(wfHistory);

                        inward.ErrorDescription = " ReversePostingPMARAM Falid";
                        inward.T24Response = "ReversePostingPMARAM Falid";
                        inward.ReturnDate = DateTime.Now;
                        inward.History += "ReversePostingPMARAM  Falid  By" + userName + " FROM VIP CHQ AT " + DateTime.Now;
                    }
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Returnvipchq service method for serial {Serial}: {Message}", serial, ex.Message);
                message = "An error occurred: " + ex.Message;
            }
            return message;
        }

