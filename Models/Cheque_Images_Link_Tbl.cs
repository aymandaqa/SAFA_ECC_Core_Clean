using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAFA_ECC_Core_Clean.Models
{
    [Table("Cheque_Images_Link_Tbl")]
    public class Cheque_Images_Link_Tbl
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [StringLength(25)]
        public string? ChqSequance { get; set; }

        [StringLength(50)]
        public string? ImageID { get; set; }

        [StringLength(10)]
        public string? ImageType { get; set; }

        public DateTime? CreationDate { get; set; }

        [StringLength(50)]
        public string? CreatedBy { get; set; }

        // Navigation Properties
        public virtual Cheque_Images_Tbl? ChequeImage { get; set; }
        public virtual Inward_Trans? InwardTransaction { get; set; }
        public virtual Outward_Trans? OutwardTransaction { get; set; }
        public virtual Post_Dated_Trans? PDCTransaction { get; set; }
    }
}

