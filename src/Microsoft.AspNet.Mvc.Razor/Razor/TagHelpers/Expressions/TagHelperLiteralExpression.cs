namespace Microsoft.AspNet.Mvc.Razor.TagHelpers
{
    public class TagHelperLiteralExpression<TBuildType> : MvcTagHelperExpression<TBuildType>
    {
        public TagHelperLiteralExpression()
            : base()
        {
        }

        public TagHelperLiteralExpression(TBuildType expression)
        {
            Expression = expression;
        }

        public TBuildType Expression { get; private set; }

        public override TBuildType Build(MvcTagHelperContext context)
        {
            return Expression;
        }
    }
}