using System.Collections.Generic;
using SAFA_ECC_Core_Clean.Models;

namespace SAFA_ECC_Core_Clean.ViewModels.OutwordViewModels
{
    public class DeleteOutChqViewModel
    {
        public List<Outward_Trans> OutwardTransToDelete { get; set; }
        public bool HasPermission { get; set; }
    }
}

