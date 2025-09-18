using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SAFA_ECC_Core_Clean.Data;
using SAFA_ECC_Core_Clean.Models;
using SAFA_ECC_Core_Clean.ViewModels.AdminViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SAFA_ECC_Core_Clean.Services
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AdminService> _logger;
        private readonly IConfiguration _configuration;
        // Assuming LogSystem is a utility class that needs to be injected or instantiated
        // private readonly LogSystem _logSystem; 
        private readonly int _applicationID = 1; // Example Application ID

        public AdminService(ApplicationDbContext context, ILogger<AdminService> logger, IConfiguration configuration/*, LogSystem logSystem*/)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
            // _logSystem = logSystem;
        }

        public async Task<List<UserPermissionViewModel>> GetUserPermissions(int userId, int appId, string userName, int currentUserId)
        {
            // Placeholder for implementation
            return new List<UserPermissionViewModel>();
        }

        public async Task<bool> AcceptRejectUserPermission(string id, string status, string app, string page, string userName, int currentUserId)
        {
            // Placeholder for implementation
            return false;
        }

        public async Task<bool> AcceptRejectFinancialSite(string id, string status, string userName, int currentUserId)
        {
            // Placeholder for implementation
            return false;
        }

        public async Task<bool> AcceptRejectGroupPermission(string id, string status, string app, string page, string userName, int currentUserId)
        {
            // Placeholder for implementation
            return false;
        }

        public async Task<bool> LogoutUser(string id, string userName, int currentUserId)
        {
            // Placeholder for implementation
            return false;
        }

        public async Task<bool> SaveAuthUser(SaveAuthUserViewModel model, string currentUserName, int currentUserId)
        {
            try
            {
                _logger.LogInformation($"START REMOVE, ADD LEVEL FOR USER: {model.UserId}, PDCL1: {model.PDC1}, PDCL2: {model.PDC2}, outdis1: {model.Outdis1}, outdis2: {model.Outdis2}, outpma1: {model.Outpma1}, outpma2: {model.Outpma2}, indis1: {model.Indis1}, indis2: {model.Indis2}, ininw1: {model.Ininw1}, ininw2: {model.Ininw2}, onus1: {model.Onus1}, onus2: {model.Onus2} BY USER: {currentUserName}, AT: {DateTime.Now}");

                var userTbl = await _context.Users_Tbl.SingleOrDefaultAsync(i => i.User_ID == model.UserId);
                if (userTbl == null)
                {
                    _logger.LogWarning($"User {model.UserId} not found for SaveAuthUser operation.");
                    return false;
                }

                var username = userTbl.User_Name;

                var userAuthList = await _context.AuthTrans_User_TBL_Auth.Where(c => c.Auth_user_ID == model.UserId).ToListAsync();
                var userNotAuthList = await _context.AuthTrans_User_TBL.Where(c => c.Auth_user_ID == model.UserId).ToListAsync();

                foreach (var item in userAuthList)
                {
                    try
                    {
                        _logger.LogInformation($"START During Remove Record From AuthTrans_User_TBL_Auth Trans_id={item.Trans_id} With UserID= {model.UserId}");
                        _context.AuthTrans_User_TBL_Auth.Remove(item);
                        await _context.SaveChangesAsync();
                        _logger.LogInformation($"END During Remove Record From AuthTrans_User_TBL_Auth Trans_id={item.Trans_id} With UserID= {model.UserId}");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error During Remove Record From AuthTrans_User_TBL_Auth Trans_id={item.Trans_id} With UserID= {model.UserId}. Error: {ex.Message}");
                    }
                }

                foreach (var item1 in userNotAuthList)
                {
                    try
                    {
                        _logger.LogInformation($"START Remove Record From AuthTrans_User_TBL Trans_id={item1.Trans_id} With UserID= {model.UserId}");
                        _context.AuthTrans_User_TBL.Remove(item1);
                        await _context.SaveChangesAsync();
                        _logger.LogInformation($"END Remove Record From AuthTrans_User_TBL Trans_id={item1.Trans_id} With UserID= {model.UserId}");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error During Remove Record From AuthTrans_User_TBL Trans_id={item1.Trans_id} With UserID= {model.UserId}. Error: {ex.Message}");
                    }
                }

                // PDC
                if (model.PDC1 || model.PDC2)
                {
                    try
                    {
                        _logger.LogInformation($"AuthTrans_User_TBL Trans_id=1, With UserID= {model.UserId} and Level 1, Level 2 is {model.PDC1}, {model.PDC2}, PDC");
                        var userAuth = new AuthTrans_User_TBL_Auth
                        {
                            Auth_level1 = model.PDC1,
                            Auth_level2 = model.PDC2,
                            Auth_user_ID = model.UserId,
                            Auth_user_Name = username,
                            group_ID = userTbl.Group_ID,
                            Auth_Trans__name = "PDC",
                            Trans_id = 1,
                            status = "Pending"
                        };
                        _context.AuthTrans_User_TBL_Auth.Add(userAuth);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error During Add Record to AuthTrans_User_TBL Trans_id=1, With UserID= {model.UserId} and Level 1, Level 2 is {model.PDC1}, {model.PDC2}, PDC. Error: {ex.Message}");
                    }
                }

                // Outward_DISCOUNT
                if (model.Outdis1 || model.Outdis2)
                {
                    try
                    {
                        _logger.LogInformation($"AuthTrans_User_TBL Trans_id=2, With UserID= {model.UserId} and Level 1, Level 2 is {model.Outdis1}, {model.Outdis2}, Outward_DISCOUNT");
                        var userAuth = new AuthTrans_User_TBL_Auth
                        {
                            Auth_level1 = model.Outdis1,
                            Auth_level2 = model.Outdis2,
                            Auth_user_ID = model.UserId,
                            Auth_user_Name = username,
                            group_ID = userTbl.Group_ID,
                            Auth_Trans__name = "Outward_DISCOUNT",
                            Trans_id = 2,
                            status = "Pending"
                        };
                        _context.AuthTrans_User_TBL_Auth.Add(userAuth);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error During Add Record to AuthTrans_User_TBL Trans_id=2, With UserID= {model.UserId} and Level 1, Level 2 is {model.Outdis1}, {model.Outdis2}, Outward_DISCOUNT. Error: {ex.Message}");
                    }
                }

                // Outward_PMA
                if (model.Outpma1 || model.Outpma2)
                {
                    try
                    {
                        _logger.LogInformation($"AuthTrans_User_TBL Trans_id=3, With UserID= {model.UserId} and Level 1, Level 2 is {model.Outpma1}, {model.Outpma2}, Outward_PMA");
                        var userAuth = new AuthTrans_User_TBL_Auth
                        {
                            Auth_level1 = model.Outpma1,
                            Auth_level2 = model.Outpma2,
                            Auth_user_ID = model.UserId,
                            Auth_user_Name = username,
                            group_ID = userTbl.Group_ID,
                            Auth_Trans__name = "Outward_PMA",
                            Trans_id = 3,
                            status = "Pending"
                        };
                        _context.AuthTrans_User_TBL_Auth.Add(userAuth);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error During Add Record to AuthTrans_User_TBL Trans_id=3, With UserID= {model.UserId} and Level 1, Level 2 is {model.Outpma1}, {model.Outpma2}, Outward_PMA. Error: {ex.Message}");
                    }
                }

                // Inoward_DISCOUNT
                if (model.Indis1 || model.Indis2)
                {
                    try
                    {
                        _logger.LogInformation($"AuthTrans_User_TBL Trans_id=4, With UserID= {model.UserId} and Level 1, Level 2 is {model.Indis1}, {model.Indis2}, Inoward_DISCOUNT");
                        var userAuth = new AuthTrans_User_TBL_Auth
                        {
                            Auth_level1 = model.Indis1,
                            Auth_level2 = model.Indis2,
                            Auth_user_ID = model.UserId,
                            Auth_user_Name = username,
                            group_ID = userTbl.Group_ID,
                            Auth_Trans__name = "Inoward_DISCOUNT",
                            Trans_id = 4,
                            status = "Pending"
                        };
                        _context.AuthTrans_User_TBL_Auth.Add(userAuth);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error During Add Record to AuthTrans_User_TBL Trans_id=4, With UserID= {model.UserId} and Level 1, Level 2 is {model.Indis1}, {model.Indis2}, Inoward_DISCOUNT. Error: {ex.Message}");
                    }
                }

                // Inoward_PMA
                if (model.Ininw1 || model.Ininw2)
                {
                    try
                    {
                        _logger.LogInformation($"AuthTrans_User_TBL Trans_id=5, With UserID= {model.UserId} and Level 1, Level 2 is {model.Ininw1}, {model.Ininw2}, Inoward_PMA");
                        var userAuth = new AuthTrans_User_TBL_Auth
                        {
                            Auth_level1 = model.Ininw1,
                            Auth_level2 = model.Ininw2,
                            Auth_user_ID = model.UserId,
                            Auth_user_Name = username,
                            group_ID = userTbl.Group_ID,
                            Auth_Trans__name = "Inoward_PMA",
                            Trans_id = 5,
                            status = "Pending"
                        };
                        _context.AuthTrans_User_TBL_Auth.Add(userAuth);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error During Add Record to AuthTrans_User_TBL Trans_id=5, With UserID= {model.UserId} and Level 1, Level 2 is {model.Ininw1}, {model.Ininw2}, Inoward_PMA. Error: {ex.Message}");
                    }
                }

                // Outward_ONUS
                if (model.Onus1 || model.Onus2)
                {
                    try
                    {
                        _logger.LogInformation($"AuthTrans_User_TBL Trans_id=6, With UserID= {model.UserId} and Level 1, Level 2 is {model.Onus1}, {model.Onus2}, Outward_ONUS");
                        var userAuth = new AuthTrans_User_TBL_Auth
                        {
                            Auth_level1 = model.Onus1,
                            Auth_level2 = model.Onus2,
                            Auth_user_ID = model.UserId,
                            Auth_user_Name = username,
                            group_ID = userTbl.Group_ID,
                            Auth_Trans__name = "Outward_ONUS",
                            Trans_id = 6,
                            status = "Pending"
                        };
                        _context.AuthTrans_User_TBL_Auth.Add(userAuth);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error During Add Record to AuthTrans_User_TBL Trans_id=6, With UserID= {model.UserId} and Level 1, Level 2 is {model.Onus1}, {model.Onus2}, Outward_ONUS. Error: {ex.Message}");
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in SaveAuthUser for UserID: {model.UserId}. Error: {ex.Message}");
                return false;
            }
        }

        public async Task<List<SAFA_ECC_Core_Clean.Models.Users_Permissions>> GetPermission(int user, int app)
        {
            try
            {
                _logger.LogInformation($"Getting permissions for user {user} and app {app}");
                var userPermissions = await _context.Users_Permissions
                    .Where(x => x.Application_ID == app && x.UserID == user && x.PageID > 0 && x.Value == true)
                    .ToListAsync();
                return userPermissions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting permissions for user {user} and app {app}. Error: {ex.Message}");
                // In a real app, you might want to log to _LogSystem here
                return new List<SAFA_ECC_Core_Clean.Models.Users_Permissions>();
            }
        }

        public async Task<bool> LogoutUser(string id, string userName)
        {
            try
            {
                _logger.LogInformation($"Start logout user by: {userName} at {DateTime.Now}");
                var user = await _context.Users_Tbl.SingleOrDefaultAsync(i => i.User_ID == id);
                if (user != null)
                {
                    user.LoginStatus = 0;
                    user.Action_History += $" | Change Login Status from 1 to 0 by {userName}";
                    await _context.SaveChangesAsync();
                }
                _logger.LogInformation($"End logout user by: {userName} at {DateTime.Now}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error logging out user {id}. Error: {ex.Message}");
                return false;
            }
        }

        public async Task<AccountInfoResponseViewModel> GetAccountInfo(string accountNo, string benName, string currency, string userName)
        {
            var response = new AccountInfoResponseViewModel();
            try
            {
                // Simulate logging
                _logger.LogInformation("Get Account Info from MQ");

                // Simulate fetching CurrencyT (replace with actual EF Core call)
                // var currencyT = await _context.CURRENCY_TBL.ToListAsync();

                // Simulate ECC_CAP_Services calls (replace with actual service calls)
                // For now, using dummy data or simplified logic
                var accobj = new SAFA_ECC_Core_Clean.Models.AccountInfo_RESPONSE(); // Assuming this model exists
                var accListobj = new SAFA_ECC_Core_Clean.Models.AccountList_RESPONSE(); // Assuming this model exists

                // Simplified logic for demonstration
                if (accountNo.Length == 12 && accountNo.StartsWith("78"))
                {
                    // Simulate service call
                    accobj.ResponseStatus = "S"; // Assume success for now
                    accobj.Currency = currency;
                    accobj.AccountName = benName;
                    accobj.OwnerBranch = "123456789"; // Dummy data
                    accobj.CustomerID = "CUST123";
                    accobj.ID = "DOC123";
                    accobj.DocumentType = "TYPEA";
                    accobj.Nationality = "NAT";
                }
                else if (accountNo.Length == 13)
                {
                    // Simulate service call
                    accobj.ResponseStatus = "S"; // Assume success for now
                    accobj.Currency = currency;
                    accobj.AccountName = benName;
                    accobj.OwnerBranch = "123456789"; // Dummy data
                    accobj.CustomerID = "CUST123";
                    accobj.ID = "DOC123";
                    accobj.DocumentType = "TYPEA";
                    accobj.Nationality = "NAT";
                }
                else
                {
                    response.ErrorMsg = "Invalid Account Number";
                    response.Status = "Faild";
                    return response;
                }

                if (accobj.ResponseStatus == "S")
                {
                    if (accobj.OwnerBranch != null && accobj.OwnerBranch.Length > 6)
                    {
                        accobj.OwnerBranch = accobj.OwnerBranch.Substring(6);
                    }

                    if (accobj.Currency == currency)
                    {
                        if (accobj.AccountName == benName)
                        {
                            response.Status = "Done";
                        }
                        else
                        {
                            response.ErrorMsg = "This AccountNo refers to another customer!";
                            response.Status = "DifName";
                        }
                    }
                    else
                    {
                        response.ErrorMsg = "This AccountNo have a different currency";
                        response.Status = "DifCurrency";
                    }
                }
                else if (accobj.ResponseStatus == "F")
                {
                    response.ErrorMsg = "Invalid Account Number";
                    response.Status = "Faild";
                }

                response.CustomerID = accobj.CustomerID;
                response.BenName = accobj.AccountName;
                response.DocId = accobj.ID;
                response.BenBrn = accobj.OwnerBranch;
                response.DocType = accobj.DocumentType;
                response.Nat = accobj.Nationality;
                // response.AccType = ObjList; // Need to map ObjList to a suitable ViewModel property
                response.Currency = accobj.Currency;
                response.ResponseStatus = accobj.ResponseStatus;
            }
            catch (Exception ex)
            {
                // Simulate error logging
                _logger.LogError(ex, "Error when get AccountInfo from MQ");
                response.ErrorMsg = "Error when get AccountInfo: " + ex.Message;
                response.Status = "Faild";
            }
            return response;
        }

        public async Task<List<SAFA_ECC_Core_Clean.Models.Groups_Permissions>> GetGroupPermission(int group, int app, string userName)
        {
            try
            {
                _logger.LogInformation($"Getting group permissions for group {group} and app {app} by user {userName}");
                var groupPermissions = await _context.Groups_Permissions
                    .Where(x => x.Application_ID == app && x.Group_Id == group && x.Page_Id > 0)
                    .ToListAsync();
                return groupPermissions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting group permissions for group {group} and app {app}. Error: {ex.Message}");
                return new List<SAFA_ECC_Core_Clean.Models.Groups_Permissions>();
            }
        }
    }
}

