using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.InwardViewModels
{
    public class InwardViewModel
    {
        [Display(Name = "رقم الإدخال")]
        public string? InwardNumber { get; set; }

        [Display(Name = "تاريخ الإدخال")]
        [DataType(DataType.Date)]
        public DateTime? InwardDate { get; set; }

        [Display(Name = "المبلغ")]
        public decimal Amount { get; set; }

        [Display(Name = "الحالة")]
        public string? Status { get; set; }

        [Display(Name = "مصدر الإشعار")]
        public string? InwardSource { get; set; }

        [Display(Name = "موضوع الإشعار")]
        public string? InwardSubject { get; set; }

        public List<InwardItemViewModel> Inwards { get; set; } = new List<InwardItemViewModel>();
    }

    public class InwardItemViewModel
    {
        public int InwardId { get; set; }
        public DateTime InwardDate { get; set; }
        public string? InwardSource { get; set; }
        public string? InwardSubject { get; set; }
    }
}
