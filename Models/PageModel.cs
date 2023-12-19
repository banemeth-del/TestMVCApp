using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestMVCApp.Models
{
    public class PageModel
    {
        public string sfdtContent { get; set; }

        public string RestrictType { get; set;}
    }

    public class CustomRestrictParameter
    {
        public string passwordBase64 { get; set; }
        public string saltBase64 { get; set; }
        public int spinCount { get; set; }
    }
}