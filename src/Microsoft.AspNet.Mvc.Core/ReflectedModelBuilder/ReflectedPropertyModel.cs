// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Microsoft.AspNet.Mvc.ReflectedModelBuilder
{
    public class ReflectedPropertyModel
    {
        public ReflectedPropertyModel(PropertyInfo propertyInfo)
        {
            PropertyInfo = propertyInfo;

            // CoreCLR returns IEnumerable<Attribute> from GetCustomAttributes - the OfType<object>
            // is needed to so that the result of ToList() is List<object>
            Attributes = propertyInfo.GetCustomAttributes(inherit: true).OfType<object>().ToList();

            PropertyName = propertyInfo.Name;
        }

        public List<object> Attributes { get; private set; }

        public PropertyInfo PropertyInfo { get; private set; }

        public string PropertyName { get; set; }
    }
}