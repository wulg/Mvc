using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNet.Razor.TagHelpers;
using Microsoft.Framework.Runtime;

namespace Microsoft.AspNet.Mvc.Razor.TagHelpers
{
    public class DefaultTagHelperDescriptorResolver : ITagHelperDescriptorResolver
    {
        private const ContentBehavior DefaultContentBehavior = ContentBehavior.None;

        private ILibraryManager _libraryManager;

        public DefaultTagHelperDescriptorResolver(ILibraryManager libraryManager)
        {
            _libraryManager = libraryManager;
        }

        public IEnumerable<TagHelperDescriptor> Resolve(string registration)
        {
            var data = registration.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string assembly = registration;
            string tagHelperType = string.Empty;

            // Specific type specified
            if (data.Length > 1)
            {
                assembly = data.First().Trim();
                tagHelperType = data[1].Trim();
            }

            var lib = Assembly.Load(new AssemblyName(_libraryManager.GetLibraryInformation(assembly).Name));
            var types = lib.DefinedTypes;
            IEnumerable<MvcTagHelperDescriptor> descriptors = null;

            if (string.IsNullOrEmpty(tagHelperType))
            {
                descriptors = types.Where(TagHelperConventions.IsTagHelper)
                                   .SelectMany(GetTagHelperDescriptors);
            }
            else
            {
                // First check for a full name match.
                descriptors = lib.DefinedTypes.Where(dt =>
                                        dt.FullName == tagHelperType &&
                                        TagHelperConventions.IsTagHelper(dt))
                                 .SelectMany(GetTagHelperDescriptors);

                // If there's no full name match, fall back and look for a "name" match.
                if (!descriptors.Any())
                {
                    descriptors = lib.DefinedTypes.Where(dt =>
                                           dt.Name == tagHelperType &&
                                           TagHelperConventions.IsTagHelper(dt))
                                     .SelectMany(GetTagHelperDescriptors);
                }
            }

            return descriptors;
        }

        private IEnumerable<MvcTagHelperDescriptor> GetTagHelperDescriptors(TypeInfo type)
        {
            var targets = GetTagHelperTargets(type);
            var tagHelperName = type.FullName;
            var tagHelperAttributes = GetTagHelperAttributes(type);
            var tagHelperContentBehavior = GetContentBehavior(type);

            // TODO: Tag name may be incorrect
            var descriptors = targets.Select(tagName =>
                                new MvcTagHelperDescriptor(tagName,
                                                           tagHelperName,
                                                           tagHelperContentBehavior,
                                                           tagHelperAttributes));

            TagHelperContentBehaviorValidator.Validate(descriptors);

            return descriptors;
        }

        private IEnumerable<string> GetTagHelperTargets(TypeInfo type)
        {
            var attributes = type.GetCustomAttributes<TagNameAttribute>(inherit: true);

            // If there aren't any attributes specifying the targets derive it form the name
            if (!attributes.Any())
            {
                var name = type.Name;

                if (name.EndsWith(TagHelperConventions.TagHelperNameEnding))
                {
                    name = name.Substring(0, name.Length - TagHelperConventions.TagHelperNameEnding.Length);
                }

                // TODO: HTMLify this name
                return new[] { name };
            }

            return attributes.SelectMany(attribute => attribute.Tags);
        }

        private IEnumerable<TagHelperAttributeInfo> GetTagHelperAttributes(TypeInfo type)
        {
            var properties = type.DeclaredProperties.Where(property => property.GetGetMethod() != null && property.GetSetMethod() != null);

            // TODO: Validate there's not conflicting properties?
            return properties.Select(info => new TagHelperAttributeInfo(GetTagHelperAttributeName(info),
                                                                        info.Name,
                                                                        TagHelperConventions.AcceptsRazorCode(info),
                                                                        GetAttributeCodeGenerator(info)));
        }

        private string GetTagHelperAttributeName(PropertyInfo info)
        {
            var attributeName = info.GetCustomAttribute<AttributeNameAttribute>();

            return attributeName != null ? attributeName.Attribute : info.Name;
        }

        private ContentBehavior GetContentBehavior(TypeInfo type)
        {
            var attribute = type.GetCustomAttribute<ContentBehaviorAttribute>(inherit: true);

            return attribute != null ? attribute.ContentBehavior : DefaultContentBehavior;
        }

        private TagHelperAttributeCodeGenerator GetAttributeCodeGenerator(PropertyInfo propertyInfo)
        {
            var type = propertyInfo.PropertyType;

            // TODO: Does GetRuntimeMethods return extension methods?  If so should we allow specifying the code generators?
            if (type.GetTypeInfo().IsPrimitive || IsPrimitiveValueType(type))
            {
                return new TagHelperPrimitiveExpressionCodeGenerator(propertyInfo);
            }
            else
            {
                // TODO: Should we be more restrictive?
                var codeGeneratorInvoker = type.GetRuntimeMethods().SingleOrDefault(methodInfo =>
                    methodInfo.IsStatic &&
                    methodInfo.Name == TagHelperConventions.TagHelperAttributeCodeGeneratorName);

                if (codeGeneratorInvoker == null)
                {
                    return new TagHelperAttributeCodeGenerator(propertyInfo);
                }

                // Can pass null because the method is static
                var codeGenerator = (TagHelperAttributeCodeGenerator)codeGeneratorInvoker.Invoke(obj: null, parameters: new[] { propertyInfo });

                return codeGenerator;
            }
        }

        private static bool IsPrimitiveValueType(Type type)
        {
            return type == typeof(decimal) || type == typeof(object) || type == typeof(string);
        }
    }
}