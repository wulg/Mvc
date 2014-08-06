using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc.Razor.TagHelpers;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Razor.TagHelpers;

namespace MvcSample.Web
{
    [ContentBehavior(ContentBehavior.Modify)]
    public class DivTagHelper : MvcTagHelper
    {
        public override Task ProcessAsync(TagBuilder builder, MvcTagHelperContext context)
        {
            builder.TagName = "em";

            builder.InnerHtml = builder.InnerHtml.Replace("HELLO", "HI, I WAS REPLACED");

            return Task.FromResult(result: true);
        }
    }
}