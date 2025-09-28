using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SAFA_ECC_Core_Clean.ViewModels.PDCViewModels
{
    public class BankBranchViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم الفرع مطلوب")]
        [Display(Name = "اسم الفرع")]
        public string? BranchName { get; set; }

        [Required(ErrorMessage = "البنك مطلوب")]
        [Display(Name = "البنك")]
        public int BankId { get; set; }

        public IEnumerable<SelectListItem>? Banks { get; set; }

        public List<BankBranchItemViewModel>? BankBranches { get; set; }
    }

    public class BankBranchItemViewModel
    {
        public int Id { get; set; }
        public string? BranchName { get; set; }
        public string? BankName { get; set; }
    }
}
