using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.PDCViewModels
{
    public class FailedT24TransactionViewModel
    {
        [Display(Name = "معرف المعاملة")]
        public string? TransactionId { get; set; }

        [Display(Name = "تاريخ المعاملة")]
        [DataType(DataType.Date)]
        public DateTime? TransactionDate { get; set; }

        [Display(Name = "المبلغ")]
        public decimal Amount { get; set; }

        [Display(Name = "العملة")]
        public string? Currency { get; set; }

        [Display(Name = "سبب الفشل")]
        public string? FailureReason { get; set; }

        public List<FailedT24TransactionItemViewModel>? Transactions { get; set; }
    }

    public class FailedT24TransactionItemViewModel
    {
        public string? TransactionId { get; set; }
        public decimal Amount { get; set; }
        public string? Currency { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? FailureReason { get; set; }
    }
}
