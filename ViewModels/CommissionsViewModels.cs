namespace SAFA_ECC_Core_Clean.ViewModels.CommissionsViewModels
{
    public class EditCommissionRecordViewModel { public int Id { get; set; } public DateTime CommissionDate { get; set; } public decimal Amount { get; set; } public int RecordId { get; set; } public decimal NewAmount { get; set; } public string? Reason { get; set; } public string? AgentName { get; set; } public string? Description { get; set; } }
    public class ExcemptionRecordViewModel { public int ExcemptionId { get; set; } public string? Description { get; set; } public decimal Amount { get; set; } public string? EmployeeName { get; set; } public string? Reason { get; set; } public DateTime StartDate { get; set; } public DateTime EndDate { get; set; } }
    public class ExcemptionsDetailsViewModel { public int Id { get; set; } public int ExcemptionId { get; set; } public string? ExcemptionName { get; set; } public DateTime StartDate { get; set; } public DateTime EndDate { get; set; } public string? Status { get; set; } public List<ExcemptedProductItemViewModel>? ExcemptedProducts { get; set; } }
    public class ExcemptedProductItemViewModel { public string? ProductName { get; set; } public string? ProductCode { get; set; } public decimal ExcemptedAmount { get; set; } }
    public class CommissionsIndexViewModel { public decimal TotalCommissions { get; set; } public decimal ActiveCommissions { get; set; } public decimal TotalAmount { get; set; } public List<CommissionRecordViewModel> Commissions { get; set; } = new List<CommissionRecordViewModel>(); }
    public class CommissionsReportViewModel { public DateTime StartDate { get; set; } public DateTime EndDate { get; set; } public decimal TotalCommissions { get; set; } public string? AgentName { get; set; } public string? CommissionType { get; set; } public List<CommissionReportItemViewModel> Commissions { get; set; } = new List<CommissionReportItemViewModel>(); }
    public class AuthorizeCommissionViewModel { public int CommissionId { get; set; } public string? CustomerName { get; set; } public decimal Amount { get; set; } public DateTime CreatedDate { get; set; } public string? Status { get; set; } public string? AuthorizationNotes { get; set; } public bool IsApproved { get; set; } }
    public class CommissionRecordViewModel { public int Id { get; set; } public string? CustomerName { get; set; } public decimal Amount { get; set; } public DateTime Date { get; set; } public string? CommisionType { get; set; } public int CommTypeId { get; set; } public int CurrencyId { get; set; } public string? ChequeSource { get; set; } public string? Description { get; set; } public DateTime? LastUpdate { get; set; } }

    public class CommissionDetailsViewModel
    {
        public int CommissionId { get; set; }
        public DateTime CommissionDate { get; set; }
        public decimal Amount { get; set; }
        public string? Status { get; set; }
        public List<CommissionItemViewModel> Items { get; set; } = new List<CommissionItemViewModel>();
    }

    public class CommissionItemViewModel
    {
        public string? ItemName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}




    public class CommissionReportItemViewModel
    {
        public int Id { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? AgentName { get; set; }
        public string? CommissionType { get; set; }
        public decimal Amount { get; set; }
        public string? Status { get; set; }
    }

