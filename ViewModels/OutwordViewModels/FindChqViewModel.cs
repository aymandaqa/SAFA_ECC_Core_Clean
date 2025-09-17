using System.Collections.Generic;
using SAFA_ECC_Core_Clean.Models;

namespace SAFA_ECC_Core_Clean.ViewModels.OutwordViewModels
{
    public class FindChqViewModel
    {
        public string ErrorMsg { get; set; }
        public List<Outward_Trans> OutwardTransList { get; set; }

        public FindChqViewModel()
        {
            OutwardTransList = new List<Outward_Trans>();
        }
    }
}

