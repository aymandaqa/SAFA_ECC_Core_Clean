using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SAFA_ECC_Core_Clean.ViewModels.FacilitiesViewModels
{
    public class FacilitiesViewModel
    {
        public List<FacilityItemViewModel>? Facilities { get; set; }
        public string? SearchQuery { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
    }

    public class FacilityItemViewModel
    {
        public int FacilityId { get; set; }
        public string? FacilityName { get; set; }
        public string? FacilityType { get; set; }
        public decimal LimitAmount { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string? Status { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerId { get; set; }
    }

    public class AddFacilityViewModel
    {
        [Required] public string? FacilityName { get; set; }
        [Required] public string? FacilityType { get; set; }
        [Required] public decimal LimitAmount { get; set; }
        [Required] public DateTime ExpiryDate { get; set; }
        public string? CustomerId { get; set; }
        public string? CustomerName { get; set; }
    }

    public class EditFacilityViewModel
    {
        public int FacilityId { get; set; }
        [Required] public string? FacilityName { get; set; }
        [Required] public string? FacilityType { get; set; }
        [Required] public decimal LimitAmount { get; set; }
        [Required] public DateTime ExpiryDate { get; set; }
        public string? Status { get; set; }
        public string? CustomerId { get; set; }
        public string? CustomerName { get; set; }
    }

    public class DeleteFacilityViewModel
    {
        public int FacilityId { get; set; }
        public string? FacilityName { get; set; }
    }

    public class FacilityDetailsViewModel
    {
        public int FacilityId { get; set; }
        public string? FacilityName { get; set; }
        public string? FacilityType { get; set; }
        public decimal LimitAmount { get; set; }
        public decimal UsedAmount { get; set; }
        public decimal AvailableAmount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string? Status { get; set; }
        public string? CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public List<FacilityTransactionViewModel>? Transactions { get; set; }
        public List<FacilityDocumentViewModel>? Documents { get; set; }
    }

    public class FacilityTransactionViewModel
    {
        public int TransactionId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? TransactionType { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
    }

    public class FacilityDocumentViewModel
    {
        public int DocumentId { get; set; }
        public string? DocumentName { get; set; }
        public string? DocumentType { get; set; }
        public string? FilePath { get; set; }
        public DateTime UploadDate { get; set; }
    }

    public class FacilityReportViewModel
    {
        public string? ReportType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? CustomerId { get; set; }
        public string? FacilityType { get; set; }
        public List<FacilityReportItemViewModel>? ReportItems { get; set; }
    }

    public class FacilityReportItemViewModel
    {
        public string? CustomerName { get; set; }
        public string? FacilityName { get; set; }
        public string? FacilityType { get; set; }
        public decimal LimitAmount { get; set; }
        public decimal UsedAmount { get; set; }
        public decimal AvailableAmount { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string? Status { get; set; }
    }
}


