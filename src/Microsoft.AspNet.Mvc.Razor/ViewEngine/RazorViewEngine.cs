using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNet.Mvc.Razor
{
    public class RazorViewEngine : IViewEngine
    {
        // TODO: These need to become user configurable
        private static readonly string[] _viewLocationFormats =
        {
            "/Areas/{area}/Views/{controller}/{action}.cshtml",
            "/Views/{controller}/{action}.cshtml",
            "/Views/Shared/{action}.cshtml",
        };

        private List<ParsedViewLocation> _viewLocations = null;

        private List<ParsedViewLocation> ViewLocations
        {
            get
            {
                if (_viewLocations == null)
                {
                    var viewLocations = new List<ParsedViewLocation>();

                    foreach (var locationFormat in _viewLocationFormats)
                    {
                        viewLocations.Add(new ParsedViewLocation(locationFormat));
                    }

                    _viewLocations = viewLocations;
                }

                return _viewLocations;
            }
        }

        private readonly IVirtualPathViewFactory _virtualPathFactory;

        public RazorViewEngine(IVirtualPathViewFactory virtualPathFactory)
        {
            _virtualPathFactory = virtualPathFactory;
        }

        public async Task<ViewEngineResult> FindView(object context, string viewName)
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
                var controllerName = actionDescriptor.Path ?? 
                                     actionDescriptor.RouteConstraints.
                                     Where(c => c.RouteKey.Equals("controller", StringComparison.OrdinalIgnoreCase)).
                                     Select(c => c.RouteValue).
                                     Single();

                var routeKvp = actionDescriptor.RouteConstraints.
                    Where(
                        rc =>
                            rc.KeyHandling == RouteKeyHandling.RequireKey && !rc.RouteKey.Equals("controller") &&
                            !rc.RouteKey.Equals("action")).
                            Select(rc => new KeyValuePair<string, string>(rc.RouteKey, rc.RouteValue)).
                            Concat(new[] {new KeyValuePair<string, string>("action", viewName), new KeyValuePair<string, string>("controller", controllerName)}).
                            ToArray();

                var searchedLocations = new List<string>();

                for (int i = 0; i < ViewLocations.Count; i++)
                {
                    string path = ViewLocations[i].BuildPath(routeKvp);

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

        private static bool IsSpecificPath(string name)
        {
            char c = name[0];
            return (name[0] == '/');
        }
    }
}
