using SAFA_ECC_Core_Clean.Models;
using System.Collections.Generic;

namespace SAFA_ECC_Core_Clean.ViewModels.OutwordViewModels
{
    public class ReturnListViewModel
    {
        public string ErrorMsg { get; set; }
        public List<Outward_Trans> LstPDC { get; set; }
        public List<Return_Codes_Tbl> LstDis { get; set; }
        public List<Return_Codes_Tbl> LstPMA { get; set; }
        public double AmountTot { get; set; }
        public double ILSAmount { get; set; }
        public double JODAmount { get; set; }
        public double USDAmount { get; set; }
        public double EURAmount { get; set; }
        public int ILSCount { get; set; }
        public int JODCount { get; set; }
        public int USDCount { get; set; }
        public int EURCount { get; set; }
        public string Locked_user { get; set; }
    }
}

