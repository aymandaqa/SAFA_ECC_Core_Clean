using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SAFA_ECC_Core_Clean.Models;
using SAFA_ECC_Core_Clean.Services;
using SAFA_ECC_Core_Clean.ViewModels.OutwordViewModels;
using System;
using System.Data;
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
            var model = await _outwordService.CheckImg();
            return View(model);
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
        public async Task<IActionResult> Hold_CHQ(Hold_CHQViewModel Hold, string HOLD_TYPE, string Reserved)
        {
            ViewBag.BindHoldType = await _outwordService.BindHoldType();
            ViewBag.Tree = await _outwordService.GetAllCategoriesForTree();

            var result = await _outwordService.Hold_CHQ(Hold, HOLD_TYPE, Reserved);

            return View(result);
        }

        public async Task<IActionResult> returndiscountchq()
        {
            ViewBag.Tree = await _outwordService.GetAllCategoriesForTree();
            var model = await _outwordService.ReturnDiscountChq();
            return View(model);
        }

        public async Task<string> Get_Deacrypted_Account(string Drw_Account, string ChqNo)
        {
            return await _outwordService.Get_Deacrypted_Account(Drw_Account, ChqNo);
        }

        public async Task<IActionResult> Rejected_Out_Request()
        {
            ViewBag.Tree = await _outwordService.GetAllCategoriesForTree();
            var model = await _outwordService.Rejected_Out_Request();
            return View(model);
        }

        public async Task<IActionResult> RepresnetDisDetails(string id)
        {
            ViewBag.Tree = await _outwordService.GetAllCategoriesForTree();
            var model = await _outwordService.RepresnetDisDetails(id);
            return View(model);
        }

        public async Task<IActionResult> GetOutwordPDC()
        {
            ViewBag.Tree = await _outwordService.GetAllCategoriesForTree();
            var model = await _outwordService.GetOutwordPDC();
            return View(model);
        }

        public async Task<IActionResult> Out_VerficationDetails(string id)
        {
            ViewBag.Tree = await _outwordService.GetAllCategoriesForTree();
            var model = await _outwordService.Out_VerficationDetails(id);
            return View(model);
        }

        public async Task<IActionResult> OUTWORD()
        {
            ViewBag.Tree = await _outwordService.GetAllCategoriesForTree();
            var model = await _outwordService.OUTWORD();
            return View(model);
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
            var model = await _outwordService.Pendding_OutWord_Request();
            return View(model);
        }

        public async Task<IActionResult> Pendding_OutWord_Request_Auth()
        {
            var model = await _outwordService.Pendding_OutWord_Request_Auth();
            return View(model);
        }

        public async Task<IActionResult> getOutword_WF_Details()
        {
            var model = await _outwordService.getOutword_WF_Details();
            return View(model);
        }

        public async Task getuser_group_permision(string pageid, string applicationid, string userid)
        {
            await _outwordService.getuser_group_permision(pageid, applicationid, userid);
        }

        public async Task<DataTable> Getpage(string page)
        {
            return await _outwordService.Getpage(page);
        }

        public async Task<bool> Ge_t(string x)
        {
            return await _outwordService.Ge_t(x);
        }

        public async Task<string> GetAllCategoriesForTree()
        {
            return await _outwordService.GetAllCategoriesForTree();
        }

        public string getlist(string x)
        {
            return _outwordService.getlist(x);
        }

        public async Task<string> GENERATE_UNIQUE_CHEQUE_SEQUANCE(string CHEQUE_NO, string BANK_NO, string BRANCH_NO, string DRAWEE_NO)
        {
            return await _outwordService.GENERATE_UNIQUE_CHEQUE_SEQUANCE(CHEQUE_NO, BANK_NO, BRANCH_NO, DRAWEE_NO);
        }

        public async Task<IActionResult> getlockedpage(int pageid)
        {
            return await _outwordService.getlockedpage(pageid);
        }

        public async Task<string> EVALUATE_AMOUNT_IN_JOD(string CURANCY, double AMOUNT)
        {
            return await _outwordService.EVALUATE_AMOUNT_IN_JOD(CURANCY, AMOUNT);
        }

        public async Task<string> GetCurrencyCode(string Currency_Symbol)
        {
            return await _outwordService.GetCurrencyCode(Currency_Symbol);
        }

        [HttpGet]
        public async Task<IActionResult> Update_ChqDate(UpdateChqDateViewModel model)
        {
            ViewBag.Tree = await _outwordService.GetAllCategoriesForTree();
            var result = await _outwordService.Update_ChqDate(model);
            return RedirectToAction("Out_VerficationDetails", new { id = model.Serial });
        }

        [HttpGet]
        public async Task<IActionResult> Update_Out_ChqDate(UpdateChqDateViewModel model)
        {
            ViewBag.Tree = await _outwordService.GetAllCategoriesForTree();
            var result = await _outwordService.Update_Out_ChqDate(model);
            return RedirectToAction("Out_VerficationDetails", new { id = model.Serial });
        }

        [HttpGet]
        public async Task<IActionResult> Update_Out_ChqDate_Accept(UpdateChqDateViewModel model)
        {
            ViewBag.Tree = await _outwordService.GetAllCategoriesForTree();
            var result = await _outwordService.Update_Out_ChqDate_Accept(model);
            return RedirectToAction("Out_VerficationDetails", new { id = model.Serial });
        }

        public async Task<IActionResult> getSearchListDis_PMA(string Branchs, string STATUS, string BenAccNo, string AccType, string FromBank, string ToBank, string Currency, string ChequeSource, string Amount, string DRWAccNo, string ChequeNo, string waspdc)
        {
            var model = await _outwordService.getSearchListDis_PMA(Branchs, STATUS, BenAccNo, AccType, FromBank, ToBank, Currency, ChequeSource, Amount, DRWAccNo, ChequeNo, waspdc);
            return PartialView("_SearchListDisPMAResults", model);
        }

        public async Task<IActionResult> getSearchList(string Currency, string ChequeSource, string WASPDC, string Branchs, string order, string inputerr, string ChequeStatus, string vip)
        {
            var model = await _outwordService.getSearchList(Currency, ChequeSource, WASPDC, Branchs, order, inputerr, ChequeStatus, vip);
            return PartialView("_SearchListResults", model);
        }

        [HttpGet]
        public async Task<IActionResult> GetTotalPerAccountAndBnk(string ChqSrc, string Cur, string Branchs, string WASPDC, string order, string inputerr)
        {
            var model = await _outwordService.GetTotalPerAccountAndBnk(ChqSrc, Cur, Branchs, WASPDC, order, inputerr);
            return PartialView("_TotalPerAccountAndBnk", model);
        }

        [HttpPost]
        public async Task<IActionResult> PresentmentDIS_Or_PDC_return([FromBody] Outward_Trans _out_, string CHQ)
        {
            var result = await _outwordService.PresentmentDIS_Or_PDC_return(_out_, CHQ);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> PresentmentDIS_Or_PDC_timeout([FromBody] Outward_Trans _out_, string CHQ)
        {
            var result = await _outwordService.PresentmentDIS_Or_PDC_timeout(_out_, CHQ);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> PresentmentPMA_OR_PDC([FromBody] Outward_Trans _out_, string CHQ)
        {
            var result = await _outwordService.PresentmentPMA_OR_PDC(_out_, CHQ);
            return Json(result);
        }

        public async Task<IActionResult> Update_oUTWORD_Details(string id)
        {
            ViewBag.Tree = await _outwordService.GetAllCategoriesForTree();
            var outChqs = await _outwordService.Update_oUTWORD_Details(id);
            if (outChqs == null)
            {
                return RedirectToAction("update_Post_Outword", "OUTWORD");
            }
            return View(outChqs);
        }

        public async Task<string> getDocType(int id)
        {
            return await _outwordService.getDocType(id);
        }

        public async Task<IActionResult> update_Post_Outword(string id)
        {
            ViewBag.Tree = await _outwordService.GetAllCategoriesForTree();
            var model = await _outwordService.update_Post_Outword(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> update_Post_Outword(string id, string Note, string returnchq, string chqNu, string chq, string chqAm, string chqBn, string chqBr, string chqAc, string chqdate, string reas, string reas1, string reas2, string reas3, string reas4, string reas5, string reas6, string reas7, string reas8, string reas9, string reas10, string reas11, string reas12, string reas13, string reas14, string reas15, string reas16, string reas17, string reas18, string reas19, string reas20, string reas21, string reas22, string reas23, string reas24, string reas25, string reas26, string reas27, string reas28, string reas29, string reas30, string reas31, string reas32, string reas33, string reas34, string reas35, string reas36, string reas37, string reas38, string reas39, string reas40, string reas41, string reas42, string reas43, string reas44, string reas45, string reas46, string reas47, string reas48, string reas49, string reas50, string reas51, string reas52, string reas53, string reas54, string reas55, string reas56, string reas57, string reas58, string reas59, string reas60, string reas61, string reas62, string reas63, string reas64, string reas65, string reas66, string reas67, string reas68, string reas69, string reas70, string reas71, string reas72, string reas73, string reas74, string reas75, string reas76, string reas77, string reas78, string reas79, string reas80, string reas81, string reas82, string reas83, string reas84, string reas85, string reas86, string reas87, string reas88, string reas89, string reas90, string reas91, string reas92, string reas93, string reas94, string reas95, string reas96, string reas97, string reas98, string reas99)
        {
            var result = await _outwordService.update_Post_Outword_Post(id, Note, returnchq, chqNu, chq, chqAm, chqBn, chqBr, chqAc, chqdate, reas, reas1, reas2, reas3, reas4, reas5, reas6, reas7, reas8, reas9, reas10, reas11, reas12, reas13, reas14, reas15, reas16, reas17, reas18, reas19, reas20, reas21, reas22, reas23, reas24, reas25, reas26, reas27, reas28, reas29, reas30, reas31, reas32, reas33, reas34, reas35, reas36, reas37, reas38, reas39, reas40, reas41, reas42, reas43, reas44, reas45, reas46, reas47, reas48, reas49, reas50, reas51, reas52, reas53, reas54, reas55, reas56, reas57, reas58, reas59, reas60, reas61, reas62, reas63, reas64, reas65, reas66, reas67, reas68, reas69, reas70, reas71, reas72, reas73, reas74, reas75, reas76, reas77, reas78, reas79, reas80, reas81, reas82, reas83, reas84, reas85, reas86, reas87, reas88, reas89, reas90, reas91, reas92, reas93, reas94, reas95, reas96, reas97, reas98, reas99);
            return Json(result);
        }

        public async Task<IActionResult> Return_Owtward_chq(string id)
        {
            ViewBag.Tree = await _outwordService.GetAllCategoriesForTree();
            var model = await _outwordService.Return_Owtward_chq(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Return_Owtward_chq(string id, string Note, string returnchq, string chqNu, string chq, string chqAm, string chqBn, string chqBr, string chqAc, string chqdate, string reas, string reas1, string reas2, string reas3, string reas4, string reas5, string reas6, string reas7, string reas8, string reas9, string reas10, string reas11, string reas12, string reas13, string reas14, string reas15, string reas16, string reas17, string reas18, string reas19, string reas20, string reas21, string reas22, string reas23, string reas24, string reas25, string reas26, string reas27, string reas28, string reas29, string reas30, string reas31, string reas32, string reas33, string reas34, string reas35, string reas36, string reas37, string reas38, string reas39, string reas40, string reas41, string reas42, string reas43, string reas44, string reas45, string reas46, string reas47, string reas48, string reas49, string reas50, string reas51, string reas52, string reas53, string reas54, string reas55, string reas56, string reas57, string reas58, string reas59, string reas60, string reas61, string reas62, string reas63, string reas64, string reas65, string reas66, string reas67, string reas68, string reas69, string reas70, string reas71, string reas72, string reas73, string reas74, string reas75, string reas76, string reas77, string reas78, string reas79, string reas80, string reas81, string reas82, string reas83, string reas84, string reas85, string reas86, string reas87, string reas88, string reas89, string reas90, string reas91, string reas92, string reas93, string reas94, string reas95, string reas96, string reas97, string reas98, string reas99)
        {
            var result = await _outwordService.Return_Owtward_chq_Post(id, Note, returnchq, chqNu, chq, chqAm, chqBn, chqBr, chqAc, chqdate, reas, reas1, reas2, reas3, reas4, reas5, reas6, reas7, reas8, reas9, reas10, reas11, reas12, reas13, reas14, reas15, reas16, reas17, reas18, reas19, reas20, reas21, reas22, reas23, reas24, reas25, reas26, reas27, reas28, reas29, reas30, reas31, reas32, reas33, reas34, reas35, reas36, reas37, reas38, reas39, reas40, reas41, reas42, reas43, reas44, reas45, reas46, reas47, reas48, reas49, reas50, reas51, reas52, reas53, reas54, reas55, reas56, reas57, reas58, reas59, reas60, reas61, reas62, reas63, reas64, reas65, reas66, reas67, reas68, reas69, reas70, reas71, reas72, reas73, reas74, reas75, reas76, reas77, reas78, reas79, reas80, reas81, reas82, reas83, reas84, reas85, reas86, reas87, reas88, reas89, reas90, reas91, reas92, reas93, reas94, reas95, reas96, reas97, reas98, reas99);
            return Json(result);
        }

        public async Task<IActionResult> Return_Owtward_chq_list()
        {
            ViewBag.Tree = await _outwordService.GetAllCategoriesForTree();
            var model = await _outwordService.Return_Owtward_chq_list();
            return View(model);
        }

        public async Task<IActionResult> Return_Owtward_chq_list_Search(string Branchs, string STATUS, string BenAccNo, string AccType, string FromBank, string ToBank, string Currency, string ChequeSource, string Amount, string DRWAccNo, string ChequeNo, string waspdc)
        {
            var model = await _outwordService.Return_Owtward_chq_list_Search(Branchs, STATUS, BenAccNo, AccType, FromBank, ToBank, Currency, ChequeSource, Amount, DRWAccNo, ChequeNo, waspdc);
            return PartialView("_SearchListDisPMAResults", model);
        }

        public async Task<IActionResult> getPospondingChq(string Currency, string ChequeSource, string WASPDC, string Branchs, string order, string inputerr, string ChequeStatus, string vip)
        {
            var model = await _outwordService.getPospondingChq(Currency, ChequeSource, WASPDC, Branchs, order, inputerr, ChequeStatus, vip);
            return PartialView("_PospondingChqResults", model);
        }

        public async Task<IActionResult> getreturnList(string Currency, string ChequeSource, string WASPDC, string Branchs, string order, string inputerr, string ChequeStatus, string vip)
        {
            var model = await _outwordService.getreturnList(Currency, ChequeSource, WASPDC, Branchs, order, inputerr, ChequeStatus, vip);
            return PartialView("_ReturnListResults", model);
        }

        public async Task<IActionResult> ReturnOWTWORDScreen(string id)
        {
            ViewBag.Tree = await _outwordService.GetAllCategoriesForTree();
            var model = await _outwordService.ReturnOWTWORDScreen(id);
            return View(model);
        }

        public async Task<IActionResult> FillClearCenter()
        {
            var model = await _outwordService.FillClearCenter();
            return PartialView("_ClearCenterList", model);
        }

        public async Task<IActionResult> FillClearCenterout()
        {
            var model = await _outwordService.FillClearCenterout();
            return PartialView("_ClearCenterList", model);
        }

        public async Task<IActionResult> getMagicscreenList(string Currency, string ChequeSource, string WASPDC, string Branchs, string order, string inputerr, string ChequeStatus, string vip)
        {
            var model = await _outwordService.getMagicscreenList(Currency, ChequeSource, WASPDC, Branchs, order, inputerr, ChequeStatus, vip);
            return PartialView("_MagicScreenListResults", model);
        }

        public async Task<IActionResult> savepostedstatus(string id, string status)
        {
            var result = await _outwordService.savepostedstatus(id, status);
            return Json(result);
        }

        public async Task<IActionResult> AllInwardoutwardChq(string Currency, string ChequeSource, string WASPDC, string Branchs, string order, string inputerr, string ChequeStatus, string vip)
        {
            var model = await _outwordService.AllInwardoutwardChq(Currency, ChequeSource, WASPDC, Branchs, order, inputerr, ChequeStatus, vip);
            return PartialView("_AllInwardOutwardChqResults", model);
        }

        public async Task<IActionResult> All_INW_OUT_CHQ(string Currency, string ChequeSource, string WASPDC, string Branchs, string order, string inputerr, string ChequeStatus, string vip)
        {
            var model = await _outwordService.All_INW_OUT_CHQ(Currency, ChequeSource, WASPDC, Branchs, order, inputerr, ChequeStatus, vip);
            return PartialView("_AllInwOutChqResults", model);
        }

        public async Task<IActionResult> deleteoutchq(string id)
        {
            var result = await _outwordService.deleteoutchq(id);
            return Json(result);
        }

        public async Task<IActionResult> deletetimeoutchq(string id)
        {
            var result = await _outwordService.deletetimeoutchq(id);
            return Json(result);
        }

        public async Task<IActionResult> RepresentReturnDis(string id)
        {
            var model = await _outwordService.RepresentReturnDis(id);
            return View(model);
        }

        public async Task<IActionResult> FindChq(string id)
        {
            var model = await _outwordService.FindChq(id);
            return View(model);
        }

        public async Task<IActionResult> Get_Outward_Slip_CCS(string id)
        {
            var model = await _outwordService.Get_Outward_Slip_CCS(id);
            return View(model);
        }

        public async Task<IActionResult> FindChqdis(string id)
        {
            var model = await _outwordService.FindChqdis(id);
            return View(model);
        }

        public async Task<IActionResult> Update_ChqDate_Represnet(string id)
        {
            var model = await _outwordService.Update_ChqDate_Represnet(id);
            return View(model);
        }

        public async Task<IActionResult> PospondingChq(string id)
        {
            var model = await _outwordService.PospondingChq(id);
            return View(model);
        }

        public async Task<IActionResult> retunedchqstate(string id)
        {
            var model = await _outwordService.retunedchqstate(id);
            return View(model);
        }

        public async Task<IActionResult> InsertOutword(string id)
        {
            var model = await _outwordService.InsertOutword(id);
            return View(model);
        }

        public async Task<IActionResult> update_Reverse_Outword(string id)
        {
            var model = await _outwordService.update_Reverse_Outword(id);
            return View(model);
        }

        public async Task<IActionResult> UpdateChqDate(string id)
        {
            var model = await _outwordService.UpdateChqDate(id);
            return View(model);
        }

        public async Task<IActionResult> updatechqstat(string id)
        {
            var model = await _outwordService.updatechqstat(id);
            return View(model);
        }

        public async Task<IActionResult> updateAllchqstat(string id)
        {
            var model = await _outwordService.updateAllchqstat(id);
            return View(model);
        }

        public async Task<IActionResult> outward_views(string id)
        {
            var model = await _outwordService.outward_views(id);
            return View(model);
        }
    }
}
