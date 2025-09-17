
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SAFA_ECC_Core_Clean.Data;
using SAFA_ECC_Core_Clean.Models;
using SAFA_ECC_Core_Clean.ViewModels.OutwordViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SAFA_ECC_Core_Clean.Services
{
    public class OutwordService : IOutwordService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<OutwordService> _logger;
        private readonly IConfiguration _configuration;
        private readonly LogSystem _logSystem; // Assuming LogSystem is a utility class
        private readonly int _applicationID = 1; // Example Application ID
        private string _loggMessage = "";
        private string userName;
        private int userId;

        public OutwordService(ApplicationDbContext context, ILogger<OutwordService> logger, IConfiguration configuration, LogSystem logSystem)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
            _logSystem = logSystem;
        }

        private string Get_OFS_HttpLink()
        {
            // This method should retrieve the OFS_HttpLink from configuration or database
            // For now, returning a placeholder
            return _configuration["OFS_HttpLink"];
        }

        public async Task<CheckImgViewModel> check_img(string serial, string userName, int userId)
        {
            _logger.LogInformation($"Executing check_img for serial: {serial}, user: {userName}");
            this.userName = userName;
            this.userId = userId;

            var viewModel = new CheckImgViewModel();
            try
            {
                var outwardTrans = await _context.Outward_Trans.SingleOrDefaultAsync(x => x.Serial == serial);
                if (outwardTrans != null)
                {
                    viewModel.HasImage = outwardTrans.HasImage == 1;
                }
                else
                {
                    viewModel.ErrorMessage = "Cheque not found.";
                }
            }
            catch (Exception ex)
            {
                _loggMessage = $"Error in check_img for serial: {serial}: {ex.Message}";
                _logSystem.WriteError(_loggMessage, _applicationID, GetType().Name, MethodBase.GetCurrentMethod().Name, userName, userId.ToString(), "", "", "");
                _logger.LogError(ex, _loggMessage);
                viewModel.ErrorMessage = "An error occurred: " + ex.Message;
            }
            return viewModel;
        }

        public async Task<bool> getPermission(string pageId, string userName, int userId)
        {
            _logger.LogInformation($"Executing getPermission for pageId: {pageId}, user: {userName}");
            this.userName = userName;
            this.userId = userId;

            try
            {
                // Assuming Auth_Tran_Details_TBL contains permission logic
                // This is a simplified example, actual implementation might involve complex roles/permissions
                var permission = await _context.Auth_Tran_Details_TBL.AnyAsync(x => x.Page_Id == pageId && x.User_Id == userId);
                return permission;
            }
            catch (Exception ex)
            {
                _loggMessage = $"Error in getPermission for pageId: {pageId}: {ex.Message}";
                _logSystem.WriteError(_loggMessage, _applicationID, GetType().Name, MethodBase.GetCurrentMethod().Name, userName, userId.ToString(), "", "", "");
                _logger.LogError(ex, _loggMessage);
                return false;
            }
        }

        public async Task<bool> getPermission1(string pageId, string userName, int userId)
        {
            _logger.LogInformation($"Executing getPermission1 for pageId: {pageId}, user: {userName}");
            this.userName = userName;
            this.userId = userId;

            try
            {
                // Similar to getPermission, but might have different logic
                var permission = await _context.Auth_Tran_Details_TBL.AnyAsync(x => x.Page_Id == pageId && x.User_Id == userId);
                return permission;
            }
            catch (Exception ex)
            {
                _loggMessage = $"Error in getPermission1 for pageId: {pageId}: {ex.Message}";
                _logSystem.WriteError(_loggMessage, _applicationID, GetType().Name, MethodBase.GetCurrentMethod().Name, userName, userId.ToString(), "", "", "");
                _logger.LogError(ex, _loggMessage);
                return false;
            }
        }

        public async Task<Hold_CHQViewModel> Hold_CHQ_Get(string serial, string userName, int userId)
        {
            _logger.LogInformation($"Executing Hold_CHQ_Get for serial: {serial}, user: {userName}");
            this.userName = userName;
            this.userId = userId;

            var viewModel = new Hold_CHQViewModel();
            try
            {
                var holdChq = await _context.Hold_CHQ.SingleOrDefaultAsync(x => x.Serial == serial);
                if (holdChq != null)
                {
                    viewModel.Serial = holdChq.Serial;
                    viewModel.HoldReason = holdChq.HoldReason;
                    viewModel.HoldDate = holdChq.HoldDate;
                    viewModel.HoldBy = holdChq.HoldBy;
                }
                else
                {
                    viewModel.ErrorMessage = "Hold record not found.";
                }
            }
            catch (Exception ex)
            {
                _loggMessage = $


        public async Task<AllInwOutChqViewModel> All_INW_OUT_CHQ(string Currency, string FromDate, string ToDate, string vip, string userName, int userId)
        {
            _logger.LogInformation($"Executing All_INW_OUT_CHQ for user: {userName}, Currency: {Currency}, FromDate: {FromDate}, ToDate: {ToDate}, VIP: {vip}");
            this.userName = userName;
            this.userId = userId;

            var viewModel = new AllInwOutChqViewModel();
            viewModel.ReturnedAmountOut = new List<Outward_Trans>();
            viewModel.ReturnedAmountInw = new List<Inward_Trans>();
            viewModel.ClearedAmountInw = new List<Inward_Trans>();
            viewModel.OutImgs = new List<Outward_Imgs>(); // Assuming this is also part of the returned data

            try
            {
                if (string.IsNullOrEmpty(Currency) || string.IsNullOrEmpty(FromDate) || string.IsNullOrEmpty(ToDate))
                {
                    viewModel.ErrorMsg = "Please Fill All Data";
                    return viewModel;
                }

                string formattedFromDate = DateTime.Parse(FromDate).ToString("yyyyMMdd");
                string formattedToDate = DateTime.Parse(ToDate).ToString("yyyyMMdd");

                string retoutQuery = "";
                string clroutQuery = "";
                string alloutQuery = "";
                string retinwQuery = "";
                string clrinwQuery = "";
                string allinQuery = "";

                string vipCondition = "";
                if (vip == "Yes")
                {
                    vipCondition = "IsVIP=1";
                }
                else if (vip == "No")
                {
                    vipCondition = "IsVIP=0";
                }
                else
                {
                    vipCondition = "1=1"; // No VIP filter
                }

                // Outward Queries
                retoutQuery = $"SELECT ISNULL(SUM(Amount), 0) AS Amount FROM Outward_Trans WHERE {vipCondition} AND ClrCenter = \'PMA\' AND ISNULL(QVFStatus ,\'Pending\')<>\'Pending\' AND ISNULL(QVFStatus ,\'RJCT\')<>\'RJCT\' AND Posted IN ({(int)AllEnums.Cheque_Status.Returne}, {(int)AllEnums.Cheque_Status.Returned})";
                clroutQuery = $"SELECT ISNULL(SUM(Amount), 0) AS Amount FROM Outward_Trans WHERE {vipCondition} AND ClrCenter = \'PMA\' AND ISNULL(QVFStatus ,\'Pending\')<>\'Pending\' AND PMAstatus=\'Exported\' AND Posted = {(int)AllEnums.Cheque_Status.Posted}";
                alloutQuery = $"SELECT ISNULL(SUM(Amount), 0) AS Amount FROM Outward_Trans WHERE {vipCondition} AND ClrCenter = \'PMA\' AND ISNULL(QVFStatus ,\'Pending\')<>\'Pending\' AND PMAstatus=\'Exported\' AND Posted IN ({(int)AllEnums.Cheque_Status.Posted}, {(int)AllEnums.Cheque_Status.Returne}, {(int)AllEnums.Cheque_Status.Returned})";

                // Inward Queries (assuming Inward_Trans also has IsVIP, ClrCenter, QVFStatus, Posted)
                retinwQuery = $"SELECT ISNULL(SUM(Amount), 0) AS Amount FROM Inward_Trans WHERE {vipCondition.Replace("IsVIP", "VIP")} AND ClrCenter = \'PMA\' AND Posted IN ({(int)AllEnums.Cheque_Status.Returne}, {(int)AllEnums.Cheque_Status.Returned})";
                clrinwQuery = $"SELECT ISNULL(SUM(Amount), 0) AS Amount FROM Inward_Trans WHERE {vipCondition.Replace("IsVIP", "VIP")} AND ClrCenter = \'PMA\' AND Posted = {(int)AllEnums.Cheque_Status.Posted}";
                allinQuery = $"SELECT ISNULL(SUM(Amount), 0) AS Amount FROM Inward_Trans WHERE {vipCondition.Replace("IsVIP", "VIP")} AND ClrCenter = \'PMA\' AND Posted IN ({(int)AllEnums.Cheque_Status.Posted}, {(int)AllEnums.Cheque_Status.Returne}, {(int)AllEnums.Cheque_Status.Returned})";

                string dateCondition = "";
                if (formattedFromDate == formattedToDate)
                {
                    dateCondition = $" AND CAST(ValueDate AS DATE) = \'{formattedFromDate}\' ";
                }
                else
                {
                    dateCondition = $" AND CAST(ValueDate AS DATE) >= \'{formattedFromDate}\' AND CAST(ValueDate AS DATE) <= \'{formattedToDate}\' ";
                }

                retoutQuery += dateCondition;
                clroutQuery += dateCondition;
                alloutQuery += dateCondition;
                retinwQuery += dateCondition;
                clrinwQuery += dateCondition;
                allinQuery += dateCondition;

                string currencyCode = "";
                if (Currency != "-1")
                {
                    switch (Currency.Trim())
                    {
                        case "1": currencyCode = "JOD"; break;
                        case "2": currencyCode = "USD"; break;
                        case "3": currencyCode = "ILS"; break;
                        case "5": currencyCode = "EUR"; break;
                        default: currencyCode = Currency.Trim(); break; // Fallback for other currency symbols
                    }
                    string currencyCondition = $" AND Currency = \'{currencyCode}\'";
                    retoutQuery += currencyCondition;
                    clroutQuery += currencyCondition;
                    alloutQuery += currencyCondition;
                    retinwQuery += currencyCondition;
                    clrinwQuery += currencyCondition;
                    allinQuery += currencyCondition;
                }

                // Execute queries and get sums
                viewModel.SumRetOut = await _context.Database.SqlQuery<double>($"{retoutQuery}").SingleOrDefaultAsync();
                viewModel.SumClrOut = await _context.Database.SqlQuery<double>($"{clroutQuery}").SingleOrDefaultAsync();
                viewModel.SumRetInw = await _context.Database.SqlQuery<double>($"{retinwQuery}").SingleOrDefaultAsync();
                viewModel.SumClrInw = await _context.Database.SqlQuery<double>($"{clrinwQuery}").SingleOrDefaultAsync();

                // Calculate total sum
                viewModel.SumTotal = viewModel.SumClrOut + viewModel.SumClrInw;

                // Format amounts based on currency
                string format = (currencyCode == "JOD") ? "N3" : "N2";
                viewModel.SumRetOut = Math.Round(viewModel.SumRetOut, (currencyCode == "JOD") ? 3 : 2);
                viewModel.SumClrOut = Math.Round(viewModel.SumClrOut, (currencyCode == "JOD") ? 3 : 2);
                viewModel.SumRetInw = Math.Round(viewModel.SumRetInw, (currencyCode == "JOD") ? 3 : 2);
                viewModel.SumClrInw = Math.Round(viewModel.SumClrInw, (currencyCode == "JOD") ? 3 : 2);
                viewModel.SumTotal = Math.Round(viewModel.SumTotal, (currencyCode == "JOD") ? 3 : 2);

                // The VB.NET code also had logic to populate lists (ReturnedAmountout, ReturnedAmountinw, clearedAmountin) but it was commented out or incomplete.
                // If these lists are needed, the SQL queries would need to select all columns and map to the respective models.
                // For now, I\'m focusing on the sum calculations as per the VB.NET logic.

                // Example of how to get the full list if needed (this is not directly from the VB.NET sum queries, but implied if lists are returned)
                // viewModel.ReturnedAmountOut = await _context.Outward_Trans.FromSqlRaw(retoutQuery.Replace("ISNULL(SUM(Amount), 0) AS Amount", "*")).ToListAsync();

            }
            catch (Exception ex)
            {
                _loggMessage = $"Error in All_INW_OUT_CHQ for user: {userName}: {ex.Message}";
                _logSystem.WriteError(_loggMessage, _applicationID, GetType().Name, MethodBase.GetCurrentMethod().Name, userName, userId.ToString(), "", "", "");
                _logger.LogError(ex, _loggMessage);
                viewModel.ErrorMsg = "An error occurred: " + ex.Message;
            }
            return viewModel;
        }



        public async Task<AllInwOutChqViewModel> All_INW_OUT_CHQ(string Currency, string FromDate, string ToDate, string vip, string userName, int userId)
        {
            _logger.LogInformation($"Executing All_INW_OUT_CHQ for user: {userName}, Currency: {Currency}, FromDate: {FromDate}, ToDate: {ToDate}, VIP: {vip}");
            this.userName = userName;
            this.userId = userId;

            var viewModel = new AllInwOutChqViewModel();
            viewModel.ReturnedAmountOut = new List<Outward_Trans>();
            viewModel.ReturnedAmountInw = new List<Inward_Trans>();
            viewModel.ClearedAmountInw = new List<Inward_Trans>();
            viewModel.OutImgs = new List<Outward_Imgs>(); // Assuming this is also part of the returned data

            try
            {
                if (string.IsNullOrEmpty(Currency) || string.IsNullOrEmpty(FromDate) || string.IsNullOrEmpty(ToDate))
                {
                    viewModel.ErrorMsg = "Please Fill All Data";
                    return viewModel;
                }

                string formattedFromDate = DateTime.Parse(FromDate).ToString("yyyyMMdd");
                string formattedToDate = DateTime.Parse(ToDate).ToString("yyyyMMdd");

                string retoutQuery = "";
                string clroutQuery = "";
                string alloutQuery = "";
                string retinwQuery = "";
                string clrinwQuery = "";
                string allinQuery = "";

                string vipCondition = "";
                if (vip == "Yes")
                {
                    vipCondition = "IsVIP=1";
                }
                else if (vip == "No")
                {
                    vipCondition = "IsVIP=0";
                }
                else
                {
                    vipCondition = "1=1"; // No VIP filter
                }

                // Outward Queries
                retoutQuery = $"SELECT ISNULL(SUM(Amount), 0) AS Amount FROM Outward_Trans WHERE {vipCondition} AND ClrCenter = \'PMA\' AND ISNULL(QVFStatus ,\'Pending\')<>\'Pending\' AND ISNULL(QVFStatus ,\'RJCT\')<>\'RJCT\' AND Posted IN ({(int)AllEnums.Cheque_Status.Returne}, {(int)AllEnums.Cheque_Status.Returned})";
                clroutQuery = $"SELECT ISNULL(SUM(Amount), 0) AS Amount FROM Outward_Trans WHERE {vipCondition} AND ClrCenter = \'PMA\' AND ISNULL(QVFStatus ,\'Pending\')<>\'Pending\' AND PMAstatus=\'Exported\' AND Posted = {(int)AllEnums.Cheque_Status.Posted}";
                alloutQuery = $"SELECT ISNULL(SUM(Amount), 0) AS Amount FROM Outward_Trans WHERE {vipCondition} AND ClrCenter = \'PMA\' AND ISNULL(QVFStatus ,\'Pending\')<>\'Pending\' AND PMAstatus=\'Exported\' AND Posted IN ({(int)AllEnums.Cheque_Status.Posted}, {(int)AllEnums.Cheque_Status.Returne}, {(int)AllEnums.Cheque_Status.Returned})";

                // Inward Queries (assuming Inward_Trans also has IsVIP, ClrCenter, QVFStatus, Posted)
                retinwQuery = $"SELECT ISNULL(SUM(Amount), 0) AS Amount FROM Inward_Trans WHERE {vipCondition.Replace("IsVIP", "VIP")} AND ClrCenter = \'PMA\' AND Posted IN ({(int)AllEnums.Cheque_Status.Returne}, {(int)AllEnums.Cheque_Status.Returned})";
                clrinwQuery = $"SELECT ISNULL(SUM(Amount), 0) AS Amount FROM Inward_Trans WHERE {vipCondition.Replace("IsVIP", "VIP")} AND ClrCenter = \'PMA\' AND Posted = {(int)AllEnums.Cheque_Status.Posted}";
                allinQuery = $"SELECT ISNULL(SUM(Amount), 0) AS Amount FROM Inward_Trans WHERE {vipCondition.Replace("IsVIP", "VIP")} AND ClrCenter = \'PMA\' AND Posted IN ({(int)AllEnums.Cheque_Status.Posted}, {(int)AllEnums.Cheque_Status.Returne}, {(int)AllEnums.Cheque_Status.Returned})";

                string dateCondition = "";
                if (formattedFromDate == formattedToDate)
                {
                    dateCondition = $" AND CAST(ValueDate AS DATE) = \'{formattedFromDate}\' ";
                }
                else
                {
                    dateCondition = $" AND CAST(ValueDate AS DATE) >= \'{formattedFromDate}\' AND CAST(ValueDate AS DATE) <= \'{formattedToDate}\' ";
                }

                retoutQuery += dateCondition;
                clroutQuery += dateCondition;
                alloutQuery += dateCondition;
                retinwQuery += dateCondition;
                clrinwQuery += dateCondition;
                allinQuery += dateCondition;

                string currencyCode = "";
                if (Currency != "-1")
                {
                    switch (Currency.Trim())
                    {
                        case "1": currencyCode = "JOD"; break;
                        case "2": currencyCode = "USD"; break;
                        case "3": currencyCode = "ILS"; break;
                        case "5": currencyCode = "EUR"; break;
                        default: currencyCode = Currency.Trim(); break; // Fallback for other currency symbols
                    }
                    string currencyCondition = $" AND Currency = \'{currencyCode}\'";
                    retoutQuery += currencyCondition;
                    clroutQuery += currencyCondition;
                    alloutQuery += currencyCondition;
                    retinwQuery += currencyCondition;
                    clrinwQuery += currencyCondition;
                    allinQuery += currencyCondition;
                }

                // Execute queries and get sums
                viewModel.SumRetOut = await _context.Database.SqlQuery<double>($"{retoutQuery}").SingleOrDefaultAsync();
                viewModel.SumClrOut = await _context.Database.SqlQuery<double>($"{clroutQuery}").SingleOrDefaultAsync();
                viewModel.SumRetInw = await _context.Database.SqlQuery<double>($"{retinwQuery}").SingleOrDefaultAsync();
                viewModel.SumClrInw = await _context.Database.SqlQuery<double>($"{clrinwQuery}").SingleOrDefaultAsync();

                // Calculate total sum
                viewModel.SumTotal = viewModel.SumClrOut + viewModel.SumClrInw;

                // Format amounts based on currency
                string format = (currencyCode == "JOD") ? "N3" : "N2";
                viewModel.SumRetOut = Math.Round(viewModel.SumRetOut, (currencyCode == "JOD") ? 3 : 2);
                viewModel.SumClrOut = Math.Round(viewModel.SumClrOut, (currencyCode == "JOD") ? 3 : 2);
                viewModel.SumRetInw = Math.Round(viewModel.SumRetInw, (currencyCode == "JOD") ? 3 : 2);
                viewModel.SumClrInw = Math.Round(viewModel.SumClrInw, (currencyCode == "JOD") ? 3 : 2);
                viewModel.SumTotal = Math.Round(viewModel.SumTotal, (currencyCode == "JOD") ? 3 : 2);

                // The VB.NET code also had logic to populate lists (ReturnedAmountout, ReturnedAmountinw, clearedAmountin) but it was commented out or incomplete.
                // If these lists are needed, the SQL queries would need to select all columns and map to the respective models.
                // For now, I\'m focusing on the sum calculations as per the VB.NET logic.

                // Example of how to get the full list if needed (this is not directly from the VB.NET sum queries, but implied if lists are returned)
                // viewModel.ReturnedAmountOut = await _context.Outward_Trans.FromSqlRaw(retoutQuery.Replace("ISNULL(SUM(Amount), 0) AS Amount", "*")).ToListAsync();

            }
            catch (Exception ex)
            {
                _loggMessage = $"Error in All_INW_OUT_CHQ for user: {userName}: {ex.Message}";
                _logSystem.WriteError(_loggMessage, _applicationID, GetType().Name, MethodBase.GetCurrentMethod().Name, userName, userId.ToString(), "", "", "");
                _logger.LogError(ex, _loggMessage);
                viewModel.ErrorMsg = "An error occurred: " + ex.Message;
            }
            return viewModel;
        }



        public async Task<string> deleteoutchq(string serial, string userName, int userId)
        {
            _logger.LogInformation($"Executing deleteoutchq for serial: {serial}, user: {userName}");
            this.userName = userName;
            this.userId = userId;

            string resultMessage = "";
            try
            {
                var outward = await _context.Outward_Trans.SingleOrDefaultAsync(x => x.Serial == serial);

                if (outward != null)
                {
                    string chqSequance = outward.ChqSequance;
                    try
                    {
                        var deleteout = new Outward_Trans_Deleted
                        {
                            Amount = outward.Amount,
                            Serial = outward.Serial,
                            AuthorizedBy = outward.AuthorizedBy,
                            AuthorizerBranch = outward.AuthorizerBranch,
                            BenAccountNo = outward.BenAccountNo,
                            BenfAccBranch = outward.BenfAccBranch,
                            BenfBnk = outward.BenfBnk,
                            BenfCardId = outward.BenfCardId,
                            BenfCardType = outward.BenfCardType,
                            BenfNationality = outward.BenfNationality,
                            BenName = outward.BenName,
                            ChqSequance = outward.ChqSequance,
                            CHQState = outward.CHQState,
                            CHQStatedate = outward.CHQStatedate,
                            ClrCenter = outward.ClrCenter,
                            ClrFileRecordID = outward.ClrFileRecordID,
                            Commision_Response = outward.Commision_Response,
                            Currency = outward.Currency,
                            DeptNo = outward.DeptNo,
                            DiscountReternedOutImgID = outward.DiscountReternedOutImgID,
                            DrwAcctNo = outward.DrwAcctNo,
                            DrwBankNo = outward.DrwBankNo,
                            DrwBranchExt = outward.DrwBranchExt,
                            DrwBranchNo = outward.DrwBranchNo,
                            DrwCardId = outward.DrwCardId,
                            DrwChqNo = outward.DrwChqNo,
                            DrwName = outward.DrwName,
                            ErrorCode = outward.ErrorCode,
                            ErrorDescription = outward.ErrorDescription,
                            FaildTrans = outward.FaildTrans,
                            History = outward.History + $"|    Chq Deleted By   {userName} At:{DateTime.Now}",
                            InputBrn = outward.InputBrn,
                            InputDate = outward.InputDate,
                            ISSAccount = outward.ISSAccount,
                            IsTimeOut = outward.IsTimeOut,
                            IsVIP = outward.IsVIP,
                            LastUpdate = DateTime.Now,
                            LastUpdateBy = userName,
                            NeedTechnicalVerification = outward.NeedTechnicalVerification,
                            OperType = outward.OperType,
                            PDCChqSequance = outward.PDCChqSequance,
                            PDCSerial = outward.PDCSerial,
                            PMAstatus = outward.PMAstatus,
                            PMAstatusDate = outward.PMAstatusDate,
                            Posted = outward.Posted,
                            QVFAddtlInf = outward.QVFAddtlInf,
                            QVFStatus = outward.QVFStatus,
                            Rejected = outward.Rejected,
                            RepresentSerial = outward.RepresentSerial,
                            Returned = outward.Returned,
                            ReturnedCode = outward.ReturnedCode,
                            ReturnedDate = outward.ReturnedDate,
                            RSFAddtlInf = outward.RSFAddtlInf,
                            RSFStatus = outward.RSFStatus,
                            SpecialHandling = outward.SpecialHandling,
                            Status = outward.Status,
                            System_Aut_Man = outward.System_Aut_Man,
                            Temenos_Message_Series = outward.Temenos_Message_Series,
                            TransCode = outward.TransCode,
                            TransDate = outward.TransDate,
                            UserName = outward.UserName,
                            ValueDate = outward.ValueDate,
                            WasPDC = outward.WasPDC,
                            WithUV = outward.WithUV
                        };

                        _context.Outward_Trans_Deleted.Add(deleteout);
                        await _context.SaveChangesAsync();
                        _logSystem.WriteTraceLogg("BEFORE During Add Deleted chq from out to delete out", _applicationID, GetType().Name, MethodBase.GetCurrentMethod().Name, userName, userId.ToString(), "", "", "");

                        _context.Outward_Trans.Remove(outward);
                        await _context.SaveChangesAsync();

                        var auth = await _context.Auth_Tran_Details_TBL.SingleOrDefaultAsync(i => i.Chq_Serial == chqSequance);
                        if (auth != null)
                        {
                            _context.Auth_Tran_Details_TBL.Remove(auth);
                            await _context.SaveChangesAsync();
                        }
                        resultMessage = "Delete Cheques Done ";
                    }
                    catch (Exception exInner)
                    {
                        _loggMessage = $"Error during deletion process for serial {serial}: {exInner.Message}";
                        _logSystem.WriteTraceLogg(_loggMessage, _applicationID, GetType().Name, MethodBase.GetCurrentMethod().Name, userName, userId.ToString(), "", "", "");
                        _logger.LogError(exInner, _loggMessage);
                        resultMessage = "An error occurred during deletion: " + exInner.Message;
                    }
                }
                else
                {
                    resultMessage = "Cheque not found.";
                }
            }
            catch (Exception ex)
            {
                _loggMessage = $"Error in deleteoutchq for serial {serial}: {ex.Message}";
                _logSystem.WriteError(_loggMessage, _applicationID, GetType().Name, MethodBase.GetCurrentMethod().Name, userName, userId.ToString(), "", "", "");
                _logger.LogError(ex, _loggMessage);
                resultMessage = "Somthing Wrong: " + ex.Message;
            }
            return resultMessage;
        }



        public async Task<string> deletetimeoutchq(string serial, string userName, int userId)
        {
            _logger.LogInformation($"Executing deletetimeoutchq for serial: {serial}, user: {userName}");
            this.userName = userName;
            this.userId = userId;

            string resultMessage = "";
            try
            {
                var outward = await _context.Outward_Trans.SingleOrDefaultAsync(x => x.Serial == serial);

                if (outward != null)
                {
                    string chqSequance = outward.ChqSequance;
                    try
                    {
                        var deleteout = new Outward_Trans_Deleted
                        {
                            Amount = outward.Amount,
                            Serial = outward.Serial,
                            AuthorizedBy = outward.AuthorizedBy,
                            AuthorizerBranch = outward.AuthorizerBranch,
                            BenAccountNo = outward.BenAccountNo,
                            BenfAccBranch = outward.BenfAccBranch,
                            BenfBnk = outward.BenfBnk,
                            BenfCardId = outward.BenfCardId,
                            BenfCardType = outward.BenfCardType,
                            BenfNationality = outward.BenfNationality,
                            BenName = outward.BenName,
                            ChqSequance = outward.ChqSequance,
                            CHQState = outward.CHQState,
                            CHQStatedate = outward.CHQStatedate,
                            ClrCenter = outward.ClrCenter,
                            ClrFileRecordID = outward.ClrFileRecordID,
                            Commision_Response = outward.Commision_Response,
                            Currency = outward.Currency,
                            DeptNo = outward.DeptNo,
                            DiscountReternedOutImgID = outward.DiscountReternedOutImgID,
                            DrwAcctNo = outward.DrwAcctNo,
                            DrwBankNo = outward.DrwBankNo,
                            DrwBranchExt = outward.DrwBranchExt,
                            DrwBranchNo = outward.DrwBranchNo,
                            DrwCardId = outward.DrwCardId,
                            DrwChqNo = outward.DrwChqNo,
                            DrwName = outward.DrwName,
                            ErrorCode = outward.ErrorCode,
                            ErrorDescription = outward.ErrorDescription,
                            FaildTrans = outward.FaildTrans,
                            History = outward.History + $"|    Chq Deleted By   {userName} At:{DateTime.Now}",
                            InputBrn = outward.InputBrn,
                            InputDate = outward.InputDate,
                            ISSAccount = outward.ISSAccount,
                            IsTimeOut = outward.IsTimeOut,
                            IsVIP = outward.IsVIP,
                            LastUpdate = DateTime.Now,
                            LastUpdateBy = userName,
                            NeedTechnicalVerification = outward.NeedTechnicalVerification,
                            OperType = outward.OperType,
                            PDCChqSequance = outward.PDCChqSequance,
                            PDCSerial = outward.PDCSerial,
                            PMAstatus = outward.PMAstatus,
                            PMAstatusDate = outward.PMAstatusDate,
                            Posted = outward.Posted,
                            QVFAddtlInf = outward.QVFAddtlInf,
                            QVFStatus = outward.QVFStatus,
                            Rejected = outward.Rejected,
                            RepresentSerial = outward.RepresentSerial,
                            Returned = outward.Returned,
                            ReturnedCode = outward.ReturnedCode,
                            ReturnedDate = outward.ReturnedDate,
                            RSFAddtlInf = outward.RSFAddtlInf,
                            RSFStatus = outward.RSFStatus,
                            SpecialHandling = outward.SpecialHandling,
                            Status = outward.Status,
                            System_Aut_Man = outward.System_Aut_Man,
                            Temenos_Message_Series = outward.Temenos_Message_Series,
                            TransCode = outward.TransCode,
                            TransDate = outward.TransDate,
                            UserName = outward.UserName,
                            ValueDate = outward.ValueDate,
                            WasPDC = outward.WasPDC,
                            WithUV = outward.WithUV
                        };

                        _context.Outward_Trans_Deleted.Add(deleteout);
                        await _context.SaveChangesAsync();
                        _logSystem.WriteTraceLogg("BEFORE During Add Deleted chq from out to delete out", _applicationID, GetType().Name, MethodBase.GetCurrentMethod().Name, userName, userId.ToString(), "", "", "");

                        _context.Outward_Trans.Remove(outward);
                        await _context.SaveChangesAsync();

                        var auth = await _context.Auth_Tran_Details_TBL.SingleOrDefaultAsync(i => i.Chq_Serial == chqSequance);
                        if (auth != null)
                        {
                            _context.Auth_Tran_Details_TBL.Remove(auth);
                            await _context.SaveChangesAsync();
                        }
                        resultMessage = "Delete Cheques Done ";
                    }
                    catch (Exception exInner)
                    {
                        _loggMessage = $"Error during deletion process for serial {serial}: {exInner.Message}";
                        _logSystem.WriteTraceLogg(_loggMessage, _applicationID, GetType().Name, MethodBase.GetCurrentMethod().Name, userName, userId.ToString(), "", "", "");
                        _logger.LogError(exInner, _loggMessage);
                        resultMessage = "An error occurred during deletion: " + exInner.Message;
                    }
                }
                else
                {
                    resultMessage = "Cheque not found.";
                }
            }
            catch (Exception ex)
            {
                _loggMessage = $"Error in deletetimeoutchq for serial {serial}: {ex.Message}";
                _logSystem.WriteError(_loggMessage, _applicationID, GetType().Name, MethodBase.GetCurrentMethod().Name, userName, userId.ToString(), "", "", "");
                _logger.LogError(ex, _loggMessage);
                resultMessage = "Somthing Wrong: " + ex.Message;
            }
            return resultMessage;
        }



        public async Task<RepresentReturnDisViewModel> RepresentReturnDis(string userName, int userId)
        {
            _logger.LogInformation($"Executing RepresentReturnDis for user: {userName}");
            this.userName = userName;
            this.userId = userId;

            var viewModel = new RepresentReturnDisViewModel();
            try
            {
                // Assuming GetAllCategoriesForTreeInternal is implemented elsewhere in the service
                viewModel.Tree = await GetAllCategoriesForTreeInternal();
            }
            catch (Exception ex)
            {
                _loggMessage = $"Error in RepresentReturnDis for user: {userName}: {ex.Message}";
                _logSystem.WriteError(_loggMessage, _applicationID, GetType().Name, MethodBase.GetCurrentMethod().Name, userName, userId.ToString(), "", "", "");
                _logger.LogError(ex, _loggMessage);
                viewModel.ErrorMessage = "An error occurred: " + ex.Message;
            }
            return viewModel;
        }



        public async Task<FindChqViewModel> FindChq(string DrwChqNo, string DrwBankNo, string DrwBranchNo, string DrwAcctNo, string BenAccountNo, string userName, int userId)
        {
            _logger.LogInformation($"Executing FindChq for user: {userName}, DrwChqNo: {DrwChqNo}, DrwBankNo: {DrwBankNo}, DrwBranchNo: {DrwBranchNo}, DrwAcctNo: {DrwAcctNo}, BenAccountNo: {BenAccountNo}");
            this.userName = userName;
            this.userId = userId;

            var viewModel = new FindChqViewModel();
            try
            {
                if (string.IsNullOrEmpty(DrwChqNo) || string.IsNullOrEmpty(DrwBankNo) || string.IsNullOrEmpty(DrwBranchNo) || string.IsNullOrEmpty(DrwAcctNo) || string.IsNullOrEmpty(BenAccountNo))
                {
                    viewModel.ErrorMsg = "Please Fill All Data";
                    return viewModel;
                }

                string selectQuery = $"SELECT TOP 1 * FROM Outward_Trans WHERE ClrCenter <> \'PMA\' AND DrwAcctNo LIKE \'%{DrwAcctNo}%\' AND DrwBankNo = \'{DrwBankNo}\' AND DrwBranchNo = {DrwBranchNo} AND DrwChqNo LIKE \'%{DrwChqNo}%\' AND BenAccountNo = \'{BenAccountNo}\' AND Posted = {(int)AllEnums.Cheque_Status.Returne} AND ISNULL([RepresentSerial], 0) = 0 AND Status <> \'Deleted\' ORDER BY Serial DESC";

                var outwardTrans = await _context.Outward_Trans.FromSqlRaw(selectQuery).ToListAsync();

                if (outwardTrans.Any())
                {
                    viewModel.OutwardTransList = outwardTrans;
                }
                else
                {
                    // Check in Outward_Trans_Discount_Old if not found in Outward_Trans
                    string oldSelectQuery = $"SELECT * FROM Outward_Trans_Discount_Old WHERE DrwAcctNo LIKE \'%{DrwAcctNo}%\' AND DrwBankNo = \'{DrwBankNo}\' AND DrwBranchNo = {DrwBranchNo} AND DrwChqNo LIKE \'%{DrwChqNo}%\' AND BenAccountNo = \'{BenAccountNo}\' AND Posted = {(int)AllEnums.Cheque_Status.Returne} AND ISNULL([RepresentSerial], 0) = 0 AND ISNULL(Status, \'\') <> \'Deleted\'";
                    var oldOutwardTrans = await _context.Outward_Trans_Discount_Old.FromSqlRaw(oldSelectQuery).ToListAsync();

                    if (oldOutwardTrans.Any())
                    {
                        // Map oldOutwardTrans to Outward_Trans and add to new system
                        foreach (var oldTrans in oldOutwardTrans)
                        {
                            var newOutwardTrans = new Outward_Trans
                            {
                                Status = oldTrans.Status,
                                Amount = oldTrans.Amount,
                                ISSAccount = string.IsNullOrEmpty(oldTrans.ISSACCOUNT) ? "" : oldTrans.ISSACCOUNT,
                                Posted = (int)AllEnums.Cheque_Status.Cleared,
                                BenAccountNo = oldTrans.BenAccountNo,
                                BenfAccBranch = oldTrans.BenfAccBranch,
                                BenfBnk = oldTrans.BenfBnk,
                                BenfCardId = oldTrans.BenfCardId,
                                BenfCardType = oldTrans.BenfCardType,
                                BenfNationality = oldTrans.BenfNationality,
                                SpecialHandling = 0,
                                ClrCenter = oldTrans.ClrCenter,
                                ValueDate = oldTrans.ValueDate,
                                ReturnedDate = oldTrans.ReturnedDate,
                                BenName = oldTrans.BenName,
                                ChqSequance = oldTrans.ChqSequance,
                                CHQState = oldTrans.CHQState,
                                ClrFileRecordID = oldTrans.ClrFileRecordID,
                                Currency = oldTrans.Currency,
                                DeptNo = oldTrans.DeptNo,
                                DiscountReternedOutImgID = oldTrans.DiscountReternedOutImgID,
                                DrwAcctNo = oldTrans.DrwAcctNo,
                                DrwBankNo = oldTrans.DrwBankNo,
                                DrwBranchExt = oldTrans.DrwBranchExt,
                                DrwBranchNo = oldTrans.DrwBranchNo,
                                DrwCardId = oldTrans.DrwCardId,
                                DrwChqNo = oldTrans.DrwChqNo,
                                DrwName = oldTrans.DrwName,
                                ErrorDescription = "",
                                InputBrn = oldTrans.InputBrn, // Assuming Session.Item("BranchID") is handled before this call
                                InputDate = DateTime.Now, // Assuming DateTime.Now for new entry
                                IsTimeOut = oldTrans.IsTimeOut,
                                IsVIP = oldTrans.IsVIP,
                                NeedTechnicalVerification = oldTrans.NeedTechnicalVerification,
                                WithUV = oldTrans.WithUV
                            };

                            _context.Outward_Trans.Add(newOutwardTrans);
                            await _context.SaveChangesAsync();

                            // Handle images if any
                            var oldImages = await _context.Outward_Trans_Discount_Old_Images.Where(img => img.Serial == oldTrans.Serial).ToListAsync();
                            foreach (var oldImg in oldImages)
                            {
                                var newImage = new Outward_Imgs
                                {
                                    Serial = newOutwardTrans.Serial, // Link to the new outward transaction
                                    Image = oldImg.Image,
                                    Image_Type = oldImg.Image_Type
                                };
                                _context.Outward_Imgs.Add(newImage);
                            }
                            await _context.SaveChangesAsync();

                            viewModel.OutwardTransList.Add(newOutwardTrans);
                        }
                    }
                    else
                    {
                        viewModel.ErrorMsg = "No cheque found matching the criteria.";
                    }
                }
            }
            catch (Exception ex)
            {
                _loggMessage = $"Error in FindChq for user: {userName}: {ex.Message}";
                _logSystem.WriteError(_loggMessage, _applicationID, GetType().Name, MethodBase.GetCurrentMethod().Name, userName, userId.ToString(), "", "", "");
                _logger.LogError(ex, _loggMessage);
                viewModel.ErrorMsg = "An error occurred: " + ex.Message;
            }
            return viewModel;
        }



        public async Task<GetOutwardSlipCCSViewModel> Get_Outward_Slip_CCS(string serial, string userName, int userId)
        {
            _logger.LogInformation($"Executing Get_Outward_Slip_CCS for serial: {serial}, user: {userName}");
            this.userName = userName;
            this.userId = userId;

            var viewModel = new GetOutwardSlipCCSViewModel();
            try
            {
                if (string.IsNullOrEmpty(userName))
                {
                    viewModel.ErrorMsg = "User not logged in.";
                    return viewModel;
                }

                viewModel.OutwardSlip = await _context.Get_Outward_Slip_CCS_VIEW.SingleOrDefaultAsync(x => x.Serial == serial);

                if (viewModel.OutwardSlip == null)
                {
                    viewModel.ErrorMsg = "Outward slip not found.";
                }
            }
            catch (Exception ex)
            {
                _loggMessage = $"Error in Get_Outward_Slip_CCS for serial {serial}: {ex.Message}";
                _logSystem.WriteError(_loggMessage, _applicationID, GetType().Name, MethodBase.GetCurrentMethod().Name, userName, userId.ToString(), "", "", "");
                _logger.LogError(ex, _loggMessage);
                viewModel.ErrorMsg = "An error occurred: " + ex.Message;
            }
            return viewModel;
        }



        public async Task<FindChqdisViewModel> FindChqdis(string DrwChqNo, string DrwBankNo, string DrwBranchNo, string DrwAcctNo, string userName, int userId)
        {
            _logger.LogInformation($"Executing FindChqdis for user: {userName}, DrwChqNo: {DrwChqNo}, DrwBankNo: {DrwBankNo}, DrwBranchNo: {DrwBranchNo}, DrwAcctNo: {DrwAcctNo}");
            this.userName = userName;
            this.userId = userId;

            var viewModel = new FindChqdisViewModel();
            try
            {
                if (string.IsNullOrEmpty(DrwChqNo) || string.IsNullOrEmpty(DrwBankNo) || string.IsNullOrEmpty(DrwBranchNo) || string.IsNullOrEmpty(DrwAcctNo))
                {
                    viewModel.ErrorMsg = "Please Fill All Data";
                    return viewModel;
                }

                // Retrieve Return_Codes_Tbl for DISCOUNT
                viewModel.DiscountReturnCodes = await _context.Return_Codes_Tbl.Where(x => x.ClrCenter == "DISCOUNT").ToListAsync();

                // Construct the SQL query for Outward_Trans
                string selectQuery = $"SELECT * FROM Outward_Trans WHERE DrwAcctNo LIKE \'%{DrwAcctNo}%\' AND DrwBankNo = {DrwBankNo} AND DrwBranchNo = {DrwBranchNo} AND DrwChqNo LIKE \'%{DrwChqNo}%\' AND Posted != {(int)AllEnums.Cheque_Status.Returne}";

                viewModel.OutwardTransList = await _context.Outward_Trans.FromSqlRaw(selectQuery).ToListAsync();
            }
            catch (Exception ex)
            {
                _loggMessage = $"Error in FindChqdis for user: {userName}: {ex.Message}";
                _logSystem.WriteError(_loggMessage, _applicationID, GetType().Name, MethodBase.GetCurrentMethod().Name, userName, userId.ToString(), "", "", "");
                _logger.LogError(ex, _loggMessage);
                viewModel.ErrorMsg = "An error occurred: " + ex.Message;
            }
            return viewModel;
        }



        public async Task<UpdateChqDateRepresnetViewModel> Update_ChqDate_Represnet(string Serial, string BenName, string BenfAccBranch, string AcctType, string DrwChqNo, string DrwBankNo, string DrwBranchNo, string DrwAcctNo, double Amount, DateTime DueDate, string Currency, string BenfBnk, string BenfCardType, string BenfCardId, string BenAccountNo, string BenfNationality, string NeedTechnicalVerification, string WithUV, string SpecialHandling, string IsVIP, string DrwName, string userName, int userId)
        {
            _logger.LogInformation($"Executing Update_ChqDate_Represnet for serial: {Serial}, user: {userName}");
            this.userName = userName;
            this.userId = userId;

            var viewModel = new UpdateChqDateRepresnetViewModel();
            try
            {
                // Permissions check (simplified, assuming getuser_group_permision is handled by a separate service or middleware)
                // var pageid = _context.App_Pages.SingleOrDefault(t => t.Page_Name_EN == "Out_VerficationDetails").Page_Id;
                // var applicationid = _context.App_Pages.SingleOrDefault(y => y.Page_Name_EN == "Out_VerficationDetails").Application_ID;
                // await getuser_group_permision(pageid.ToString(), applicationid.ToString(), userId.ToString());

                var Out = await _context.Outward_Trans.SingleOrDefaultAsync(Y => Y.Serial == Serial);

                if (Out != null)
                {
                    string CHQSEQ = await GENERATE_UNIQUE_CHEQUE_SEQUANCE(Out.DrwChqNo, Out.DrwBankNo, Out.DrwBranchNo, Out.DrwAcctNo);

                    var new_out = new Outward_Trans
                    {
                        BenfAccBranch = BenfAccBranch,
                        ChqSequance = CHQSEQ,
                        BenName = BenName,
                        OperType = Out.OperType,
                        ValueDate = Out.ValueDate,
                        System_Aut_Man = Out.System_Aut_Man,
                        TransCode = Out.TransCode,
                        Returned = 0,
                        Rejected = 0,
                        BenfCardType = Out.BenfCardType,
                        BenfCardId = Out.BenfCardId,
                        DrwBranchExt = Out.DrwBranchExt,
                        DrwCardId = Out.DrwCardId,
                        AuthorizerBranch = Out.AuthorizerBranch,
                        ClrCenter = "DISCOUNT",
                        DrwName = DrwName,
                        DrwChqNo = DrwChqNo,
                        DrwBankNo = DrwBankNo,
                        DrwBranchNo = DrwBranchNo,
                        WasPDC = Out.WasPDC,
                        InputBrn = Out.InputBrn, // Assuming BranchID is passed or retrieved differently
                        DeptNo = Out.DeptNo,
                        InputDate = Out.InputDate,
                        UserName = Out.UserName,
                        DrwAcctNo = DrwAcctNo,
                        ISSAccount = Out.ISSAccount,
                        Amount = Amount,
                        TransDate = DueDate,
                        Currency = Currency,
                        BenfBnk = BenfBnk,
                        BenAccountNo = BenAccountNo,
                        BenfNationality = BenfNationality,
                        NeedTechnicalVerification = NeedTechnicalVerification,
                        WithUV = WithUV,
                        SpecialHandling = SpecialHandling,
                        IsVIP = IsVIP,
                        Posted = (int)AllEnums.Cheque_Status.New,
                        LastUpdate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        LastUpdateBy = userName,
                        History = Out.History + $"Record Added Befor represent by {userName}, on {DateTime.Now}",
                        Status = "Accept",
                        RepresentSerial = Out.Serial
                    };

                    _context.Outward_Trans.Add(new_out);
                    await _context.SaveChangesAsync();

                    var oldchq = await _context.Outward_Trans.SingleOrDefaultAsync(x => x.Serial == Serial && x.ClrCenter == "DISCOUNT");
                    if (oldchq != null)
                    {
                        oldchq.Status = "Returned";
                        oldchq.LastUpdate = DateTime.Now;
                        oldchq.LastUpdateBy = userName;
                        oldchq.History += "|" + "Discount chq Reprsented and this old one deleted";
                        _context.Outward_Trans.Update(oldchq);
                        await _context.SaveChangesAsync();
                    }

                    // Handle image linking
                    var img = await _context.Cheque_Images_Link_Tbl.SingleOrDefaultAsync(i => i.ChqSequance == Out.ChqSequance && i.Serial == Out.Serial);
                    if (img != null)
                    {
                        var img_ = new Cheque_Images_Link_Tbl
                        {
                            Cheque_ype = img.Cheque_ype,
                            ChqSequance = CHQSEQ,
                            Serial = new_out.Serial,
                            ImageSerial = img.ImageSerial,
                            TransDate = DateTime.Now
                        };
                        _context.Cheque_Images_Link_Tbl.Add(img_);
                        await _context.SaveChangesAsync();
                    }

                    // Handle authorization details
                    var Accobj = await _safaT24EccSvcClient.ACCOUNT_INFOAsync(Out.BenAccountNo, 1);

                    int CUS_POSTING_RESTRICTION = string.IsNullOrEmpty(Accobj.CustPosting) ? 0 : int.Parse(Accobj.CustPosting);
                    int ACC_POSTING_RESTRICTION = string.IsNullOrEmpty(Accobj.AcctPosting) ? 0 : int.Parse(Accobj.AcctPosting);

                    string Post_Rest_Description = await Get_Final_Posting_Restrection(CUS_POSTING_RESTRICTION, ACC_POSTING_RESTRICTION, 1);
                    Post_Rest_Description = Post_Rest_Description.Split(';')[0].Split('=')[1]; // Adjusted parsing

                    if (Post_Rest_Description == "AUTHORIZATION_REQUIERED")
                    {
                        var _Auth_Detail = await _context.Auth_Tran_Details_TBL.SingleOrDefaultAsync(o => o.Chq_Serial == CHQSEQ);
                        if (_Auth_Detail != null)
                        {
                            _Auth_Detail.Post_Code = $"Post Restriction :{CUS_POSTING_RESTRICTION};{ACC_POSTING_RESTRICTION}";
                            _Auth_Detail.PostRestriction = Post_Rest_Description;
                            _context.Auth_Tran_Details_TBL.Update(_Auth_Detail);
                            await _context.SaveChangesAsync();
                        }
                    }

                    // Handle limits (assuming USER_Limits_Auth_Amount is a stored procedure or function)
                    // var limits = await _context.USER_Limits_Auth_Amount(userId, 1, "c", await EVALUATE_AMOUNT_IN_JOD(Out.Currency, Out.Amount)).ToListAsync();

                    var Auth_Detail_After_Limit = await _context.Auth_Tran_Details_TBL.SingleOrDefaultAsync(o => o.Chq_Serial == CHQSEQ);
                    if (Auth_Detail_After_Limit != null)
                    {
                        Auth_Detail_After_Limit.Amount = Amount;
                        Auth_Detail_After_Limit.Amount_JOD = await EVALUATE_AMOUNT_IN_JOD(Out.Currency, Out.Amount);
                        Auth_Detail_After_Limit.Status = "Pending";
                        Auth_Detail_After_Limit.First_level_status = "";
                        Auth_Detail_After_Limit.Second_level_status = "";
                        _context.Auth_Tran_Details_TBL.Update(Auth_Detail_After_Limit);
                        await _context.SaveChangesAsync();
                    }
                }
                else
                {
                    viewModel.ErrorMessage = "Original cheque not found.";
                }
            }
            catch (Exception ex)
            {
                _loggMessage = $"Error in Update_ChqDate_Represnet for serial {Serial}: {ex.Message}";
                _logSystem.WriteError(_loggMessage, _applicationID, GetType().Name, MethodBase.GetCurrentMethod().Name, userName, userId.ToString(), "", "", "");
                _logger.LogError(ex, _loggMessage);
                viewModel.ErrorMessage = "An error occurred: " + ex.Message;
            }
            return viewModel;
        }



        public async Task<UpdateChqDateRepresnetViewModel> Update_ChqDate_Represnet(string Serial, string BenName, string BenfAccBranch, string AcctType, string DrwChqNo, string DrwBankNo, string DrwBranchNo, string DrwAcctNo, double Amount, DateTime DueDate, string Currency, string BenfBnk, string BenfCardType, string BenfCardId, string BenAccountNo, string BenfNationality, string NeedTechnicalVerification, string WithUV, string SpecialHandling, string IsVIP, string DrwName, string userName, int userId)
        {
            _logger.LogInformation($"Executing Update_ChqDate_Represnet for serial: {Serial}, user: {userName}");
            this.userName = userName;
            this.userId = userId;

            var viewModel = new UpdateChqDateRepresnetViewModel();
            try
            {
                // Permissions check (simplified, assuming getuser_group_permision is handled by a separate service or middleware)
                // var pageid = _context.App_Pages.SingleOrDefault(t => t.Page_Name_EN == "Out_VerficationDetails").Page_Id;
                // var applicationid = _context.App_Pages.SingleOrDefault(y => y.Page_Name_EN == "Out_VerficationDetails").Application_ID;
                // await getuser_group_permision(pageid.ToString(), applicationid.ToString(), userId.ToString());

                var Out = await _context.Outward_Trans.SingleOrDefaultAsync(Y => Y.Serial == Serial);

                if (Out != null)
                {
                    string CHQSEQ = await GENERATE_UNIQUE_CHEQUE_SEQUANCE(Out.DrwChqNo, Out.DrwBankNo, Out.DrwBranchNo, Out.DrwAcctNo);

                    var new_out = new Outward_Trans
                    {
                        BenfAccBranch = BenfAccBranch,
                        ChqSequance = CHQSEQ,
                        BenName = BenName,
                        OperType = Out.OperType,
                        ValueDate = Out.ValueDate,
                        System_Aut_Man = Out.System_Aut_Man,
                        TransCode = Out.TransCode,
                        Returned = 0,
                        Rejected = 0,
                        BenfCardType = Out.BenfCardType,
                        BenfCardId = Out.BenfCardId,
                        DrwBranchExt = Out.DrwBranchExt,
                        DrwCardId = Out.DrwCardId,
                        AuthorizerBranch = Out.AuthorizerBranch,
                        ClrCenter = "DISCOUNT",
                        DrwName = DrwName,
                        DrwChqNo = DrwChqNo,
                        DrwBankNo = DrwBankNo,
                        DrwBranchNo = DrwBranchNo,
                        WasPDC = Out.WasPDC,
                        InputBrn = Out.InputBrn, // Assuming BranchID is passed or retrieved differently
                        DeptNo = Out.DeptNo,
                        InputDate = Out.InputDate,
                        UserName = Out.UserName,
                        DrwAcctNo = DrwAcctNo,
                        ISSAccount = Out.ISSAccount,
                        Amount = Amount,
                        TransDate = DueDate,
                        Currency = Currency,
                        BenfBnk = BenfBnk,
                        BenAccountNo = BenAccountNo,
                        BenfNationality = BenfNationality,
                        NeedTechnicalVerification = NeedTechnicalVerification,
                        WithUV = WithUV,
                        SpecialHandling = SpecialHandling,
                        IsVIP = IsVIP,
                        Posted = (int)AllEnums.Cheque_Status.New,
                        LastUpdate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        LastUpdateBy = userName,
                        History = Out.History + $"Record Added Befor represent by {userName}, on {DateTime.Now}",
                        Status = "Accept",
                        RepresentSerial = Out.Serial
                    };

                    _context.Outward_Trans.Add(new_out);
                    await _context.SaveChangesAsync();

                    var oldchq = await _context.Outward_Trans.SingleOrDefaultAsync(x => x.Serial == Serial && x.ClrCenter == "DISCOUNT");
                    if (oldchq != null)
                    {
                        oldchq.Status = "Returned";
                        oldchq.LastUpdate = DateTime.Now;
                        oldchq.LastUpdateBy = userName;
                        oldchq.History += "|" + "Discount chq Reprsented and this old one deleted";
                        _context.Outward_Trans.Update(oldchq);
                        await _context.SaveChangesAsync();
                    }

                    // Handle image linking
                    var img = await _context.Cheque_Images_Link_Tbl.SingleOrDefaultAsync(i => i.ChqSequance == Out.ChqSequance && i.Serial == Out.Serial);
                    if (img != null)
                    {
                        var img_ = new Cheque_Images_Link_Tbl
                        {
                            Cheque_ype = img.Cheque_ype,
                            ChqSequance = CHQSEQ,
                            Serial = new_out.Serial,
                            ImageSerial = img.ImageSerial,
                            TransDate = DateTime.Now
                        };
                        _context.Cheque_Images_Link_Tbl.Add(img_);
                        await _context.SaveChangesAsync();
                    }

                    // Handle authorization details
                    var Accobj = await _safaT24EccSvcClient.ACCOUNT_INFOAsync(Out.BenAccountNo, 1);

                    int CUS_POSTING_RESTRICTION = string.IsNullOrEmpty(Accobj.CustPosting) ? 0 : int.Parse(Accobj.CustPosting);
                    int ACC_POSTING_RESTRICTION = string.IsNullOrEmpty(Accobj.AcctPosting) ? 0 : int.Parse(Accobj.AcctPosting);

                    string Post_Rest_Description = await Get_Final_Posting_Restrection(CUS_POSTING_RESTRICTION, ACC_POSTING_RESTRICTION, 1);
                    Post_Rest_Description = Post_Rest_Description.Split(
';
')[0].Split('=
')[1]; // Adjusted parsing

                    if (Post_Rest_Description == "AUTHORIZATION_REQUIERED")
                    {
                        var _Auth_Detail = await _context.Auth_Tran_Details_TBL.SingleOrDefaultAsync(o => o.Chq_Serial == CHQSEQ);
                        if (_Auth_Detail != null)
                        {
                            _Auth_Detail.Post_Code = $"Post Restriction :{CUS_POSTING_RESTRICTION};{ACC_POSTING_RESTRICTION}";
                            _Auth_Detail.PostRestriction = Post_Rest_Description;
                            _context.Auth_Tran_Details_TBL.Update(_Auth_Detail);
                            await _context.SaveChangesAsync();
                        }
                    }

                    // Handle limits (assuming USER_Limits_Auth_Amount is a stored procedure or function)
                    // var limits = await _context.USER_Limits_Auth_Amount(userId, 1, "c", await EVALUATE_AMOUNT_IN_JOD(Out.Currency, Out.Amount)).ToListAsync();

                    var Auth_Detail_After_Limit = await _context.Auth_Tran_Details_TBL.SingleOrDefaultAsync(o => o.Chq_Serial == CHQSEQ);
                    if (Auth_Detail_After_Limit != null)
                    {
                        Auth_Detail_After_Limit.Amount = Amount;
                        Auth_Detail_After_Limit.Amount_JOD = await EVALUATE_AMOUNT_IN_JOD(Out.Currency, Out.Amount);
                        Auth_Detail_After_Limit.Status = "Pending";
                        Auth_Detail_After_Limit.First_level_status = "";
                        Auth_Detail_After_Limit.Second_level_status = "";
                        _context.Auth_Tran_Details_TBL.Update(Auth_Detail_After_Limit);
                        await _context.SaveChangesAsync();
                    }
                }
                else
                {
                    // Handle case where original cheque is from Outward_Trans_Discount_Old
                    var olout = await _context.Outward_Trans_Discount_Old.SingleOrDefaultAsync(Y => Y.Serial == Serial);
                    if (olout != null)
                    {
                        string CHQSEQ = await GENERATE_UNIQUE_CHEQUE_SEQUANCE(olout.DrwChqNo, olout.DrwBankNo, olout.DrwBranchNo, olout.DrwAcctNo);

                        var new_out = new Outward_Trans
                        {
                            BenfAccBranch = BenfAccBranch,
                            ChqSequance = CHQSEQ,
                            BenName = BenName,
                            OperType = olout.OperType,
                            ValueDate = olout.ValueDate,
                            System_Aut_Man = olout.System_Aut_Man,
                            TransCode = olout.TransCode,
                            Returned = 0,
                            Rejected = 0,
                            BenfCardType = olout.BenfCardType,
                            BenfCardId = olout.BenfCardId,
                            DrwBranchExt = olout.DrwBranchExt,
                            DrwCardId = olout.DrwCardId,
                            AuthorizerBranch = "",
                            ClrCenter = AcctType,
                            DrwName = DrwName,
                            DrwChqNo = DrwChqNo,
                            DrwBankNo = DrwBankNo,
                            DrwBranchNo = DrwBranchNo,
                            WasPDC = olout.WasPDC,
                            InputBrn = olout.InputBrn,
                            DeptNo = olout.DeptNo,
                            InputDate = olout.InputDate,
                            UserName = olout.UserName,
                            DrwAcctNo = DrwAcctNo,
                            ISSAccount = olout.ISSACCOUNT,
                            Amount = Amount,
                            TransDate = DueDate,
                            Currency = Currency,
                            BenfBnk = BenfBnk,
                            BenAccountNo = BenAccountNo,
                            BenfNationality = BenfNationality,
                            NeedTechnicalVerification = NeedTechnicalVerification,
                            WithUV = WithUV,
                            SpecialHandling = SpecialHandling,
                            IsVIP = IsVIP,
                            Posted = (int)AllEnums.Cheque_Status.New,
                            LastUpdate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            LastUpdateBy = userName,
                            History = olout.History + $"Record Added Befor represent From Old System by {userName}, on {DateTime.Now}",
                            Status = "Accept",
                            RepresentSerial = olout.Serial
                        };
                        _context.Outward_Trans.Add(new_out);
                        await _context.SaveChangesAsync();

                        // Handle authorization details for old system
                        var Accobj = await _safaT24EccSvcClient.ACCOUNT_INFOAsync(olout.BenAccountNo, 1);

                        int CUS_POSTING_RESTRICTION = string.IsNullOrEmpty(Accobj.CustPosting) ? 0 : int.Parse(Accobj.CustPosting);
                        int ACC_POSTING_RESTRICTION = string.IsNullOrEmpty(Accobj.AcctPosting) ? 0 : int.Parse(Accobj.AcctPosting);

                        string Post_Rest_Description = await Get_Final_Posting_Restrection(CUS_POSTING_RESTRICTION, ACC_POSTING_RESTRICTION, 1);
                        Post_Rest_Description = Post_Rest_Description.Split(
';
')[0].Split('=
')[1]; // Adjusted parsing

                        if (Post_Rest_Description == "AUTHORIZATION_REQUIERED")
                        {
                            var _Auth_Detail = await _context.Auth_Tran_Details_TBL.SingleOrDefaultAsync(o => o.Chq_Serial == CHQSEQ);
                            if (_Auth_Detail != null)
                            {
                                _Auth_Detail.Post_Code = $"Post Restriction :{CUS_POSTING_RESTRICTION};{ACC_POSTING_RESTRICTION}";
                                _Auth_Detail.PostRestriction = Post_Rest_Description;
                                _context.Auth_Tran_Details_TBL.Update(_Auth_Detail);
                                await _context.SaveChangesAsync();
                            }
                        }

                        var Auth_Detail_After_Limit = await _context.Auth_Tran_Details_TBL.SingleOrDefaultAsync(o => o.Chq_Serial == CHQSEQ);
                        if (Auth_Detail_After_Limit != null)
                        {
                            Auth_Detail_After_Limit.Amount = Amount;
                            Auth_Detail_After_Limit.Amount_JOD = await EVALUATE_AMOUNT_IN_JOD(olout.Currency, olout.Amount);
                            Auth_Detail_After_Limit.Status = "Pending";
                            Auth_Detail_After_Limit.First_level_status = "";
                            Auth_Detail_After_Limit.Second_level_status = "";
                            _context.Auth_Tran_Details_TBL.Update(Auth_Detail_After_Limit);
                            await _context.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        viewModel.ErrorMessage = "Cheque not found in either current or old system.";
                    }
                }
            }
            catch (Exception ex)
            {
                _loggMessage = $"Error in Update_ChqDate_Represnet for serial {Serial}: {ex.Message}";
                _logSystem.WriteError(_loggMessage, _applicationID, GetType().Name, MethodBase.GetCurrentMethod().Name, userName, userId.ToString(), "", "", "");
                _logger.LogError(ex, _loggMessage);
                viewModel.ErrorMessage = "An error occurred: " + ex.Message;
            }
            return viewModel;
        }



        public async Task<PospondingChqViewModel> PospondingChq(string userName, int userId)
        {
            _logger.LogInformation($"Executing PospondingChq for user: {userName}");
            this.userName = userName;
            this.userId = userId;

            var viewModel = new PospondingChqViewModel();
            try
            {
                // Assuming GetAllCategoriesForTree() is already implemented and returns a string or list of TreeNodes
                // For now, we'll just get the tree as a string, similar to how it's used in the controller
                viewModel.Tree = await GetAllCategoriesForTree(); // Assuming this returns List<TreeNode>

                viewModel.ClearingCenters = await _context.ClearingCenters.ToListAsync();
            }
            catch (Exception ex)
            {
                _loggMessage = $"Error in PospondingChq for user: {userName}: {ex.Message}";
                _logSystem.WriteError(_loggMessage, _applicationID, GetType().Name, MethodBase.GetCurrentMethod().Name, userName, userId.ToString(), "", "", "");
                _logger.LogError(ex, _loggMessage);
                viewModel.ErrorMessage = "An error occurred: " + ex.Message;
            }
            return viewModel;
        }

