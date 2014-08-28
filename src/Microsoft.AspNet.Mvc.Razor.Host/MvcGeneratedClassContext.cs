using System;
using Microsoft.AspNet.Razor;
using Microsoft.AspNet.Razor.Generator;

namespace Microsoft.AspNet.Mvc.Razor
{
    public class MvcGeneratedClassContext : GeneratedClassContext
    {
	    public MvcGeneratedClassContext()
            : base(
                new GeneratedTagHelperRenderingContext(
                    tagHelperRendererName: "__tagHelperRenderer",
                    tagHelperViewContextAccessor: "ViewContext",
                    tagHelperRendererAddAttributeBuilderName: "AddAttributeBuilder",
                    tagHelperRendererAddBufferedAttributeBuilderName: "AddBufferedAttributeBuilder",
                    tagHelperRendererAddAttributeName: "AddAttribute",
                    tagHelperRendererTagBuilderName: "TagBodyWriter",
                    tagHelperRendererPrepareMethodName: "PrepareTagHelper",
                    tagHelperRendererStartMethodName: "StartTagHelper",
                    tagHelperRendererEndMethodName: "EndTagHelper",
                    tagHelperRendererBodyBufferName: "TagBodyBuffer",
                    tagHelperUseWriterName: "ViewContext.UseWriter",
                    tagHelperNextWriterName: "ViewContext.NextWriter"),
                executeMethodName: "ExecuteAsync",
                writeMethodName: "Write",
                writeLiteralMethodName: "WriteLiteral",
                writeToMethodName: "WriteTo",
                writeLiteralToMethodName: "WriteLiteralTo",
                templateTypeName: "Microsoft.AspNet.Mvc.Razor.HelperResult",
                defineSectionMethodName: "DefineSection")
	    {
            ResolveUrlMethodName = "Href";
        }
    }
}