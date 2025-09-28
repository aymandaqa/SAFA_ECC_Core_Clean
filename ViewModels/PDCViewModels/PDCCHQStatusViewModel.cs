using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.PDCViewModels
{
    public class PDCCHQStatusViewModel
    {
        [Display(Name = "رقم الشيك")]
        public string? ChequeNumber { get; set; }

        [Display(Name = "تاريخ الاستحقاق")]
        [DataType(DataType.Date)]
        public DateTime? DueDate { get; set; }

        [Display(Name = "المبلغ")]
        public decimal Amount { get; set; }

        [Display(Name = "الحالة الحالية")]
        public string? CurrentStatus { get; set; }

        [Display(Name = "الحالة الجديدة")]
        public string? NewStatus { get; set; }

        [Display(Name = "ملاحظات")]
        public string? Notes { get; set; }

        public List<ChequeStatusLogViewModel>? StatusLog { get; set; }
    }

    public class ChequeStatusLogViewModel
    {
        public DateTime ChangeDate { get; set; }
        public string? ChangedBy { get; set; }
        public string? OldStatus { get; set; }
        public string? NewStatus { get; set; }
        public string? Remarks { get; set; }
    }
}
