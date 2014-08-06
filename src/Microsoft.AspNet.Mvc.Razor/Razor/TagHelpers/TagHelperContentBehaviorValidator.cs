using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Razor.TagHelpers;

namespace Microsoft.AspNet.Mvc.Razor.TagHelpers
{
    public static class TagHelperContentBehaviorValidator
    {
        public static void Validate(IEnumerable<MvcTagHelperDescriptor> descriptors)
        {
            var behaviors = descriptors.Where(desc => desc.ContentBehavior != ContentBehavior.None);

            if (!behaviors.Any())
            {
                return;
            }

            var baseline = behaviors.First();

            if (!behaviors.Where(desc => desc.ContentBehavior == baseline.ContentBehavior)
                          .SequenceEqual(behaviors))
            {
                // TODO: Make resource
                throw new InvalidOperationException(
                    string.Format(
                        "TagHelpers that target '{0}' must all have ContentBehavior 'None' or identical ContentBehavior's.",
                        baseline.TagName.ToString()));
            }
        }
    }
}