namespace SAFA_ECC_Core_Clean.Models
{
    public class AccountList_RESPONSE
    {
        public List<AccountInfo> Account { get; set; } = new List<AccountInfo>();
        public string Note { get; set; }
    }

    public class AccountInfo
    {
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public string Currency { get; set; }
        public string OwnerBranch { get; set; }
    }
}

