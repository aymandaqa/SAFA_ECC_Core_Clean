using System;
using System.Collections.Generic;

namespace SAFA_ECC_Core_Clean.ViewModels.ReportViewModels
{
    public class DashboardViewModel
    {
        public int TotalInwardChequesToday { get; set; }
        public int TotalOutwardChequesToday { get; set; }
        public decimal TotalInwardAmountToday { get; set; }
        public decimal TotalOutwardAmountToday { get; set; }
        public int TotalPendingInward { get; set; }
        public int TotalPendingOutward { get; set; }
        public List<RecentActivityViewModel>? RecentActivities { get; set; }
    }

    public class RecentActivityViewModel
    {
        public DateTime ActivityDate { get; set; }
        public string? Description { get; set; }
        public string? UserName { get; set; }
    }
}
