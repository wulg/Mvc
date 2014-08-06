using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Razor.TagHelpers;

namespace Microsoft.AspNet.Mvc.Razor.TagHelpers
{
    public class TagHelperRenderingContext
    {
	    public TagHelperRenderingContext(TagBuilder tagBuilder, ContentBehavior contentBehavior)
	    {
            TagBuilder = tagBuilder;
            ContentBehavior = contentBehavior;
            TagHelpers = new List<MvcTagHelper>();

            if(ContentBehavior == ContentBehavior.Modify)
            {
                BodyBuffer = new TagBuilderStringWriter(TagBuilder);
            }
	    }

        public TagBuilder TagBuilder { get; private set; }
        public List<MvcTagHelper> TagHelpers { get; private set; }
        public ContentBehavior ContentBehavior { get; private set; }
        public TextWriter BodyBuffer { get; set; }
    }
}