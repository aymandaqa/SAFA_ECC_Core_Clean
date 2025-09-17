using SAFA_ECC_Core_Clean.Models;
using System.Collections.Generic;

namespace SAFA_ECC_Core_Clean.ViewModels.InwardViewModels
{
    public class ReturnInwardStoppedChequeDetailsViewModel
    {
        public Inward_Trans InwardCheque { get; set; }
        public INWARD_IMAGES InwardImages { get; set; }
        public List<CURRENCY_TBL> Currencies { get; set; }
        public List<Return_Codes_Tbl> ReturnCodes { get; set; }
        public string Title { get; set; }
        public object Tree { get; set; }
        public string ErrorMessage { get; set; }
    }
}

