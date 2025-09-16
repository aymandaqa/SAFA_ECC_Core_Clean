
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SAFA_ECC_Core_Clean.ViewModels
{


    public class AllOutChqViewModel 
    {
        public List<OutwardViewModel>? OutwardChecks { get; set; }
    }
    public class CheckImageViewModel { public string? ImageUrl { get; set; } }
    public class AmanitestViewModel { }
    public class BankBranchViewModel { public int Id { get; set; } public string? BranchName { get; set; } public int BankId { get; set; } public List<BankBranchItemViewModel>? BankBranches { get; set; } }


    public class ConcentrateDetailsViewModel { public string? ConcentrateName { get; set; } public string? ConcentrateCode { get; set; } public DateTime? ProductionDate { get; set; } public DateTime? ExpiryDate { get; set; } public string? Description { get; set; } public List<DiscountItemViewModel>? Components { get; set; } public List<ChangeLogItemViewModel>? ChangeLog { get; set; } }
    public class ConcentrateEmailsSettingViewModel 
    {
        public string? SenderEmail { get; set; }
        public string? SenderName { get; set; }
        public string? SenderPassword { get; set; }
        public string? SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public bool EnableSsl { get; set; }
        public string? RecipientEmails { get; set; }
        public string? CcEmails { get; set; }
        public string? BccEmails { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public bool UseSsl { get; set; }
        public List<ChangeLogItemViewModel>? History { get; set; }
    }

    public class E_ChannelsChqViewModel { }
    public class EmailDetailViewModel { public string? Subject { get; set; } public string? Sender { get; set; } public string? Recipient { get; set; } }
    public class EmailViewModel { public string? ToEmail { get; set; } public string? Subject { get; set; } public string? Body { get; set; } public bool HighPriority { get; set; } }
    public class GET_RETURN_CHQ_BY_CLRCENTER_ViewModel 
    {
        public int? ClearingCenterId { get; set; }
        public List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>? ClearingCenters { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? StatusId { get; set; }
        public List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>? Statuses { get; set; }
    }


    public class ImportExportViewModel { }

    public class MagicScreenViewModel 
    {
        public string? SomeProperty { get; set; }
        public MagicScreenSearchViewModel? SearchCriteria { get; set; }
        public List<MagicScreenResultViewModel>? Results { get; set; }
        public List<SelectListItem>? Banks { get; set; }
        public List<ChequeViewModel>? Cheques { get; set; }
        public int TotalCheques { get; set; }
        public List<SelectListItem>? ReturnCodes { get; set; }
    }

    public class MagicScreenSearchViewModel
    {
        public string? Field1 { get; set; }
        public string? Field2 { get; set; }
    }

    public class MagicScreenResultViewModel
    {
        public int Id { get; set; }
        public string? Column1 { get; set; }
        public string? Column2 { get; set; }
    }

    public class ChequeViewModel
    {
        public int Id { get; set; }
        public string? ChequeNumber { get; set; }
        public decimal Amount { get; set; }
        public string? Status { get; set; }
    }

    public class PrintChqQrViewModel 
    {
        public string? ChequeNumber { get; set; }
        public string? AccountNumber { get; set; }
        public decimal Amount { get; set; }
        public string? BeneficiaryName { get; set; }
        public DateTime IssueDate { get; set; }
        public string? BankName { get; set; }
        public string? QRData { get; set; }
    }
    public class RepresentDisDetailsViewModel
    {
        public string? DiscountNumber { get; set; }
        public DateTime? DiscountDate { get; set; }
        public string? CustomerName { get; set; }
        public decimal? DiscountAmount { get; set; }
        public string? Currency { get; set; }
        public List<DiscountItemViewModel>? DiscountItems { get; set; } = new List<DiscountItemViewModel>();
    }

    public class DiscountItemViewModel
    {
        public int Id { get; set; }
        public string? ItemName { get; set; }
        public decimal Amount { get; set; }
        public string? ComponentName { get; set; }
        public int Quantity { get; set; }
        public string? Unit { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
    }
    public class ResendINWViewModel 
    {
        public int FileId { get; set; }
        public string? Reason { get; set; }
        public int ResendAttempts { get; set; }
        public string? Priority { get; set; }
        public List<ResendHistoryItemViewModel>? ResendHistory { get; set; }
    }

    public class ResendHistoryItemViewModel
    {
        public DateTime Date { get; set; }
        public string? Status { get; set; }
        public string? Notes { get; set; }
    }
    public class ReturnOnUsStoppedChequeDetailsViewModel { public string? ChequeNumber { get; set; } public string? PayeeName { get; set; } public string? DrawerName { get; set; } public string? DrawerAccountNumber { get; set; } public string? StopReason { get; set; } public DateTime StopDate { get; set; } }
    public class ReturnStoppedChequesViewModel { }
    public class SecuritySettingsViewModel { }

    public class CommissionReportViewModel { }




}




    public class BankBranchItemViewModel
    {
        public int Id { get; set; }
        public string? BranchName { get; set; }
        public int BankId { get; set; }
        public string? BankName { get; set; }
    }




    public class ChangeLogItemViewModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string? User { get; set; }
        public string? Operation { get; set; }
        public string? Details { get; set; }
    }



    public class CustomerConcentrateViewModel
    {
        public List<CustomerConcentrateItemViewModel>? CustomerConcentrates { get; set; }
    }

    public class CustomerConcentrateItemViewModel
    {
        public int Id { get; set; }
        public string? CustomerName { get; set; }
        public string? Region { get; set; }
        public decimal ConcentrationPercentage { get; set; }
    }





    public class AttachmentViewModel
    {
        public string? FileName { get; set; }
        public string? FileType { get; set; }
        public string? FilePath { get; set; }
    }


