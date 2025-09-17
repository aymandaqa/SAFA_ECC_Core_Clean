
using System;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.OutwordViewModels
{
    public class Hold_CHQViewModel
    {
        [Required]
        public string DrwAcctNo { get; set; }
        [Required]
        public string DrwBankNo { get; set; }
        [Required]
        public string DrwBranchNo { get; set; }
        [Required]
        public string DrwChqNo { get; set; }
        public string Note1 { get; set; }
        public string Reson1 { get; set; }
        public string Reson2 { get; set; }
        public string Type { get; set; }
        public int Reserved { get; set; }
        public string InputBy { get; set; }
        public DateTime InputDate { get; set; }
        public string History { get; set; }
    }
}

