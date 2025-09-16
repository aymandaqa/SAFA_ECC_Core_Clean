using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAFA_ECC_Core_Clean.Models
{
    [Table("Companies_Tbl")]
    public partial class Companies_Tbl
    {
        [Key]
        public int Company_ID { get; set; }

        [Required]
        [StringLength(10)]
        public string Company_Code { get; set; } = string.Empty;

        [StringLength(50)]
        public string? Company_Name_EN { get; set; }

        [StringLength(50)]
        public string? Company_Name_AR { get; set; }

        public int Company_Type { get; set; }

        // Computed Properties for compatibility
        [NotMapped]
        public string Company_Name => !string.IsNullOrEmpty(Company_Name_AR) ? Company_Name_AR : Company_Name_EN ?? "";

        [NotMapped]
        public bool Is_Active { get; set; } = true; // Made settable for controller

        // Navigation Properties
        public virtual Company_Types_Tbl? Company_Types_Tbl { get; set; }
        public virtual ICollection<Users_Tbl> Users { get; set; } = new HashSet<Users_Tbl>(); // Changed Users_Tbl to Users
    }
}


