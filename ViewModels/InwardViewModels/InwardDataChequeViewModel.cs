using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.InwardViewModels
{
    public class InwardDataChequeViewModel
    {
        public InwardChequeSearchViewModel SearchModel { get; set; } = new InwardChequeSearchViewModel();
        public List<InwardChequeItemViewModel> Cheques { get; set; } = new List<InwardChequeItemViewModel>();
    }

    public class InwardChequeSearchViewModel
    {
        [Display(Name = "رقم الشيك")]
        public string? ChequeNumber { get; set; }

        [Display(Name = "اسم البنك")]
        public string? BankName { get; set; }

        [Display(Name = "من تاريخ")]
        [DataType(DataType.Date)]
        public DateTime? FromDate { get; set; }

        [Display(Name = "إلى تاريخ")]
        [DataType(DataType.Date)]
        public DateTime? ToDate { get; set; }

        [Display(Name = "الحالة")]
        public string? Status { get; set; }
    }

    public class InwardChequeItemViewModel
    {
        public int Id { get; set; }
        public string? ChequeNumber { get; set; }
        public string? BankName { get; set; }
        public decimal Amount { get; set; }
        public DateTime DueDate { get; set; }
        public string? Status { get; set; }
    }
}
