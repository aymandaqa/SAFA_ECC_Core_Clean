using System.Threading.Tasks;
using SAFA_ECC_Core_Clean.Models;

namespace SAFA_ECC_Core_Clean.Services
{
    public class SAFA_T24_ECC_SVCSoapClient
    {
        public Task<AccountList_RESPONSE> AccountList(string customerNumber, int param2)
        {
            // This is a placeholder for the actual SOAP service call.
            // In a real scenario, this would involve WCF or gRPC client generation.
            var response = new AccountList_RESPONSE();
            // Simulate some data for testing purposes
            if (customerNumber == "12345")
            {
                response.Account.Add(new AccountInfo { AccountNumber = "123456789012", AccountName = "Test Account 1", Currency = "JOD", OwnerBranch = "PS001001" });
                response.Account.Add(new AccountInfo { AccountNumber = "987654321098", AccountName = "Test Account 2", Currency = "USD", OwnerBranch = "PS001002" });
            }
            else
            {
                response.Note = "No Accounts Exist";
            }
            return Task.FromResult(response);
        }

        public Task<AccountInfo_RESPONSE> ACCOUNT_INFO(string customerNumber, int param2)
        {
            // Placeholder for ACCOUNT_INFO method
            var response = new AccountInfo_RESPONSE();
            if (customerNumber == "012345" || customerNumber == "123456789012" || customerNumber == "1234567890123")
            {
                response.OwnerBranch = "PS001001";
            }
            return Task.FromResult(response);
        }
    }

    public class AccountInfo_RESPONSE
    {
        public string OwnerBranch { get; set; }
    }
}

