using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAFA_ECC_Core_Clean.Models
{
    [Table("Post_Dated_Trans")]
    public partial class Post_Dated_Trans
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal Serial { get; set; }

        [StringLength(50)]
        public string? ChqSequance { get; set; }

        [StringLength(50)]
        public string? BenfAccBranch { get; set; }

        [StringLength(50)]
        public string? AcctType { get; set; }

        [StringLength(50)]
        public string? DeptNo { get; set; }

        public DateTime? TransDate { get; set; }

        [Column(TypeName = "decimal(18,0)")]
        public decimal? Posted { get; set; }

        [StringLength(50)]
        public string? UserId { get; set; }

        [StringLength(50)]
        public string? ClrCenter { get; set; }

        public decimal? OperType { get; set; }

        [StringLength(50)]
        public string? DrwChqNo { get; set; }

        [StringLength(50)]
        public string? DrwBankNo { get; set; }

        [StringLength(50)]
        public string? DrwBranchNo { get; set; }

        [StringLength(50)]
        public string? DrwAcctNo { get; set; }

        [Column(TypeName = "decimal(19,4)")]
        public decimal? Amount { get; set; }

        public DateTime? DueDate { get; set; }

        public decimal? System_Aut_Man { get; set; }

        [StringLength(50)]
        public string? TransCode { get; set; }

        public decimal? CollDays { get; set; }

        public decimal? IntrDays { get; set; }

        public decimal? Special { get; set; }

        public decimal? Rejected { get; set; }

        [StringLength(50)]
        public string? Currency { get; set; }

        [StringLength(50)]
        public string? BenfBnk { get; set; }

        [StringLength(50)]
        public string? InputBrn { get; set; }

        [StringLength(50)]
        public string? BenfCardType { get; set; }

        [StringLength(50)]
        public string? BenfCardId { get; set; }

        public string? BenName { get; set; }

        [StringLength(50)]
        public string? BenAccountNo { get; set; }

        [StringLength(50)]
        public string? BenfNationality { get; set; }

        [StringLength(50)]
        public string? DrwBranchExt { get; set; }

        public int? NeedTechnicalVerification { get; set; }

        public DateTime? InputDate { get; set; }

        [StringLength(50)]
        public string? ISSAccount { get; set; }

        public int? WithUV { get; set; }

        public bool? SpecialHandling { get; set; }

        public bool? IsVIP { get; set; }

        public string? DrwName { get; set; }

        public string? DrwCardId { get; set; }

        public DateTime? LastUpdate { get; set; }

        [StringLength(50)]
        public string? LastUpdateBy { get; set; }

        public string? History { get; set; }

        [StringLength(50)]
        public string? Status { get; set; }

        [StringLength(50)]
        public string? AuthorizerBranch { get; set; }

        public string? Note1 { get; set; }

        [StringLength(25)]
        public string? ErrorCode { get; set; }

        public string? ErrorDescription { get; set; }

        public bool? IsTimeOut { get; set; }

        [StringLength(50)]
        public string? VerfiedBy { get; set; }

        [StringLength(50)]
        public string? AuthorizedBy { get; set; }

        public int? FaildTrans { get; set; }

        [StringLength(300)]
        public string? PDCReversalSettlement { get; set; }

        [StringLength(300)]
        public string? Commision_Response { get; set; }

        [StringLength(50)]
        public string? DrwCardType { get; set; }

        [StringLength(300)]
        public string? Out_Commision_Response { get; set; }

        [StringLength(50)]
        public string? Temenos_Message_Series { get; set; }

        [StringLength(50)]
        public string? chq_state_id { get; set; }

        [StringLength(50)]
        public string? CHQState { get; set; }

        // Computed Properties for compatibility
        [NotMapped]
        public int Trans_ID => (int)Serial;

        [NotMapped]
        public string? Cheque_No => DrwChqNo;

        [NotMapped]
        public string? Drawer_Name => DrwName;

        [NotMapped]
        public DateTime? Trans_Date => TransDate;

        [NotMapped]
        public int? Bank_ID => !string.IsNullOrEmpty(DrwBankNo) ? int.TryParse(DrwBankNo, out int bankId) ? bankId : null : null;

        [NotMapped]
        public int DaysToMaturity => DueDate.HasValue ? (int)(DueDate.Value - DateTime.Now).TotalDays : 0;

        [NotMapped]
        public bool IsOverdue => DueDate.HasValue && DueDate.Value < DateTime.Now;

        [NotMapped]
        public bool IsDueToday => DueDate.HasValue && DueDate.Value.Date == DateTime.Now.Date;

        // Navigation Properties
        public virtual Banks_Tbl? Bank { get; set; }
        public virtual ICollection<Cheque_Images_Link_Tbl> ChequeImages { get; set; } = new List<Cheque_Images_Link_Tbl>();
    }
}

