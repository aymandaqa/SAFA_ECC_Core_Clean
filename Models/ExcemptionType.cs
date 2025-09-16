using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAFA_ECC_Core_Clean.Models
{
    [Table("ExcemptionTypes")]
    public partial class ExcemptionType
    {
        public ExcemptionType()
        {
            CommisionExemptions = new HashSet<CommisionExemption>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(200)]
        public string? NameAR { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        public bool? IsActive { get; set; } = true;

        public DateTime? CreatedDate { get; set; } = DateTime.Now;

        public string? CreatedBy { get; set; }

        // Navigation Properties
        public virtual ICollection<CommisionExemption> CommisionExemptions { get; set; }
    }
}

