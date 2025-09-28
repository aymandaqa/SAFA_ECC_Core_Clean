using System;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.OutwardViewModels
{
    public class DeleteOutwardChequeViewModel
    {
        [Display(Name = "معرف الشيك")]
        public int ChequeId { get; set; }

        [Display(Name = "رقم الشيك")]
        public string? ChequeNumber { get; set; }

        [Display(Name = "تاريخ الحذف")]
        [DataType(DataType.Date)]
        public DateTime? DeletionDate { get; set; }

        [Display(Name = "سبب الحذف")]
        public string? Reason { get; set; }

        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
    }
}
