using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.OutwardViewModels
{
    public class OutwardChqViewModel
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

        [Display(Name = "الحالة")]
        public string? Status { get; set; }

        public List<OutwardChqItemViewModel>? Cheques { get; set; }
    }

    public class OutwardChqItemViewModel
    {
        public string? ChequeNumber { get; set; }
        public decimal Amount { get; set; }
        public string? BeneficiaryName { get; set; }
        public string? BankName { get; set; }
        public DateTime DueDate { get; set; }
        public string? Status { get; set; }
    }
}
