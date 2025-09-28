using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAFA_ECC_Core_Clean.ViewModels.InwardViewModels
{
    public class PMA_DataViewModel
    {
        public int TotalFiles { get; set; }
        public int TodayFiles { get; set; }
        public int PendingFiles { get; set; }
        public int RejectedFiles { get; set; }
        public List<PMA_File_History> Files { get; set; }
    }

    public class PMA_File_History
    {
        public int Serial { get; set; }
        public string File_Name { get; set; }
        public string File_Type { get; set; }
        public DateTime Upload_Date { get; set; }
        public long File_Size { get; set; }
        public int? Records_Count { get; set; }
        public string Status { get; set; }
        public string Uploaded_By { get; set; }
    }
}
