using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAFA_ECC_Core_Clean.Models
{
    [Table("Cheque_Images_Tbl")]
    public partial class Cheque_Images_Tbl
    {
        [Key]
        public int Serial { get; set; }

        public byte[]? FrontImg { get; set; }

        public byte[]? RearImg { get; set; }

        public byte[]? UVImage { get; set; }

        public DateTime? LastUpdateDate { get; set; }

        // Computed Properties for compatibility
        [NotMapped]
        public int Image_ID => Serial;

        [NotMapped]
        public bool HasFrontImage => FrontImg != null && FrontImg.Length > 0;

        [NotMapped]
        public bool HasRearImage => RearImg != null && RearImg.Length > 0;

        [NotMapped]
        public bool HasUVImage => UVImage != null && UVImage.Length > 0;

        [NotMapped]
        public string FrontImageBase64 => FrontImg != null ? Convert.ToBase64String(FrontImg) : string.Empty;

        [NotMapped]
        public string RearImageBase64 => RearImg != null ? Convert.ToBase64String(RearImg) : string.Empty;

        [NotMapped]
        public string UVImageBase64 => UVImage != null ? Convert.ToBase64String(UVImage) : string.Empty;

        // Navigation Properties
        public virtual ICollection<Cheque_Images_Link_Tbl> ImageLinks { get; set; } = new List<Cheque_Images_Link_Tbl>();
    }
}

