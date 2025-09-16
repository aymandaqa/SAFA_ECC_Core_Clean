using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAFA_ECC_Core_Clean.Models
{
    [Table("POSTING_RESTRICTION_TBL")]
    public class POSTING_RESTRICTION_TBL
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string AccountNumber { get; set; }

        [StringLength(10)]
        public string? BankCode { get; set; }

        [StringLength(10)]
        public string? BranchCode { get; set; }

        [Required]
        [StringLength(50)]
        public string RestrictionType { get; set; }

        [StringLength(500)]
        public string? RestrictionReason { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [StringLength(50)]
        public string Status { get; set; } = "ACTIVE";

        [StringLength(100)]
        public string? CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [StringLength(100)]
        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        [StringLength(1000)]
        public string? Notes { get; set; }

        public bool IsActive { get; set; } = true;

        [StringLength(100)]
        public string? ReferenceNumber { get; set; }

        [StringLength(200)]
        public string? AuthorizedBy { get; set; }

        public DateTime? AuthorizedDate { get; set; }
    }
}

