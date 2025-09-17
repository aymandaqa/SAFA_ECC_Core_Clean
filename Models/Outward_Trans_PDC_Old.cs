using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAFA_ECC_Core_Clean.Models
{
    [Table("Outward_Trans_PDC_Old")]
    public partial class Outward_Trans_PDC_Old
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal Serial { get; set; }

        [StringLength(50)]
        public string? DrwAcctNo { get; set; }

        [StringLength(50)]
        public string? DrwChqNo { get; set; }

        [StringLength(50)]
        public string? DrwBankNo { get; set; }

        [StringLength(50)]
        public string? DrwBranchNo { get; set; }

        [StringLength(50)]
        public string? Currency { get; set; }

        public decimal Amount { get; set; }

        public DateTime ValueDate { get; set; }

        [StringLength(50)]
        public string? BenAccountNo { get; set; }

        [StringLength(250)]
        public string? BenName { get; set; }

        [StringLength(50)]
        public string? InputBrn { get; set; }

        [StringLength(50)]
        public string? ClrCenter { get; set; }

        [StringLength(50)]
        public string? WasPDC { get; set; }

        [StringLength(50)]
        public string? ReturnedCode { get; set; }

        [StringLength(50)]
        public string? Status { get; set; }
    }
}

