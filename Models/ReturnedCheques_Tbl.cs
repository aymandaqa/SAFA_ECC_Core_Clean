using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAFA_ECC_Core_Clean.Models
{
    public class ReturnedCheques_Tbl
    {
        [Key]
        public int ID { get; set; }
        public string Serial { get; set; }
        public string ChqSequance { get; set; }
        public string DrwChqNo { get; set; }
        public string DrwBankNo { get; set; }
        public string DrwBranchNo { get; set; }
        public string DrwAcctNo { get; set; }
        public string DrwName { get; set; }
        public string BenAccountNo { get; set; }
        public string BenfBnk { get; set; }
        public string BenfAccBranch { get; set; }
        public string BenName { get; set; }
        public double Amount { get; set; }
        public string Currency { get; set; }
        public DateTime TransDate { get; set; }
        public DateTime ValueDate { get; set; }
        public string ClrCenter { get; set; }
        public string ReturnCode { get; set; }
        public int Posted { get; set; }
        public bool Returned { get; set; }
        public int NoOfReturn { get; set; }
        public string History { get; set; }
        public string ReturnDateHistory { get; set; }
        public DateTime InputDate { get; set; }
        public string InputBy { get; set; }
        public DateTime LastUpdate { get; set; }
        public string LastUpdateBy { get; set; }
        public string Status { get; set; }
        public string System_Aut_Man { get; set; }
        public string TransCode { get; set; }
        public string UserId { get; set; }
        public string IntrBkSttlmDt { get; set; }
        public string ISSAccount { get; set; }
        public string DrwCardId { get; set; }
        public string DrwCardType { get; set; }
        public string BenfCardId { get; set; }
        public string BenfNationality { get; set; }
        public string OperType { get; set; }
        public string Discount_Img_ID { get; set; }
        public string DrwBranchExt { get; set; }
    }
}
