using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.FacilitiesViewModels
{
    public class ConcentrateEmailsSettingViewModel
    {
        [Display(Name = "اسم التركيز")]
        public string? ConcentrateName { get; set; }

        [Display(Name = "قائمة البريد الإلكتروني")]
        public List<string>? EmailList { get; set; }

        [Display(Name = "تمكين الإشعارات")]
        public bool EnableNotifications { get; set; }

        [Display(Name = "بريد المرسل الإلكتروني")]
        public string? SenderEmail { get; set; }

        [Display(Name = "اسم المرسل")]
        public string? SenderName { get; set; }

        [Display(Name = "خادم SMTP")]
        public string? SmtpServer { get; set; }

        [Display(Name = "منفذ SMTP")]
        public int SmtpPort { get; set; }

        [Display(Name = "استخدام SSL")]
        public bool UseSsl { get; set; }

        [Display(Name = "اسم المستخدم")]
        public string? Username { get; set; }

        [Display(Name = "كلمة المرور")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        public List<EmailSettingHistoryItemViewModel>? History { get; set; }
    }

    public class EmailSettingHistoryItemViewModel
    {
        public DateTime ModificationDate { get; set; }
        public string? SenderEmail { get; set; }
        public string? SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string? Username { get; set; }
        public string? ModifiedBy { get; set; }
    }
}
