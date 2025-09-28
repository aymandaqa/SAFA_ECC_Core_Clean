namespace SAFA_ECC_Core_Clean.ViewModels.Enums
{
    public static class AllEnums
    {
        public enum Cheque_Status
        {
            New = 0,
            Pending = 1,
            Approved = 2,
            Rejected = 3,
            Cancelled = 4,
            Converted = 5,
            Returned = 6,
            Posted = 7
        }

        public enum TransStatus
        {
            Success = 0,
            Failed = 1
        }

        public enum Group_Status
        {
            SystemAdmin = 1,
            AdminAuthorized = 2
        }
    }
}
