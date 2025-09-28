using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.PDCViewModels
{
    public class AuthrizationCodeViewModel
    {
        [Display(Name = "رمز التفويض")]
        public string? AuthorizationCode { get; set; }

        [Display(Name = "الوصف")]
        public string? Description { get; set; }

        [Display(Name = "الرمز")]
        public string? Code { get; set; }

        [Display(Name = "نشط")]
        public bool IsActive { get; set; }

        public List<AuthrizationCodeItemViewModel>? AuthrizationCodes { get; set; }
    }

    public class AuthrizationCodeItemViewModel
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}
