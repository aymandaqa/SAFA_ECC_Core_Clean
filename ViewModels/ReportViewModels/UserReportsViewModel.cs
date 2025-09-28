using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.ReportViewModels
{
    public class UserReportsViewModel
    {
        [Display(Name = "اسم المستخدم")]
        public string? UserName { get; set; }

        [Display(Name = "من تاريخ")]
        [DataType(DataType.Date)]
        public DateTime? FromDate { get; set; }

        [Display(Name = "إلى تاريخ")]
        [DataType(DataType.Date)]
        public DateTime? ToDate { get; set; }

        [Display(Name = "نوع النشاط")]
        public string? ActivityType { get; set; }

        public List<UserReportItemViewModel>? ReportItems { get; set; }
    }

    public class UserReportItemViewModel
    {
        public DateTime ActivityDate { get; set; }
        public string? ActivityType { get; set; }
        public string? Description { get; set; }
        public string? IPAddress { get; set; }
    }
}
