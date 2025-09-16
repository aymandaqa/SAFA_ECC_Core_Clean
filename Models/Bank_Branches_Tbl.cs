using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAFA_ECC_Core_Clean.Models
{
    [Table("Bank_Branches_Tbl")]
    public partial class Bank_Branches_Tbl
    {
        [Key, Column(Order = 0)]
        [StringLength(50)]
        public string BankCode { get; set; } = string.Empty;

        [Key, Column(Order = 1)]
        [StringLength(50)]
        public string BranchCode { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Branch_NameEn { get; set; }

        [StringLength(100)]
        public string? Branch_NameAr { get; set; }

        public string? Branch_Address { get; set; }

        [StringLength(50)]
        public string? MappedAccountNo { get; set; }

        // Computed Properties for compatibility
        [NotMapped]
        public string Branch_Name => !string.IsNullOrEmpty(Branch_NameAr) ? Branch_NameAr : Branch_NameEn ?? "";

        // Navigation Properties
        public virtual Banks_Tbl? Bank { get; set; }
    }
}

