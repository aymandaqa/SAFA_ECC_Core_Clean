using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAFA_ECC_Core_Clean.Models
{
    [Table("ClrFiles_Tbl")]
    public class ClrFiles_Tbl
    {
        [Key]
        public int FileId { get; set; }

        [Required]
        [StringLength(255)]
        public string FileName { get; set; }

        [StringLength(50)]
        public string? FileType { get; set; }

        public DateTime FileDate { get; set; }

        [StringLength(500)]
        public string? FilePath { get; set; }

        [StringLength(50)]
        public string Status { get; set; } = "NEW";

        public long? FileSize { get; set; }

        public int? RecordCount { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        public decimal? TotalAmount { get; set; }

        [StringLength(100)]
        public string? CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [StringLength(100)]
        public string? UpdatedBy { get; set; }

        public DateTime? LastUpdate { get; set; }

        [StringLength(1000)]
        public string? Notes { get; set; }

        [StringLength(200)]
        public string? BatchNumber { get; set; }

        [StringLength(50)]
        public string? ClearingCenter { get; set; }

        public DateTime? ProcessedDate { get; set; }

        [StringLength(100)]
        public string? ProcessedBy { get; set; }

        [StringLength(1000)]
        public string? ErrorMessage { get; set; }

        public int? RetryCount { get; set; } = 0;

        public DateTime? LastRetryDate { get; set; }

        public bool IsActive { get; set; } = true;

        [StringLength(100)]
        public string? OriginalFileName { get; set; }

        [StringLength(50)]
        public string? FileFormat { get; set; }

        [StringLength(200)]
        public string? CheckSum { get; set; }
    }
}

