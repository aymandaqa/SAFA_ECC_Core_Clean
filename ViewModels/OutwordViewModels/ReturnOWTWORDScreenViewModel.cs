using System;
using System.Collections.Generic;
using SAFA_ECC_Core_Clean.Models;

namespace SAFA_ECC_Core_Clean.ViewModels.OutwordViewModels
{
    public class ReturnOWTWORDScreenViewModel
    {
        public Outward_Trans OutwardTrans { get; set; }
        public Outward_Trans_Discount_Old OutwardTransDiscountOld { get; set; }
        public Outward_Trans_PDC_Old OutwardTransPDCOld { get; set; }
        public List<Return_Codes_Tbl> ReturnCodes { get; set; }
        public string ErrorMessage { get; set; }
        public string LockedUser { get; set; }
        public string UserName { get; set; }
        public string UserBranch { get; set; }
        public string UserCompany { get; set; }
        public string PageId { get; set; }
        public string RC { get; set; }
    }
}

