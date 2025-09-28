using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.InwardViewModels
{
    public class GET_RETURN_CHQ_BY_CLRCENTER_ViewModel
    {
        [Display(Name = "معرف مركز المقاصة")]
        public string? ClearingCenterId { get; set; }

        public List<LookupItemViewModel>? ClearingCenters { get; set; }

        [Display(Name = "من تاريخ")]
        [DataType(DataType.Date)]
        public DateTime? FromDate { get; set; }

        [Display(Name = "إلى تاريخ")]
        [DataType(DataType.Date)]
        public DateTime? ToDate { get; set; }

        [Display(Name = "معرف الحالة")]
        public string? StatusId { get; set; }

        public List<LookupItemViewModel>? Statuses { get; set; }

        public List<ReturnCheckViewModel>? ReturnChecks { get; set; }
    }

    public class ReturnCheckViewModel
    {
        public string? CheckNumber { get; set; }
        public decimal Amount { get; set; }
        public DateTime ReturnDate { get; set; }
        public string? ClearingCenterName { get; set; }
        public string? StatusName { get; set; }
    }

    public class LookupItemViewModel
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
    }
}
