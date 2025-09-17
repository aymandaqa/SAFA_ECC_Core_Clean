
using Microsoft.AspNetCore.Mvc.Rendering;
using SAFA_ECC_Core_Clean.Models;
using System.Collections.Generic;

namespace SAFA_ECC_Core_Clean.ViewModels.OutwordViewModels
{
    public class OutwordPDCViewModel
    {
        public List<CHEQUE_STATUS_ENU> ChequeStatuses { get; set; }
        public List<Companies_Tbl> Branches { get; set; }
        public List<ClearingCenter> ClearingCenters { get; set; }
        public List<CURRENCY_TBL> Currencies { get; set; }
        public List<Users_Tbl> Users { get; set; }
        // Add any other properties needed for the view
    }
}

