using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAFA_ECC_Core_Clean.Models
{
    public class Category
    {
        [Key]
        public int SubMenu_ID { get; set; }
        public string SubMenu_Name_EN { get; set; }
        public int Related_Page_ID { get; set; }
        public int? Parent_ID { get; set; }
    }
}

