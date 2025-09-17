using System.Collections.Generic;
using SAFA_ECC_Core_Clean.Models;

namespace SAFA_ECC_Core_Clean.ViewModels.OutwordViewModels
{
    public class PospondingChqViewModel
    {
        public List<TreeNode> Tree { get; set; }
        public List<ClearingCenter> ClearingCenters { get; set; }
        public string ErrorMessage { get; set; }

        public PospondingChqViewModel()
        {
            Tree = new List<TreeNode>();
            ClearingCenters = new List<ClearingCenter>();
        }
    }
}

