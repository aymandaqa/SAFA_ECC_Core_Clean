using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAFA_ECC_Core_Clean.Models
{
    [Table("Cheque_XML_Templete")]
    public class Cheque_XML_Templete
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        public string? XML_Cheque_Type { get; set; }

        public string? XML_Templete { get; set; }
    }
}

