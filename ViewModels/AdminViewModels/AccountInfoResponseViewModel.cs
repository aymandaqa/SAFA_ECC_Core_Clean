using System.Collections.Generic;

namespace SAFA_ECC_Core_Clean.ViewModels.AdminViewModels
{
    public class AccountInfoResponseViewModel
    {
        public string ErrorMsg { get; set; }
        public string CustomerID { get; set; }
        public string Status { get; set; }
        public string BenName { get; set; }
        public string DocId { get; set; }
        public string BenBrn { get; set; }
        public string DocType { get; set; }
        public string Nat { get; set; }
        public object AccType { get; set; } // This can be a list of SelectListItem or a string
        public string Currency { get; set; }
        public string ResponseStatus { get; set; }
    }
}
