using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.InwardViewModels
{
    public class PMADataVerificationViewModel
    {
        [Display(Name = "معرف الملف")]
        public int FileId { get; set; }

        [Display(Name = "اسم الملف")]
        public string? FileName { get; set; }

        [Display(Name = "تاريخ التحقق")]
        [DataType(DataType.Date)]
        public DateTime VerificationDate { get; set; }

        [Display(Name = "الحالة")]
        public string? Status { get; set; }

        public List<PMADataItemViewModel>? DataItems { get; set; }
    }

    public class PMADataItemViewModel
    {
        public int ItemId { get; set; }
        public string? ChequeNumber { get; set; }
        public decimal Amount { get; set; }
        public string? VerificationStatus { get; set; }
        public string? Remarks { get; set; }
    }
}
