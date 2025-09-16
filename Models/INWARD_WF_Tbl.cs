using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAFA_ECC_Core_Clean.Models
{
    [Table("INWARD_WF_Tbl")]
    public class INWARD_WF_Tbl
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,0)")]
        public decimal Serial { get; set; }

        [StringLength(50)]
        public string? Status { get; set; }

        [StringLength(50)]
        public string? Final_Status { get; set; }

        [StringLength(100)]
        public string? Clr_Center { get; set; }

        public int? WF_LEVEL { get; set; }

        [StringLength(100)]
        public string? AssignedTo { get; set; }

        public DateTime? AssignedDate { get; set; }

        public DateTime InputDate { get; set; } = DateTime.Now;

        [StringLength(100)]
        public string? InputBy { get; set; }

        public DateTime? ProcessedDate { get; set; }

        [StringLength(100)]
        public string? ProcessedBy { get; set; }

        [StringLength(1000)]
        public string? Notes { get; set; }

        [StringLength(50)]
        public string? Action { get; set; }

        [StringLength(500)]
        public string? ActionReason { get; set; }

        public DateTime? ActionDate { get; set; }

        [StringLength(100)]
        public string? ActionBy { get; set; }

        [StringLength(50)]
        public string? Priority { get; set; }

        public DateTime? DueDate { get; set; }

        [StringLength(200)]
        public string? WorkflowPath { get; set; }

        public bool IsActive { get; set; } = true;

        [StringLength(100)]
        public string? PreviousAssignee { get; set; }

        public DateTime? EscalationDate { get; set; }

        [StringLength(100)]
        public string? EscalatedTo { get; set; }

        [StringLength(500)]
        public string? EscalationReason { get; set; }

        public int? StepNumber { get; set; }

        [StringLength(200)]
        public string? StepName { get; set; }

        [StringLength(1000)]
        public string? WorkflowHistory { get; set; }
    }
}

