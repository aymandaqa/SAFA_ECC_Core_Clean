using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.InwardViewModels
{
    public class AmanitestViewModel
    {
        [Display(Name = "معرف الاختبار")]
        public int TestId { get; set; }

        [Display(Name = "اسم الاختبار")]
        public string? TestName { get; set; }

        [Display(Name = "تاريخ الاختبار")]
        [DataType(DataType.Date)]
        public DateTime TestDate { get; set; }

        [Display(Name = "النتيجة")]
        public string? Result { get; set; }

        public List<AmaniTestItemViewModel>? TestItems { get; set; }
    }

    public class AmaniTestItemViewModel
    {
        public int ItemId { get; set; }
        public string? ItemDescription { get; set; }
        public bool IsPassed { get; set; }
    }
}
