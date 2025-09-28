using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.InwardViewModels
{
    public class ResendINWViewModel
    {
        [Required(ErrorMessage = "رقم الشيك مطلوب")]
        [Display(Name = "رقم الشيك")]
        public string? ChequeNumber { get; set; }

        [Required(ErrorMessage = "تاريخ الإرسال مطلوب")]
        [Display(Name = "تاريخ الإرسال")]
        [DataType(DataType.Date)]
        public DateTime? ResendDate { get; set; }

        public string? Message { get; set; }
    }
}
