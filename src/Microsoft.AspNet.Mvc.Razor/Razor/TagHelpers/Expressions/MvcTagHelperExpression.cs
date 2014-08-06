using System;
using Microsoft.AspNet.Razor.TagHelpers;

namespace Microsoft.AspNet.Mvc.Razor.TagHelpers
{
    public abstract class MvcTagHelperExpression<TBuildType> : TagHelperExpression<TBuildType>
    {
        public MvcTagHelperExpression()
            : base()
        {
        }

        public override TBuildType Build(TagHelperContext context)
        {
            return Build(context as MvcTagHelperContext);
        }

        public abstract TBuildType Build(MvcTagHelperContext context);
    }
}