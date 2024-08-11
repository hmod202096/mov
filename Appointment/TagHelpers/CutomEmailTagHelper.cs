using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointment.TagHelpers
{
    public class CutomEmailTagHelper : TagHelper
    {
        public string myEmail { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";
            output.Attributes.SetAttribute("href", $"mailto:{myEmail}");
            output.Attributes.Add("id", "my-email-id");
            output.Content.SetContent("my-email");
        }
    }
}
