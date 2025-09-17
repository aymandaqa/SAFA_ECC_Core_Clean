using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using SAFA_ECC_Core_Clean.Models;

namespace SAFA_ECC_Core_Clean.ViewModels.OtherViewModels
{
    public class BankBranchItemViewModel
    {
        public int BankId { get; set; }
        public string? BankName { get; set; }
        public int BranchId { get; set; }
        public string? BranchName { get; set; }
    }

    public class ChangeLogItemViewModel
    {
        public DateTime ChangeDate { get; set; }
        public string? ChangedBy { get; set; }
        public string? Description { get; set; }
    }

    public class CustomerConcentrateViewModel
    {
        public string? CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public List<CustomerConcentrateItemViewModel>? Items { get; set; }
    }

    public class CustomerConcentrateItemViewModel
    {
        public string? ChequeNumber { get; set; }
        public decimal Amount { get; set; }
        public DateTime ChequeDate { get; set; }
        public string? Status { get; set; }
    }

    public class EmailViewModel
    {
        public string? To { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
        public List<string>? Attachments { get; set; }
    }

    public class ReturnOnUsStoppedChequeDetailsViewModel
    {
        public string? ChequeNumber { get; set; }
        public string? BankName { get; set; }
        public string? AccountNumber { get; set; }
        public string? Status { get; set; }
        public string? StopReason { get; set; }
        public DateTime StopDate { get; set; }
        public string? ReturnReason { get; set; }
        public DateTime ReturnDate { get; set; }
        public List<OnUs_Imgs>? OnUsImages { get; set; }
        public List<OnUs_Tbl>? OnUsData { get; set; }
    }


}


