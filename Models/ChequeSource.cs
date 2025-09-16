using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAFA_ECC_Core_Clean.Models
{
    [Table("ChequeSource")]
    public partial class ChequeSource
    {
        public ChequeSource()
        {
            Commisions = new HashSet<Commision>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Description { get; set; } = string.Empty;

        [StringLength(200)]
        public string? DescriptionAR { get; set; }

        public bool? IsActive { get; set; } = true;

        public DateTime? CreatedDate { get; set; } = DateTime.Now;

        public string? CreatedBy { get; set; }

        // Navigation Properties
        public virtual ICollection<Commision> Commisions { get; set; }
    }
}

