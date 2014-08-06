using System;
using System.Diagnostics;
using System.Reflection;
using Microsoft.AspNet.Razor.TagHelpers;

namespace Microsoft.AspNet.Mvc.Razor.TagHelpers
{
    public static class TagHelperConventions
    {
        public static readonly string TagHelperNameEnding = "TagHelper";
        public static readonly string TagHelperAttributeCodeGeneratorName = "CreateCodeGenerator";

        public static bool AcceptsRazorCode([NotNull] PropertyInfo propertyInfo)
        {
            return typeof(TagHelperRazorExpression).IsAssignableFrom(propertyInfo.PropertyType);
        }

        public static bool IsTagHelper([NotNull] TypeInfo typeInfo)
        {
            if (!typeInfo.IsClass ||
                typeInfo.IsAbstract ||
                typeInfo.ContainsGenericParameters)
            {
                return false;
            }

            return typeof(MvcTagHelper).GetTypeInfo().IsAssignableFrom(typeInfo);
        }
    }
}