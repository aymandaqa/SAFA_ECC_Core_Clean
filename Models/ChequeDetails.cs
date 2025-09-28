using System;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.Models
{
    public class ChequeDetails
    {
        public int Id { get; set; }

        [Display(Name = "رقم الشيك")]
        public string? ChequeNumber { get; set; }

        [Display(Name = "تاريخ الشيك")]
        [DataType(DataType.Date)]
        public DateTime ChequeDate { get; set; }

        [Display(Name = "المبلغ")]
        public decimal Amount { get; set; }

        [Display(Name = "اسم المستفيد")]
        public string? PayeeName { get; set; }

        [Display(Name = "اسم البنك")]
        public string? BankName { get; set; }

        [Display(Name = "رقم الحساب")]
        public string? AccountNumber { get; set; }

        [Display(Name = "الحالة")]
        public string? Status { get; set; }

        [Display(Name = "صورة الشيك الأمامية")]
        public string? FrontImageBase64 { get; set; }

        [Display(Name = "صورة الشيك الخلفية")]
        public string? RearImageBase64 { get; set; }
    }
}
