using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAFA_ECC_Core_Clean.Models
{
    [Table("Group_Permissions")]
    public class Group_Permissions
    {
        [Key]
        public int Application_ID { get; set; }

        [Key]
        public int GroupID { get; set; }

        [Key]
        public int PageID { get; set; }

        [Key]
        public int ActionID { get; set; }

        [StringLength(100)]
        public string Page_Name { get; set; } = string.Empty; // Added Page_Name

        [StringLength(100)]
        public string Action_Name { get; set; } = string.Empty; // Added Action_Name

        public bool Is_Allowed { get; set; }

        // Navigation Property
        [ForeignKey("GroupID")]
        public virtual Groups_Tbl Group { get; set; } = null!;
    }
}


