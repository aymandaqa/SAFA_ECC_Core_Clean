using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.ChequeViewerViewModels
{
    public class ChequeViewerIndexViewModel
    {
        public ChequeSearchViewModel SearchModel { get; set; } = new ChequeSearchViewModel();
        public List<BankViewModel> Banks { get; set; } = new List<BankViewModel>();
        public List<ChequeDetailsViewModel> Cheques { get; set; } = new List<ChequeDetailsViewModel>();
    }

    public class ChequeSearchViewModel
    {
        [Display(Name = "رقم الشيك")]
        public string? ChequeNumber { get; set; }

        [Display(Name = "اسم الساحب")]
        public string? DrawerName { get; set; }

        [Display(Name = "من تاريخ")]
        [DataType(DataType.Date)]
        public DateTime? FromDate { get; set; }

        [Display(Name = "إلى تاريخ")]
        [DataType(DataType.Date)]
        public DateTime? ToDate { get; set; }

        [Display(Name = "معرف البنك")]
        public string? BankId { get; set; }

        [Display(Name = "المبلغ من")]
        public decimal? AmountFrom { get; set; }

        [Display(Name = "المبلغ إلى")]
        public decimal? AmountTo { get; set; }

        [Display(Name = "الحالة")]
        public string? Status { get; set; }
    }

    public class BankViewModel
    {
        public string? Bank_ID { get; set; }
        public string? Bank_Name { get; set; }
    }

    public class ChequeDetailsViewModel
    {
        public int Id { get; set; }
        public string? ChequeNumber { get; set; }
        public string? BeneficiaryName { get; set; }
        public decimal Amount { get; set; }
        public DateTime? ChequeDate { get; set; }
        public string? BankName { get; set; }
        public string? Status { get; set; }
        public List<ChequeImageViewModel>? ChequeImages { get; set; }
    }

    public class ChequeImageViewModel
    {
        public string? ImagePath { get; set; }
    }
}
