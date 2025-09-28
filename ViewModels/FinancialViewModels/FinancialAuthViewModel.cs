using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.FinancialViewModels
{
    public class FinancialAuthViewModel
    {
        [Display(Name = "معرف المعاملة")]
        public int TransactionId { get; set; }

        [Display(Name = "تاريخ المعاملة")]
        [DataType(DataType.Date)]
        public DateTime TransactionDate { get; set; }

        [Display(Name = "المبلغ")]
        public decimal Amount { get; set; }

        [Display(Name = "الوصف")]
        public string? Description { get; set; }

        [Display(Name = "الحالة")]
        public string? Status { get; set; }

        public List<FinancialTransactionItemViewModel>? Transactions { get; set; }
    }

    public class FinancialTransactionItemViewModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string? Type { get; set; }
        public decimal Amount { get; set; }
        public string? Details { get; set; }
    }
}
