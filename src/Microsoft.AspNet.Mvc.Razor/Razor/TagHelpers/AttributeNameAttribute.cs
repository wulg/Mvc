using System;

namespace Microsoft.AspNet.Mvc.Razor.TagHelpers
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class AttributeNameAttribute : Attribute
    {
	    public AttributeNameAttribute([NotNull] string attribute)
	    {
            Attribute = attribute;
	    }

        public string Attribute { get; private set; }
    }
}