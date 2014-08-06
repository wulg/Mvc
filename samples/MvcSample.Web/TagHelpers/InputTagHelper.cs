using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc.Razor.TagHelpers;
using Microsoft.AspNet.Mvc.Rendering;

namespace MvcSample.Web
{
    public class InputTagHelper : MvcTagHelper
    {
        public TagHelperModelExpression<string> For { get; set; }
        [AttributeName("repeat-model")]
        public TagHelperModelExpression<int> RepeatModel { get; set; }

        public override Task ProcessAsync(TagBuilder builder, MvcTagHelperContext context)
        {
            if (For.IsSet)
            {
                builder.Attributes["type"] = "text";
                builder.Attributes["placeholder"] = "Enter in text...";
                builder.Attributes["value"] = For.Build(context);
                builder.Attributes["id"] = For.Expression;
                builder.Attributes["name"] = For.Expression;
            }

            if(RepeatModel.IsSet)
            {
                var forVal = For.Build(context);
                string val = string.Empty;

                for(var i = 0; i < RepeatModel.Build(context); i++)
                {
                    val += forVal;
                }

                builder.Attributes["value"] = val;
            }

            return Task.FromResult(result: true);
        }
    }
}