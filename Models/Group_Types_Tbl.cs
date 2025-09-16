using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAFA_ECC_Core_Clean.Models
{
    [Table("Group_Types_Tbl")]
    public partial class Group_Types_Tbl
    {
        [Key]
        public int ID { get; set; }

        [StringLength(100)]
        public string? Type_Name_EN { get; set; }

        [StringLength(100)]
        public string? Type_Name_Ar { get; set; }

        // Computed Properties for compatibility
        [NotMapped]
        public string Type_Name => !string.IsNullOrEmpty(Type_Name_Ar) ? Type_Name_Ar : Type_Name_EN ?? "";

        // Navigation Properties
        public virtual ICollection<Groups_Tbl> Groups_Tbl { get; set; } = new HashSet<Groups_Tbl>();
    }
}

