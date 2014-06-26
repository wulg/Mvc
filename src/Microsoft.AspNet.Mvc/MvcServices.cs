// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.AspNet.Mvc.Filters;
using Microsoft.AspNet.Mvc.ModelBinding;
using Microsoft.AspNet.Mvc.Razor;
using Microsoft.AspNet.Mvc.Razor.Compilation;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Security;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.DependencyInjection.NestedProviders;

namespace Microsoft.AspNet.Mvc
{
    public class MvcServices
    {
        public static IEnumerable<IServiceDescriptor> GetDefaultServices()
        {
            return GetDefaultServices(new Configuration());
        }

        public static IEnumerable<IServiceDescriptor> GetDefaultServices(IConfiguration configuration)
        {
            var describe = new ServiceDescriber(configuration);

            return new IServiceDescriptor[] {
                describe.Transient<IControllerFactory, DefaultControllerFactory>(),
                describe.Singleton<IControllerActivator, DefaultControllerActivator>(),
                describe.Scoped<IActionSelector, DefaultActionSelector>(),
                describe.Transient<IActionInvokerFactory, ActionInvokerFactory>(),
                describe.Transient<IControllerAssemblyProvider, DefaultControllerAssemblyProvider>(),
                describe.Transient<IActionDiscoveryConventions, DefaultActionDiscoveryConventions>(),

                describe.Instance<IMvcRazorHost>(new MvcRazorHost(typeof(RazorView).FullName)),

                describe.Transient<ICompilationService, RoslynCompilationService>(),

                describe.Transient<IRazorCompilationService, RazorCompilationService>(),
                describe.Transient<IVirtualPathViewFactory, VirtualPathViewFactory>(),
                describe.Scoped<IViewEngine, RazorViewEngine>(),

                describe.Transient<INestedProvider<ActionDescriptorProviderContext>,
                                            ReflectedActionDescriptorProvider>(),
                describe.Transient<INestedProvider<ActionInvokerProviderContext>,
                                            ReflectedActionInvokerProvider>(),
                describe.Singleton<IActionDescriptorsCollectionProvider,
                                            DefaultActionDescriptorsCollectionProvider>(),

                describe.Transient<IModelMetadataProvider, DataAnnotationsModelMetadataProvider>(),
                describe.Scoped<IActionBindingContextProvider, DefaultActionBindingContextProvider>(),

                describe.Transient<IValueProviderFactory, RouteValueValueProviderFactory>(),
                describe.Transient<IValueProviderFactory, QueryStringValueProviderFactory>(),
                describe.Transient<IValueProviderFactory, FormValueProviderFactory>(),

                describe.Transient<IModelBinder, TypeConverterModelBinder>(),
                describe.Transient<IModelBinder, TypeMatchModelBinder>(),
                describe.Transient<IModelBinder, GenericModelBinder>(),
                describe.Transient<IModelBinder, MutableObjectModelBinder>(),
                describe.Transient<IModelBinder, ComplexModelDtoModelBinder>(),

                describe.Transient<IInputFormatter, JsonInputFormatter>(),
                describe.Transient<IInputFormatterProvider, TempInputFormatterProvider>(),

                describe.Transient<INestedProvider<FilterProviderContext>, DefaultFilterProvider>(),

                describe.Transient<IModelValidatorProvider, DataAnnotationsModelValidatorProvider>(),
                describe.Transient<IModelValidatorProvider, DataMemberModelValidatorProvider>(),

                describe.Scoped<IUrlHelper, UrlHelper>(),

                describe.Transient<IViewComponentSelector, DefaultViewComponentSelector>(),
                describe.Transient<IViewComponentInvokerFactory, DefaultViewComponentInvokerFactory>(),
                describe.Transient<INestedProvider<ViewComponentInvokerProviderContext>,
                                       DefaultViewComponentInvokerProvider>(),
                describe.Transient<IViewComponentHelper, DefaultViewComponentHelper>(),

                describe.Transient<IAuthorizationService, DefaultAuthorizationService>(),
                describe.Singleton<IClaimUidExtractor, DefaultClaimUidExtractor>(),
                describe.Singleton<AntiForgery, AntiForgery>(),
                describe.Singleton<IAntiForgeryAdditionalDataProvider,
                                    DefaultAntiForgeryAdditionalDataProvider>(),

                describe.Describe(
                   typeof(INestedProviderManager<>),
                   typeof(NestedProviderManager<>),
                   implementationInstance: null,
                   lifecycle: LifecycleKind.Transient),

                describe.Describe(
                    typeof(INestedProviderManagerAsync<>),
                    typeof(NestedProviderManagerAsync<>),
                    implementationInstance: null,
                    lifecycle: LifecycleKind.Transient),

                describe.Transient<IHtmlHelper, HtmlHelper>(),

                describe.Describe(
                    typeof(IHtmlHelper<>),
                    typeof(HtmlHelper<>),
                    implementationInstance: null,
                    lifecycle: LifecycleKind.Transient)};
        }
    }
}
