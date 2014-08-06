// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNet.Razor.Generator;
using Microsoft.AspNet.Razor.Generator.Compiler;
using Microsoft.AspNet.Razor.Generator.Compiler.CSharp;

namespace Microsoft.AspNet.Mvc.Razor
{
    public abstract class MvcCodeVisitor : MvcCSharpChunkVisitor
    {
        public MvcCodeVisitor([NotNull] CSharpCodeWriter writer,
                                    [NotNull] CodeGeneratorContext context)
            : base(writer, context)
        {
        }

        protected override void Visit(InjectChunk chunk)
        {
        }
        protected override void Visit(ModelChunk chunk)
        {
        }
    }
}