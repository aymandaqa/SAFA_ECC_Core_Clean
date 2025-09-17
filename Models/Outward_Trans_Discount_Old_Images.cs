using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAFA_ECC_Core_Clean.Models
{
    [Table("Outward_Trans_Discount_Old_Images")]
    public partial class Outward_Trans_Discount_Old_Images
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal Serial { get; set; }

        public byte[]? FrontImg { get; set; }

        public byte[]? RearImg { get; set; }

        public byte[]? UVImg { get; set; }
    }
}

