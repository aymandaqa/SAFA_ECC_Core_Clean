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
using Microsoft.AspNetCore.Mvc.Rendering;
using SAFA_ECC_Core_Clean.ViewModels.InwardViewModels;
using All_CLASSES;
using SAFA_ECC_Core_Clean.Data;

namespace SAFA_ECC_Core_Clean.Services
{

    public class InwardService : IInwardService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<InwardService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        private string _applicationID = "1";

        public InwardService(ApplicationDbContext context, ILogger<InwardService> logger, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
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

        private string GetMethodName() => new System.Diagnostics.StackFrame(1).GetMethod().Name;

        public async Task<object> InwardFinanicalWFDetailsONUS_NEW(string id)
        {
            _httpContextAccessor.HttpContext.Session.SetString("ErrorMessage", "");
            string branch = _httpContextAccessor.HttpContext.Session.GetString("BranchID");

            if (string.IsNullOrEmpty(GetUserName()))
            {
                return new { redirectTo = "/Login/Login" };
            }

            string methodName = "InwardFinanicalWFDetailsONUS";
            int userId = GetUserID();

            try
            {
                var appPage = await _context.App_Pages.SingleOrDefaultAsync(t => t.Page_Name_EN == methodName);
                if (appPage == null) return new { list = (object)null, Sta = "F", Message = "App page not found" };

                int pageId = appPage.Page_Id;
                int applicationId = appPage.Application_ID;

                var wf = await _context.INWARD_WF_Tbl.SingleOrDefaultAsync(z => z.Serial == id && z.Final_Status != "Accept" && z.Clr_Center == "Outward_ONUS");
                if (wf == null) return new { redirectTo = "/INWARD/InsufficientFunds" };

                var incObj = await _context.OnUs_Tbl.SingleOrDefaultAsync(y => y.Serial == id && (y.Posted == (int)AllEnums.Cheque_Status.New || y.Posted == (int)AllEnums.Cheque_Status.Posted));
                if (incObj == null) return new { redirectTo = "/INWARD/InsufficientFunds" };

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
                    recomdationbtn = true;

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

                incObj.Amount = decimal.Round(incObj.Amount, 2, MidpointRounding.AwayFromZero);
                if (incObj.Status == "S")
                {
                    incObj.Status = "Success";
                }
                if (incObj.Status == "F")
                {
                    incObj.Status = "Faild";
                }

                return new
                {
                    list = incObj,
                    Sta = "S",
                    Title = appPage.ENG_DESC,
                    GUAR_CUSTOMER = guarCustomerInfo,
                    Reject = reject,
                    RecomdationBtn = recomdationbtn,
                    Approve = approve
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in InwardFinanicalWFDetailsONUS_NEW: {Message}", ex.Message);
                _httpContextAccessor.HttpContext.Session.SetString("ErrorMessage", "An error occurred: " + ex.Message);
                return new { list = (object)null, Sta = "F" };
            }
        }

        public async Task<DataTable> Getpage(string page)
        {
            try
            {
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
                    if (_page == "0")
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
                return false;
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
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetPermission1: {Message}", ex.Message);
                return false;
            }
        }

        public async Task<JsonResult> InwardFinanicalWFDetailsONUS_Auth(string id)
        {
            throw new NotImplementedException();
        }
    }
}

