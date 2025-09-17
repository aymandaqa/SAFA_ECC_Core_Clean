using System;

namespace SAFA_ECC_Core_Clean.ViewModels.OutwordViewModels
{
    public class ReturnedChequeDetailsViewModel
    {
        public string ReturnedCode { get; set; }
        public DateTime? ValueDate { get; set; }
        public string Serial { get; set; }
        public string ISSAccount { get; set; }
        public DateTime? InputDate { get; set; }
        public string DrwChqNo { get; set; }
        public string DrwBankNo { get; set; }
        public string DrwBranchNo { get; set; }
        public string Currency { get; set; }
        public decimal? Amount { get; set; }
        public string DrwName { get; set; }
        public string BenAccountNo { get; set; }
        public string BenName { get; set; }
        public string DrwAcctNo { get; set; }
        public string ClrCenter { get; set; }
        public string ErrorDescription { get; set; }
    }
}

