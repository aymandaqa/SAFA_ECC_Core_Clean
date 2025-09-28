using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.InwardViewModels
{
    public class InwardIndexViewModel
    {
        [Display(Name = "إجمالي الشيكات الواردة")]
        public int TotalInwardCheques { get; set; }

        [Display(Name = "الشيكات المعلقة")]
        public int PendingInwardCheques { get; set; }

        [Display(Name = "الشيكات المعتمدة")]
        public int ApprovedInwardCheques { get; set; }

        [Display(Name = "الشيكات المرفوضة")]
        public int RejectedInwardCheques { get; set; }

        public List<InwardShortChequeViewModel>? RecentInwardCheques { get; set; }

        public InwardSearchViewModel SearchModel { get; set; } = new InwardSearchViewModel();
        public List<InwardDetailsViewModel> Transactions { get; set; } = new List<InwardDetailsViewModel>();
    }

    public class InwardShortChequeViewModel
    {
        public string? ChequeNumber { get; set; }
        public decimal Amount { get; set; }
        public DateTime DueDate { get; set; }
        public string? Status { get; set; }
    }

    public class InwardSearchViewModel
    {
        public string? ChqSequance { get; set; }
        public string? DrwChqNo { get; set; }
        public string? DrwAcctNo { get; set; }
        public string? BenName { get; set; }
        [DataType(DataType.Date)]
        public DateTime? FromDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? ToDate { get; set; }
        public decimal? MinAmount { get; set; }
        public decimal? MaxAmount { get; set; }
        public string? Status { get; set; }
    }

    public class InwardDetailsViewModel
    {
        public string? Serial { get; set; }
        public string? DrwChqNo { get; set; }
        public string? DrwAcctNo { get; set; }
        public decimal? Amount { get; set; }
        public string? Currency { get; set; }
        public string? BenName { get; set; }
        public DateTime? TransDate { get; set; }
        public string? Status { get; set; }
    }
}
