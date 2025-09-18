using System.Collections.Generic;

namespace SAFA_ECC_Core_Clean.ViewModels.AuthenticationViewModels
{
    public class UserPagePermissionsResultViewModel
    {
        public bool ACCESS { get; set; }
        public string Page_Id { get; set; }
        public string Group_Id { get; set; }
        public string User_ID { get; set; }
        public string Application_ID { get; set; }
        public bool Add { get; set; }
        public bool Delete { get; set; }
        public bool Reverse { get; set; }
        public bool Update { get; set; }
        public bool Post { get; set; }
        public string Pagename { get; set; }
        public string AccessPage { get; set; }
        public List<UserPagePermissionsResultViewModel> GroupPermissions { get; set; }
    }
}

