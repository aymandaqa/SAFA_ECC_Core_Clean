using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.T24_JOBViewModel
{
    public class T24_JOBViewModel
    {
        public string? JobId { get; set; }
        public string? JobName { get; set; }
        public string? Status { get; set; }
        public DateTime? LastRun { get; set; }
        public DateTime? NextRun { get; set; }
        public string? Schedule { get; set; }
        public string? Description { get; set; }
    }

    public class T24_TellerChequeViewModel
    {
        public string? ChequeId { get; set; }
        public string? TellerId { get; set; }
        public string? AccountNumber { get; set; }
        public decimal Amount { get; set; }
        public string? Currency { get; set; }
        public DateTime ChequeDate { get; set; }
        public string? BeneficiaryName { get; set; }
        public string? Status { get; set; }
        public string? Description { get; set; }
    }
}


