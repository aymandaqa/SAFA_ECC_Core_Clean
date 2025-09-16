using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAFA_ECC_Core_Clean.Models
{
    [Table("Password_Policies_TBL")]
    public class Password_Policies_TBL
    {
        [Key]
        public int Policy_ID { get; set; }

        [Required]
        [StringLength(100)]
        public string Policy_Name { get; set; } = string.Empty;

        public int? Min_Length { get; set; }
        public int? Max_Length { get; set; }
        public bool? Require_Uppercase { get; set; }
        public bool? Require_Lowercase { get; set; }
        public bool? Require_Numbers { get; set; }
        public bool? Require_Special_Chars { get; set; }
        public int? Password_Expiry_Days { get; set; }
        public int? Password_History_Count { get; set; }
        public int? Max_Login_Attempts { get; set; }
        public int? Lockout_Duration_Minutes { get; set; }
        public bool? Is_Active { get; set; }
        public DateTime? Creation_Date { get; set; } // Made settable
        public string? Created_By { get; set; }
        public DateTime? Last_Update { get; set; }
        public string? Updated_By { get; set; }
    }
}


