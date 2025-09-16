using System;

namespace SAFA_ECC_Core_Clean.Models
{
    public class Get_Outward_Slip_CCS_VIEW
    {
        public int Serial { get; set; }
        public string? DrwChqNo { get; set; }
        public string? DrwAcctNo { get; set; }
        public string? DrwBankNo { get; set; }
        public string? DrwBranchNo { get; set; }
        public string? BenAccountNo { get; set; }
        public string? DrwName { get; set; }
        public string? Currency { get; set; }
        public decimal Amount { get; set; }
        public DateTime InputDate { get; set; }
        public DateTime ValueDate { get; set; }
        public string? ClrCenter { get; set; }
        public string? Status { get; set; }
        public string? ErrorMsg { get; set; }
        public string? Locked_user { get; set; }
    }
}

