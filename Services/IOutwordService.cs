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
