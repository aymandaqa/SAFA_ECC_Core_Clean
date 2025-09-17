
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace SAFA_ECC_Core_Clean.ViewModels.OutwordViewModels
{
    public class OutwordSearchViewModel
    {
        public List<SelectListItem> Currencies { get; set; }
        public List<SelectListItem> ClearingCenters { get; set; }
        public List<SelectListItem> ChequeStatuses { get; set; }
        public List<SelectListItem> Branches { get; set; }
        public List<SelectListItem> Users { get; set; }
        // Add properties for search criteria if needed
        public string SelectedCurrency { get; set; }
        public string SelectedClearingCenter { get; set; }
        public string SelectedChequeStatus { get; set; }
        public string SelectedBranch { get; set; }
        public string SelectedUser { get; set; }

        public string Branchs { get; set; }
        public string STATUS { get; set; }
        public string BenAccNo { get; set; }
        public string AccType { get; set; }
        public string FromBank { get; set; }
        public string ToBank { get; set; }
        public string Currency { get; set; }
        public string ChequeSource { get; set; }
        public string Amount { get; set; }
        public string DRWAccNo { get; set; }
        public string ChequeNo { get; set; }
        public string waspdc { get; set; }
        public string order { get; set; }
        public string inputerr { get; set; }
        public string vip { get; set; }
    }
}

