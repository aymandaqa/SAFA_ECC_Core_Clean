using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.PDCViewModels
{
    public class PDCIndexViewModel
    {
        [Display(Name = "إجمالي الشيكات")]
        public int TotalCheques { get; set; }

        [Display(Name = "الشيكات المعلقة")]
        public int PendingCheques { get; set; }

        [Display(Name = "الشيكات المعتمدة")]
        public int ApprovedCheques { get; set; }

        [Display(Name = "الشيكات المرفوضة")]
        public int RejectedCheques { get; set; }

        public List<PDCShortChequeViewModel>? RecentCheques { get; set; }

        public PDCSearchViewModel SearchModel { get; set; } = new PDCSearchViewModel();
        public List<PDCDetailsViewModel> Transactions { get; set; } = new List<PDCDetailsViewModel>();
    }

    public class PDCShortChequeViewModel
    {
        public string? ChequeNumber { get; set; }
        public decimal Amount { get; set; }
        public DateTime DueDate { get; set; }
        public string? Status { get; set; }
    }

    public class PDCSearchViewModel
    {
        public string? ChqSequance { get; set; }
        public string? DrwChqNo { get; set; }
        public string? DrwAcctNo { get; set; }
        public string? BenName { get; set; }
        [DataType(DataType.Date)]
        public DateTime? FromDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? ToDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? FromDueDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? ToDueDate { get; set; }
        public string? Status { get; set; }
        public bool DueToday { get; set; }
        public bool Overdue { get; set; }
    }

    public class PDCDetailsViewModel
    {
        public string? Serial { get; set; }
        public string? DrwChqNo { get; set; }
        public string? DrwAcctNo { get; set; }
        public decimal Amount { get; set; }
        public string? Currency { get; set; }
        public string? BenName { get; set; }
        public DateTime? TransDate { get; set; }
        public DateTime? DueDate { get; set; }
        public int? CollDays { get; set; }
        public string? Status { get; set; }
    }
}
