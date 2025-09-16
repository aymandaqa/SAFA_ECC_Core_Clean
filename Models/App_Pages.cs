using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAFA_ECC_Core_Clean.Models
{
    [Table("App_Pages")]
    public class App_Pages
    {
        [Key]
        public int Page_ID { get; set; }

        [Required]
        [StringLength(100)]
        public string Page_Name_EN { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Page_Name_AR { get; set; }

        [StringLength(200)]
        public string? Page_Description { get; set; }

        [StringLength(100)]
        public string? Controller_Name { get; set; }

        [StringLength(100)]
        public string? Action_Name { get; set; }

        public int? Application_ID { get; set; }
        public bool? Is_Active { get; set; }
        public int? Parent_Page_ID { get; set; }
        public int? Sort_Order { get; set; }
        public DateTime? Creation_Date { get; set; }
        public string? Created_By { get; set; }
        public DateTime? Last_Update { get; set; }
        public string? Updated_By { get; set; }
    }
}

