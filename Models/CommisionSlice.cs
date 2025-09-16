using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAFA_ECC_Core_Clean.Models
{
    [Table("CommisionSlices")]
    public partial class CommisionSlice
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal FromAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ToAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        public int CommisionId { get; set; }

        [StringLength(3)]
        public string? Currency { get; set; }

        public int? CommisionTypeId { get; set; }

        public bool? IsActive { get; set; } = true;

        public DateTime? CreatedDate { get; set; } = DateTime.Now;

        public string? CreatedBy { get; set; }

        public DateTime? LastUpdate { get; set; }

        public string? LastUpdatedBy { get; set; }

        // Navigation Properties
        [ForeignKey("CommisionId")]
        public virtual Commision? Commision { get; set; }

        [ForeignKey("CommisionTypeId")]
        public virtual CommisionType? CommisionType { get; set; }
    }
}

