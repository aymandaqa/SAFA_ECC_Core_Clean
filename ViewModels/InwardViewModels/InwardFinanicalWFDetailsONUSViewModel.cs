using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SAFA_ECC_Core_Clean.Models;

namespace SAFA_ECC_Core_Clean.ViewModels.InwardViewModels
{
    public class InwardFinanicalWFDetailsONUSViewModel
    {
        public OnUs_Tbl InwardCheque { get; set; }
        public object Tree { get; set; }
        public string BookedBalance { get; set; }
        public string ClearBalance { get; set; }
        public string AccountStatus { get; set; }
        public string GuarranteedCustomerAccounts { get; set; } // Renamed from GUAR_CUSTOMER for clarity
        public bool CanReject { get; set; }
        public bool CanApprove { get; set; }
        public bool ShowRecommendationButton { get; set; }
        public string Title { get; set; }
        public List<CURRENCY_TBL> Currencies { get; set; }
        public bool IsVIP { get; set; }

        // Properties expected by the view
        [Display(Name = "رقم المعاملة")]
        public string? TransactionNumber { get; set; }

        [Display(Name = "تاريخ الإنشاء")]
        [DataType(DataType.Date)]
        public DateTime CreationDate { get; set; }

        [Display(Name = "الحالة")]
        public string? Status { get; set; }

        [Display(Name = "المبلغ")]
        public decimal Amount { get; set; }

        [Display(Name = "العملة")]
        public string? Currency { get; set; }

        [Display(Name = "البنك")]
        public string? BankName { get; set; }

        public List<WorkflowHistoryItemViewModel>? WorkflowHistory { get; set; }
    }

    public class WorkflowHistoryItemViewModel
    {
        public DateTime Date { get; set; }
        public string? Action { get; set; }
        public string? Notes { get; set; }
        public string? User { get; set; }
    }
}
