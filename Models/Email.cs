using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAFA_ECC_Core_Clean.Models
{
    [Table("Email")]
    public class Email
    {
        [Key]
        public int EmailId { get; set; }

        [Required]
        [StringLength(255)]
        public string ToEmail { get; set; }

        [StringLength(255)]
        public string? FromEmail { get; set; }

        [Required]
        [StringLength(500)]
        public string Subject { get; set; }

        [Required]
        public string Body { get; set; }

        public DateTime SendDate { get; set; }

        [StringLength(100)]
        public string? SentBy { get; set; }

        [StringLength(50)]
        public string Status { get; set; } = "PENDING";

        public bool HighPriority { get; set; } = false;

        public int? RetryCount { get; set; } = 0;

        public DateTime? LastRetryDate { get; set; }

        [StringLength(1000)]
        public string? ErrorMessage { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? UpdatedDate { get; set; }

        [StringLength(100)]
        public string? UpdatedBy { get; set; }
    }
}

