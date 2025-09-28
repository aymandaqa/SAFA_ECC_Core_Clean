using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.CommissionsViewModels
{
    public class AuthorizeCommissionViewModel
    {
        [Display(Name = "معرف العمولة")]
        public int CommissionId { get; set; }

        [Display(Name = "اسم العميل")]
        public string? CustomerName { get; set; }

        [Display(Name = "المبلغ")]
        public decimal Amount { get; set; }

        [Display(Name = "تاريخ الإنشاء")]
        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "الحالة")]
        public string? Status { get; set; }

        [Display(Name = "ملاحظات التفويض")]
        public string? AuthorizationNotes { get; set; }

        [Display(Name = "الموافقة على التفويض")]
        public bool IsApproved { get; set; }

        public List<CommissionItemViewModel>? CommissionItems { get; set; }
    }

    public class CommissionItemViewModel
    {
        public int ItemId { get; set; }
        public string? Description { get; set; }
        public decimal Value { get; set; }
    }
}
