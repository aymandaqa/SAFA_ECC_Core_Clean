using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SAFA_ECC_Core_Clean.ViewModels
{
    public enum ReportTypeViewModel
    {
        Daily,
        Weekly,
        Monthly
    }

    public class MonthlyReportItemViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime Date { get; set; }
        public int NumberOfTransactions { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class MonthlyReportsViewModel
    {
        public List<MonthlyReportItemViewModel> Reports { get; set; } = new List<MonthlyReportItemViewModel>();
        public int Month { get; set; }
        public int Year { get; set; }
        public List<SelectListItem> AvailableMonths { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> AvailableYears { get; set; } = new List<SelectListItem>();
    }

    public class ChequeReportsViewModel
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? ChequeNumber { get; set; }
        public string? BankName { get; set; }
        public string? Status { get; set; }
        public List<ChequeReportItemViewModel> Cheques { get; set; } = new List<ChequeReportItemViewModel>();
    }

    public class FinancialReportViewModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string SelectedReportType { get; set; }
        public string SelectedCurrency { get; set; }
        public List<SelectListItem> AvailableReportTypes { get; set; }
        public List<SelectListItem> AvailableCurrencies { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal NetProfit { get; set; }
        public List<ReportDataViewModel> ReportData { get; set; }
    }

    public class ReportDataViewModel
    {
        public string Category { get; set; }
        public decimal Amount { get; set; }
        public string? ItemName { get; set; }
        public decimal Percentage { get; set; }
    }

    public class CustomerReportsViewModel
    {
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<CustomerReportItemViewModel> Reports { get; set; } = new List<CustomerReportItemViewModel>();
        public bool SearchAttempted { get; set; }
    }

    public class CustomerReportItemViewModel
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string OrderStatus { get; set; }
    }

    public class DashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int TotalTransactions { get; set; }
        public decimal TotalTransactionAmount { get; set; }
        public int PendingTransactions { get; set; }
        public int RejectedTransactions { get; set; }
        public int PDCOverdue { get; set; }
        public int InwardToday { get; set; }
        public decimal InwardAmountToday { get; set; }
        public int OutwardToday { get; set; }
        public decimal OutwardAmountToday { get; set; }
        public int PDCDueToday { get; set; }
        public decimal PDCAmountDueToday { get; set; }
        public int OnlineUsers { get; set; }
        public int InwardThisMonth { get; set; }
        public int InwardPending { get; set; }
        public int InwardAuthorized { get; set; }
        public int InwardReturned { get; set; }
        public decimal InwardAmountThisMonth { get; set; }
        public int OutwardThisMonth { get; set; }
        public int OutwardPending { get; set; }
        public int OutwardAuthorized { get; set; }
        public int OutwardPosted { get; set; }
        public int OutwardReturned { get; set; }
        public decimal OutwardAmountThisMonth { get; set; }
        public int PDCTotal { get; set; }

    }

    public class DailyReportViewModel
    {
        public DateTime ReportDate { get; set; }
        public int TotalTransactions { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class ChequeReportItemViewModel
    {
        public int Id { get; set; }
        public string? ChequeNumber { get; set; }
        public string? BankName { get; set; }
        public decimal Amount { get; set; }
        public DateTime DueDate { get; set; }
        public string? Status { get; set; }
    }

    public class TransactionReportsViewModel
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? TransactionType { get; set; }
        public string? AccountNumber { get; set; }
        public decimal? MinAmount { get; set; }
        public decimal? MaxAmount { get; set; }
        public string? Status { get; set; }
        public List<TransactionReportItemViewModel>? Transactions { get; set; }
    }

    public class TransactionReportItemViewModel
    {
        public int Id { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? TransactionType { get; set; }
        public decimal Amount { get; set; }
        public string? AccountNumber { get; set; }
        public string? Status { get; set; }
        public string? Description { get; set; }
    }

    public class UserReportsViewModel
    {
        public string? UserName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<UserReportItemViewModel>? UserActivities { get; set; }
    }

    public class UserReportItemViewModel
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? Activity { get; set; }
        public DateTime Timestamp { get; set; }
        public string? Description { get; set; }
    }





}


