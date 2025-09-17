using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.InwardViewModels
{
    public class InwardFinanicalWFDetailsPMADIS_AuthViewModel
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        public string App { get; set; }
        [Required]
        public string Page { get; set; }
    }
}

