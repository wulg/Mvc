using System;
using Microsoft.AspNet.Razor.Generator;
using Microsoft.AspNet.Razor.Generator.Compiler;
using Microsoft.AspNet.Razor.Parser;

namespace Microsoft.AspNet.Mvc.Razor
{
    public interface IRazorCodeParserProvider
    {
	    ParserBase GetCodeParser(ParserBase incomingCodeParser, string baseType);
    }
}