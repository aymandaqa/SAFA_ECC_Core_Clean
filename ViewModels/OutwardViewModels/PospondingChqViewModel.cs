using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.OutwardViewModels
{
    public class PospondingChqViewModel
    {
        public PospondedChequeViewModel NewCheque { get; set; } = new PospondedChequeViewModel();
        public List<PospondedChequeViewModel> Cheques { get; set; } = new List<PospondedChequeViewModel>();
    }

    public class PospondedChequeViewModel
    {
        [Display(Name = "رقم الشيك")]
        public string? ChequeNumber { get; set; }

        [Display(Name = "تاريخ الشيك")]
        [DataType(DataType.Date)]
        public DateTime ChequeDate { get; set; }

        [Display(Name = "المستفيد")]
        public string? Beneficiary { get; set; }

        [Display(Name = "المبلغ")]
        public decimal Amount { get; set; }

        [Display(Name = "سبب التأجيل")]
        public string? Reason { get; set; }
    }
}
