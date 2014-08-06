using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Razor.TagHelpers;

namespace Microsoft.AspNet.Mvc.Razor.TagHelpers
{
    public abstract class MvcTagHelper : TagHelper
    {
        public abstract Task ProcessAsync(TagBuilder builder, MvcTagHelperContext context);
    }
}