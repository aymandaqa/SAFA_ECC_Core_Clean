namespace SAFA_ECC_Core_Clean.ViewModels
{
    public class OutwardViewModel 
    {
        public int Id { get; set; }
        public string? ChequeNumber { get; set; }
        public decimal Amount { get; set; }
        public string? BeneficiaryName { get; set; }
        public string? OutwardNumber { get; set; }
        public DateTime OutwardDate { get; set; }
        public string? Subject { get; set; }
        public string? Description { get; set; }
        public string? Sender { get; set; }
        public string? Recipient { get; set; }
        public List<AttachmentViewModel>? Attachments { get; set; }
        public DateTime RequestDate { get; set; }
        public string? EmployeeName { get; set; }
        public string? TimeOutType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Status { get; set; }
        public string? ChqNumber { get; set; }
        public DateTime ChqDate { get; set; }
        public string? BankName { get; set; }
        public string? Notes { get; set; }
        public DateTime ChequeDate { get; set; }
        public string? CustomerName { get; set; }
        public string? ReturnStatus { get; set; }
        public DateTime ReturnDate { get; set; }
        public string? RequestNumber { get; set; }
        public string? RejectionReason { get; set; }
    }
}

