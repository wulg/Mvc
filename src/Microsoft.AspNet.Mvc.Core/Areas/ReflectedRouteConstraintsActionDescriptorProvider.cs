using System;
using System.Linq;
using System.Reflection;

namespace Microsoft.AspNet.Mvc
{
    public class ReflectedRouteConstraintsActionDescriptorProvider : IActionDescriptorProvider
    {
        public int Order
        {
            get { return 100; }
        }

        public void Invoke([NotNull]ActionDescriptorProviderContext context, Action callNext)
        {
            // Iterate all the Reflected Action Descriptor providers and add area or other route constraints
            if (context.Results != null)
            {
                foreach (var actionDescriptor in context.Results.OfType<ReflectedActionDescriptor>())
                {
                    var routeConstraints = actionDescriptor.
                                           ControllerDescriptor.
                                           ControllerTypeInfo.
                                           GetCustomAttributes<RouteConstraintAttribute>();

                    foreach (var routeConstraint in routeConstraints)
                    {
                        actionDescriptor.RouteConstraints.Add(new RouteDataActionConstraint(routeConstraint.RouteKey, routeConstraint.RouteValue));
                    }
                }
            }

            callNext();
        }
    }
}
