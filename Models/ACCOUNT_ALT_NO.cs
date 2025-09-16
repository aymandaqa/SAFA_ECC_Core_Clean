using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAFA_ECC_Core_Clean.Models
{
    [Table("ACCOUNT_ALT_NO")]
    public class ACCOUNT_ALT_NO
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(20)]
        public string Account_Number { get; set; } = string.Empty;

        [StringLength(20)]
        public string? Alt_Account_Number { get; set; }

        [StringLength(10)]
        public string? Bank_Code { get; set; }

        [StringLength(10)]
        public string? Branch_Code { get; set; }

        public bool? Is_Active { get; set; }

        public DateTime? Creation_Date { get; set; }

        [StringLength(50)]
        public string? Created_By { get; set; }

        public DateTime? Last_Update { get; set; }

        [StringLength(50)]
        public string? Updated_By { get; set; }

        // Navigation Properties
        [ForeignKey("Bank_Code")]
        public virtual Banks_Tbl? BankNavigation { get; set; }
    }
}

