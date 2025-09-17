using System;
using System.Collections.Generic;
using SAFA_ECC_Core_Clean.Models;

namespace SAFA_ECC_Core_Clean.ViewModels.OutwordViewModels
{
    public class UpdateChqDateRepresnetViewModel
    {
        public string ErrorMessage { get; set; }
        public Outward_Trans OutwardTrans { get; set; }
        public List<TreeNode> Tree { get; set; }
        public bool HasPermission { get; set; }
    }
}

