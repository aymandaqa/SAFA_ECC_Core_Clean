using System.Collections.Generic;
using SAFA_ECC_Core_Clean.Models;

namespace SAFA_ECC_Core_Clean.ViewModels.OutwordViewModels
{
    public class FindChqdisViewModel
    {
        public string ErrorMsg { get; set; }
        public List<Outward_Trans> OutwardTransList { get; set; }
        public List<Return_Codes_Tbl> DiscountReturnCodes { get; set; }

        public FindChqdisViewModel()
        {
            OutwardTransList = new List<Outward_Trans>();
            DiscountReturnCodes = new List<Return_Codes_Tbl>();
        }
    }
}

