using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAFA_ECC_Core_Clean.Models
{
    public class Discount_Commisions
    {
        [Key]
        public int Id { get; set; }
        public string BranchCode { get; set; }
        public string T24CommisionTypeId { get; set; }
        public int NumberOfCheques { get; set; }
        public DateTime CreatedAt { get; set; }
        public string History { get; set; }
    }
}
