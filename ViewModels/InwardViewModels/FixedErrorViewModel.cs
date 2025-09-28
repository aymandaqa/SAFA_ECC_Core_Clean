using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.InwardViewModels
{
    public class FixedErrorViewModel
    {
        [Display(Name = "معرف الخطأ")]
        public int ErrorId { get; set; }

        [Display(Name = "وصف الخطأ")]
        public string? ErrorDescription { get; set; }

        [Display(Name = "تاريخ الإصلاح")]
        [DataType(DataType.Date)]
        public DateTime? FixDate { get; set; }

        [Display(Name = "ملاحظات الإصلاح")]
        public string? FixNotes { get; set; }

        [Display(Name = "تاريخ الخطأ")]
        [DataType(DataType.Date)]
        public DateTime? ErrorDate { get; set; }

        [Display(Name = "المستخدم الذي أبلغ عن الخطأ")]
        public string? ReportedByUser { get; set; }

        [Display(Name = "حالة الخطأ")]
        public string? ErrorStatus { get; set; }

        public List<ErrorLogItemViewModel>? ErrorLog { get; set; }
    }

    public class ErrorLogItemViewModel
    {
        public DateTime Timestamp { get; set; }
        public string? Message { get; set; }
        public string? User { get; set; }
    }
}
