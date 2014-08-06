using System;
using System.Collections.Generic;
using Microsoft.AspNet.Mvc.ModelBinding;
using Microsoft.AspNet.Razor.TagHelpers;

namespace Microsoft.AspNet.Mvc.Razor.TagHelpers
{
    public class MvcTagHelperContext : TagHelperContext
    {
        public MvcTagHelperContext(ViewContext viewContext, IModelMetadataProvider metadataProvider)
            : base()
        {
            ViewContext = viewContext;
            MetadataProvider = metadataProvider;
            AttributeSource = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        }

        public object Model
        {
            get
            {
                return ViewData.Model;
            }
        }

        public IDictionary<string, object> AttributeSource { get; private set; }
        public ViewContext ViewContext { get; set; }
        public ViewDataDictionary ViewData
        {
            get
            {
                return ViewContext.ViewData;
            }
        }
        public IModelMetadataProvider MetadataProvider { get; private set; }
    }
}