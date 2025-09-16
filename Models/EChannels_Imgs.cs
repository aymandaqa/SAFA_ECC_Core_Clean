using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAFA_ECC_Core_Clean.Models
{
    [Table("EChannels_Imgs")]
    public class EChannels_Imgs
    {
        [Key]
        public decimal Serial { get; set; }

        [StringLength(50)]
        public string? ChqSequance { get; set; }

        [StringLength(255)]
        public string? ImagePath { get; set; }

        [StringLength(100)]
        public string? ImageName { get; set; }

        public int? ImageOrder { get; set; }

        [StringLength(20)]
        public string? ImageType { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? UploadDate { get; set; }

        [StringLength(50)]
        public string? UploadedBy { get; set; }

        public long? FileSize { get; set; }

        [StringLength(50)]
        public string? ContentType { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        public bool? IsActive { get; set; } = true;

        // Navigation properties
        public virtual E_Channels_Cheques? ECheque { get; set; }
    }
}

