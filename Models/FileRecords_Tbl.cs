using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAFA_ECC_Core_Clean.Models
{
    [Table("FileRecords_Tbl")]
    public class FileRecords_Tbl
    {
        [Key]
        public decimal Serial { get; set; }

        [StringLength(100)]
        public string? FileName { get; set; }

        [StringLength(255)]
        public string? FilePath { get; set; }

        [StringLength(20)]
        public string? FileType { get; set; }

        public long? FileSize { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? UploadDate { get; set; }

        [StringLength(50)]
        public string? UploadedBy { get; set; }

        [StringLength(20)]
        public string? Status { get; set; }

        public int? RecordCount { get; set; }

        public int? ProcessedCount { get; set; }

        public int? ErrorCount { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? ProcessingDate { get; set; }

        [StringLength(50)]
        public string? ProcessedBy { get; set; }

        [StringLength(500)]
        public string? ErrorMessage { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        public bool? IsActive { get; set; } = true;

        [Column(TypeName = "datetime")]
        public DateTime? LastUpdate { get; set; }

        [StringLength(50)]
        public string? LastUpdateBy { get; set; }
    }
}

