using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAFA_ECC_Core_Clean.Models
{
    [Table("InwardCustomerDuesReport")]
    public class InwardCustomerDuesReport
    {
        [Key]
        public int ID { get; set; }

        [StringLength(10)]
        public string? Clearing_Center { get; set; }

        [StringLength(20)]
        public string? Customer_ID { get; set; }

        public decimal? Amount { get; set; }

        [StringLength(50)]
        public string? Drawee_Account { get; set; }

        [StringLength(100)]
        public string? Drawee_Name { get; set; }

        public int? Serial { get; set; }

        [StringLength(50)]
        public string? CHQ_SEQ { get; set; }

        [StringLength(10)]
        public string? Branch_Code { get; set; }

        public DateTime? Creation_Date { get; set; }

        [StringLength(50)]
        public string? Created_By { get; set; }

        public DateTime? Last_Update { get; set; }

        [StringLength(50)]
        public string? Updated_By { get; set; }

        // Navigation Properties
        [ForeignKey("Serial")]
        public virtual Inward_Trans? InwardTransaction { get; set; }
    }
}

