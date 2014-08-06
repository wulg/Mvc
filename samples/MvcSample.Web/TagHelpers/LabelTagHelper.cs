using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc.Razor.TagHelpers;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Mvc.Rendering.Expressions;
using Microsoft.AspNet.Razor.TagHelpers;

namespace MvcSample.Web
{
    [ContentBehavior(ContentBehavior.Replace)]
    public class LabelTagHelper : MvcTagHelper
    {
        public TagHelperModelExpression<string> For { get; set; }
        public TagHelperRazorExpression Body { get; set; }

        public override Task ProcessAsync(TagBuilder builder, MvcTagHelperContext context)
        {
            if (For.IsSet)
            {
                var metadata = ExpressionMetadataProvider.FromStringExpression(For.Expression, context.ViewContext.ViewData, context.MetadataProvider);
                var resolvedDisplayName = metadata.PropertyName ??
                    (string.IsNullOrEmpty(For.Expression)
                        ? string.Empty
                        : For.Expression.Split('.').Last());

                builder.Attributes["for"] = For.Expression;
                builder.InnerHtml = resolvedDisplayName;
            }

            if(Body.IsSet)
            {
                builder.InnerHtml = Body.Build(context);
            }

            return Task.FromResult(result: true);
        }
    }
}