using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using SAFA_ECC_Core_Clean.Models;

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
}


