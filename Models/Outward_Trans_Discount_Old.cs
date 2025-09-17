using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAFA_ECC_Core_Clean.Models
{
    public class Outward_Trans_Discount_Old
    {
        [Key]
        public int Serial { get; set; }
        public string ChqSequance { get; set; }
        public int DrwBankNo { get; set; }
        public int DrwBranchNo { get; set; }
        public string DrwAcctNo { get; set; }
        public int BenfBnk { get; set; }
        public int BenfAccBranch { get; set; }
        public string BenAccountNo { get; set; }
        public string DrwChqNo { get; set; }
        public double Amount { get; set; }
        public string Currency { get; set; }
        public bool IsVIP { get; set; }
        public bool WasPDC { get; set; }
        public string ErrorDescription { get; set; }
        public string LastUpdateBy { get; set; }
        public int IsTimeOut { get; set; }
        public string History { get; set; }
        public int Posted { get; set; }
        public string Status { get; set; }
        public string ErrorCode { get; set; }
        public int FaildTrans { get; set; }
        public DateTime LastUpdate { get; set; }
        public DateTime TransDate { get; set; }
        public string Commision_Response { get; set; }
    }
}

