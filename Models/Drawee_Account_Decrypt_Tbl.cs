using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAFA_ECC_Core_Clean.Models
{
    [Table("Drawee_Account_Decrypt_Tbl")]
    public class Drawee_Account_Decrypt_Tbl
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Encrypted_Account { get; set; } = string.Empty;

        [StringLength(50)]
        public string? Decrypted_Account { get; set; }

        [StringLength(20)]
        public string? Cheque_Number { get; set; }

        [StringLength(10)]
        public string? Bank_Code { get; set; }

        [StringLength(10)]
        public string? Branch_Code { get; set; }

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

