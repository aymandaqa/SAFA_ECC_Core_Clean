using System;

namespace SAFA_ECC_Core_Clean.ViewModels.InwardViewModels
{
    public class ResendHistoryItemViewModel
    {
        public int Id { get; set; }
        public string? BatchId { get; set; }
        public DateTime ResendDate { get; set; }
        public string? Status { get; set; }
        public string? ResentBy { get; set; }
    }
}

