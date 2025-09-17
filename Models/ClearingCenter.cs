using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAFA_ECC_Core_Clean.Models
{
    [Table("ClearingCenter")]
    public class ClearingCenter
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
