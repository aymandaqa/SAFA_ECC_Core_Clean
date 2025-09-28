using SAFA_ECC_Core_Clean.Models;
using System.Collections.Generic;
using SAFA_ECC_Core_Clean.ViewModels.Enums;

namespace SAFA_ECC_Core_Clean.ViewModels.OutwordViewModels
{
    public class ReturnOwtwardViewModel
    {
        public List<CHEQUE_STATUS_ENU> ChequeStatuses { get; set; }
        public List<ChequeSource> ChequeSources { get; set; }
        public string Title { get; set; }
    }
}

