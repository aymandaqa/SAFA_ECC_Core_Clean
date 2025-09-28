using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAFA_ECC_Core_Clean.ViewModels.AdminViewModels
{
    public class UserPermissionViewModel
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string PageID { get; set; }
        public string PageName { get; set; }
        public bool Value { get; set; }
        public int ActionID { get; set; }
        public string ActionName { get; set; }
        public int Application_ID { get; set; }
    }
}

