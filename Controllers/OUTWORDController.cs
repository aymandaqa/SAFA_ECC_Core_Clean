using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SAFA_ECC_Core_Clean.Models;
using SAFA_ECC_Core_Clean.Services;
using System.Threading.Tasks;

namespace SAFA_ECC_Core_Clean.Controllers
{
    public class OUTWORDController : Controller
    {
        private readonly ILogger<OUTWORDController> _logger;
        private readonly IOutwordService _outwordService;

        public OUTWORDController(ILogger<OUTWORDController> logger, IOutwordService outwordService)
        {
            _logger = logger;
            _outwordService = outwordService;
        }

        public async Task<IActionResult> check_img()
        {
            return await _outwordService.CheckImg();
        }
    }
}


        public async Task<bool> getPermission(string id, string _page, string _groupid)
        {
            return await _outwordService.GetPermission(id, _page, _groupid);
        }

        public async Task<bool> getPermission1(string id, string _page, string _groupid)
        {
            return await _outwordService.GetPermission1(id, _page, _groupid);
        }


        public async Task<IActionResult> Hold_CHQ()
        {
            ViewBag.BindHoldType = await _outwordService.BindHoldType();
            ViewBag.Tree = await _outwordService.GetAllCategoriesForTree();
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Hold_CHQ(SAFA_ECC_Core_Clean.ViewModels.OutwordViewModels.Hold_CHQ Hold, string HOLD_TYPE, string Reserved)
        {
            ViewBag.BindHoldType = await _outwordService.BindHoldType();
            ViewBag.Tree = await _outwordService.GetAllCategoriesForTree();

            // Assuming Session.Item("Hold_CHQ_ERR") is handled via TempData or ModelState in ASP.NET Core
            // For now, we'll pass the error message via ViewData or a ViewModel property

            var result = await _outwordService.Hold_CHQ(Hold, HOLD_TYPE, Reserved);

            // The service method returns an ActionResult, which can be directly returned by the controller.
            // If the service method returns a ViewResult, it will contain the model and view name.
            return result;
        }



        public async Task<IActionResult> returndiscountchq()
        {
            ViewBag.Tree = await _outwordService.GetAllCategoriesForTree();
            return View();
        }



        public async Task<string> Get_Deacrypted_Account(string Drw_Account, string ChqNo)
        {
            return await _outwordService.Get_Deacrypted_Account(Drw_Account, ChqNo);
        }



        public async Task<IActionResult> Rejected_Out_Request()
        {
            ViewBag.Tree = await _outwordService.GetAllCategoriesForTree();
            return await _outwordService.Rejected_Out_Request();
        }



        public async Task<IActionResult> RepresnetDisDetails(string id)
        {
            ViewBag.Tree = await _outwordService.GetAllCategoriesForTree();
            return await _outwordService.RepresnetDisDetails(id);
        }



        public async Task<IActionResult> RepresnetDisDetails(string id)
        {
            ViewBag.Tree = await _outwordService.GetAllCategoriesForTree();
            return await _outwordService.RepresnetDisDetails(id);
        }



        public async Task<IActionResult> GetOutwordPDC()
        {
            ViewBag.Tree = await _outwordService.GetAllCategoriesForTree();
            return await _outwordService.GetOutwordPDC();
        }



        public async Task<IActionResult> Out_VerficationDetails(string id)
        {
            ViewBag.Tree = await _outwordService.GetAllCategoriesForTree();
            return await _outwordService.Out_VerficationDetails(id);
        }



        public async Task<IActionResult> OUTWORD()
        {
            ViewBag.Tree = await _outwordService.GetAllCategoriesForTree();
            return await _outwordService.OUTWORD();
        }



        public async Task<DataTable> Get_Post_Rest_Code(string CUSTOMER_ID, string ACCOUNT_NUMBER)
        {
            return await _outwordService.Get_Post_Rest_Code(CUSTOMER_ID, ACCOUNT_NUMBER);
        }



        public async Task<string> Get_Final_Posting_Restrection(int Customer_Post_Rest, int Acc_Post_Rest, int Language)
        {
            return await _outwordService.Get_Final_Posting_Restrection(Customer_Post_Rest, Acc_Post_Rest, Language);
        }



        public async Task<IActionResult> Pendding_OutWord_Request()
        {
            // The original VB.NET code had logic for session checks and permissions.
            // In ASP.NET Core, session checks are typically handled by middleware or filters.
            // Permissions are handled by authorization attributes.

            // Call the service method to handle the business logic.
            var result = await _outwordService.Pendding_OutWord_Request();

            // Assuming the service returns an IActionResult, we can directly return it.
            // If the service returns data, the controller would then pass it to a view.
            return View(); // Or return result if it's a JSON result or similar
        }



        public async Task<IActionResult> Pendding_OutWord_Request_Auth()
        {
            var result = await _outwordService.Pendding_OutWord_Request_Auth();
            // In a real application, you would pass the data from the service to the view.
            return View(); // Or return result if it's a JSON result or similar
        }



        public async Task<IActionResult> getOutword_WF_Details()
        {
            var result = await _outwordService.getOutword_WF_Details();
            // In a real application, you would pass the data from the service to the view.
            return View(); // Or return result if it's a JSON result or similar
        }



        public async Task<IActionResult> getuser_group_permision(string pageid, string applicationid, string userid)
        {
            await _outwordService.getuser_group_permision(pageid, applicationid, userid);
            // This method primarily handles permissions and session state in the original VB.NET.
            // In ASP.NET Core, this would typically be handled by authorization policies or filters.
            // For now, we return an empty OkResult or a specific view if needed.
            return Ok();
        }



        public async Task<IActionResult> Getpage(string page)
        {
            var result = await _outwordService.Getpage(page);
            return Ok(result); // Returning DataTable directly is not ideal, consider converting to a ViewModel or JSON.
        }



        public async Task<IActionResult> Ge_t(string x)
        {
            var result = await _outwordService.Ge_t(x);
            return Ok(result);
        }



        public async Task<IActionResult> GetAllCategoriesForTree()
        {
            var tree = await _outwordService.GetAllCategoriesForTree();
            return Json(tree); // Assuming the service returns a structure that can be serialized to JSON
        }



        public async Task<IActionResult> GetAllCategoriesForTree()
        {
            var treeHtml = await _outwordService.GetAllCategoriesForTree();
            return Content(treeHtml, "text/html");
        }



        public IActionResult getlist(string x)
        {
            var result = _outwordService.getlist(x);
            return Ok(result);
        }



        public async Task<IActionResult> GENERATE_UNIQUE_CHEQUE_SEQUANCE(string CHEQUE_NO, string BANK_NO, string BRANCH_NO, string DRAWEE_NO)
        {
            var result = await _outwordService.GENERATE_UNIQUE_CHEQUE_SEQUANCE(CHEQUE_NO, BANK_NO, BRANCH_NO, DRAWEE_NO);
            return Ok(result);
        }



        public async Task<IActionResult> getlockedpage(int pageid)
        {
            var result = await _outwordService.getlockedpage(pageid);
            return result;
        }



        public async Task<IActionResult> EVALUATE_AMOUNT_IN_JOD(string CURANCY, double AMOUNT)
        {
            var result = await _outwordService.EVALUATE_AMOUNT_IN_JOD(CURANCY, AMOUNT);
            return Ok(result);
        }



        public async Task<IActionResult> GetCurrencyCode(string Currency_Symbol)
        {
            var result = await _outwordService.GetCurrencyCode(Currency_Symbol);
            return Ok(result);
        }



        [HttpGet]
        public async Task<IActionResult> Update_ChqDate(string Serial, string BenName, string BenfAccBranch, string AcctType, string DrwChqNo, string DrwBankNo, string DrwBranchNo, string DrwAcctNo, double Amount, DateTime DueDate, string Currency, string BenfBnk, string BenfCardType, string BenfCardId, string BenAccountNo, string BenfNationality, string NeedTechnicalVerification, string WithUV, string SpecialHandling, string IsVIP, string DrwName)
        {
            // Permission checks should be handled by authorization attributes or policies.
            // For now, we assume the user is authorized.

            // Populate ViewBag.Tree if needed (e.g., from a shared layout or view component)
            ViewBag.Tree = await _outwordService.GetAllCategoriesForTree();

            var result = await _outwordService.Update_ChqDate(Serial, BenName, BenfAccBranch, AcctType, DrwChqNo, DrwBankNo, DrwBranchNo, DrwAcctNo, Amount, DueDate, Currency, BenfBnk, BenfCardType, BenfCardId, BenAccountNo, BenfNationality, NeedTechnicalVerification, WithUV, SpecialHandling, IsVIP, DrwName);

            if (result is OkResult)
            {
                // Handle success, maybe redirect or return a success view
                return RedirectToAction("Out_VerficationDetails", new { id = Serial }); // Example redirect
            }
            else if (result is NotFoundResult)
            {
                return NotFound();
            }
            else if (result is StatusCodeResult statusCodeResult && statusCodeResult.StatusCode == 500)
            {
                return StatusCode(500, "An error occurred during the update.");
            }
            return View("Error"); // Default error view
        }



        [HttpGet]
        public async Task<IActionResult> Update_Out_ChqDate(string Serial, string BenName, string BenfAccBranch, string AcctType, string DrwChqNo, string DrwBankNo, string DrwBranchNo, string DrwAcctNo, double Amount, DateTime DueDate, string Currency, string BenfBnk, string BenfCardType, string BenfCardId, string BenAccountNo, string BenfNationality, string NeedTechnicalVerification, string WithUV, string SpecialHandling, string IsVIP, string DrwName)
        {
            // Permission checks should be handled by authorization attributes or policies.
            // For now, we assume the user is authorized.

            // Populate ViewBag.Tree if needed (e.g., from a shared layout or view component)
            ViewBag.Tree = await _outwordService.GetAllCategoriesForTree();

            var result = await _outwordService.Update_Out_ChqDate(Serial, BenName, BenfAccBranch, AcctType, DrwChqNo, DrwBankNo, DrwBranchNo, DrwAcctNo, Amount, DueDate, Currency, BenfBnk, BenfCardType, BenfCardId, BenAccountNo, BenfNationality, NeedTechnicalVerification, WithUV, SpecialHandling, IsVIP, DrwName);

            if (result is OkResult)
            {
                // Handle success, maybe redirect or return a success view
                return RedirectToAction("Out_VerficationDetails", new { id = Serial }); // Example redirect
            }
            else if (result is NotFoundResult)
            {
                return NotFound();
            }
            else if (result is StatusCodeResult statusCodeResult && statusCodeResult.StatusCode == 500)
            {
                return StatusCode(500, "An error occurred during the update.");
            }
            return View("Error"); // Default error view
        }



        [HttpGet]
        public async Task<IActionResult> Update_Out_ChqDate_Accept(string Serial, string BenName, string BenfAccBranch, string AcctType, string DrwChqNo, string DrwBankNo, string DrwBranchNo, string DrwAcctNo, double Amount, DateTime DueDate, string Currency, string BenfBnk, string BenfCardType, string BenfCardId, string BenAccountNo, string BenfNationality, string NeedTechnicalVerification, string WithUV, string SpecialHandling, string IsVIP, string DrwName)
        {
            // Permission checks should be handled by authorization attributes or policies.
            // For now, we assume the user is authorized.

            // Populate ViewBag.Tree if needed (e.g., from a shared layout or view component)
            ViewBag.Tree = await _outwordService.GetAllCategoriesForTree();

            var result = await _outwordService.Update_Out_ChqDate_Accept(Serial, BenName, BenfAccBranch, AcctType, DrwChqNo, DrwBankNo, DrwBranchNo, DrwAcctNo, Amount, DueDate, Currency, BenfBnk, BenfCardType, BenfCardId, BenAccountNo, BenfNationality, NeedTechnicalVerification, WithUV, SpecialHandling, IsVIP, DrwName);

            if (result is OkResult)
            {
                // Handle success, maybe redirect or return a success view
                return RedirectToAction("Out_VerficationDetails", new { id = Serial }); // Example redirect
            }
            else if (result is NotFoundResult)
            {
                return NotFound();
            }
            else if (result is StatusCodeResult statusCodeResult && statusCodeResult.StatusCode == 500)
            {
                return StatusCode(500, "An error occurred during the update.");
            }
            return View("Error"); // Default error view
        }



        public async Task<IActionResult> getSearchListDis_PMA(string Branchs, string STATUS, string BenAccNo, string AccType, string FromBank, string ToBank, string Currency, string ChequeSource, string Amount, string DRWAccNo, string ChequeNo, string waspdc)
        {
            var result = await _outwordService.getSearchListDis_PMA(Branchs, STATUS, BenAccNo, AccType, FromBank, ToBank, Currency, ChequeSource, Amount, DRWAccNo, ChequeNo, waspdc);
            return result;
        }



        public async Task<IActionResult> getSearchList(string Currency, string ChequeSource, string WASPDC, string Branchs, string order, string inputerr, string ChequeStatus, string vip)
        {
            var result = await _outwordService.getSearchList(Currency, ChequeSource, WASPDC, Branchs, order, inputerr, ChequeStatus, vip);
            return result;
        }



        [HttpGet]
        public async Task<IActionResult> GetTotalPerAccountAndBnk(string ChqSrc, string Cur, string Branchs, string WASPDC, string order, string inputerr)
        {
            var result = await _outwordService.GetTotalPerAccountAndBnk(ChqSrc, Cur, Branchs, WASPDC, order, inputerr);
            return result;
        }



        [HttpPost]
        public async Task<IActionResult> PresentmentDIS_Or_PDC_return([FromBody] Outward_Trans _out_, string CHQ)
        {
            var result = await _outwordService.PresentmentDIS_Or_PDC_return(_out_, CHQ);
            return result;
        }



        [HttpPost]
        public async Task<IActionResult> PresentmentDIS_Or_PDC_timeout([FromBody] Outward_Trans _out_, string CHQ)
        {
            var result = await _outwordService.PresentmentDIS_Or_PDC_timeout(_out_, CHQ);
            return Ok(result);
        }



        [HttpPost]
        public async Task<IActionResult> PresentmentPMA_OR_PDC([FromBody] Outward_Trans _out_, string CHQ)
        {
            var result = await _outwordService.PresentmentPMA_OR_PDC(_out_, CHQ);
            return Ok(result);
        }



        public async Task<IActionResult> Update_oUTWORD_Details(string id)
        {
            ViewBag.Tree = await _outwordService.GetAllCategoriesForTree();
            var outChqs = await _outwordService.Update_oUTWORD_Details(id);

            if (outChqs == null)
            {
                // Handle case where outChqs is null, e.g., redirect to an error page or another action
                return RedirectToAction("update_Post_Outword", "OUTWORD");
            }

            return View(outChqs);
        }



        public async Task<string> getDocType(int id)
        {
            return await _outwordService.getDocType(id);
        }



        public async Task<IActionResult> Deleteoutchq()
        {
            ViewBag.Tree = await _outwordService.GetAllCategoriesForTree();
            var result = await _outwordService.Deleteoutchq();
            // Assuming the service returns an OkObjectResult with the list of Outward_Trans
            if (result is OkObjectResult okResult && okResult.Value is List<Outward_Trans> outList)
            {
                return View(outList);
            }
            return View(new List<Outward_Trans>()); // Return an empty list if no data or error
        }



        public async Task<IActionResult> outward_views(string id)
        {
            ViewBag.Tree = await _outwordService.GetAllCategoriesForTree();
            var result = await _outwordService.outward_views(id);

            if (result is OkObjectResult okResult && okResult.Value is OutChqs outChqs)
            {
                return View(outChqs);
            }
            else if (result is RedirectToActionResult redirectResult)
            {
                return RedirectToAction(redirectResult.ActionName, redirectResult.ControllerName);
            }
            return View("Error"); // Default error view
        }



        public async Task<IActionResult> ReturnOwtward()
        {
            ViewBag.Tree = await _outwordService.GetAllCategoriesForTree();
            var result = await _outwordService.ReturnOwtward();

            if (result is OkObjectResult okResult && okResult.Value is ReturnOwtwardViewModel model)
            {
                ViewBag.Title = model.Title;
                ViewBag.chq_status = model.ChequeStatuses;
                ViewBag.clr_center = model.ChequeSources;
                return View();
            }
            else if (result is RedirectToActionResult redirectResult)
            {
                return RedirectToAction(redirectResult.ActionName, redirectResult.ControllerName);
            }
            return View("Error");
        }



        [HttpPost]
        public async Task<IActionResult> updateAllchqstat(string serial, string chqstat, string chqsource)
        {
            var result = await _outwordService.updateAllchqstat(serial, chqstat, chqsource);
            return result;
        }



        public string Get_ALT_Acc_No(string accountNo)
        {
            return _outwordService.Get_ALT_Acc_No(accountNo);
        }



        public async Task<IActionResult> getreturnList(string ClrCenter, string STATUS, string TransDate, string chqNo, string payAcc)
        {
            var result = await _outwordService.getreturnList(ClrCenter, STATUS, TransDate, chqNo, payAcc);
            return result;
        }



        [HttpGet]
        public async Task<IActionResult> savepostedstatus(string serial, string TBLNAME, string posted)
        {
            var result = await _outwordService.savepostedstatus(serial, TBLNAME, posted);
            return result;
        }



        public async Task<IActionResult> repostTimeoutchq(string serial)
        {
            var result = await _outwordService.repostTimeoutchq(serial);
            return result;
        }



        public async Task<IActionResult> deletetimeoutchq(string serial)
        {
            var result = await _outwordService.deletetimeoutchq(serial);
            return result;
        }

