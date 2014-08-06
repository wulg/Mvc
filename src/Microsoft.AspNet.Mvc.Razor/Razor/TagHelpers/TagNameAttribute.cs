using System;

namespace Microsoft.AspNet.Mvc.Razor.TagHelpers
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class TagNameAttribute : Attribute
    {
        public TagNameAttribute([NotNull] params string[] tags)
        {
            Tags = tags;
        }

        public string[] Tags { get; private set; }
    }
}