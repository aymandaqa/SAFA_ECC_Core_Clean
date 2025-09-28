using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.InwardViewModels
{
    public class IndextestViewModel
    {
        [Display(Name = "معرف الاختبار")]
        public int TestId { get; set; }

        [Display(Name = "اسم الاختبار")]
        public string? TestName { get; set; }

        [Display(Name = "تاريخ الاختبار")]
        [DataType(DataType.Date)]
        public DateTime TestDate { get; set; }

        [Display(Name = "النتيجة")]
        public string? Result { get; set; }

        public List<TestItemViewModel>? TestItems { get; set; }

        // Properties expected by the view
        public string? Message { get; set; }
        public string? InputField { get; set; }
        public List<TableDataItemViewModel>? TableData { get; set; }
    }

    public class TestItemViewModel
    {
        public int ItemId { get; set; }
        public string? ItemName { get; set; }
        public string? ItemValue { get; set; }
    }

    public class TableDataItemViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
