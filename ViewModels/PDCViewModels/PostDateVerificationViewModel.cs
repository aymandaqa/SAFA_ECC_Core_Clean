using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.PDCViewModels
{
    public class PostDateVerificationViewModel
    {
        [Display(Name = "من تاريخ")]
        [DataType(DataType.Date)]
        public DateTime? FromDate { get; set; }

        [Display(Name = "إلى تاريخ")]
        [DataType(DataType.Date)]
        public DateTime? ToDate { get; set; }

        [Display(Name = "رقم الشيك")]
        public string? ChequeNumber { get; set; }

        [Display(Name = "اسم البنك")]
        public string? BankName { get; set; }

        public List<PostDateVerificationItemViewModel>? ChequesToVerify { get; set; }
    }

    public class PostDateVerificationItemViewModel
    {
        public string? ChequeNumber { get; set; }
        public decimal Amount { get; set; }
        public string? BeneficiaryName { get; set; }
        public string? BankName { get; set; }
        public DateTime ChequeDate { get; set; }
        public bool IsVerified { get; set; }
    }
}
