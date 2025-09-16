using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAFA_ECC_Core_Clean.Models
{
    [Table("E_Channels_Cheques")]
    public class E_Channels_Cheques
    {
        [Key]
        public decimal Serial { get; set; }

        [StringLength(50)]
        public string? ChqSequance { get; set; }

        [StringLength(20)]
        public string? DrwChqNo { get; set; }

        [StringLength(30)]
        public string? DrwAcctNo { get; set; }

        [StringLength(100)]
        public string? BenName { get; set; }

        [StringLength(30)]
        public string? BenAccountNo { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        public decimal? Amount { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? TransDate { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? InputDate { get; set; }

        [StringLength(20)]
        public string? Status { get; set; }

        [StringLength(10)]
        public string? DrwBankNo { get; set; }

        [StringLength(10)]
        public string? InputBrn { get; set; }

        [StringLength(50)]
        public string? InputBy { get; set; }

        [StringLength(3)]
        public string? Currency { get; set; }

        [StringLength(20)]
        public string? ChqSource { get; set; }

        [StringLength(500)]
        public string? History { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? LastUpdate { get; set; }

        [StringLength(50)]
        public string? LastUpdateBy { get; set; }

        // Navigation properties
        public virtual Banks_Tbl? Bank { get; set; }
        public virtual List<EChannels_Imgs> Images { get; set; } = new List<EChannels_Imgs>();
    }
}

