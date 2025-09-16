using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAFA_ECC_Core_Clean.Models
{
    [Table("Return_Codes_Tbl")]
    public partial class Return_Codes_Tbl
    {
        [Key]
        [StringLength(5)]
        public string ReturnCode { get; set; } = string.Empty;

        [StringLength(50)]
        public string? ClrCenter { get; set; }

        public string? Description_AR { get; set; }

        public string? Description_EN { get; set; }

        [StringLength(50)]
        public string? DiscountPMAMapping { get; set; }

        public string? Notes { get; set; }

        public bool? Is_Active { get; set; } = true;

        // Computed Properties for compatibility
        [NotMapped]
        public string Return_Code => ReturnCode;

        [NotMapped]
        public string Description => !string.IsNullOrEmpty(Description_AR) ? Description_AR : Description_EN ?? "";

        [NotMapped]
        public string Code => ReturnCode;

        // Navigation Properties
        public virtual ICollection<Inward_Trans> InwardTransactions { get; set; } = new List<Inward_Trans>();
        public virtual ICollection<Outward_Trans> OutwardTransactions { get; set; } = new List<Outward_Trans>();
    }
}

