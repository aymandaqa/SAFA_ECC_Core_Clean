using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using SAFA_ECC_Core_Clean.Models;

namespace SAFA_ECC_Core_Clean.ViewModels.OutwordViewModels
{
    public class AllInwardOutwardChqViewModel
    {
        public List<CURRENCY_TBL> CurrencyList { get; set; }
        public List<TreeNode> Tree { get; set; } // Assuming TreeNode is a model for the tree structure
        public string ErrorMessage { get; set; }
    }
}

