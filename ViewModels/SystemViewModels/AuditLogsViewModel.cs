using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.SystemViewModels
{
    public class AuditLogsViewModel
    {
        [Display(Name = "من تاريخ")]
        [DataType(DataType.Date)]
        public DateTime? FromDate { get; set; }

        [Display(Name = "إلى تاريخ")]
        [DataType(DataType.Date)]
        public DateTime? ToDate { get; set; }

        [Display(Name = "اسم المستخدم")]
        public string? UserName { get; set; }

        [Display(Name = "نوع الإجراء")]
        public string? ActionType { get; set; }

        public List<AuditLogItemViewModel>? AuditLogItems { get; set; }
    }

    public class AuditLogItemViewModel
    {
        public DateTime Timestamp { get; set; }
        public string? UserName { get; set; }
        public string? Action { get; set; }
        public string? Details { get; set; }
        public string? IPAddress { get; set; }
    }
}
