using System;
using System.Collections.Generic;

namespace SAFA_ECC_Core_Clean.ViewModels.SharedViewModels
{
    public class BreadcrumbViewModel 
    {
        public List<BreadcrumbItem> Items { get; set; } = new List<BreadcrumbItem>();
    }

    public class BreadcrumbItem
    {
        public string? Text { get; set; }
        public string? Url { get; set; }
        public bool IsActive { get; set; }
    }
    public class ConfirmationViewModel 
    {
        public string? Title { get; set; }
        public string? Message { get; set; }
        public string? CancelButtonText { get; set; }
        public string? ConfirmButtonText { get; set; }
    }
    public class NotificationViewModel 
    {
        public string? Title { get; set; }
        public string? Message { get; set; }
        public string? Type { get; set; } // e.g., "success", "info", "warning", "danger"
        public DateTime? Timestamp { get; set; }
    }
    public class LoadingViewModel
    {
        public string? Message { get; set; }
        public bool ShowProgressBar { get; set; }
        public int Progress { get; set; }
    }
    public class AttachmentViewModel
    {
        public int Id { get; set; }
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
    }

    public class TableDataItem
    {
        public string? Key { get; set; }
        public string? Value { get; set; }
    }
}