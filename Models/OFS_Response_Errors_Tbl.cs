using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAFA_ECC_Core_Clean.Models
{
    [Table("OFS_Response_Errors_Tbl")]
    public class OFS_Response_Errors_Tbl
    {
        [Key]
        public decimal Serial { get; set; }

        [StringLength(50)]
        public string ChqSequance { get; set; }

        [StringLength(50)]
        public string Cheque_Type { get; set; }

        public string LastErrorMessage { get; set; }

        public string History { get; set; }

        [StringLength(50)]
        public string LastAmendBy { get; set; }

        public DateTime LastAmendDate { get; set; }
    }
}

