using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.InwardViewModels
{
    public class InwardCustomerDuesViewModel
    {
        [Display(Name = "اسم العميل")]
        public string? CustomerName { get; set; }

        [Display(Name = "رقم الحساب")]
        public string? AccountNumber { get; set; }

        [Display(Name = "المبلغ المستحق")]
        public decimal DueAmount { get; set; }

        [Display(Name = "تاريخ الاستحقاق")]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        [Display(Name = "الحالة")]
        public string? Status { get; set; }

        public List<CustomerDueItemViewModel>? CustomerDues { get; set; }
    }

    public class CustomerDueItemViewModel
    {
        public string? ChequeNumber { get; set; }
        public decimal Amount { get; set; }
        public DateTime DueDate { get; set; }
        public string? Status { get; set; }
    }
}
