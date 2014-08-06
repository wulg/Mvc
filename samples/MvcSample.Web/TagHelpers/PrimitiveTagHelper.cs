using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc.Razor.TagHelpers;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Razor.TagHelpers;

namespace MvcSample.Web
{
    [ContentBehavior(ContentBehavior.Replace)]
    public class PrimitiveTagHelper : MvcTagHelper
    {
        public bool Boolval { get; set; }
        public byte Byteval { get; set; }
        public char Charval { get; set; }
        public decimal Decimalval { get; set; }
        public double Doubleval { get; set; }
        // TODO: Validate enums work?  Do we care?
        public float Floatval { get; set; }
        public int Intval { get; set; }
        public long Longval { get; set; }
        public sbyte Sbyteval { get; set; }
        public short Shortval { get; set; }
        // TODO: Validate struct works?  Do we care?
        public uint Uintval { get; set; }
        public ulong Ulongval { get; set; }
        public ushort Ushortval { get; set; }
        public string Stringval { get; set; }
        public object Objectval { get; set; }

        public override Task ProcessAsync(TagBuilder builder, MvcTagHelperContext context)
        {
            builder.TagName = "ul";
            builder.InnerHtml =
                string.Format(@"
<li><strong>Bool value: </strong>{0}</li>
<li><strong>Byte value: </strong>{1}</li>
<li><strong>Char value: </strong>{2}</li>
<li><strong>Decimal value: </strong>{3}</li>
<li><strong>Double value: </strong>{4}</li>
<li><strong>Float value: </strong>{5}</li>
<li><strong>Int value: </strong>{6}</li>
<li><strong>Long value: </strong>{7}</li>
<li><strong>SByte value: </strong>{8}</li>
<li><strong>Short value: </strong>{9}</li>
<li><strong>UInt value: </strong>{10}</li>
<li><strong>ULong value: </strong>{11}</li>
<li><strong>UShort value: </strong>{12}</li>
<li><strong>String value: </strong>{13}</li>
<li><strong>Object value: </strong>{14}</li>",
Boolval, Byteval, Charval, Decimalval, Doubleval,
Floatval, Intval, Longval, Sbyteval, Shortval,
Uintval, Ulongval, Ushortval, Stringval, Objectval);

            return Task.FromResult(result: true);
        }
    }
}