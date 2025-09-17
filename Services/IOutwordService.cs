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

