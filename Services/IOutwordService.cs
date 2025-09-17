using System.Collections.Generic;
using System.Threading.Tasks;
using SAFA_ECC_Core_Clean.Models;

namespace SAFA_ECC_Core_Clean.Services
{
    public interface IOutwordService
    {
        Task<bool> GetPermission(string id, string page, string groupId);
        Task<bool> GetPermission1(string id, string page, string groupId);
    }
}


        Task<IActionResult> CheckImg();


        Task<List<SelectListItem>> BindHoldType();


        Task<List<SelectListItem>> GetAllCategoriesForTree();


        Task<ActionResult> Hold_CHQ(Hold_CHQ Hold, string HOLD_TYPE, string Reserved);


        Task<ActionResult> Hold_CHQ(Hold_CHQ Hold, string HOLD_TYPE, string Reserved);


        Task<ActionResult> GetHold_CHQ();


        Task<IActionResult> ReturnDiscountChq();


        Task<string> Get_Deacrypted_Account(string Drw_Account, string ChqNo);


        Task<IActionResult> Rejected_Out_Request();


        Task<IActionResult> RepresnetDisDetails(string id);


        Task<IActionResult> GetOutwordPDC();



        Task<IActionResult> Out_VerficationDetails(string id);



        Task<IActionResult> OutwordDateVerfication(string id);



        Task<IActionResult> OUTWORD();



        Task<IActionResult> OUTWORD(Outward_Trans outwardTrans, string actionType);



        Task<DataTable> Get_Post_Rest_Code(string CUSTOMER_ID, string ACCOUNT_NUMBER);



        Task<string> Get_Final_Posting_Restrection(int Customer_Post_Rest, int Acc_Post_Rest, int Language);



        Task<IActionResult> Pendding_OutWord_Request();



        Task<IActionResult> Pendding_OutWord_Request_Auth();



  
        Task<IActionResult> getOutword_WF_Details();



        Task<string> Get_OFS_HttpLink();



        List<TreeNode> FillRecursive(List<Category> flatObjects, int? parentId = null);



        Task getuser_group_permision(string pageid, string applicationid, string userid);



        Task<DataTable> Getpage(string page);



        Task<bool> Ge_t(string x);



        Task<string> GetAllCategoriesForTree();



        string getlist(string x);



        Task<string> GENERATE_UNIQUE_CHEQUE_SEQUANCE(string CHEQUE_NO, string BANK_NO, string BRANCH_NO, string DRAWEE_NO);



        Task<IActionResult> getlockedpage(int pageid);



        Task<string> EVALUATE_AMOUNT_IN_JOD(string CURANCY, double AMOUNT);



        Task<string> GetCurrencyCode(string Currency_Symbol);



        Task<IActionResult> Update_ChqDate(string Serial, string BenName, string BenfAccBranch, string AcctType, string DrwChqNo, string DrwBankNo, string DrwBranchNo, string DrwAcctNo, double Amount, DateTime DueDate, string Currency, string BenfBnk, string BenfCardType, string BenfCardId, string BenAccountNo, string BenfNationality, string NeedTechnicalVerification, string WithUV, string SpecialHandling, string IsVIP, string DrwName);

