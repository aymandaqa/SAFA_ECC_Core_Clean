using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAFA_ECC_Core_Clean.Models
{
    [Table("CommisionExemptions")]
    public partial class CommisionExemption
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        public string? T24Account { get; set; }

        [StringLength(50)]
        public string? Account { get; set; }

        public int CommisionId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        public int ExcemptionTypeId { get; set; }

        [StringLength(3)]
        public string? CurrencyId { get; set; }

        public bool? Deleted { get; set; } = false;

        [StringLength(50)]
        public string? InsertBy { get; set; }

        public DateTime? InsertDate { get; set; }

        [StringLength(50)]
        public string? LastUpdatedBy { get; set; }

        public DateTime? LastUpdate { get; set; }

        public string? History { get; set; }

        // Navigation Properties
        [ForeignKey("CommisionId")]
        public virtual Commision? Commision { get; set; }

        [ForeignKey("ExcemptionTypeId")]
        public virtual ExcemptionType? ExcemptionType { get; set; }
    }
}

