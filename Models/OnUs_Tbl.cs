using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAFA_ECC_Core_Clean.Models
{
    [Table("OnUs_Tbl")]
    public class OnUs_Tbl
    {
        [Key]
        public int Serial { get; set; }

        [StringLength(50)]
        public string? CHQ_SEQ { get; set; }

        [StringLength(20)]
        public string? Account_Number { get; set; }

        [StringLength(20)]
        public string? Cheque_Number { get; set; }

        public decimal? Amount { get; set; }

        public DateTime? Cheque_Date { get; set; }

        public DateTime? Value_Date { get; set; }

        [StringLength(10)]
        public string? Bank_Code { get; set; }

        [StringLength(10)]
        public string? Branch_Code { get; set; }

        [StringLength(50)]
        public string? Status { get; set; }

        public int? Return_Code { get; set; }

        [StringLength(200)]
        public string? Return_Reason { get; set; }

        public DateTime? Processing_Date { get; set; }

        public int? Inputer_ID { get; set; }

        public int? Authorized_By { get; set; }

        public DateTime? Authorization_Date { get; set; }

        public DateTime? Creation_Date { get; set; }

        public DateTime? Last_Update { get; set; }

        [StringLength(50)]
        public string? Created_By { get; set; }

        [StringLength(50)]
        public string? Updated_By { get; set; }

        public bool? Is_Deleted { get; set; }

        [StringLength(500)]
        public string? Comments { get; set; }

        // Navigation Properties
        [ForeignKey("Return_Code")]
        public virtual Return_Codes_Tbl? ReturnCodeNavigation { get; set; }

        [ForeignKey("Bank_Code")]
        public virtual Banks_Tbl? BankNavigation { get; set; }

        [ForeignKey("Inputer_ID")]
        public virtual Users_Tbl? InputerNavigation { get; set; }

        [ForeignKey("Authorized_By")]
        public virtual Users_Tbl? AuthorizerNavigation { get; set; }
    }
}

