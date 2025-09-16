using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAFA_ECC_Core_Clean.Models
{
    [Table("Outward_Imgs")]
    public class Outward_Imgs
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "decimal(18,0)")]
        public decimal? Serial { get; set; }

        [StringLength(50)]
        public string? ChqSequance { get; set; }

        [StringLength(50)]
        public string? ChqNo { get; set; }

        [StringLength(50)]
        public string? AccountNo { get; set; }

        [StringLength(10)]
        public string? BankCode { get; set; }

        [StringLength(10)]
        public string? BranchCode { get; set; }

        public byte[]? FrontImage { get; set; }

        public byte[]? BackImage { get; set; }

        [StringLength(255)]
        public string? FrontImagePath { get; set; }

        [StringLength(255)]
        public string? BackImagePath { get; set; }

        [StringLength(100)]
        public string? ImageFormat { get; set; }

        public long? FrontImageSize { get; set; }

        public long? BackImageSize { get; set; }

        public DateTime? ImageDate { get; set; }

        [StringLength(100)]
        public string? CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [StringLength(100)]
        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        [StringLength(50)]
        public string? Status { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }

        public bool IsActive { get; set; } = true;

        [StringLength(200)]
        public string? OriginalFileName { get; set; }

        [StringLength(100)]
        public string? CompressionType { get; set; }

        public int? CompressionRatio { get; set; }

        [StringLength(200)]
        public string? CheckSum { get; set; }

        [StringLength(50)]
        public string? ImageSource { get; set; }

        public bool HasFrontImage { get; set; } = false;

        public bool HasBackImage { get; set; } = false;
    }
}

