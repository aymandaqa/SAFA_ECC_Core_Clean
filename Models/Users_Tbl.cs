using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAFA_ECC_Core_Clean.Models
{
    [Table("Users_Tbl")]
    public partial class Users_Tbl
    {
        [Key]
        public int User_ID { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; } = string.Empty;

        [StringLength(150)]
        public string? Password { get; set; }

        [StringLength(100)]
        public string? FullNameEN { get; set; }

        [StringLength(100)]
        public string? FullNameAR { get; set; }

        public int? Group_ID { get; set; }

        public int? Company_ID { get; set; }

        public int? Security_Level { get; set; }

        public bool? IsLockedOut { get; set; }

        public bool? IsDisabled { get; set; }

        public short? No_Of_Attempts { get; set; }

        public DateTime? Creation_Date { get; set; }

        [StringLength(50)]
        public string? Created_By { get; set; }

        public DateTime? LastLoginDate { get; set; }

        [StringLength(50)]
        public string? Last_Amend_By { get; set; }

        public DateTime? Last_Amend_Date { get; set; }

        public DateTime? Last_Password_Change_Date { get; set; }

        public DateTime? Next_Password_Change_Date { get; set; }

        public string? Action_History { get; set; }

        [StringLength(256)]
        public string? companyname { get; set; }

        [StringLength(512)]
        public string? groupname { get; set; }

        public bool? Is_Authorized { get; set; }

        public bool? LoginStatus { get; set; }

        public string? Email { get; set; }

        public bool IsActive { get; set; } // Added IsActive property
        public UserRole Role { get; set; } // Added Role property

        // Computed Properties for compatibility
        [NotMapped]
        public string Full_Name => !string.IsNullOrEmpty(FullNameAR) ? FullNameAR : FullNameEN ?? UserName;

        // Navigation Properties
        public virtual Companies_Tbl? Companies_Tbl { get; set; }
        public virtual Groups_Tbl? Groups_Tbl { get; set; }
        public virtual ICollection<Users_Permissions> UserPermissions { get; set; } = new List<Users_Permissions>();
        public virtual ICollection<AuthTrans_User_TBL_Auth> AuthorizedTransactions { get; set; } = new List<AuthTrans_User_TBL_Auth>();
    }

    public enum UserRole
    {
        Admin,
        User
    }
}


