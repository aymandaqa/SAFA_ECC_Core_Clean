using System;

namespace SAFA_ECC_Core_Clean.ViewModels.SharedViewModels
{
    public class ChangeLogItemViewModel
    {
        public DateTime ChangeDate { get; set; }
        public string? ChangedBy { get; set; }
        public string? Description { get; set; }
    }
}
