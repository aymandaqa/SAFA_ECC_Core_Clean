using System;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.OutwardViewModels
{
    public class UpdateReverseOutwordViewModel
    {
        [Display(Name = "معرف الصادر")]
        public int OutwordId { get; set; }

        [Display(Name = "رقم الشيك")]
        public string? ChequeNumber { get; set; }

        [Display(Name = "تاريخ العكس")]
        [DataType(DataType.Date)]
        public DateTime? ReverseDate { get; set; }

        [Display(Name = "سبب العكس")]
        public string? ReverseReason { get; set; }

        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
    }
}
