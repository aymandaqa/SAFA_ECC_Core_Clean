using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAFA_ECC_Core_Clean.Models
{
    [Table("Hold_CHQ")]
    public partial class Hold_CHQ
    {
        [Key]
        [Column(Order = 0)]
        [Required]
        [StringLength(50)]
        [Display(Name = "رقم الشيك")]
        public string DrwChqNo { get; set; } = string.Empty;

        [Key]
        [Column(Order = 1)]
        [Required]
        [StringLength(10)]
        [Display(Name = "رقم البنك")]
        public string DrwBankNo { get; set; } = string.Empty;

        [Key]
        [Column(Order = 2)]
        [Required]
        [StringLength(10)]
        [Display(Name = "رقم الفرع")]
        public string DrwBranchNo { get; set; } = string.Empty;

        [Key]
        [Column(Order = 3)]
        [Required]
        [StringLength(50)]
        [Display(Name = "رقم الحساب")]
        public string DrwAcctNo { get; set; } = string.Empty;

        [StringLength(200)]
        [Display(Name = "السبب الأول")]
        public string? Reson1 { get; set; }

        [StringLength(200)]
        [Display(Name = "السبب الثاني")]
        public string? Reson2 { get; set; }

        [StringLength(50)]
        [Display(Name = "النوع")]
        public string? Type { get; set; }

        [Display(Name = "محجوز")]
        public bool? Reserved { get; set; }

        [Display(Name = "تاريخ الإدخال")]
        public DateTime? InputDate { get; set; }

        [StringLength(50)]
        [Display(Name = "أدخل بواسطة")]
        public string? InputBy { get; set; }

        [Display(Name = "تاريخ التحديث")]
        public DateTime? UpdateDate { get; set; }

        [StringLength(50)]
        [Display(Name = "حدث بواسطة")]
        public string? UpdateBy { get; set; }

        [Display(Name = "التاريخ")]
        public string? History { get; set; }

        [StringLength(500)]
        [Display(Name = "ملاحظة")]
        public string? Note1 { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        [Display(Name = "المبلغ")]
        public decimal? Amount { get; set; }

        [StringLength(20)]
        [Display(Name = "الحالة")]
        public string? Status { get; set; }

        [Display(Name = "رقم التسلسل")]
        public decimal Serial { get; set; }

        [StringLength(200)]
        [Display(Name = "سبب الحجز")]
        public string? Hold_Reason { get; set; }

        [StringLength(50)]
        [Display(Name = "محجوز بواسطة")]
        public string? Hold_By { get; set; }

        // Navigation properties
        [ForeignKey("DrwBankNo")]
        public virtual Banks_Tbl? Bank { get; set; }
    }
}

