using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace SAFA_ECC_Core_Clean.ViewModels.FilesViewModels
{
    public class ImportExportViewModel
    {
        [Display(Name = "نوع العملية")]
        public string? OperationType { get; set; } // "Import" or "Export"

        [Display(Name = "نوع الملف")]
        public string? FileType { get; set; } // e.g., "Excel", "CSV", "PDF"

        [Display(Name = "ملف للإستيراد")]
        public IFormFile? ImportFile { get; set; }

        [Display(Name = "اسم الملف للتصدير")]
        public string? ExportFileName { get; set; }

        [Display(Name = "حالة العملية")]
        public string? StatusMessage { get; set; }
    }
}
