using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc.Razor.TagHelpers;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Razor.TagHelpers;

namespace MvcSample.Web
{
    [TagName("a")]
    public class ButtonTagHelper : MvcTagHelper
    {
        public override Task ProcessAsync(TagBuilder builder, MvcTagHelperContext context)
        {
            bool modify = false;

            if (builder.TagName == "a")
            {
                string cls;

                builder.Attributes.TryGetValue("class", out cls);

                if(cls != null && cls.Contains("btn"))
                {
                    modify = true;
                }
            }
            else
            {
                modify = true;
            }

            if(modify)
            {
                if (builder.Attributes.ContainsKey("style"))
                {
                    builder.Attributes["style"] += ";font-size:2em;";
                }
                else
                {
                    builder.Attributes.Add("style", "font-size:2em;");
                }
            }

            return Task.FromResult(result: true);
        }
    }
}