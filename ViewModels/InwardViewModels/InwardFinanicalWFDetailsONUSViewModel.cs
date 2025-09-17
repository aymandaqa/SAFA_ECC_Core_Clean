using System.Collections.Generic;
using SAFA_ECC_Core_Clean.Models;

namespace SAFA_ECC_Core_Clean.ViewModels.InwardViewModels
{
    public class InwardFinanicalWFDetailsONUSViewModel
    {
        public OnUs_Tbl InwardCheque { get; set; }
        public object Tree { get; set; }
        public string BookedBalance { get; set; }
        public string ClearBalance { get; set; }
        public string AccountStatus { get; set; }
        public string GuarranteedCustomerAccounts { get; set; } // Renamed from GUAR_CUSTOMER for clarity
        public bool CanReject { get; set; }
        public bool CanApprove { get; set; }
        public bool ShowRecommendationButton { get; set; }
        public string Title { get; set; }
        public List<CURRENCY_TBL> Currencies { get; set; }
        public bool IsVIP { get; set; }
    }
}

