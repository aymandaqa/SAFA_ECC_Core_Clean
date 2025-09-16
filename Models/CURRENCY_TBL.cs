using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAFA_ECC_Core_Clean.Models
{
    [Table("CURRENCY_TBL")]
    public class CURRENCY_TBL
    {
        [Key]
        public int Currency_ID { get; set; }

        [Required]
        [StringLength(10)]
        public string Currency_Code { get; set; } = string.Empty;

        [StringLength(50)]
        public string? Currency_Name_EN { get; set; }

        [StringLength(50)]
        public string? Currency_Name_AR { get; set; }

        [StringLength(10)]
        public string? Currency_Symbol { get; set; }

        public decimal? Exchange_Rate { get; set; }

        public bool? Is_Active { get; set; }

        public bool? Is_Base_Currency { get; set; }

        public DateTime? Creation_Date { get; set; }

        [StringLength(50)]
        public string? Created_By { get; set; }

        public DateTime? Last_Update { get; set; }

        [StringLength(50)]
        public string? Updated_By { get; set; }
    }
}

