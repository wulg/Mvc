using System;
using Microsoft.Framework.DependencyInjection;

namespace Microsoft.AspNet.Mvc.Razor
{
    public class DefaultMvcRazorHostProvider : IMvcRazorHostProvider
    {
        private ITypeActivator _activator;
        private IServiceProvider _serviceProvider;

	    public DefaultMvcRazorHostProvider(ITypeActivator activator, IServiceProvider serviceProvider)
	    {
            _activator = activator;
            _serviceProvider = serviceProvider;
	    }

        public IMvcRazorHost GetHost()
        {
            return _activator.CreateInstance<MvcRazorHost>(_serviceProvider);
        }
    }
}