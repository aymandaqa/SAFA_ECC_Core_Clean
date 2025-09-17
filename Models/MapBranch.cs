using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAFA_ECC_Core_Clean.Models
{
    [Table("MapBranches")]
    public class MapBranch
    {
        [Key]
        public int Id { get; set; } // Assuming a primary key is needed

        [StringLength(50)]
        public string? OwnerBrn { get; set; }

        [StringLength(50)]
        public string? Branch { get; set; }
    }
}

