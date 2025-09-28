using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.OutwardViewModels
{
    public class PenddingOutWordRequestViewModel
    {
        [Display(Name = "رقم الطلب")]
        public string? RequestNumber { get; set; }

        [Display(Name = "تاريخ الطلب")]
        [DataType(DataType.Date)]
        public DateTime? RequestDate { get; set; }

        [Display(Name = "المبلغ")]
        public decimal Amount { get; set; }

        [Display(Name = "الحالة")]
        public string? Status { get; set; }

        public List<PenddingRequestItemViewModel>? PenddingRequests { get; set; }
    }

    public class PenddingRequestItemViewModel
    {
        public int RequestId { get; set; }
        public DateTime RequestDate { get; set; }
        public string? RequestType { get; set; }
        public string? Status { get; set; }
    }
}
