using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SAFA_ECC_Core_Clean.Models;
using SAFA_ECC_Core_Clean.ViewModels.OutwordViewModels;
using AllEnums;
using System.Reflection;
using All_CLASSES;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace SAFA_ECC_Core_Clean.Services
{
    public class OutwordService : IOutwordService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<OutwordService> _logger;
        private readonly SAFA_T24_ECC_SVCSoapClient _safaT24EccSvcClient;
        private readonly AllStoredProcesures _logSystem;
        private string _applicationID = "1";

        public OutwordService(ApplicationDbContext context, ILogger<OutwordService> logger, SAFA_T24_ECC_SVCSoapClient safaT24EccSvcClient, AllStoredProcesures logSystem)
        {
            _context = context;
            _logger = logger;
            _safaT24EccSvcClient = safaT24EccSvcClient;
            _logSystem = logSystem;
        }

        // Helper method to get user info (if needed, otherwise remove)
        private string GetMethodName() => new System.Diagnostics.StackFrame(1).GetMethod().Name;

        // Re-implement methods one by one

        public async Task<CheckImgViewModel> CheckImg()
        {
            var viewModel = new CheckImgViewModel();
            try
            {
                // Original VB.NET logic (hardcoded test case)
                var retOut = await _context.Outward_Trans.SingleOrDefaultAsync(i => i.InputBrn == "808" && i.Posted == (int)AllEnums.Cheque_Status.Returned && i.Serial == "1146042");

                if (retOut != null)
                {
                    var chqSeq = retOut.ChqSequance;
                    viewModel.OutwardImg = await _context.Outward_Imgs.SingleOrDefaultAsync(i => i.ChqSequance == chqSeq);
                }
                else
                {
                    viewModel.ErrorMessage = "No matching outward transaction found for check_img test case.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CheckImg service method.");
                viewModel.ErrorMessage = "An error occurred: " + ex.Message;
            }
            return viewModel;
        }

        public async Task<bool> GetPermission(string id, string page, string groupId)
        {
            List<Users_Permissions> usersPermissionPage = new List<Users_Permissions>();
            List<Groups_Permissions> groupPermissionPage = new List<Groups_Permissions>();

            try
            {
                groupPermissionPage = await _context.Groups_Permissions.Where(x => x.Group_Id == groupId && x.Page_Id == page && x.Application_ID == 1 && x.Access == true).ToListAsync();

                if (groupPermissionPage.Count == 0)
                {
                    if (page == "0")
                    {
                        usersPermissionPage = await _context.Users_Permissions.Where(x => x.UserID == id && x.PageID == page && x.Application_ID == 1).ToListAsync();
                        return true;
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
                        if (int.Parse(page) >= 1300 && int.Parse(page) <= 1400 && groupId == GroupType.Group_Status.AdminAuthorized)
                        {
                            return true;
                        }
                        if (int.Parse(page) >= 1 && int.Parse(page) <= 100 && groupId == GroupType.Group_Status.SystemAdmin)
                        {
                            return true;
                        }
                        if (!(int.Parse(page) >= 1) || !(int.Parse(page) <= 100) && (!(int.Parse(page) >= 1300) || !(int.Parse(page) <= 1400)))
                        {
                            return true;
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

                        usersPermissionPage = await _context.Users_Permissions.Where(x => x.UserID == id && x.PageID == page && x.Value == true && x.Application_ID == 1 && x.ActionID == 6).ToListAsync();
                        if (usersPermissionPage.Count > 0)
                        {
                            if (int.Parse(page) >= 1300 && int.Parse(page) <= 1400 && groupId == GroupType.Group_Status.AdminAuthorized)
                            {
                                return true;
                            }
                            if (int.Parse(page) >= 1 && int.Parse(page) <= 100 && groupId == GroupType.Group_Status.SystemAdmin)
                            {
                                return true;
                            }
                            if (!(int.Parse(page) >= 1) || !(int.Parse(page) <= 100) && (!(int.Parse(page) >= 1300) || !(int.Parse(page) <= 1400)))
                            {
                                return true;
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
                _logger.LogError(ex, "Error when get GetPermission service method.");
                _logger.LogError(ex, "Error in GetPermission service method.");
            }
            return false;
        }

        public async Task<bool> GetPermission1(string id, string page, string groupId)
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
                        if (int.Parse(page) >= 1300 && int.Parse(page) <= 1400 && groupId == GroupType.Group_Status.AdminAuthorized)
                        {
                            return true;
                        }
                        if (int.Parse(page) >= 1 && int.Parse(page) <= 100 && groupId == GroupType.Group_Status.SystemAdmin)
                        {
                            return true;
                        }
                        if (!(int.Parse(page) >= 1) || !(int.Parse(page) <= 100) && (!(int.Parse(page) >= 1300) || !(int.Parse(page) <= 1400)))
                        {
                            return true;
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
                            if (int.Parse(page) >= 1300 && int.Parse(page) <= 1400 && groupId == GroupType.Group_Status.AdminAuthorized)
                            {
                                return true;
                            }
                            if (int.Parse(page) >= 1 && int.Parse(page) <= 100 && groupId == GroupType.Group_Status.SystemAdmin)
                            {
                                return true;
                            }
                            if (!(int.Parse(page) >= 1) || !(int.Parse(page) <= 100) && (!(int.Parse(page) >= 1300) || !(int.Parse(page) >= 1400)))
                            {
                                return true;
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
                _logger.LogError(ex, "Error in GetPermission1 service method.");
                _logger.LogError(ex, "Error in GetPermission1 service method.");
            }
            return false;
        }

        public async Task<Hold_CHQViewModel> GetHold_CHQData()
        {
            var viewModel = new Hold_CHQViewModel();
            try
            {
                viewModel.BindHoldType = await BindHoldType();
                viewModel.Tree = await GetAllCategoriesForTree();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetHold_CHQData service method.");
                _logger.LogError(ex, "Error in GetHold_CHQData service method.");
                viewModel.ErrorMessage = "An error occurred: " + ex.Message;
            }
            return viewModel;
        }

        public async Task<bool> SaveHold_CHQ(Hold_CHQViewModel hold, string HOLD_TYPE, string Reserved, string userName)
        {
            try
            {
                if (string.IsNullOrEmpty(hold.Note1) || string.IsNullOrEmpty(hold.DrwAcctNo) || string.IsNullOrEmpty(hold.DrwBankNo) || string.IsNullOrEmpty(hold.DrwBranchNo) || string.IsNullOrEmpty(hold.DrwChqNo) || (string.IsNullOrEmpty(hold.Reson1) && string.IsNullOrEmpty(hold.Reson2)))
                {
                    return false; // Indicate validation failure
                }

                var existingHold = await _context.Hold_CHQ.SingleOrDefaultAsync(c => c.DrwAcctNo == hold.DrwAcctNo && c.DrwBankNo == hold.DrwBankNo && c.DrwBranchNo == hold.DrwBranchNo && c.DrwChqNo == hold.DrwChqNo);

                if (existingHold == null)
                {
                    var newHold = new Hold_CHQ
                    {
                        DrwAcctNo = hold.DrwAcctNo,
                        DrwBankNo = hold.DrwBankNo,
                        DrwBranchNo = hold.DrwBranchNo,
                        DrwChqNo = hold.DrwChqNo,
                        InputBy = userName,
                        InputDate = DateTime.Now,
                        History = $" Record Added by : {userName}, AT: {DateTime.Now}",
                        Reson1 = hold.Reson1,
                        Reson2 = hold.Reson2,
                        Type = HOLD_TYPE,
                        Note1 = hold.Note1,
                        Reserved = (Reserved.ToUpper() == "ON") ? 1 : 0
                    };

                    _context.Hold_CHQ.Add(newHold);
                    await _context.SaveChangesAsync();
                    return true; // Indicate success
                }
                else
                {
                    return false; // Indicate that cheque already exists
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SaveHold_CHQ service method.");
                _logger.LogError(ex, "Error in SaveHold_CHQ service method.");
                return false; // Indicate failure
            }
        }

        // Dummy method for BindHoldType, needs actual implementation based on VB.NET
        private async Task<List<SelectListItem>> BindHoldType()
        {
            // Implement actual logic to bind hold types from DB or other source
            return await Task.FromResult(new List<SelectListItem>
            {
                new SelectListItem { Value = "Type1", Text = "Hold Type 1" },
                new SelectListItem { Value = "Type2", Text = "Hold Type 2" }
            });
        }

        // Dummy method for GetAllCategoriesForTree, needs actual implementation based on VB.NET
        public async Task<List<TreeNode>> GetAllCategoriesForTree()
        {
            // Implement actual logic to get tree nodes
            return await Task.FromResult(new List<TreeNode>
            {
                new TreeNode { id = "1", parent = "#", text = "Root" },
                new TreeNode { id = "2", parent = "1", text = "Child 1" }
            });
        }

        public async Task<object> ReturnDiscountChq()
        {
            // Placeholder for actual implementation
            return await Task.FromResult(new object());
        }

        public async Task<string> Get_Deacrypted_Account(string Drw_Account, string ChqNo)
        {
            string result = "";
            try
            {
                _logSystem.WriteTraceLogg($"get Get_Deacrypted_Account", _applicationID, GetType().Name, GetMethodName(), "", "", "", "", "");

                // Assuming CONNECTION class and Get_One_Data method are available or have C# equivalents
                // This part needs careful conversion based on the actual CONNECTION class implementation
                // For now, return a dummy value
                result = "DecryptedAccount";
            }
            catch (Exception ex)
            {
                _logSystem.WriteError($"Error in Get_Deacrypted_Account: {ex.Message}", _applicationID, GetType().Name, GetMethodName(), "", "", "", "", "");
                _logger.LogError(ex, "Error in Get_Deacrypted_Account service method.");
                result = "Error";
            }
            return result;
        }

        public async Task<RejectedOutRequestViewModel> Rejected_Out_Request()
        {
            // Placeholder for actual implementation
            return await Task.FromResult(new RejectedOutRequestViewModel());
        }

        public async Task<OutChqsViewModel> RepresnetDisDetails(string id)
        {
            // Placeholder for actual implementation
            return await Task.FromResult(new OutChqsViewModel());
        }

        public async Task<OutwordDateVerificationViewModel> Out_VerficationDetails(string id)
        {
            // Placeholder for actual implementation
            return await Task.FromResult(new OutwordDateVerificationViewModel());
        }

        public async Task<object> OUTWORD()
        {
            // Placeholder for actual implementation
            return await Task.FromResult(new object());
        }

        public async Task<DataTable> Get_Post_Rest_Code(string CUSTOMER_ID, string ACCOUNT_NUMBER)
        {
            // Placeholder for actual implementation
            return await Task.FromResult(new DataTable());
        }

        public async Task<string> Get_Final_Posting_Restrection(int Customer_Post_Rest, int Acc_Post_Rest, int Language)
        {
            // Placeholder for actual implementation
            return await Task.FromResult("FinalPostingRestriction");
        }

        public async Task<PenddingOutWordRequestViewModel> Pendding_OutWord_Request()
        {
            // Placeholder for actual implementation
            return await Task.FromResult(new PenddingOutWordRequestViewModel());
        }

        public async Task<object> Pendding_OutWord_Request_Auth()
        {
            // Placeholder for actual implementation
            return await Task.FromResult(new object());
        }

        public async Task<object> getOutword_WF_Details()
        {
            // Placeholder for actual implementation
            return await Task.FromResult(new object());
        }

        public async Task getuser_group_permision(string pageid, string applicationid, string userid)
        {
            // Placeholder for actual implementation
            await Task.CompletedTask;
        }

        public async Task<DataTable> Getpage(string page)
        {
            // Placeholder for actual implementation
            return await Task.FromResult(new DataTable());
        }

        public async Task<bool> Ge_t(string x)
        {
            // Placeholder for actual implementation
            return await Task.FromResult(false);
        }

        public string getlist(string x)
        {
            // Placeholder for actual implementation
            return "List";
        }

        public async Task<string> GENERATE_UNIQUE_CHEQUE_SEQUANCE(string CHEQUE_NO, string BANK_NO, string BRANCH_NO, string DRAWEE_NO)
        {
            // Placeholder for actual implementation
            return await Task.FromResult("UniqueChequeSequence");
        }

        public async Task<object> getlockedpage(int pageid)
        {
            // Placeholder for actual implementation
            return await Task.FromResult(new object());
        }

        public async Task<string> EVALUATE_AMOUNT_IN_JOD(string CURANCY, double AMOUNT)
        {
            // Placeholder for actual implementation
            return await Task.FromResult("EvaluatedAmount");
        }

        public async Task<string> GetCurrencyCode(string Currency_Symbol)
        {
            // Placeholder for actual implementation
            return await Task.FromResult("CurrencyCode");
        }

        public async Task<bool> Update_ChqDate(UpdateChqDateViewModel model, string userName, int userId)
        {
            // Placeholder for actual implementation
            return await Task.FromResult(false);
        }

        public async Task<bool> Update_ChqDate_Post(string serial, DateTime dueDate, string userName, int userId)
        {
            // Placeholder for actual implementation
            return await Task.FromResult(false);
        }

        public async Task<object> Update_Out_ChqDate(UpdateChqDateViewModel model)
        {
            // Placeholder for actual implementation
            return await Task.FromResult(new object());
        }

        public async Task<object> Update_Out_ChqDate_Accept(UpdateChqDateViewModel model)
        {
            // Placeholder for actual implementation
            return await Task.FromResult(new object());
        }

        public async Task<object> getSearchListDis_PMA(string Branchs, string STATUS, string BenAccNo, string AccType, string FromBank, string ToBank, string Currency, string ChequeSource, string Amount, string DRWAccNo, string ChequeNo, string waspdc)
        {
            // Placeholder for actual implementation
            return await Task.FromResult(new object());
        }

        public async Task<object> getSearchList(string Currency, string ChequeSource, string WASPDC, string Branchs, string order, string inputerr, string ChequeStatus, string vip)
        {
            // Placeholder for actual implementation
            return await Task.FromResult(new object());
        }

        public async Task<object> GetTotalPerAccountAndBnk(string ChqSrc, string Cur, string Branchs, string WASPDC, string order, string inputerr)
        {
            // Placeholder for actual implementation
            return await Task.FromResult(new object());
        }

        public async Task<bool> PresentmentDIS_Or_PDC_return(Outward_Trans _out_, string CHQ)
        {
            // Placeholder for actual implementation
            return await Task.FromResult(false);
        }

        public async Task<bool> PresentmentDIS_Or_PDC_timeout(Outward_Trans _out_, string CHQ)
        {
            // Placeholder for actual implementation
            return await Task.FromResult(true);
        }

        public async Task<bool> PresentmentPMA_OR_PDC(Outward_Trans _out_, string CHQ)
        {
            // Placeholder for actual implementation
            return await Task.FromResult(true);
        }

        public async Task<OutChqsViewModel> outward_views(string id, string userName, int userId)
        {
            // Placeholder for actual implementation
            return await Task.FromResult(new OutChqsViewModel());
        }

        public async Task<OutChqsViewModel> Update_oUTWORD_Details(string id)
        {
            // Placeholder for actual implementation
            return await Task.FromResult(new OutChqsViewModel());
        }

        public async Task<string> getDocType(int id)
        {
            // Placeholder for actual implementation
            return await Task.FromResult("DocType");
        }

        public async Task<object> update_Post_Outword(string id)
        {
            // Placeholder for actual implementation
            return await Task.FromResult(new object());
        }

        public async Task<object> update_Post_Outword_Post(string id, string Note, string returnchq, string chqNu, string chq, string chqAm, string chqBn, string chqBr, string chqAc, string chqdate, string reas, string reas1, string reas2, string reas3, string reas4, string reas5, string reas6, string reas7, string reas8, string reas9, string reas10, string reas11, string reas12, string reas13, string reas14, string reas15, string reas16, string reas17, string reas18, string reas19, string reas20, string reas21, string reas22, string reas23, string reas24, string reas25, string reas26, string reas27, string reas28, string reas29, string reas30, string reas31, string reas32, string reas33, string reas34, string reas35, string reas36, string reas37, string reas38, string reas39, string reas40, string reas41, string reas42, string reas43, string reas44, string reas45, string reas46, string reas47, string reas48, string reas49, string reas50, string reas51, string reas52, string reas53, string reas54, string reas55, string reas56, string reas57, string reas58, string reas59, string reas60, string reas61, string reas62, string reas63, string reas64, string reas65, string reas66, string reas67, string reas68, string reas69, string reas70, string reas71, string reas72, string reas73, string reas74, string reas75, string reas76, string reas77, string reas78, string reas79, string reas80, string reas81, string reas82, string reas83, string reas84, string reas85, string reas86, string reas87, string reas88, string reas89, string reas90, string reas91, string reas92, string reas93, string reas94, string reas95, string reas96, string reas97, string reas98, string reas99)
        {
            // Placeholder for actual implementation
            return await Task.FromResult(new object());
        }

        public async Task<object> Return_Owtward_chq(string id)
        {
            // Placeholder for actual implementation
            return await Task.FromResult(new object());
        }

        public async Task<object> Return_Owtward_chq_Post(string id, string Note, string returnchq, string chqNu, string chq, string chqAm, string chqBn, string chqBr, string chqAc, string chqdate, string reas, string reas1, string reas2, string reas3, string reas4, string reas5, string reas6, string reas7, string reas8, string reas9, string reas10, string reas11, string reas12, string reas13, string reas14, string reas15, string reas16, string reas17, string reas18, string reas19, string reas20, string reas21, string reas22, string reas23, string reas24, string reas25, string reas26, string reas27, string reas28, string reas29, string reas30, string reas31, string reas32, string reas33, string reas34, string reas35, string reas36, string reas37, string reas38, string reas39, string reas40, string reas41, string reas42, string reas43, string reas44, string reas45, string reas46, string reas47, string reas48, string reas49, string reas50, string reas51, string reas52, string reas53, string reas54, string reas55, string reas56, string reas57, string reas58, string reas59, string reas60, string reas61, string reas62, string reas63, string reas64, string reas65, string reas66, string reas67, string reas68, string reas69, string reas70, string reas71, string reas72, string reas73, string reas74, string reas75, string reas76, string reas77, string reas78, string reas79, string reas80, string reas81, string reas82, string reas83, string reas84, string reas85, string reas86, string reas87, string reas88, string reas89, string reas90, string reas91, string reas92, string reas93, string reas94, string reas95, string reas96, string reas97, string reas98, string reas99)
        {
            // Placeholder for actual implementation
            return await Task.FromResult(new object());
        }

        public async Task<object> Return_Owtward_chq_list()
        {
            // Placeholder for actual implementation
            return await Task.FromResult(new object());
        }

        public async Task<object> Return_Owtward_chq_list_Search(string Branchs, string STATUS, string BenAccNo, string AccType, string FromBank, string ToBank, string Currency, string ChequeSource, string Amount, string DRWAccNo, string ChequeNo, string waspdc)
        {
            // Placeholder for actual implementation
            return await Task.FromResult(new object());
        }

        public async Task<object> Return_Onus_chq(string id)
        {
            // Placeholder for actual implementation
            return await Task.FromResult(new object());
        }

        public async Task<object> Return_Onus_chq_Post(string id, string Note, string returnchq, string chqNu, string chq, string chqAm, string chqBn, string chqBr, string chqAc, string chqdate, string reas, string reas1, string reas2, string reas3, string reas4, string reas5, string reas6, string reas7, string reas8, string reas9, string reas10, string reas11, string reas12, string reas13, string reas14, string reas15, string reas16, string reas17, string reas18, string reas19, string reas20, string reas21, string reas22, string reas23, string reas24, string reas25, string reas26, string reas27, string reas28, string reas29, string reas30, string reas31, string reas32, string reas33, string reas34, string reas35, string reas36, string reas37, string reas38, string reas39, string reas40, string reas41, string reas42, string reas43, string reas44, string reas45, string reas46, string reas47, string reas48, string reas49, string reas50, string reas51, string reas52, string reas53, string reas54, string reas55, string reas56, string reas57, string reas58, string reas59, string reas60, string reas61, string reas62, string reas63, string reas64, string reas65, string reas66, string reas67, string reas68, string reas69, string reas70, string reas71, string reas72, string reas73, string reas74, string reas75, string reas76, string reas77, string reas78, string reas79, string reas80, string reas81, string reas82, string reas83, string reas84, string reas85, string reas86, string reas87, string reas88, string reas89, string reas90, string reas91, string reas92, string reas93, string reas94, string reas95, string reas96, string reas97, string reas98, string reas99)
        {
            // Placeholder for actual implementation
            return await Task.FromResult(new object());
        }

        public async Task<GetOutwardSlipCCSViewModel> Get_Outward_Slip_CCS(string slip_id)
        {
            // Placeholder for actual implementation
            return await Task.FromResult(new GetOutwardSlipCCSViewModel());
        }

        public async Task<string> Get_URL(string slip_id)
        {
            // Placeholder for actual implementation
            return await Task.FromResult("URL");
        }

        public async Task<string> Get_Account_List(string CUST_NO, int Language)
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AccountList");
        }

        public async Task<string> Get_Cheque_Info(string Account_No, int Cheque_No, int Language)
        {
            // Placeholder for actual implementation
            return await Task.FromResult("ChequeInfo");
        }

        public async Task<string> Convert_Numbers_To_Words_AR(string _Number)
        {
            // Placeholder for actual implementation
            return await Task.FromResult("Words");
        }

        public async Task<OutwordSearchViewModel> OutwardSearch()
        {
            // Placeholder for actual implementation
            return await Task.FromResult(new OutwordSearchViewModel());
        }

        public async Task<object> OutwardSearch(OutwardSearchClass search)
        {
            // Placeholder for actual implementation
            return await Task.FromResult(new object());
        }

        public async Task<object> GetTotalPerAccount(string ChqSrc, string Cur, string Branchs, string WASPDC, string order, string inputerr, string ChequeStatus, string vip)
        {
            // Placeholder for actual implementation
            return await Task.FromResult(new object());
        }

        public async Task<object> GetTotalPerBnk(string ChqSrc, string Cur, string Branchs, string WASPDC, string order, string inputerr, string ChequeStatus, string vip)
        {
            // Placeholder for actual implementation
            return await Task.FromResult(new object());
        }

        public async Task Delete(string id)
        {
            // Placeholder for actual implementation
            await Task.CompletedTask;
        }

        public async Task Delete_Outward_Trans_Discount_Old(string id)
        {
            // Placeholder for actual implementation
            await Task.CompletedTask;
        }

        public async Task Auth_Tran(string id, string status)
        {
            // Placeholder for actual implementation
            await Task.CompletedTask;
        }

        public async Task Resend_Request(string id)
        {
            // Placeholder for actual implementation
            await Task.CompletedTask;
        }

        public async Task<object> GetSlip(string id)
        {
            // Placeholder for actual implementation
            return await Task.FromResult(new object());
        }

        public async Task<byte[]> PrintAll(string[] Ids)
        {
            // Placeholder for actual implementation
            return await Task.FromResult(new byte[0]);
        }

        public async Task<object> PrintOutwordRecipt()
        {
            // Placeholder for actual implementation
            return await Task.FromResult(new object());
        }

        public async Task<object> getCustomerAccounts(string customer_number)
        {
            // Placeholder for actual implementation
            return await Task.FromResult(new object());
        }

        public async Task<object> validatebranch(string brnch, string Bnk)
        {
            // Placeholder for actual implementation
            return await Task.FromResult(new object());
        }

        public async Task<object> validatebank(string Bnk)
        {
            // Placeholder for actual implementation
            return await Task.FromResult(new object());
        }

        public async Task<object> PrintCheques(string _customerID, string _accountNo, string _Slides)
        {
            // Placeholder for actual implementation
            return await Task.FromResult(new object());
        }

        public async Task<string> Get_OFS_HttpLink()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("OFS_HttpLink");
        }

        public async Task<string> GetMethodName()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("MethodName");
        }

        public async Task<string> Get_Auto_Post_Flag()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostFlag");
        }

        public async Task<string> Get_Auto_Post_Time()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostTime");
        }

        public async Task<string> Get_Auto_Post_User()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostUser");
        }

        public async Task<string> Get_Auto_Post_Pass()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostPass");
        }

        public async Task<string> Get_Auto_Post_Company()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostCompany");
        }

        public async Task<string> Get_Auto_Post_Dept()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostDept");
        }

        public async Task<string> Get_Auto_Post_Trans_Code()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostTransCode");
        }

        public async Task<string> Get_Auto_Post_Dr_Acct()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostDrAcct");
        }

        public async Task<string> Get_Auto_Post_Cr_Acct()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostCrAcct");
        }

        public async Task<string> Get_Auto_Post_Dr_Ccy()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostDrCcy");
        }

        public async Task<string> Get_Auto_Post_Cr_Ccy()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostCrCcy");
        }

        public async Task<string> Get_Auto_Post_Dr_Amt()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostDrAmt");
        }

        public async Task<string> Get_Auto_Post_Cr_Amt()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostCrAmt");
        }

        public async Task<string> Get_Auto_Post_Dr_Val_Date()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostDrValDate");
        }

        public async Task<string> Get_Auto_Post_Cr_Val_Date()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostCrValDate");
        }

        public async Task<string> Get_Auto_Post_Dr_Txn_Code()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostDrTxnCode");
        }

        public async Task<string> Get_Auto_Post_Cr_Txn_Code()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostCrTxnCode");
        }

        public async Task<string> Get_Auto_Post_Dr_Nar()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostDrNar");
        }

        public async Task<string> Get_Auto_Post_Cr_Nar()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostCrNar");
        }

        public async Task<string> Get_Auto_Post_Dr_Ref()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostDrRef");
        }

        public async Task<string> Get_Auto_Post_Cr_Ref()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostCrRef");
        }

        public async Task<string> Get_Auto_Post_Dr_Rate()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostDrRate");
        }

        public async Task<string> Get_Auto_Post_Cr_Rate()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostCrRate");
        }

        public async Task<string> Get_Auto_Post_Dr_Branch()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostDrBranch");
        }

        public async Task<string> Get_Auto_Post_Cr_Branch()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostCrBranch");
        }

        public async Task<string> Get_Auto_Post_Dr_Cus()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostDrCus");
        }

        public async Task<string> Get_Auto_Post_Cr_Cus()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostCrCus");
        }

        public async Task<string> Get_Auto_Post_Dr_Acct_Officer()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostDrAcctOfficer");
        }

        public async Task<string> Get_Auto_Post_Cr_Acct_Officer()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostCrAcctOfficer");
        }

        public async Task<string> Get_Auto_Post_Dr_Cus_Cat()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostDrCusCat");
        }

        public async Task<string> Get_Auto_Post_Cr_Cus_Cat()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostCrCusCat");
        }

        public async Task<string> Get_Auto_Post_Dr_Cus_Type()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostDrCusType");
        }

        public async Task<string> Get_Auto_Post_Cr_Cus_Type()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostCrCusType");
        }

        public async Task<string> Get_Auto_Post_Dr_Cus_Nat()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostDrCusNat");
        }

        public async Task<string> Get_Auto_Post_Cr_Cus_Nat()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostCrCusNat");
        }

        public async Task<string> Get_Auto_Post_Dr_Cus_Res()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostDrCusRes");
        }

        public async Task<string> Get_Auto_Post_Cr_Cus_Res()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostCrCusRes");
        }

        public async Task<string> Get_Auto_Post_Dr_Cus_Ind()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostDrCusInd");
        }

        public async Task<string> Get_Auto_Post_Cr_Cus_Ind()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostCrCusInd");
        }

        public async Task<string> Get_Auto_Post_Dr_Cus_Sec()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostDrCusSec");
        }

        public async Task<string> Get_Auto_Post_Cr_Cus_Sec()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostCrCusSec");
        }

        public async Task<string> Get_Auto_Post_Dr_Cus_Tar()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostDrCusTar");
        }

        public async Task<string> Get_Auto_Post_Cr_Cus_Tar()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostCrCusTar");
        }

        public async Task<string> Get_Auto_Post_Dr_Cus_Cap()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostDrCusCap");
        }

        public async Task<string> Get_Auto_Post_Cr_Cus_Cap()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostCrCusCap");
        }

        public async Task<string> Get_Auto_Post_Dr_Cus_Cntry()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostDrCusCntry");
        }

        public async Task<string> Get_Auto_Post_Cr_Cus_Cntry()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostCrCusCntry");
        }

        public async Task<string> Get_Auto_Post_Dr_Cus_City()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostDrCusCity");
        }

        public async Task<string> Get_Auto_Post_Cr_Cus_City()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostCrCusCity");
        }

        public async Task<string> Get_Auto_Post_Dr_Cus_St()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostDrCusSt");
        }

        public async Task<string> Get_Auto_Post_Cr_Cus_St()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostCrCusSt");
        }

        public async Task<string> Get_Auto_Post_Dr_Cus_Zip()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostDrCusZip");
        }

        public async Task<string> Get_Auto_Post_Cr_Cus_Zip()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostCrCusZip");
        }

        public async Task<string> Get_Auto_Post_Dr_Cus_Phn()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostDrCusPhn");
        }

        public async Task<string> Get_Auto_Post_Cr_Cus_Phn()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostCrCusPhn");
        }

        public async Task<string> Get_Auto_Post_Dr_Cus_Fax()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostDrCusFax");
        }

        public async Task<string> Get_Auto_Post_Cr_Cus_Fax()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostCrCusFax");
        }

        public async Task<string> Get_Auto_Post_Dr_Cus_Email()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostDrCusEmail");
        }

        public async Task<string> Get_Auto_Post_Cr_Cus_Email()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostCrCusEmail");
        }

        public async Task<string> Get_Auto_Post_Dr_Cus_Po()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostDrCusPo");
        }

        public async Task<string> Get_Auto_Post_Cr_Cus_Po()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostCrCusPo");
        }

        public async Task<string> Get_Auto_Post_Dr_Cus_Add()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostDrCusAdd");
        }

        public async Task<string> Get_Auto_Post_Cr_Cus_Add()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostCrCusAdd");
        }

        public async Task<string> Get_Auto_Post_Dr_Cus_Add2()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostDrCusAdd2");
        }

        public async Task<string> Get_Auto_Post_Cr_Cus_Add2()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostCrCusAdd2");
        }

        public async Task<string> Get_Auto_Post_Dr_Cus_Add3()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostDrCusAdd3");
        }

        public async Task<string> Get_Auto_Post_Cr_Cus_Add3()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostCrCusAdd3");
        }

        public async Task<string> Get_Auto_Post_Dr_Cus_Add4()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostDrCusAdd4");
        }

        public async Task<string> Get_Auto_Post_Cr_Cus_Add4()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostCrCusAdd4");
        }

        public async Task<string> Get_Auto_Post_Dr_Cus_Add5()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostDrCusAdd5");
        }

        public async Task<string> Get_Auto_Post_Cr_Cus_Add5()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostCrCusAdd5");
        }

        public async Task<string> Get_Auto_Post_Dr_Cus_Add6()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostDrCusAdd6");
        }

        public async Task<string> Get_Auto_Post_Cr_Cus_Add6()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostCrCusAdd6");
        }

        public async Task<string> Get_Auto_Post_Dr_Cus_Add7()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostDrCusAdd7");
        }

        public async Task<string> Get_Auto_Post_Cr_Cus_Add7()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostCrCusAdd7");
        }

        public async Task<string> Get_Auto_Post_Dr_Cus_Add8()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostDrCusAdd8");
        }

        public async Task<string> Get_Auto_Post_Cr_Cus_Add8()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostCrCusAdd8");
        }

        public async Task<string> Get_Auto_Post_Dr_Cus_Add9()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostDrCusAdd9");
        }

        public async Task<string> Get_Auto_Post_Cr_Cus_Add9()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostCrCusAdd9");
        }

        public async Task<string> Get_Auto_Post_Dr_Cus_Add10()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostDrCusAdd10");
        }

        public async Task<string> Get_Auto_Post_Cr_Cus_Add10()
        {
            // Placeholder for actual implementation
            return await Task.FromResult("AutoPostCrCusAdd10");
        }

        public async Task<ReturnOwtwardViewModel> ReturnOwtward(string userName, int userId, string groupId)
        {
            // Placeholder for actual implementation
            return await Task.FromResult(new ReturnOwtwardViewModel());
        }

        public async Task<List<ReturnedChequeViewModel>> retunedchqstate(string ChequeSource, string ChequeStat, string drqchqnumber, string drwAccNo, string Fromdate, string Todate, string BenfBranch, string BnfAccNo, string userName, int userId, string companyId, string branchId)
        {
            // Placeholder for actual implementation
            return await Task.FromResult(new List<ReturnedChequeViewModel>());
        }

        public async Task<List<Outward_Trans>> getPospondingChq(string ChequeSource, string Bank, string UnlockDate, string BenfBranch, string filedate, string CHQStatus, string CHQSt, string BenfrBranch, string userName, int userId, string companyId, string branchId)
        {
            // Placeholder for actual implementation
            return await Task.FromResult(new List<Outward_Trans>());
        }

        public async Task<List<ReturnedChequeDetailsViewModel>> getreturnList(string ClrCenter, string STATUS, string TransDate, string chqNo, string payAcc, string userName, int userId, string companyId, string branchId)
        {
            // Placeholder for actual implementation
            return await Task.FromResult(new List<ReturnedChequeDetailsViewModel>());
        }

        public async Task<ReturnOWTWORDScreenViewModel> ReturnOWTWORDScreen(string serial, string RC, string userName, int userId, string companyId, string branchId, string pageId)
        {
            // Placeholder for actual implementation
            return await Task.FromResult(new ReturnOWTWORDScreenViewModel());
        }

        public async Task<List<SelectListItem>> FillClearCenter()
        {
            // Placeholder for actual implementation
            return await Task.FromResult(new List<SelectListItem>());
        }

        public async Task<List<SelectListItem>> FillClearCenterout()
        {
            // Placeholder for actual implementation
            return await Task.FromResult(new List<SelectListItem>());
        }

        public async Task<List<Outward_Trans>> getMagicscreenList(string ClrCenter, string TransDate, string chqNo, string userName, int userId, string companyId, string branchId)
        {
            // Placeholder for actual implementation
            return await Task.FromResult(new List<Outward_Trans>());
        }

        public async Task<string> savepostedstatus(string serial, string TBLNAME, string posted, string userName, int userId)
        {
            // Placeholder for actual implementation
            return await Task.FromResult("PostedStatus");
        }

        public async Task<AllInwardOutwardChqViewModel> AllInwardoutwardChq(string Currency, string FromDate, string ToDate, string vip, string userName, int userId)
        {
            // Placeholder for actual implementation
            return await Task.FromResult(new AllInwardOutwardChqViewModel());
        }

        public async Task<AllInwOutChqViewModel> All_INW_OUT_CHQ(string Currency, string FromDate, string ToDate, string vip, string userName, int userId)
        {
            // Placeholder for actual implementation
            return await Task.FromResult(new AllInwOutChqViewModel());
        }

        public async Task<string> deleteoutchq(string serial, string userName, int userId)
        {
            // Placeholder for actual implementation
            return await Task.FromResult("DeleteOutChqResult");
        }

        public async Task<string> deletetimeoutchq(string serial, string userName, int userId)
        {
            // Placeholder for actual implementation
            return await Task.FromResult("DeleteTimeoutChqResult");
        }

        public async Task<RepresentReturnDisViewModel> RepresentReturnDis(string serial, string userName, int userId)
        {
            // Placeholder for actual implementation
            return await Task.FromResult(new RepresentReturnDisViewModel());
        }

        public async Task<FindChqViewModel> FindChq(string serial, string userName, int userId)
        {
            // Placeholder for actual implementation
            return await Task.FromResult(new FindChqViewModel());
        }

        public async Task<FindChqdisViewModel> FindChqdis(string serial, string userName, int userId)
        {
            // Placeholder for actual implementation
            return await Task.FromResult(new FindChqdisViewModel());
        }

        public async Task<UpdateChqDateRepresnetViewModel> Update_ChqDate_Represnet(string serial, string userName, int userId)
        {
            // Placeholder for actual implementation
            return await Task.FromResult(new UpdateChqDateRepresnetViewModel());
        }

        public async Task<PospondingChqViewModel> PospondingChq(string serial, string userName, int userId)
        {
            // Placeholder for actual implementation
            return await Task.FromResult(new PospondingChqViewModel());
        }
    }
}



        public async Task<DataTable> Getpage(string page)
        {
            var dt = new DataTable();
            var conn = _context.Database.GetDbConnection();
            await conn.OpenAsync();
            var command = conn.CreateCommand();
            command.CommandText = $"SELECT * FROM Pages WHERE Page_ID = {page}";
            var reader = await command.ExecuteReaderAsync();
            dt.Load(reader);
            reader.Close();
            return dt;
        }

