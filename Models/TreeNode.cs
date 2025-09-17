using System.Collections.Generic;

namespace SAFA_ECC_Core_Clean.Models
{
    public class TreeNode
    {
        public string SubMenu_Name_EN { get; set; }
        public int SubMenu_ID { get; set; }
        public int Related_Page_ID { get; set; }
        public List<TreeNode> Children { get; set; }
    }
}

