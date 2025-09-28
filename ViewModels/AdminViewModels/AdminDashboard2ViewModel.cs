using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.AdminViewModels
{
    public class AdminDashboard2ViewModel
    {
        [Display(Name = "إجمالي المستخدمين")]
        public int TotalUsers { get; set; }

        [Display(Name = "المستخدمون النشطون")]
        public int ActiveUsers { get; set; }

        [Display(Name = "المستخدمون غير النشطين")]
        public int InactiveUsers { get; set; }

        [Display(Name = "إجمالي الأدوار")]
        public int TotalRoles { get; set; }

        [Display(Name = "أحدث الأنشطة")]
        public List<ActivityLogViewModel>? LatestActivities { get; set; }

        public SystemStatsViewModel? SystemStats { get; set; }
        public List<RecentActivityViewModel>? RecentActivities { get; set; }
        public UserLoginStatsViewModel? UserLoginStats { get; set; }
    }

    public class SystemStatsViewModel
    {
        public int TotalTransactions { get; set; }
        public int ProcessedToday { get; set; }
        public int PendingProcessing { get; set; }
        public int ErrorCount { get; set; }
    }

    public class RecentActivityViewModel
    {
        public string? ActivityType { get; set; }
        public string? Description { get; set; }
        public string? UserName { get; set; }
        public DateTime? ActivityDate { get; set; }
    }

    public class UserLoginStatsViewModel
    {
        public int TodayLogins { get; set; }
        public int ActiveSessions { get; set; }
        public int FailedAttempts { get; set; }
    }

    public class ActivityLogViewModel
    {
        public string? ActivityDescription { get; set; }
        public string? Timestamp { get; set; }
        public string? UserName { get; set; }
    }
}
