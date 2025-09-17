using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.OutwordViewModels
{
    public class UpdatePostOutwordViewModel
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
        public bool? WasPDC { get; set; }
        public string History { get; set; }
        public string DrwName { get; set; }
        public string BenfBnk { get; set; }
        public string BenfAccBranch { get; set; }
        public string BenfNationality { get; set; }
        public string WithUV { get; set; }
        public string SpecialHandling { get; set; }
        public bool? IsVIP { get; set; }
        public bool? NeedTechnicalVerification { get; set; }
        public AllEnums.Cheque_Status? Posted { get; set; }
        public string Status { get; set; }
        public int? ErrorCode { get; set; }
        public int? IsTimeOut { get; set; }
        public AllEnums.TransStatus? FaildTrans { get; set; }
        public DateTime? LastUpdate { get; set; }
        public DateTime? TransDate { get; set; }

        // Properties for reasons (up to 100 reasons)
        public List<string> Reasons { get; set; } = new List<string>();

        public string Note { get; set; }
        public string ReturnChq { get; set; }
        public string ChqNu { get; set; }
        public string Chq { get; set; }
        public string ChqAm { get; set; }
        public string ChqBn { get; set; }
        public string ChqBr { get; set; }
        public string ChqAc { get; set; }
        public string ChqDate { get; set; }
    }
}

