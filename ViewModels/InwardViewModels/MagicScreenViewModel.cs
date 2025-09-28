using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.InwardViewModels
{
    public class MagicScreenViewModel
    {
        [Display(Name = "رقم الشيك")]
        public string? ChequeNumber { get; set; }

        [Display(Name = "تاريخ الشيك")]
        [DataType(DataType.Date)]
        public DateTime? ChequeDate { get; set; }

        [Display(Name = "المبلغ")]
        public decimal Amount { get; set; }

        [Display(Name = "اسم البنك")]
        public string? BankName { get; set; }

        [Display(Name = "حالة الشيك")]
        public string? ChequeStatus { get; set; }

        [Display(Name = "اسم المستفيد")]
        public string? BeneficiaryName { get; set; }

        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
    }
}
