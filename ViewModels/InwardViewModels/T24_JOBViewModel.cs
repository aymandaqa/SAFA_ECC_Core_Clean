using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.InwardViewModels
{
    public class T24_JOBViewModel
    {
        [Display(Name = "معرف الوظيفة")]
        public int JobId { get; set; }

        [Display(Name = "اسم الوظيفة")]
        public string? JobName { get; set; }

        [Display(Name = "تاريخ البدء")]
        [DataType(DataType.DateTime)]
        public DateTime? StartTime { get; set; }

        [Display(Name = "تاريخ الانتهاء")]
        [DataType(DataType.DateTime)]
        public DateTime? EndTime { get; set; }

        [Display(Name = "الحالة")]
        public string? Status { get; set; }

        [Display(Name = "الرسالة")]
        public string? Message { get; set; }
    }
}
