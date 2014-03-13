using System;

namespace Microsoft.AspNet.Mvc
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public abstract class RouteConstraintAttribute : Attribute
    {
        protected RouteConstraintAttribute([NotNull]string routeKey, [NotNull]string routeValue)
        {
            RouteKey = routeKey;
            RouteValue = routeValue;
        }

        public string RouteKey { get; private set; }
        public string RouteValue { get; private set; }
    }
}
