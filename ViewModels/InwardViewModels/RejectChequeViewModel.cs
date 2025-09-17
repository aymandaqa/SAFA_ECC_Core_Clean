using System;
using System.Collections.Generic;

namespace SAFA_ECC_Core_Clean.ViewModels.InwardViewModels
{
    public class RejectChequeViewModel
    {
        public int ChequeId { get; set; }
        public string? ChequeNumber { get; set; }
        public string? Reason { get; set; }
        public string? AccountName { get; set; }
        public decimal Amount { get; set; }
        public string? RejectReason { get; set; }
    }
}


