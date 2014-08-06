using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Razor.TagHelpers;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Razor.TagHelpers;

namespace MvcSample.Web
{
    [ContentBehavior(ContentBehavior.Append)]
    public class FormTagHelper : MvcTagHelper
    {
        private IUrlHelper _urlHelper;

        public FormTagHelper(IUrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        public TagHelperLiteralExpression<string> Action { get; set; }
        public TagHelperLiteralExpression<string> Controller { get; set; }

        public override Task ProcessAsync(TagBuilder builder, MvcTagHelperContext context)
        {
            if (Action.IsSet && Controller.IsSet)
            {
                builder.Attributes["action"] = _urlHelper.Action(Action.Build(context), Controller.Build(context));
                builder.Attributes["method"] = "post";
                builder.InnerHtml = "<input type=\"hidden\" value=\"something\" />";
            }

            return Task.FromResult(result: true);
        }
    }
}