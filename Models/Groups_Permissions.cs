using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAFA_ECC_Core_Clean.Models
{
    [Table("Groups_Permissions")]
    public class Groups_Permissions
    {
        [Key]
        public int Id { get; set; } // Assuming a primary key exists
        public int Application_ID { get; set; }
        public int Group_Id { get; set; }
        public int Page_Id { get; set; }
        public bool Value { get; set; } // Assuming there's a 'Value' property based on Users_Permissions
    }
}
