using System.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using SAFA_ECC_Core_Clean.Models;
using SAFA_ECC_Core_Clean.ViewModels.InwardViewModels;
using SAFA_ECC_Core_Clean.ViewModels.OutwordViewModels; // Added for OutChqs
using SAFA_ECC_Core_Clean.ViewModels; // Added for INChqs, Email, TreeNode

namespace SAFA_ECC_Core_Clean.Services
{
    public interface IInwardService
    {
        Task<object> InwardFinanicalWFDetailsONUS_NEW(string id);
        Task<DataTable> Getpage(string page);
        Task<bool> GetPermission(string id, string _page, string _groupid);
        Task<bool> GetPermission1(string id, string _page, string _groupid);
        Task<bool> Ge_t(string x);
        Task<List<TreeNode>> GetAllCategoriesForTree();
        Task<object> InwardFinanicalWFDetailsPMADIS_Auth(string id);
        Task<object> getSearchListInitalAccept_reject(string Branchs, string STATUS, string ChequeSource, string FAmount, string TAmount, string Chequeno, string DrwAcc, string Authorize, string Currency, string vip);
        Task<object> VIEW_WF(string serial, string clrcanter);
        Task<object> save_Fix_Ret_CHQ(string serial, string RC, string clecenter, string BnfBranch, string ChequeType);
        Task<object> getSearchList(string Branchs, string STATUS, string FromDate, string ToDate, string RSF, string trans,
            string FromReturnedDate, string ToReturnedDate, string BenAccNo,
            string FromBank, string ToBank,
            string Currency, string ChequeSource, string Amount, string DRWAccNo, string ChequeNo, string FromTransDate, string ToTransDatet, string tot, string vip);
        Task<object> returnsuspen(string chqseq, string clr_center, string Account, string retuen_code, string serial);
        Task<string> GetCustomerDues(string Customer_id);
        Task<string> EVALUATE_AMOUNT_IN_JOD(string CURANCY, double AMOUNT);
        Task<object> FixedCHQ(string Status, string _chqseq, string clr_center, string returncode, string drwchq, string drwacc);
        Task<bool> return_Suspense(string chq_seq, string serial, string currancy, string retuen_code);
        Task<bool> Use_Suspense(string chq_seq, string serial, string currancy);
        Task<bool> return_SuspenseONUS(string chq_seq, string serial, string currancy, string retuen_code);
        Task<object> VerifyAllDiscountCHQ(List<string> Serials);
        Task<bool> POSTCheques_ONUS(string Serials);
        Task<object> InsufficientFunds();
        Task<InwardFinanicalWFDetailsONUSViewModel> GetInwardFinanicalWFDetailsONUSData(string id, string userName, string branchId, string comId);
        Task<InwardFinanicalWFDetailsPMADISViewModel> GetInwardFinanicalWFDetailsPMADISData(string id, string userName, string branchId, string comId);
        Task<bool> POSTCheques(string Serials);
        Task<object> GetMagicscreenList(string clrCenter, string status, string transDate, string chqNo, string drwAccNo, string userName, string comId, string pageId);
        Task<object> ReturnMajicScreen(string serial, string clrcenter, string rc, string patch, string status, string userName, string userId);
        Task<object> GetSearchWFStage(string chqNo, string drwAcc, string fromDate, string toDate, string userName, string pageId);
        Task<object> UpdateReturnCheque(string tDate, string userName);
        Task<object> GetChqStatus(string userName, string userId);
        Task<object> ResendInwFile();
        Task<object> T24Job(string userName, string userId);
        Task<object> ReturnStoppedCheques(string userName, string userId, string branchId);
        Task<object> ReturnInwardStoppedChequeDetails(int id, string userName);
        Task<object> ReturnInwardStoppedCheque_Reverse(int id, string retcode, string userName);
        Task<object> ReverseAllINHOUSE(string userName);
        Task<object> ReverseAllPMA(string userName);
        Task<object> ReverseAllDISCOUNT(string userName);
        Task<object> ReverseAllPDC(string userName);
        Task<object> Reversevip(string userName);
        Task<object> ReversePMAINWARAD(string tDate1, string userName);
        Task<object> ReversePMAOUTWARD(string userName);
        Task<object> Insufficient_Funds(string userName);
        Task<object> ReverseAllINHOUSE_PDC(string userName);
        Task<object> Fix_Ret_CHQ();
        Task<object> PrintRemove(string Branchs, string Currency, string ChequeSource, string Amount, string DRWAccNo, string ChequeNo, string vip, string userName, string pageId, string groupId);
        Task<object> getSearchList_WF_fixederror(string Branchs, string Currency, string ChequeSource, string Amount, string DRWAccNo, string ChequeNo, string vip, string userName, string groupId);
        Task<object> GetAcctWF(string Branchs, string Currency, string ChequeSource, string Amount, string DRWAccNo, string ChequeNo, string vip, string userName, string comId);
        Task<object> getSearchList_WF(string Branchs, string Currency, string ChequeSource, string Amount, string DRWAccNo, string ChequeNo, string WASPDC, string vip, string CustomerType, string userName, string pageId, string comId);
        Task<object> Reject_CHQ(string userName, string pageId, string groupId);
        Task<List<SelectListItem>> bind_chq_source();
        Task<List<SelectListItem>> bindchqsource();
        Task<List<SelectListItem>> BindCustomerType(string companyCode);
        Task<object> get_Rjected_SearchList(string Branchs, string chqsource, string FromDate, string FromTransDate, string ToTransDatet, string ToDate, string FromInputDate, string ToInputDate, string FromReturnedDate, string ToReturnedDate, string BenAccNo, string AccType, string FromBank, string ToBank, string Currency, string ChequeSource, string Amount, string DRWAccNo, string ChequeNo, string userName);
        Task<OutChqs> ViewDetailsOutward(int id, string userName, string pageId, string applicationId, int userId);
        Task<INChqs> ViewDetailsinwarad(int id, string userName, string pageId, string applicationId, int userId);
        Task<object> Updatedata(string Serial, string BenName, string BenfAccBranch, string DrwChqNo, string DrwBankNo, string DrwBranchNo, string BenfBnk, string TBL_NAME, string userName);
        Task<bool> SENDINWARDSMS(Inward_Trans inwardTrans);
        Task<string> GetTechRes(string retCode, string clrCenter, string language);
        Task<bool> SENDINHOUSESMS(OnUs_Tbl onusTrans);
        Task<object> fixtimeout(string serial, string clecanter, string userName);
        Task<object> Reposttimeout(string serial, string clecanter, string userName, int userId);
        Task<List<Email>> GetEmailList(string userName, int userId);
        void ADDEmail();
        void INWTIMEOUT();
        Task<object> Save_Email(string name, string subject, string body, string toemail, string ccemail, string userName);
        Task<bool> ReturnChqTBL(Inward_Trans inwardTrans, string userName);
        Task<bool> GenerateDiscountCommissionFile();
        Task<string> GetFinalOnusCodeError(string retCode, string retCodeFinancial, string clrCenter);
        Task<Tuple<string, int>> Print_CHQ_QR(string userName);
        Task<INChqs> PrintChqViewer(string id);
        Task<VIPCHQViewModel> VIPCHQ(string userName, int userId);
        Task<Tuple<List<Inward_Trans>, List<Return_Codes_Tbl>>> FINDVIPCHQ(string branch, string userName);
        Task<string> Returnvipchq(string serial, string rc, string userName, int userId);
    }
}

