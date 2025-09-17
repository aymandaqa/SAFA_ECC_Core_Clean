
using System;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.OutwordViewModels
{
    public class UpdateChqDateViewModel
    {
        public string Serial { get; set; }
        public string BenName { get; set; }
        public string BenfAccBranch { get; set; }
        public string AcctType { get; set; }
        public string DrwChqNo { get; set; }
        public string DrwBankNo { get; set; }
        public string DrwBranchNo { get; set; }
        public string DrwAcctNo { get; set; }
        public double Amount { get; set; }
        public DateTime DueDate { get; set; }
        public string Currency { get; set; }
        public string BenfBnk { get; set; }
        public string BenfCardType { get; set; }
        public string BenfCardId { get; set; }
        public string BenAccountNo { get; set; }
        public string BenfNationality { get; set; }
        public string NeedTechnicalVerification { get; set; }
        public string WithUV { get; set; }
        public string SpecialHandling { get; set; }
        public string IsVIP { get; set; }
        public string DrwName { get; set; }
    }
}

