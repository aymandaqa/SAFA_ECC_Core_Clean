using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.PDCViewModels
{
    public class PDCWithdrowViewModel
    {
        [Display(Name = "رقم الشيك")]
        public string? ChequeNumber { get; set; }

        [Display(Name = "تاريخ السحب")]
        [DataType(DataType.Date)]
        public DateTime? WithdrawalDate { get; set; }

        [Display(Name = "المبلغ")]
        public decimal Amount { get; set; }

        [Display(Name = "اسم المستفيد")]
        public string? BeneficiaryName { get; set; }

        [Display(Name = "ملاحظات")]
        public string? Notes { get; set; }

        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
    }
}
