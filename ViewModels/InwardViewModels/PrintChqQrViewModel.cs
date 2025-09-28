using System;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.InwardViewModels
{
    public class PrintChqQrViewModel
    {
        [Display(Name = "رقم الشيك")]
        public string? ChequeNumber { get; set; }

        [Display(Name = "اسم البنك")]
        public string? BankName { get; set; }

        [Display(Name = "المبلغ")]
        public decimal Amount { get; set; }

        [Display(Name = "تاريخ الشيك")]
        [DataType(DataType.Date)]
        public DateTime ChequeDate { get; set; }

        [Display(Name = "رمز QR")]
        public string? QrCodeImageBase64 { get; set; }
    }
}
