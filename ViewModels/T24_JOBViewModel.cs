using System;
using System.Collections.Generic;

namespace SAFA_ECC_Core_Clean.ViewModels
{
    public class T24_JOBViewModel
    {
        // Properties inferred from T24_TELLER_CHEQUE.vb and build errors
        public string JobName { get; set; } // From build error
        public string Status { get; set; } // From build error
        public DateTime? LastRun { get; set; } // From build error
        public List<T24_JOBViewModel> Jobs { get; set; } = new List<T24_JOBViewModel>(); // From build error, assuming it's a collection of itself or similar structure

        // Properties from T24_TELLER_CHEQUE.vb (if applicable, for a more complete model)
        public DateTime? ReturnDate { get; set; }
        public string Currency { get; set; }
        public string FromBnk { get; set; }
        public string FromBrn { get; set; }
        public string ToBnk { get; set; }
        public string ToBrn { get; set; }
        public string ChqNo { get; set; }
        public string AcctNo { get; set; }
        public double? Amount { get; set; }
        public string ReturningSide { get; set; }
        public string TransType { get; set; }
        public string RetCode { get; set; }
        public string BenAccount { get; set; }
        public string NATIONALITY { get; set; }
        public string DOCUMENT_TYPE { get; set; }
        public string CardId { get; set; }
        public string DrwName { get; set; }
        public string ISSAccount { get; set; }
        public string Precs { get; set; }
        public string ALT_ACC { get; set; }
    }
}


