using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNet.Mvc.Razor
{
    public class RazorViewEngine : IViewEngine
    {
        private IEnumerable<string> _viewLocationFormats;
        protected readonly List<ParsedViewLocation> _viewLocations;
        private readonly IVirtualPathViewFactory _virtualPathFactory;

        public RazorViewEngine(IVirtualPathViewFactory virtualPathFactory)
        {
            _virtualPathFactory = virtualPathFactory;
            _viewLocations = new List<ParsedViewLocation>();

            Initialize(_defaultViewLocationFormats);
        }

        public virtual void Initialize([NotNull]IEnumerable<string> viewLocationFormats)
        {
            _viewLocations.Clear();

            foreach (var locationFormat in viewLocationFormats)
            {
                _viewLocations.Add(new ParsedViewLocation(locationFormat));
            }
        }

        private static readonly string[] _defaultViewLocationFormats =
        {
            "/Areas/{area}/Views/{controller}/{action}.cshtml",
            "/Views/{controller}/{action}.cshtml",
            "/Views/Shared/{action}.cshtml",
        };

        public virtual async Task<ViewEngineResult> FindView(object context, string viewName)
        {
            var actionContext = (ActionContext)context;
            var actionDescriptor = actionContext.ActionDescriptor;
            
            if (actionDescriptor == null)
            {
                return null;
            }

            if (string.IsNullOrEmpty(viewName))
            {
                viewName = actionDescriptor.Name;
            }

            if (string.IsNullOrEmpty(viewName))
            {
                throw new ArgumentException(Resources.ArgumentCannotBeNullOrEmpty, "viewName");
            }

            var nameRepresentsPath = IsSpecificPath(viewName);

            if (nameRepresentsPath)
            {
                var view = await _virtualPathFactory.CreateInstance(viewName);
                return view != null ? ViewEngineResult.Found(view) :
                                      ViewEngineResult.NotFound(new[] { viewName });
            }
            else
            {
                var searchedLocations = new List<string>();

                var viewKvp = BuildKeyValuePairs(actionDescriptor, viewName);

                for (int i = 0; i < _viewLocations.Count; i++)
                {
                    string path = _viewLocations[i].BuildPath(viewKvp);

                    if (path != null)
                    {
                        IView view = await _virtualPathFactory.CreateInstance(path);
                        if (view != null)
                        {
                            return ViewEngineResult.Found(view);
                        }

                        searchedLocations.Add(path);
                    }
                }

                return ViewEngineResult.NotFound(searchedLocations);
            }
        }

        public virtual KeyValuePair<string, string>[] BuildKeyValuePairs(ActionDescriptor actionDescriptor, string viewName)
        {
            var controllerName = actionDescriptor.Path ??
                     actionDescriptor.RouteConstraints.
                     Where(c => c.RouteKey.Equals("controller", StringComparison.OrdinalIgnoreCase)).
                     Select(c => c.RouteValue).
                     Single();

            var routeKvp = actionDescriptor.RouteConstraints.
                Where(rc =>
                        rc.KeyHandling == RouteKeyHandling.RequireKey && !rc.RouteKey.Equals("controller") && !rc.RouteKey.Equals("action")).
                        Select(rc => new KeyValuePair<string, string>(rc.RouteKey, rc.RouteValue)).
                        Concat(new[] { new KeyValuePair<string, string>("action", viewName), new KeyValuePair<string, string>("controller", controllerName) }).
                        ToArray();


            return routeKvp;
        }

        private static bool IsSpecificPath(string name)
        {
            char c = name[0];
            return (name[0] == '/');
        }
    }
}
