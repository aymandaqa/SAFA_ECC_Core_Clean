using System;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.InwardViewModels
{
    public class EmailDetailViewModel
    {
        [Display(Name = "معرف البريد الإلكتروني")]
        public int EmailId { get; set; }

        [Display(Name = "من")]
        public string? From { get; set; }

        [Display(Name = "إلى")]
        public string? To { get; set; }

        [Display(Name = "الموضوع")]
        public string? Subject { get; set; }

        [Display(Name = "تاريخ الإرسال")]
        [DataType(DataType.DateTime)]
        public DateTime SentDate { get; set; }

        [Display(Name = "المحتوى")]
        public string? Body { get; set; }

        [Display(Name = "هل تمت القراءة؟")]
        public bool IsRead { get; set; }
    }
}
