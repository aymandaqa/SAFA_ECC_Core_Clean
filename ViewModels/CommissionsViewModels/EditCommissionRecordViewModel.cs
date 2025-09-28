using System;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.CommissionsViewModels
{
    public class EditCommissionRecordViewModel
    {
        [Display(Name = "معرف العمولة")]
        public int Id { get; set; }

        [Display(Name = "تاريخ العمولة")]
        [DataType(DataType.Date)]
        public DateTime CommissionDate { get; set; }

        [Display(Name = "المبلغ")]
        public decimal Amount { get; set; }

        [Display(Name = "اسم الوكيل")]
        public string? AgentName { get; set; }

        [Display(Name = "الوصف")]
        public string? Description { get; set; }
    }
}
