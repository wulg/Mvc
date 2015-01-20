// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNet.Mvc.Rendering;

namespace Microsoft.AspNet.Mvc
{
    /// <summary>
    /// Represents a <see cref="ViewResultBase"/> that renders a view to the response.
    /// </summary>
    public class ViewResult : ViewResultBase
    {
        /// <inheritdoc />
        protected override ViewEngineResult FindView([NotNull] IViewEngine viewEngine,
                                                     [NotNull] ActionContext context,
                                                     [NotNull] string viewName)
        {
            return viewEngine.FindView(context, viewName);
        }
    }
}
