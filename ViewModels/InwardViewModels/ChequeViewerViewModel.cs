using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.InwardViewModels
{
    public class ChequeViewerViewModel
    {
        [Display(Name = "رقم الشيك")]
        public string? ChequeNumber { get; set; }

        [Display(Name = "اسم البنك")]
        public string? BankName { get; set; }

        [Display(Name = "المبلغ")]
        public decimal Amount { get; set; }

        [Display(Name = "تاريخ الشيك")]
        [DataType(DataType.Date)]
        public DateTime ChequeDate { get; set; }

        [Display(Name = "صورة الشيك الأمامية")]
        public string? FrontImageBase64 { get; set; }

        [Display(Name = "صورة الشيك الخلفية")]
        public string? RearImageBase64 { get; set; }

        public List<ChequeHistoryItemViewModel>? History { get; set; }
    }

    public class ChequeHistoryItemViewModel
    {
        public DateTime Date { get; set; }
        public string? Event { get; set; }
        public string? User { get; set; }
    }
}
