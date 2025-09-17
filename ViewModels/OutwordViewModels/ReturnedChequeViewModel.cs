using System;

namespace SAFA_ECC_Core_Clean.ViewModels.OutwordViewModels
{
    public class ReturnedChequeViewModel
    {
        public decimal Amount { get; set; }
        public string History { get; set; }
        public string Serial { get; set; }
        public string ChqSequance { get; set; }
        public DateTime? LastUpdate { get; set; }
        public string LastUpdateBy { get; set; }
        public DateTime? TransDate { get; set; }
        public string DrwAcctNo { get; set; }
        public string DrwChqNo { get; set; }
        public string BenfBnk { get; set; }
        public string BenAccountNo { get; set; }
        public string BenfAccBranch { get; set; }
        public string ClrCenter { get; set; }
        public string BenName { get; set; }
        public string DrwName { get; set; }
        public string DrwBankNo { get; set; }
        public string DrwBranchNo { get; set; }
        public string Currency { get; set; }
        public string ISSAccount { get; set; }
        public string CHQState { get; set; }
        public string ReturnedCode { get; set; }
    }
}

