using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAFA_ECC_Core_Clean.Models
{
    [Table("Groups_Tbl")]
    public partial class Groups_Tbl
    {
        [Key]
        public int Group_Id { get; set; }

        [StringLength(100)]
        public string? Group_Name_EN { get; set; }

        [StringLength(100)]
        public string? Group_Name_AR { get; set; }

        public int? Group_Type { get; set; }

        [StringLength(100)]
        public string? Created_By { get; set; }

        public DateTime? Creation_Date { get; set; }

        public DateTime? Last_Amend_Date { get; set; }

        public string? Amend_History { get; set; }

        public string? grouptype { get; set; }

        // Added Group_Description for compatibility with AdminController
        [StringLength(255)]
        public string? Group_Description { get; set; }

        // Properties for compatibility with AdminController
        [NotMapped]
        public string Group_Name
        {
            get => !string.IsNullOrEmpty(Group_Name_AR) ? Group_Name_AR : Group_Name_EN ?? "";
            set { /* Setter for compatibility, actual storage is EN/AR */ }
        }

        [NotMapped]
        public bool Is_Active { get; set; } = true; // Make settable for controller

        // Navigation Properties
        public virtual Group_Types_Tbl? Group_Types_Tbl { get; set; }
        public virtual ICollection<Users_Tbl> Users { get; set; } = new HashSet<Users_Tbl>();
        public virtual ICollection<AuthTrans_User_TBL_Auth> AuthorizedTransactions { get; set; } = new List<AuthTrans_User_TBL_Auth>();
    }
}


