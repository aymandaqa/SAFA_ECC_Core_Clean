using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAFA_ECC_Core_Clean.Models
{
    [Table("Permissions_Tbl")]
    public class Permissions_Tbl
    {
        [Key]
        public int Permission_ID { get; set; }

        [Required]
        [StringLength(100)]
        public string Permission_Name { get; set; } = string.Empty;

        [StringLength(255)]
        public string? Description { get; set; }

        public bool Is_Active { get; set; }

        public DateTime Creation_Date { get; set; }

        [StringLength(50)]
        public string? Created_By { get; set; }

        public DateTime? Last_Amend_Date { get; set; }

        [StringLength(50)]
        public string? Last_Amend_By { get; set; }
    }
}


