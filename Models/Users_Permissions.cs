using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAFA_ECC_Core_Clean.Models
{
    [Table("Users_Permissions")]
    public partial class Users_Permissions
    {
        [Key, Column(Order = 0)]
        public int Application_ID { get; set; }

        [Key, Column(Order = 1)]
        public int UserID { get; set; }

        [Key, Column(Order = 2)]
        public int PageID { get; set; }

        [Key, Column(Order = 3)]
        public int ActionID { get; set; }

        [StringLength(100)]
        public string Page_Name { get; set; } = string.Empty; // Added Page_Name

        [StringLength(100)]
        public string Action_Name { get; set; } = string.Empty; // Added Action_Name

        public bool? Value { get; set; }

        // Navigation Properties
        public virtual Users_Tbl? User { get; set; }
    }
}


