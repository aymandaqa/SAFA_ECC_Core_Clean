using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAFA_ECC_Core_Clean.Models
{
    [Table("Auth_Tran_Details_TBL")]
    public class Auth_Tran_Details_TBL
    {
        [Key]
        public int ID { get; set; }

        [StringLength(50)]
        public string? Chq_Serial { get; set; }

        [StringLength(50)]
        public string? Auth_User { get; set; }

        public DateTime? Auth_Date { get; set; }

        [StringLength(50)]
        public string? Auth_Type { get; set; }

        [StringLength(50)]
        public string? Auth_Status { get; set; }

        [StringLength(50)]
        public string? Auth_Branch { get; set; }

        [StringLength(50)]
        public string? Auth_Comment { get; set; }
    }
}

