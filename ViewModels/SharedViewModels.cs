using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SAFA_ECC_Core_Clean.ViewModels.SharedViewModels
{
    public class ConfirmationModalViewModel
    {
        public string? Id { get; set; }
        public string? Title { get; set; }
        public string? Message { get; set; }
        public string? ConfirmButtonText { get; set; }
        public string? CancelButtonText { get; set; }
        public string? ConfirmAction { get; set; }
        public string? ConfirmController { get; set; }
        public object? RouteValues { get; set; }
    }

    public class BreadcrumbItemViewModel
    {
        public string? Text { get; set; }
        public string? Action { get; set; }
        public string? Controller { get; set; }
        public bool IsActive { get; set; }
    }

    public class NotificationViewModel
    {
        public string? Type { get; set; } // e.g., "success", "error", "warning", "info"
        public string? Message { get; set; }
        public bool IsDismissible { get; set; }
    }

    public class TableDataItem
    {
        public string? Key { get; set; }
        public string? Value { get; set; }
    }

    public class TableViewModel
    {
        public List<TableDataItem>? Headers { get; set; }
        public List<List<TableDataItem>>? Rows { get; set; }
    }
}


