using System;

namespace SAFA_ECC_Core_Clean.Models
{
    public class FT_ResponseClass
    {
        public FT_Response FT_Res { get; set; }
    }

    public class FT_Response
    {
        public string ResponseStatus { get; set; }
        public string ResponseDescription { get; set; }
        public string ErrorMessage { get; set; }
    }
}

