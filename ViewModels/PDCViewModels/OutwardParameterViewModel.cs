using System;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.PDCViewModels
{
    public class OutwardParameterViewModel
    {
        [Display(Name = "معرف المعاملة")]
        public int TransactionId { get; set; }

        [Display(Name = "اسم المعاملة")]
        public string? TransactionName { get; set; }

        [Display(Name = "القيمة")]
        public string? Value { get; set; }

        [Display(Name = "الوصف")]
        public string? Description { get; set; }
    }
}
