using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc.Razor.TagHelpers;
using Microsoft.AspNet.Mvc.Rendering;

namespace MvcSample.Web
{
    [TagName("*")]
    public class CatchAllTagHelper : MvcTagHelper
    {
        public TagHelperModelExpression<string> Bar { get; set; }
        public TagHelperLiteralExpression<string> Action { get; set; }

        public override Task ProcessAsync(TagBuilder builder, MvcTagHelperContext context)
        {
            builder.Attributes["runat"] = "server"; // :trollface:

            return Task.FromResult(result: true);
        }
    }
}