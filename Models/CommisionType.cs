using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAFA_ECC_Core_Clean.Models
{
    [Table("CommisionTypes")]
    public partial class CommisionType
    {
        public CommisionType()
        {
            Commisions = new HashSet<Commision>();
            CommisionSlices = new HashSet<CommisionSlice>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Description { get; set; }

        public bool? IsActive { get; set; } = true;

        public DateTime? CreatedDate { get; set; } = DateTime.Now;

        public string? CreatedBy { get; set; }

        public DateTime? LastUpdate { get; set; }

        public string? LastUpdatedBy { get; set; }

        // Navigation Properties
        public virtual ICollection<Commision> Commisions { get; set; }
        public virtual ICollection<CommisionSlice> CommisionSlices { get; set; }
    }
}

