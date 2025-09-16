using System;
using System.Collections.Generic;

namespace SAFA_ECC_Core_Clean.ViewModels.ChequeViewerViewModels
{
    public class ChequeViewerSearchModel
    {
        public string? ChequeNumber { get; set; }
        public string? DrawerName { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? BankId { get; set; }
        public decimal? AmountFrom { get; set; }
        public decimal? AmountTo { get; set; }
        public string? Status { get; set; }
    }

    public class ChequeViewerIndexViewModel
    {
        public ChequeViewerSearchModel SearchModel { get; set; } = new ChequeViewerSearchModel();
        public List<SAFA_ECC_Core_Clean.Models.Bank_Tbl> Banks { get; set; } = new List<SAFA_ECC_Core_Clean.Models.Bank_Tbl>();
        public List<ChequeDetails> Cheques { get; set; } = new List<ChequeDetails>();
    }

    public class ChequesSearchViewModel 
    {
        public List<ChequeDetails>? Cheques { get; set; }
    }
    public class ChequeSearchViewModel 
    {
        public string? ChequeNumber { get; set; }
        public string? BankName { get; set; }
        public string? BeneficiaryName { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public List<ChequeDetails>? Cheques { get; set; }
    }
    
    public class PrintChequeDetailsViewModel 
    {
        public string? ChequeNumber { get; set; }
        public DateTime ChequeDate { get; set; }
        public decimal Amount { get; set; }
        public string? PayeeName { get; set; }
        public string? BankName { get; set; }
        public string? AccountNumber { get; set; }
        public string? Status { get; set; }
        public List<ChequeDetails>? Items { get; set; }
    }
    public class ChequeDetailsViewModel
    {
        public string? ChequeNumber { get; set; }
        public DateTime? ChequeDate { get; set; }
        public decimal? Amount { get; set; }
        public string? PayeeName { get; set; }
        public string? BankName { get; set; }
        public string? AccountNumber { get; set; }
        public string? Status { get; set; }
        public string? Notes { get; set; }
        public List<ChequeDetails>? Cheques { get; set; }
    }

    public class ChequeDetails
    {
        public string? ChequeNumber { get; set; }
        public decimal Amount { get; set; }
        public int Id { get; set; }
        public int ChequeId { get; set; }
        public DateTime? ChequeDate { get; set; }
        public DateTime Date { get; set; }
        public string? BeneficiaryName { get; set; }
        public string? Status { get; set; }
        public string? BankName { get; set; }
        public string? BranchName { get; set; }
        public string? AccountNumber { get; set; }
        public DateTime DueDate { get; set; }
        public string? PayeeName { get; set; }
        public string? Description { get; set; }
        public string? Cheque_No { get; set; }
        public string? Drawer_Name { get; set; }
        public DateTime? Trans_Date { get; set; }
        public SAFA_ECC_Core_Clean.Models.Banks_Tbl? Bank { get; set; }
        public List<SAFA_ECC_Core_Clean.Models.Cheque_Images_Link_Tbl>? ChequeImages { get; set; }
        public int Trans_ID { get; set; }
    }
    public class PrintAllOutwardCHQViewModel 
    {
        public SearchCriteria SearchCriteria { get; set; } = new SearchCriteria();
        public List<ChequeDetails> Cheques { get; set; } = new List<ChequeDetails>();
    }

    public class SearchCriteria
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? ChequeNumber { get; set; }
        public string? PayeeName { get; set; }
    }
}
