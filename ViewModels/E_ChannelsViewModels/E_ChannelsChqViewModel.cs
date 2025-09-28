using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.E_ChannelsViewModels
{
    public class E_ChannelsChqViewModel
    {
        [Display(Name = "رقم الشيك")]
        public string? ChequeNumber { get; set; }

        [Display(Name = "رقم الحساب")]
        public string? AccountNumber { get; set; }

        [Display(Name = "تاريخ البدء")]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [Display(Name = "تاريخ الانتهاء")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        [Display(Name = "المبلغ")]
        public decimal Amount { get; set; }

        [Display(Name = "تاريخ الشيك")]
        [DataType(DataType.Date)]
        public DateTime ChequeDate { get; set; }

        [Display(Name = "حالة الشيك")]
        public string? ChequeStatus { get; set; }

        public List<E_ChannelsChqItemViewModel>? Cheques { get; set; }
    }

    public class E_ChannelsChqItemViewModel
    {
        public int Id { get; set; }
        public string? ChequeNumber { get; set; }
        public string? AccountNumber { get; set; }
        public decimal Amount { get; set; }
        public DateTime ChequeDate { get; set; }
        public string? Status { get; set; }
    }
}
