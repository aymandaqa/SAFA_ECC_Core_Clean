using System;
using System.Collections.Generic;

using SAFA_ECC_Core_Clean.Models;

namespace SAFA_ECC_Core_Clean.ViewModels.OutwordViewModels
{
    public class AllInwOutChqViewModel
    {
        public string ErrorMsg { get; set; }
        public double SumClrOut { get; set; }
        public double SumRetOut { get; set; }
        public double SumClrInw { get; set; }
        public double SumRetInw { get; set; }
        public double SumInsett { get; set; }
        public double SumOutChq { get; set; }
        public double SumTotal { get; set; }
        public List<Outward_Trans> ReturnedAmountOut { get; set; }
        public List<Inward_Trans> ReturnedAmountInw { get; set; }
        public List<Inward_Trans> ClearedAmountInw { get; set; }
        public List<Outward_Imgs> OutImgs { get; set; }
    }
}

