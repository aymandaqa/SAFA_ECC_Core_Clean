using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAFA_ECC_Core_Clean.Models
{
    [Table("AuthTrans_User_TBL")]
    public class AuthTrans_User_TBL
    {
        [Key]
        public int ID { get; set; }

        public int Trans_id { get; set; }

        public int Auth_user_ID { get; set; }

        [StringLength(50)]
        public string? Auth_user_Name { get; set; }

        public int? group_ID { get; set; }

        [StringLength(50)]
        public string? Trans_Type { get; set; }

        public decimal? Trans_Amount { get; set; }

        [StringLength(20)]
        public string? status { get; set; }

        public DateTime? Trans_Date { get; set; }

        public DateTime? Auth_Date { get; set; }

        [StringLength(50)]
        public string? Authorized_By { get; set; }

        public DateTime? Creation_Date { get; set; }

        [StringLength(50)]
        public string? Created_By { get; set; }

        // Navigation Properties
        public virtual Users_Tbl? User { get; set; }
        public virtual Groups_Tbl? Group { get; set; }
    }
}

