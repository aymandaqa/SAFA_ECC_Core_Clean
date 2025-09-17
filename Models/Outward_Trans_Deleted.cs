using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAFA_ECC_Core_Clean.Models
{
    [Table("Outward_Trans_Deleted")]
    public partial class Outward_Trans_Deleted
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal Serial { get; set; }

        [StringLength(50)]
        public string? ChqSequance { get; set; }

        [StringLength(50)]
        public string? BenfAccBranch { get; set; }

        [StringLength(50)]
        public string? DeptNo { get; set; }

        public DateTime? TransDate { get; set; }

        [Column(TypeName = "decimal(18,0)")]
        public decimal? Posted { get; set; }

        [StringLength(50)]
        public string? UserName { get; set; }

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

        public bool? Returned { get; set; }

        [StringLength(5)]
        public string? ReturnedCode { get; set; }

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

        public decimal? PDCSerial { get; set; }

        [StringLength(25)]
        public string? PDCChqSequance { get; set; }

        public DateTime? LastUpdate { get; set; }

        [StringLength(50)]
        public string? LastUpdateBy { get; set; }

        public string? History { get; set; }

        public DateTime? ReturnedDate { get; set; }

        [StringLength(50)]
        public string? Status { get; set; }

        [StringLength(50)]
        public string? AuthorizerBranch { get; set; }

        [StringLength(25)]
        public string? ErrorCode { get; set; }

        public string? ErrorDescription { get; set; }

        public decimal? ClrFileRecordID { get; set; }

        [StringLength(20)]
        public string? DiscountReternedOutImgID { get; set; }

        public bool? IsTimeOut { get; set; }

        [StringLength(50)]
        public string? AuthorizedBy { get; set; }

        public int? FaildTrans { get; set; }

        public decimal? RepresentSerial { get; set; }

        [StringLength(50)]
        public string? CHQState { get; set; }

        [StringLength(50)]
        public string? PMAstatus { get; set; }

        public DateTime? PMAstatusDate { get; set; }

        [StringLength(50)]
        public string? QVFStatus { get; set; }

        [StringLength(500)]
        public string? QVFAddtlInf { get; set; }

        [StringLength(50)]
        public string? RSFStatus { get; set; }

        [StringLength(500)]
        public string? RSFAddtlInf { get; set; }

        [StringLength(300)]
        public string? Commision_Response { get; set; }

        [StringLength(50)]
        public string? Temenos_Message_Series { get; set; }

        [StringLength(50)]
        public string? chq_state_id { get; set; }
    }
}

