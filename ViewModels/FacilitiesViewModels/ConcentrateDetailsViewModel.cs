using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SAFA_ECC_Core_Clean.ViewModels.SharedViewModels;

namespace SAFA_ECC_Core_Clean.ViewModels.FacilitiesViewModels
{
    public class ConcentrateDetailsViewModel
    {
        [Display(Name = "اسم التركيز")]
        public string? ConcentrateName { get; set; }

        [Display(Name = "كود التركيز")]
        public string? ConcentrateCode { get; set; }

        [Display(Name = "تاريخ الإنتاج")]
        [DataType(DataType.Date)]
        public DateTime? ProductionDate { get; set; }

        [Display(Name = "تاريخ انتهاء الصلاحية")]
        [DataType(DataType.Date)]
        public DateTime? ExpiryDate { get; set; }

        [Display(Name = "الوصف")]
        public string? Description { get; set; }

        [Display(Name = "الكمية")]
        public int Quantity { get; set; }

        [Display(Name = "الوحدة")]
        public string? Unit { get; set; }

        public List<ComponentItemViewModel>? Components { get; set; }
        public List<ChangeLogItemViewModel>? ChangeLog { get; set; }
    }

    public class ComponentItemViewModel
    {
        public int Id { get; set; }
        public string? ComponentName { get; set; }
        public int Quantity { get; set; }
        public string? Unit { get; set; }
    }
}
