using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAFA_ECC_Core_Clean.Models
{
    [Table("Accounts")]
    public class ACCOUNT
    {
        [Key]
        public int Id { get; set; } // Assuming a primary key is needed

        [StringLength(50)]
        public string? ACCOUNT_NUMBER { get; set; }

        [StringLength(50)]
        public string? CUSTOMER_ID { get; set; }
    }
}

