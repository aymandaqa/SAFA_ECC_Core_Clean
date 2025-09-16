using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAFA_ECC_Core_Clean.Models
{
    [Table("Commisions")]
    public partial class Commision
    {
        [Key]
        public int Id { get; set; }

        public int? CommTypeId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Amount { get; set; }

        [StringLength(3)]
        public string? CurrencyId { get; set; }

        public int? CheqSourceId { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        public bool? Deleted { get; set; } = false;

        public int? T24ComTypeId { get; set; }

        public DateTime? LastUpdate { get; set; }

        public string? History { get; set; }

        // Navigation Properties
        [ForeignKey("CommTypeId")]
        public virtual CommisionType? CommisionType { get; set; }

        [ForeignKey("CheqSourceId")]
        public virtual ChequeSource? ChequeSource { get; set; }
    }
}

