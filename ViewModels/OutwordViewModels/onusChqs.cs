using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAFA_ECC_Core_Clean.Models;

namespace SAFA_ECC_Core_Clean.ViewModels.OutwordViewModels
{
    public class onusChqs
    {
        public OnUs_Tbl onus { get; set; }
        public Get_Outward_Slip_CCS_VIEW onus163 { get; set; }
        public OnUs_Imgs Imgs { get; set; }
        public string Bank_Name { get; set; }
        public string RetCode_Descreption { get; set; }
        public string Branch_Name { get; set; }
    }
}

