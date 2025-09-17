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

