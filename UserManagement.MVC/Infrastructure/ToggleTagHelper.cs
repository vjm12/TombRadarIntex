using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.MVC.Infrastructure
{
    public class ToggleTagHelper
    {
        public string DivClass { get; set; }
        public bool DivClassEnabled { get; set; }
        public string DivClassNormal { get; set; }
        public string DivClassSelected { get; set; }



        // Add more target elements here on a new line if you want to target more than just div.  Example: [HtmlTargetElement("a")] to hide/show links
        [HtmlTargetElement("div")]
        public class VisibilityTagHelper : TagHelper
        {
            // default to true otherwise all existing target elements will not be shown, because bool's default to false
            public bool IsVisible { get; set; } = true;

            // You only need one of these Process methods, but just showing the sync and async versions
            public override void Process(TagHelperContext context, TagHelperOutput output)
            {
                if (!IsVisible)
                    output.SuppressOutput();

                base.Process(context, output);
            }

            public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
            {
                if (!IsVisible)
                    output.SuppressOutput();

                return base.ProcessAsync(context, output);
            }
        }
    }
}
