using System;

namespace Microsoft.AspNet.Mvc.Razor
{
    public interface IMvcRazorHostProvider
    {
        IMvcRazorHost GetHost();
    }
}