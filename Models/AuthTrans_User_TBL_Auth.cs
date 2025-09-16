using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAFA_ECC_Core_Clean.Models
{
    [Table("AuthTrans_User_TBL_Auth")]
    public partial class AuthTrans_User_TBL_Auth
    {
        [Key]
        public int ID { get; set; } // Changed from Auth_Trans__ID to ID

        public int Auth_user_ID { get; set; }

        [StringLength(50)]
        public string? Auth_user_Name { get; set; }

        [StringLength(100)]
        public string? Auth_Trans__name { get; set; }

        public bool? Auth_level1 { get; set; }

        public bool? Auth_level2 { get; set; }

        public int? group_ID { get; set; }

        public int? Trans_id { get; set; }

        public int? Brancg_Code { get; set; }

        [StringLength(50)]
        public string? status { get; set; }

        // Navigation Properties
        public virtual Users_Tbl? User { get; set; }
        public virtual Groups_Tbl? Group { get; set; }
        public DateTime? Creation_Date { get; set; }

        [StringLength(50)]
        public string? Created_By { get; set; }
    }
}


