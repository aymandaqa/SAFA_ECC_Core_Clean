using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.InwardViewModels
{
    public class EmailViewModel
    {
        [Display(Name = "معرف البريد الإلكتروني")]
        public int EmailId { get; set; }

        [Display(Name = "من")]
        public string? From { get; set; }

        [Display(Name = "الموضوع")]
        public string? Subject { get; set; }

        [Display(Name = "تاريخ الإرسال")]
        [DataType(DataType.DateTime)]
        public DateTime SentDate { get; set; }

        [Display(Name = "هل تمت القراءة؟")]
        public bool IsRead { get; set; }

        public List<EmailItemViewModel>? Emails { get; set; }
    }

    public class EmailItemViewModel
    {
        public int Id { get; set; }
        public string? Sender { get; set; }
        public string? Subject { get; set; }
        public DateTime Date { get; set; }
        public bool IsRead { get; set; }
    }
}
