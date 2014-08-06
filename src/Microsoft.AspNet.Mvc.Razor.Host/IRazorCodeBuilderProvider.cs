using System;
using Microsoft.AspNet.Razor.Generator;
using Microsoft.AspNet.Razor.Generator.Compiler;

namespace Microsoft.AspNet.Mvc.Razor
{
    public interface IRazorCodeBuilderProvider
    {
	    CodeBuilder GetCodeBuilder(CodeBuilder incomingCodeBuilder, 
                                   MvcRazorHostOptions options, 
                                   CodeGeneratorContext context);
    }
}