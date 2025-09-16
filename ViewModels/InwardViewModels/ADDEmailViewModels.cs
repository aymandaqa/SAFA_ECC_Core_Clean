using System;
using System.Collections.Generic;

namespace SAFA_ECC_Core_Clean.ViewModels.InwardViewModels
{
    public class AddEmailViewModel { public string? EmailAddress { get; set; } public string? Subject { get; set; } public string? Body { get; set; } public string? CustomerName { get; set; } public string? EmailType { get; set; } public List<EmailItemViewModel>? Emails { get; set; } }

    public class EmailItemViewModel
    {
        public int Id { get; set; }
        public string? EmailAddress { get; set; }
        public string? CustomerName { get; set; }
        public string? EmailType { get; set; }
    }
}

