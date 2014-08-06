using System;
using Microsoft.AspNet.Razor.TagHelpers;

namespace Microsoft.AspNet.Mvc.Razor.TagHelpers
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ContentBehaviorAttribute : Attribute
    {
	    public ContentBehaviorAttribute(ContentBehavior contentBehavior)
	    {
            ContentBehavior = contentBehavior;
	    }

        public ContentBehavior ContentBehavior { get; private set; }
    }
}