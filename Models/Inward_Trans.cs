using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAFA_ECC_Core_Clean.Models
{
    [Table("Inward_Trans")]
    public partial class Inward_Trans
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal Serial { get; set; }

        public int? ClrFileRecordID { get; set; }

        [StringLength(50)]
        public string? ChqSequance { get; set; }

        [StringLength(50)]
        public string? BenfAccBranch { get; set; }

        [StringLength(50)]
        public string? DeptNo { get; set; }

        public DateTime? TransDate { get; set; }

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

        public DateTime? ValueDate { get; set; }

        public decimal? System_Aut_Man { get; set; }

        [StringLength(50)]
        public string? TransCode { get; set; }

        public decimal? Rejected { get; set; }

        [StringLength(50)]
        public string? Currency { get; set; }

        [StringLength(50)]
        public string? BenfBnk { get; set; }

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

        public bool? VIP { get; set; }

        public int? NeedTechnicalVerification { get; set; }

        [StringLength(50)]
        public string? ISSAccount { get; set; }

        public bool? SpecialHandling { get; set; }

        public string? DrwName { get; set; }

        public string? DrwCardId { get; set; }

        [StringLength(25)]
        public string? PDCSequance { get; set; }

        [StringLength(20)]
        public string? DiscountReternedOutImgID { get; set; }

        public DateTime? LastUpdate { get; set; }

        [StringLength(50)]
        public string? LastUpdateBy { get; set; }

        public string? History { get; set; }

        [StringLength(50)]
        public string? Status { get; set; }

        public bool? Returned { get; set; }

        [StringLength(5)]
        public string? ReturnCode { get; set; }

        public DateTime? IntrBkSttlmDt { get; set; }

        public bool? Was_PDC { get; set; }

        [StringLength(25)]
        public string? PDC_Serial { get; set; }

        [StringLength(300)]
        public string? T24Response { get; set; }

        public string? ErrorDescription { get; set; }

        [StringLength(25)]
        public string? ErrorCode { get; set; }

        [Column(TypeName = "decimal(18,0)")]
        public decimal? Posted { get; set; }

        public DateTime? InputDate { get; set; }

        public bool? verifyStatus { get; set; }

        public bool? VerifiedTechnically { get; set; }

        public bool? VerifiedFinancially { get; set; }

        public bool? NeedFinanciallyWF { get; set; }

        public bool? NeedFixedError { get; set; }

        [StringLength(50)]
        public string? WFStage { get; set; }

        public bool? Stoped { get; set; }

        public string? specialNote { get; set; }

        [StringLength(50)]
        public string? Company_Code { get; set; }

        public string? Note2 { get; set; }

        public bool? IsTimeOut { get; set; }

        [StringLength(50)]
        public string? AltAccount { get; set; }

        [StringLength(50)]
        public string? SecoundLevelStatus { get; set; }

        [StringLength(500)]
        public string? SecoundLevelUser { get; set; }

        public DateTime? SecoundLevelDate { get; set; }

        public DateTime? ReturnDate { get; set; }

        [StringLength(50)]
        public string? DrwCardType { get; set; }

        [StringLength(50)]
        public string? CHQState { get; set; }

        [StringLength(50)]
        public string? PMAstatus { get; set; }

        public DateTime? PMAstatusDate { get; set; }

        [StringLength(5)]
        public string? ReturnCodeFinancail { get; set; }

        [StringLength(50)]
        public string? RSF { get; set; }

        [StringLength(50)]
        public string? Legal_Doc_Type { get; set; }

        public bool? ISneedCommision { get; set; }

        public bool? RevCommision { get; set; }

        [StringLength(50)]
        public string? QVFStatus { get; set; }

        [StringLength(500)]
        public string? QVFAddtlInf { get; set; }

        [StringLength(500)]
        public string? RSFAddtlInf { get; set; }

        [StringLength(50)]
        public string? category { get; set; }

        [StringLength(50)]
        public string? FirstLevelStatus { get; set; }

        [StringLength(500)]
        public string? FirstLevelUser { get; set; }

        public DateTime? FirstLevelDate { get; set; }

        [StringLength(50)]
        public string? Temenos_Message_Series { get; set; }

        [StringLength(5)]
        public string? FinalRetCode { get; set; }

        [StringLength(50)]
        public string? OLDDrwBranchNo { get; set; }

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

        // Navigation Properties
        public virtual Return_Codes_Tbl? ReturnCodeNavigation { get; set; }
        public virtual Banks_Tbl? Bank { get; set; }
        public virtual ICollection<Cheque_Images_Link_Tbl> ChequeImages { get; set; } = new List<Cheque_Images_Link_Tbl>();
    }
}

