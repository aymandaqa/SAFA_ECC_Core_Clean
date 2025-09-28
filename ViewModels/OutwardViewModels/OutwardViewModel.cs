using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.OutwardViewModels
{
    public class OutwardViewModel
    {
        [Display(Name = "معرف الصادر")]
        public int Id { get; set; }

        [Display(Name = "رقم الصادر")]
        public string? OutwardNumber { get; set; }

        [Display(Name = "تاريخ الصادر")]
        [DataType(DataType.Date)]
        public DateTime? OutwardDate { get; set; }

        [Display(Name = "المبلغ")]
        public decimal Amount { get; set; }

        [Display(Name = "الحالة")]
        public string? Status { get; set; }

        [Display(Name = "الموضوع")]
        public string? Subject { get; set; }

        [Display(Name = "المرسل")]
        public string? Sender { get; set; }

        [Display(Name = "المستلم")]
        public string? Recipient { get; set; }

        public List<AttachmentViewModel>? Attachments { get; set; }
    }

    public class AttachmentViewModel
    {
        public string? FileName { get; set; }
        public string? FileType { get; set; }
        public string? FilePath { get; set; }
    }
}
