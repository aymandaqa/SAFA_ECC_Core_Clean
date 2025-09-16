using System;

namespace SAFA_ECC_Core_Clean.Models
{
    public class OnUs_Imgs
    {
        public decimal Serial { get; set; }
        public string? ChqSequance { get; set; }
        public byte[]? FrontImg { get; set; }
        public byte[]? RearImg { get; set; }
        public byte[]? UVImage { get; set; }
        public DateTime? TransDate { get; set; }
    }
}

