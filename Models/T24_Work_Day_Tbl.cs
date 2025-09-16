using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAFA_ECC_Core_Clean.Models
{
    [Table("T24_Work_Day_Tbl")]
    public class T24_Work_Day_Tbl
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime WorkDate { get; set; }

        [StringLength(50)]
        public string? Status { get; set; }

        [StringLength(100)]
        public string? JobName { get; set; }

        [StringLength(500)]
        public string? JobDescription { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        [StringLength(50)]
        public string? JobStatus { get; set; }

        [StringLength(1000)]
        public string? ErrorMessage { get; set; }

        public int? ProcessedRecords { get; set; }

        public int? SuccessfulRecords { get; set; }

        public int? FailedRecords { get; set; }

        [StringLength(100)]
        public string? ExecutedBy { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? UpdatedDate { get; set; }

        [StringLength(100)]
        public string? UpdatedBy { get; set; }

        [StringLength(1000)]
        public string? Notes { get; set; }

        public bool IsCompleted { get; set; } = false;

        [StringLength(200)]
        public string? BatchNumber { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        public decimal? TotalAmount { get; set; }
    }
}

