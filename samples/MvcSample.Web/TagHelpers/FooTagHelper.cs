using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc.Razor.TagHelpers;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Razor.TagHelpers;

namespace MvcSample.Web
{
    [ContentBehavior(ContentBehavior.Replace)]
    public class FooTagHelper : MvcTagHelper
    {
        public TagHelperModelExpression<string> Bar { get; set; }

        public override Task ProcessAsync(TagBuilder builder, MvcTagHelperContext context)
        {
            builder.TagName = "h1";
            builder.InnerHtml = Bar.IsSet ? Bar.Build(context) : "Bar not set";

            return Task.FromResult(result: true);
        }
    }
}