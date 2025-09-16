using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAFA_ECC_Core_Clean.Models
{
    [Table("Banks_Tbl")]
    public partial class Banks_Tbl
    {
        [Key]
        [StringLength(50)]
        public string Bank_Id { get; set; } = string.Empty;

        [StringLength(50)]
        public string? Category { get; set; }

        [StringLength(50)]
        public string? Clearing_Center { get; set; }

        [StringLength(50)]
        public string? BIC_Code { get; set; }

        [StringLength(100)]
        public string? Bank_Name { get; set; }

        [StringLength(50)]
        public string? Bank_Region { get; set; }

        [StringLength(50)]
        public string? BIC_Code_Test { get; set; }

        public bool? Is_Merged { get; set; }

        // Computed Properties for compatibility
        [NotMapped]
        public int Bank_ID => int.TryParse(Bank_Id, out int id) ? id : 0;

        [NotMapped]
        public string Bank_Code => Bank_Id;

        [NotMapped]
        public bool Is_Active => !Is_Merged ?? true;

        // Navigation Properties
        public virtual ICollection<Inward_Trans> InwardTransactions { get; set; } = new List<Inward_Trans>();
        public virtual ICollection<Outward_Trans> OutwardTransactions { get; set; } = new List<Outward_Trans>();
        public virtual ICollection<Post_Dated_Trans> PostDatedTransactions { get; set; } = new List<Post_Dated_Trans>();
        public virtual ICollection<Bank_Branches_Tbl> Branches { get; set; } = new List<Bank_Branches_Tbl>();
    }
}

