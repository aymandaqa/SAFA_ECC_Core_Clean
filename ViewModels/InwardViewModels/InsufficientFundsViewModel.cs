using System.Collections.Generic;
using SAFA_ECC_Core_Clean.Models;

namespace SAFA_ECC_Core_Clean.ViewModels.InwardViewModels
{
    public class InsufficientFundsViewModel
    {
        public List<ClearingCenter> ClearingCenters { get; set; }
        public List<CompanyViewModel> Branches { get; set; }
        public List<CustomerType> CustomerTypes { get; set; }
        public List<CURRENCY_TBL> Currencies { get; set; }
        public string AdminAuthorized { get; set; }
        public object Tree { get; set; } // Assuming this is for a tree view structure
    }

    public class CompanyViewModel
    {
        public string Company_ID { get; set; }
        public string Company_Code { get; set; }
        public string Company_Name { get; set; }
    }

    // Placeholder for CustomerType - needs actual definition if it's a model
    public class CustomerType
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}

