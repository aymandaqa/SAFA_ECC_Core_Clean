using System.Collections.Generic;
using SAFA_ECC_Core_Clean.Models;

namespace SAFA_ECC_Core_Clean.ViewModels.InwardViewModels
{
    public class MagicScreenListViewModel
    {
        public List<Inward_Trans> Cheques { get; set; }
        public double TotalAmount { get; set; }
        public double TotalJOD { get; set; }
        public double TotalUSD { get; set; }
        public double TotalILS { get; set; }
        public double TotalEUR { get; set; }
        public int CountJOD { get; set; }
        public int CountUSD { get; set; }
        public int CountILS { get; set; }
        public int CountEUR { get; set; }
        public string ErrorMsg { get; set; }
    }
}

