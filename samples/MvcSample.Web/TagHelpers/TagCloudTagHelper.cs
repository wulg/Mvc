using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Razor.TagHelpers;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Razor.TagHelpers;
using MvcSample.Web.Components;

namespace MvcSample.Web
{
    [TagName("tag-cloud")]
    [ContentBehavior(ContentBehavior.Replace)]
    public class TagCloudTagHelper : MvcTagHelper
    {
        private readonly IViewComponentHelper _viewComponenetHelper;

        public TagCloudTagHelper(IViewComponentHelper viewComponenetHelper)
        {
            _viewComponenetHelper = viewComponenetHelper;
        }

        public TagHelperLiteralExpression<int> Count { get; set; }

        public override async Task ProcessAsync(TagBuilder builder, MvcTagHelperContext context)
        {
            if (Count.IsSet)
            {
                builder.TagName = null;
                builder.InnerHtml = (await _viewComponenetHelper.InvokeAsync<TagCloud>(Count.Build(context), "Content")).ToString();
            }
        }
    }
}