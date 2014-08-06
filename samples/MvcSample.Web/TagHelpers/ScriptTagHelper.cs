using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc.Razor.TagHelpers;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Razor.TagHelpers;

namespace MvcSample.Web
{
    [ContentBehavior(ContentBehavior.None)]
    public class ScriptTagHelper : MvcTagHelper
    {
        public TagHelperLiteralExpression<string> Src { get; set; }

        public override Task ProcessAsync(TagBuilder builder, MvcTagHelperContext context)
        {
            if (Src.IsSet)
            {
                var src = Src.Build(context);

                // Check if there is a min version of this path on disk and if so render it instead
                builder.Attributes["src"] = src.Replace(".js", ".min.js");
            }

            return Task.FromResult(result: true);
        }
    }
}