using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.CommissionsViewModels
{
    public class CommissionsReportViewModel
    {
        [Display(Name = "تاريخ البدء")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Display(Name = "تاريخ الانتهاء")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Display(Name = "نوع العمولة")]
        public string? CommissionType { get; set; }

        [Display(Name = "اسم الوكيل")]
        public string? AgentName { get; set; }

        public List<CommissionReportItemViewModel>? Commissions { get; set; }
    }

    public class CommissionReportItemViewModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string? Description { get; set; }
        public decimal Amount { get; set; }
        public string? Status { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? AgentName { get; set; }
    }
}
