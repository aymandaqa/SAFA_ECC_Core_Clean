using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.PDCViewModels
{
    public class FaildPostDateVerficationDetailsViewModel
    {
        [Display(Name = "رقم الشيك")]
        public string? ChequeNumber { get; set; }

        [Display(Name = "تاريخ الاستحقاق")]
        [DataType(DataType.Date)]
        public DateTime? DueDate { get; set; }

        [Display(Name = "المبلغ")]
        public decimal Amount { get; set; }

        [Display(Name = "سبب الفشل")]
        public string? FailureReason { get; set; }

        public List<FailedVerificationItemViewModel>? FailedItems { get; set; }

        // Properties expected by the view
        [Display(Name = "معرف المعاملة")]
        public string? TransactionId { get; set; }

        [Display(Name = "تاريخ التحقق")]
        [DataType(DataType.Date)]
        public DateTime? VerificationDate { get; set; }

        [Display(Name = "الحالة")]
        public string? Status { get; set; }

        [Display(Name = "السبب")]
        public string? Reason { get; set; }

        [Display(Name = "ملاحظات")]
        public string? Notes { get; set; }

        public List<ChangeLogItemViewModel>? ChangeLog { get; set; }
    }

    public class FailedVerificationItemViewModel
    {
        public string? ChequeNumber { get; set; }
        public decimal Amount { get; set; }
        public DateTime DueDate { get; set; }
        public string? Reason { get; set; }
    }

    public class ChangeLogItemViewModel
    {
        public DateTime Date { get; set; }
        public string? User { get; set; }
        public string? Operation { get; set; }
        public string? Details { get; set; }
    }
}
