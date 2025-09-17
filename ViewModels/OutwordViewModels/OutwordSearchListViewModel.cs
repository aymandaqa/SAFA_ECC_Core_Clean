using System;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.OutwordViewModels
{
    public class OutwordSearchListViewModel
    {
        public string Serial { get; set; }
        public string InputBrn { get; set; }
        public string ChqSequance { get; set; }
        public string InputDate { get; set; }
        public string DrwChqNo { get; set; }
        public string Commision_Response { get; set; }
        public string DrwBankNo { get; set; }
        public string DrwBranchNo { get; set; }
        public string Currency { get; set; }
        public decimal? Amount { get; set; }
        public string ClrCenter { get; set; }
        public string ValueDate { get; set; }
        public string BenAccountNo { get; set; }
        public string BenName { get; set; }
        public string UserName { get; set; }
        public string AuthorizedBy { get; set; }
        public string RSFAddtlInf { get; set; }
        public string QVFStatus { get; set; }
        public string QVFAddtlInf { get; set; }
        public string RSFStatus { get; set; }
        public string DrwAcctNo { get; set; }
        public string ErrorDescription { get; set; }
        public string WasPDC { get; set; }
        public string History { get; set; }
    }
}

