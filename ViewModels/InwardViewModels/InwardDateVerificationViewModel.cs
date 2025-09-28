using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.InwardViewModels
{
    public class InwardDateVerificationViewModel
    {
        [Display(Name = "رقم الإدخال")]
        public string? InwardNumber { get; set; }

        [Display(Name = "تاريخ التحقق")]
        [DataType(DataType.Date)]
        public DateTime? VerificationDate { get; set; }

        public bool HasSearched { get; set; }
        public List<InwardVerificationResultViewModel>? VerificationResults { get; set; }
    }

    public class InwardVerificationResultViewModel
    {
        public string? InwardNumber { get; set; }
        public DateTime InwardDate { get; set; }
        public bool IsVerified { get; set; }
        public string? Notes { get; set; }
    }
}
