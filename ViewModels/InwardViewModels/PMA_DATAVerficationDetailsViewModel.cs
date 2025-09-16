using System;
using System.Collections.Generic;
using SAFA_ECC_Core_Clean.Models;
using SAFA_ECC_Core_Clean.ViewModels.SharedViewModels;

namespace SAFA_ECC_Core_Clean.ViewModels.InwardViewModels
{
    public class PMA_DATAVerficationDetailsViewModel
    {
        public OnUs_Tbl? Onus { get; set; }
        public Get_Outward_Slip_CCS_VIEW? Onus163 { get; set; }
        public OnUs_Imgs? Imgs { get; set; }
        public List<Bank_Tbl>? Banks { get; set; }
        public List<Bank_Branches_Tbl>? Branchs { get; set; }
        public string? Bank_Name { get; set; }
        public string? RetCode_Descreption { get; set; }
        public string? FronImage { get; set; }
        public string? RearImage { get; set; }
        public string? Branch_Name { get; set; }
        public string? Recom { get; set; }
        public string? Approve { get; set; }
        public string? BookedBalance { get; set; }
        public string? ClearBalance { get; set; }
        public string? AccountStatus { get; set; }
        public string? Reject { get; set; }
        public string? GUAR_CUSTOMER { get; set; }
        public string? Details { get; set; }
        public List<SharedViewModels.TableDataItem>? Items { get; set; }
        public string? DetailField1 { get; set; }
        public string? DetailField2 { get; set; }
        public string? DetailField3 { get; set; }
        public int ItemId { get; set; }
        public string? ItemName { get; set; }
        public string? VerificationStatus { get; set; }
        public string? VerifierNotes { get; set; }
        public List<SharedViewModels.ChangeLogItemViewModel>? ChangeLog { get; set; }
    }
}

