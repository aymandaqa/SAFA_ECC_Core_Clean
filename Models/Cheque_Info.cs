namespace SAFA_ECC_Core_Clean.Models
{
    public class Cheque_Info
    {
        public string DrwBranch { get; set; }
        public string DrwBank { get; set; }
        public string DrwChequeNo { get; set; }
        public string DrwAccountNo { get; set; }
        public decimal Amount { get; set; }
        public string ISSAccount { get; set; } // Assuming this is needed for GetBnefNameAR
    }
}

