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

