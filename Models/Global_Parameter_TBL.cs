using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAFA_ECC_Core_Clean.Models
{
    [Table("Global_Parameter_TBL")]
    public class Global_Parameter_TBL
    {
        [Key]
        public int Id { get; set; }
        public string Parameter_Name { get; set; }
        public string Parameter_Value { get; set; }
    }
}
