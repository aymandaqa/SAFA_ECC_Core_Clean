using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SAFA_ECC_Core_Clean.ViewModels.PDCViewModels
{
    public class PDCDetailsViewModel
    {
        public string? ChequeNumber { get; set; }
        public string? BankName { get; set; }
        public string? AccountNumber { get; set; }
        public decimal Amount { get; set; }
        public DateTime ChequeDate { get; set; }
        public string? BeneficiaryName { get; set; }
        public string? Status { get; set; }
        public DateTime DueDate { get; set; }
        public string? DrawerName { get; set; }
        public string? DrawerAccount { get; set; }
        public string? Description { get; set; }
    }

    public class PDCListViewModel
    {
        public List<PDCDetailsViewModel>? PDCs { get; set; }
        public string? SearchQuery { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
    }

    public class AddPDCViewModel
    {
        [Required] public string? ChequeNumber { get; set; }
        [Required] public string? BankName { get; set; }
        [Required] public string? AccountNumber { get; set; }
        [Required] public decimal Amount { get; set; }
        [Required] public DateTime ChequeDate { get; set; }
        [Required] public string? BeneficiaryName { get; set; }
        [Required] public DateTime DueDate { get; set; }
        public string? DrawerName { get; set; }
        public string? DrawerAccount { get; set; }
        public string? Description { get; set; }
    }

    public class EditPDCViewModel
    {
        public int Id { get; set; }
        [Required] public string? ChequeNumber { get; set; }
        [Required] public string? BankName { get; set; }
        [Required] public string? AccountNumber { get; set; }
        [Required] public decimal Amount { get; set; }
        [Required] public DateTime ChequeDate { get; set; }
        [Required] public string? BeneficiaryName { get; set; }
        [Required] public DateTime DueDate { get; set; }
        public string? DrawerName { get; set; }
        public string? DrawerAccount { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
    }

    public class DeletePDCViewModel
    {
        public int Id { get; set; }
        public string? ChequeNumber { get; set; }
    }

    public class PDCReportViewModel
    {
        public string? ReportType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Status { get; set; }
        public List<PDCReportItemViewModel>? ReportItems { get; set; }
    }

    public class PDCReportItemViewModel
    {
        public DateTime ChequeDate { get; set; }
        public string? ChequeNumber { get; set; }
        public decimal Amount { get; set; }
        public string? BeneficiaryName { get; set; }
        public string? Status { get; set; }
        public DateTime DueDate { get; set; }
    }
}


