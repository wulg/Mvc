using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNet.Razor.Generator;
using Microsoft.AspNet.Razor.Generator.Compiler.CSharp;
using Microsoft.AspNet.Razor.TagHelpers;

namespace Microsoft.AspNet.Mvc.Razor.TagHelpers
{
    public class TagHelperPrimitiveExpressionCodeGenerator : TagHelperAttributeCodeGenerator
    {
        private static readonly Dictionary<string, string> _castMappings = new Dictionary<string, string>()
        {
            { typeof(int).FullName, "int" },
            { typeof(long).FullName, "long" },
            { typeof(short).FullName, "short" }
        };

        private string _defaultValue;
        private string _castType;

        public TagHelperPrimitiveExpressionCodeGenerator(PropertyInfo propertyInfo)
            : base(propertyInfo)
        {
            var type = propertyInfo.PropertyType;

            if (type == typeof(string))
            {
                _defaultValue = string.Empty;
            }
            else if (type == typeof(object))
            {
                _defaultValue = "null";
            }
            else
            {
                _defaultValue = Activator.CreateInstance(type).ToString().ToLower();
            }

            var propertyName = propertyInfo.PropertyType.FullName;
            _castType = _castMappings.ContainsKey(propertyName) ? _castMappings[propertyName] : propertyName;
        }

        public override void GenerateCode(CSharpCodeWriter writer, CodeGeneratorContext context, Action renderAttributeValue)
        {
            writer.Write("(")
                  .Write(_castType)
                  .Write(")");

            if (renderAttributeValue != null)
            {
                GenerateValue(writer, renderAttributeValue);
            }
            else
            {
                // TODO: Remove scope
                GenerateValue(writer, renderAttributeValue: () =>
                {
                    writer.Write(_defaultValue);
                });
            }
        }
    }
}