using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAFA_ECC_Core_Clean.Models
{
    [Table("Company_Types_Tbl")]
    public partial class Company_Types_Tbl
    {
        [Key]
        public int ID { get; set; }

        [StringLength(100)]
        public string? Compant_Type_EN { get; set; }

        [StringLength(100)]
        public string? Compant_Type_AR { get; set; }

        // Computed Properties for compatibility
        [NotMapped]
        public string Company_Type_Name => !string.IsNullOrEmpty(Compant_Type_AR) ? Compant_Type_AR : Compant_Type_EN ?? "";

        // Navigation Properties
        public virtual ICollection<Companies_Tbl> Companies_Tbl { get; set; } = new HashSet<Companies_Tbl>();
    }
}

