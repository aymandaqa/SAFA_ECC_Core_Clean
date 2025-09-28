using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.OutwardViewModels
{
    public class GetReturnedINHOUSESlipDetailsViewModel
    {
        [Display(Name = "رقم الإيصال")]
        public string? SlipNumber { get; set; }

        [Display(Name = "تاريخ الإرجاع")]
        [DataType(DataType.Date)]
        public DateTime? ReturnDate { get; set; }

        [Display(Name = "اسم العميل")]
        public string? CustomerName { get; set; }

        [Display(Name = "المبلغ الإجمالي")]
        public decimal TotalAmount { get; set; }

        public List<ReturnedSlipItemViewModel>? Items { get; set; }
    }

    public class ReturnedSlipItemViewModel
    {
        public string? ItemName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }
    }
}
