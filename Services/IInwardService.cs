using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SAFA_ECC_Core_Clean.Services
{
    public interface IInwardService
    {
        Task<JsonResult> InwardFinanicalWFDetailsONUS_NEW(string id);
        Task<DataTable> Getpage(string page);
        Task<bool> GetPermission(string id, string _page, string _groupid);
        Task<bool> GetPermission1(string id, string _page, string _groupid);
        Task<bool> Ge_t(string x);
        Task<string> GetAllCategoriesForTree();
    }
}



        Task<JsonResult> InwardFinanicalWFDetailsPMADIS_Auth(string id);



        Task<JsonResult> getSearchListInitalAccept_reject(string Branchs, string STATUS, string ChequeSource, string FAmount, string TAmount, string Chequeno, string DrwAcc, string Authorize, string Currency, string vip);



        Task<JsonResult> VIEW_WF(string serial, string clrcanter);



        Task<JsonResult> save_Fix_Ret_CHQ(string serial, string RC, string clecenter, string BnfBranch, string ChequeType);



        Task<JsonResult> getSearchList(string Branchs, string STATUS, string FromDate, string ToDate, string RSF, string trans,
            string FromReturnedDate, string ToReturnedDate, string BenAccNo,
            string FromBank, string ToBank,
            string Currency, string ChequeSource, string Amount, string DRWAccNo, string ChequeNo, string FromTransDate, string ToTransDatet, string tot, string vip);



        Task<JsonResult> returnsuspen(string chqseq, string clr_center, string Account, string retuen_code, string serial);



        Task<string> GetCustomerDues(string Customer_id);



        Task<string> EVALUATE_AMOUNT_IN_JOD(string CURANCY, double AMOUNT);



        Task<JsonResult> FixedCHQ(string Status, string _chqseq, string clr_center, string returncode, string drwchq, string drwacc);



        Task<bool> return_Suspense(string chq_seq, string serial, string currancy, string retuen_code);



        Task<bool> Use_Suspense(string chq_seq, string serial, string currancy);



        Task<bool> return_SuspenseONUS(string chq_seq, string serial, string currancy, string retuen_code);



        Task<JsonResult> VerifyAllDiscountCHQ(List<string> Serials);



        Task<bool> POSTCheques_ONUS(string Serials);



        Task<IActionResult> InsufficientFunds();



        Task<InwardFinanicalWFDetailsONUSViewModel> GetInwardFinanicalWFDetailsONUSData(string id, string userName, string branchId, string comId);



        Task<InwardFinanicalWFDetailsPMADISViewModel> GetInwardFinanicalWFDetailsPMADISData(string id, string userName, string branchId, string comId);



        Task<bool> POSTCheques(string Serials);



        Task<JsonResult> GetMagicscreenList(string clrCenter, string status, string transDate, string chqNo, string drwAccNo, string userName, string comId, string pageId);



        Task<JsonResult> ReturnMajicScreen(string serial, string clrcenter, string rc, string patch, string status, string userName, string userId);



        Task<JsonResult> GetSearchWFStage(string chqNo, string drwAcc, string fromDate, string toDate, string userName, string pageId);



        Task<JsonResult> UpdateReturnCheque(string tDate, string userName);



        Task<IActionResult> GetChqStatus(string userName, string userId);



        Task<IActionResult> ResendInwFile();



        Task<IActionResult> T24Job(string userName, string userId);



        Task<IActionResult> ReturnStoppedCheques(string userName, string userId, string branchId);



        Task<IActionResult> ReturnInwardStoppedChequeDetails(int id, string userName);



        Task<JsonResult> ReturnInwardStoppedCheque_Reverse(int id, string retcode, string userName);



        Task<JsonResult> ReverseAllINHOUSE(string userName);



        Task<JsonResult> ReverseAllPMA(string userName);



        Task<JsonResult> ReverseAllDISCOUNT(string userName);



        Task<JsonResult> ReverseAllPDC(string userName);



        Task<JsonResult> Reversevip(string userName);



        Task<JsonResult> ReversePMAINWARAD(string tDate1, string userName);



        Task<JsonResult> ReversePMAOUTWARD(string userName);



        Task<JsonResult> Insufficient_Funds(string userName);



        Task<JsonResult> ReverseAllINHOUSE_PDC(string userName);



        Task<ActionResult> Fix_Ret_CHQ();



        Task<JsonResult> PrintRemove(string Branchs, string Currency, string ChequeSource, string Amount, string DRWAccNo, string ChequeNo, string vip, string userName, string pageId, string groupId);



        Task<JsonResult> getSearchList_WF_fixederror(string Branchs, string Currency, string ChequeSource, string Amount, string DRWAccNo, string ChequeNo, string vip, string userName, string groupId);



        Task<JsonResult> GetAcctWF(string Branchs, string Currency, string ChequeSource, string Amount, string DRWAccNo, string ChequeNo, string vip, string userName, string comId);



        Task<JsonResult> getSearchList_WF(string Branchs, string Currency, string ChequeSource, string Amount, string DRWAccNo, string ChequeNo, string WASPDC, string vip, string CustomerType, string userName, string pageId, string comId);



        Task<IActionResult> Reject_CHQ(string userName, string pageId, string groupId);



        Task<List<SelectListItem>> bind_chq_source();



        Task<List<SelectListItem>> bindchqsource();



        Task<List<SelectListItem>> BindCustomerType(string companyCode);



        Task<JsonResult> get_Rjected_SearchList(string Branchs, string chqsource, string FromDate, string FromTransDate, string ToTransDatet, string ToDate, string FromInputDate, string ToInputDate, string FromReturnedDate, string ToReturnedDate, string BenAccNo, string AccType, string FromBank, string ToBank, string Currency, string ChequeSource, string Amount, string DRWAccNo, string ChequeNo, string userName);



        Task<OutChqs> ViewDetailsOutward(int id, string userName, string pageId, string applicationId, int userId);



        Task<INChqs> ViewDetailsinwarad(int id, string userName, string pageId, string applicationId, int userId);



        Task<IActionResult> Updatedata(string Serial, string BenName, string BenfAccBranch, string DrwChqNo, string DrwBankNo, string DrwBranchNo, string BenfBnk, string TBL_NAME, string userName);



        Task<bool> SENDINWARDSMS(Inward_Trans inwardTrans);



        Task<string> GetTechRes(string retCode, string clrCenter, string language);



        Task<bool> SENDINHOUSESMS(OnUs_Tbl onusTrans);



        Task<JsonResult> fixtimeout(string serial, string clecanter, string userName);



        Task<JsonResult> Reposttimeout(string serial, string clecanter, string userName, int userId);



        Task<List<Email>> GetEmailList(string userName, int userId);



        void ADDEmail();



        void INWTIMEOUT();



        Task<JsonResult> Save_Email(string name, string subject, string body, string toemail, string ccemail, string userName);



        Task<bool> ReturnChqTBL(Inward_Trans inwardTrans, string userName);



        Task<bool> GenerateDiscountCommissionFile();



        Task<string> GetFinalOnusCodeError(string retCode, string retCodeFinancial, string clrCenter);



        Task<Tuple<string, int>> Print_CHQ_QR(string userName);



        Task<INChqs> PrintChqViewer(string id);



        Task<VIPCHQViewModel> VIPCHQ(string userName, int userId);



        Task<Tuple<List<Inward_Trans>, List<Return_Codes_Tbl>>> FINDVIPCHQ(string branch, string userName);



        Task<string> Returnvipchq(string serial, string rc, string userName, int userId);

