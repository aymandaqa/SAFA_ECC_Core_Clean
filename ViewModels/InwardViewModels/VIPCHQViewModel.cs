using System.Collections.Generic;
using SAFA_ECC_Core_Clean.Models;

namespace SAFA_ECC_Core_Clean.ViewModels.InwardViewModels
{
    public class VIPCHQViewModel
    {
        public string Title { get; set; }
        public List<Category> Tree { get; set; }
        public List<Bank_Branches_Tbl> Branches { get; set; }
        public List<Return_Codes_Tbl> ReturnDescriptions { get; set; }
    }
}
