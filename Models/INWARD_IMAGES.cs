using System;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.Models
{
    public class INWARD_IMAGES
    {
        [Key]
        public int Id { get; set; }
        public string Serial { get; set; }
        public byte[] FrontImg { get; set; }
        public byte[] RearImg { get; set; }
        public byte[] UVImage { get; set; }
        public DateTime? InputDate { get; set; }
    }
}
