using System;
using System.Reflection;
using Microsoft.AspNet.Razor.Generator;
using Microsoft.AspNet.Razor.Generator.Compiler.CSharp;
using Microsoft.AspNet.Razor.TagHelpers;

namespace Microsoft.AspNet.Mvc.Razor.TagHelpers
{
    public class TagHelperModelExpressionCodeGenerator : TagHelperAttributeCodeGenerator
    {
        private const string ModelLambdaName = "__modelName";

        public TagHelperModelExpressionCodeGenerator(PropertyInfo propertyInfo)
            : base(propertyInfo)
        {
        }

        public override void GenerateCode(CSharpCodeWriter writer, CodeGeneratorContext context, Action renderAttributeValue)
        {
            // TODO: What if someone overrides TagHelperModelExpression and there's no generic arguments?
            var buildType = PropertyInfo.PropertyType.GetGenericArguments()[0];
            writer.Write("new ")
                  .Write(GetNonGenericName(typeof(TagHelperModelExpressionHelper<,>).GetTypeInfo()))
                  .Write("<")
                  .Write((string)context.VisitorState[ModelChunkVisitor.MvcModelVisitorStateMember])
                  .Write(", ")
                  .Write(buildType.FullName)
                  .Write(">(");

            if (renderAttributeValue != null)
            {
                writer.Write(ModelLambdaName)
                      .Write(" => ")
                      .Write(ModelLambdaName)
                      .Write(".");

                // TODO: Project
                renderAttributeValue();

                writer.WriteEndMethodInvocation(endLine: false);

                GenerateIsSetProperty(writer, isSet: true);
            }
            else
            {
                writer.WriteEndMethodInvocation(endLine: false);
            }
        }
    }
}