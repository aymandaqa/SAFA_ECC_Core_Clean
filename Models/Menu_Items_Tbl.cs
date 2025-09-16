using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAFA_ECC_Core_Clean.Models
{
    [Table("Menu_Items_Tbl")]
    public class Menu_Items_Tbl
    {
        [Key]
        public int SubMenu_ID { get; set; }

        [Required]
        [StringLength(100)]
        public string SubMenu_Name_EN { get; set; } = string.Empty;

        [StringLength(100)]
        public string? SubMenu_Name_AR { get; set; }

        [StringLength(200)]
        public string? SubMenu_Description { get; set; }

        public int? Parent_SubMenu_ID { get; set; }

        public int? Related_Page_ID { get; set; }

        [StringLength(200)]
        public string? SubMenu_URL { get; set; }

        [StringLength(50)]
        public string? SubMenu_Icon { get; set; }

        public int? Sort_Order { get; set; }

        public bool? Is_Active { get; set; }

        public bool? Is_Visible { get; set; }

        public int? Application_ID { get; set; }

        public DateTime? Creation_Date { get; set; }

        [StringLength(50)]
        public string? Created_By { get; set; }

        public DateTime? Last_Update { get; set; }

        [StringLength(50)]
        public string? Updated_By { get; set; }

        // Navigation Properties
        [ForeignKey("Parent_SubMenu_ID")]
        public virtual Menu_Items_Tbl? ParentMenu { get; set; }

        [ForeignKey("Related_Page_ID")]
        public virtual App_Pages? RelatedPage { get; set; }

        public virtual ICollection<Menu_Items_Tbl> ChildMenus { get; set; } = new List<Menu_Items_Tbl>();
    }
}

