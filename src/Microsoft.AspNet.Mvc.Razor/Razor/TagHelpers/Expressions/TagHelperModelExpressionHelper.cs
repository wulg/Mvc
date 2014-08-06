using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using Microsoft.AspNet.Mvc.ModelBinding;
using Microsoft.AspNet.Mvc.Rendering.Expressions;
using Microsoft.AspNet.Razor.Generator.Compiler.CSharp;

namespace Microsoft.AspNet.Mvc.Razor.TagHelpers
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class TagHelperModelExpressionHelper<TModelType, TBuildType> : TagHelperModelExpression<TBuildType>
    {
        public TagHelperModelExpressionHelper()
            : base()
        {
        }

        public TagHelperModelExpressionHelper(Expression<Func<TModelType, TBuildType>> evaluator)
            : base(ExpressionHelper.GetExpressionText(evaluator))
        {
        }
    }
}