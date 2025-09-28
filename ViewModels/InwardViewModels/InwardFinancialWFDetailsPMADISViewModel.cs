using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.InwardViewModels
{
    public class InwardFinancialWFDetailsPMADISViewModel
    {
        [Display(Name = "معرف المعاملة")]
        public int TransactionId { get; set; }

        [Display(Name = "رقم الشيك")]
        public string? ChequeNumber { get; set; }

        [Display(Name = "المبلغ")]
        public decimal Amount { get; set; }

        [Display(Name = "تاريخ المعاملة")]
        [DataType(DataType.Date)]
        public DateTime TransactionDate { get; set; }

        [Display(Name = "حالة سير العمل")]
        public string? WorkflowStatus { get; set; }

        [Display(Name = "ملاحظات")]
        public string? Notes { get; set; }

        // Properties expected by the view
        [Display(Name = "رقم المعاملة")]
        public string? TransactionNumber { get; set; }

        [Display(Name = "تاريخ الإنشاء")]
        [DataType(DataType.Date)]
        public DateTime CreationDate { get; set; }

        [Display(Name = "الحالة")]
        public string? Status { get; set; }

        [Display(Name = "العملة")]
        public string? Currency { get; set; }

        [Display(Name = "البنك")]
        public string? BankName { get; set; }

        public List<WorkflowHistoryItemViewModel>? WorkflowHistory { get; set; }
    }
}
