using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.OutwardViewModels
{
    public class RepresentReturnDisViewModel
    {
        public SearchCriteriaViewModel SearchCriteria { get; set; } = new SearchCriteriaViewModel();
        public List<ResultItemViewModel> Results { get; set; } = new List<ResultItemViewModel>();
    }

    public class SearchCriteriaViewModel
    {
        [Display(Name = "حقل البحث 1")]
        public string? Field1 { get; set; }

        [Display(Name = "حقل البحث 2")]
        public string? Field2 { get; set; }
    }

    public class ResultItemViewModel
    {
        public int Id { get; set; }
        public string? Column1 { get; set; }
        public string? Column2 { get; set; }
        public string? Column3 { get; set; }
    }
}
