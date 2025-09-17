
using SAFA_ECC_Core_Clean.Models;
using System.Collections.Generic;

namespace SAFA_ECC_Core_Clean.ViewModels.OutwordViewModels
{
    public class OutwordDateVerificationViewModel
    {
        public Outward_Trans OutwardTrans { get; set; }
        public Outward_Imgs OutwardImgs { get; set; }
        public List<CURRENCY_TBL> Currencies { get; set; }
        // Add any other properties needed for the view
    }
}

