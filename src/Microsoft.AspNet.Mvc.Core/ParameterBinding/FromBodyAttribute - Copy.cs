// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Microsoft.AspNet.Mvc
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public abstract class UberBindingAttribute : Attribute
    {
        public abstract IUberBinding GetBinding(Descriptor descriptor);
    }

    public interface IUberBinding
    {
        bool CanBind(Type modelType);

        void Bind(UberBindingContext context);
    }

    public class UberBindingContext
    {
        public ActionContext ActionContext { get; set; }

        public object Model { get; set; }
    }

    public class Descriptor
    {
    }


    #region Header

    public class FromHeaderAttribute : UberBindingAttribute
    {
        private string _headerKey;

        public FromHeaderAttribute(string headerKey)
        {
            _headerKey = headerKey;
        }

        public override IUberBinding GetBinding(Descriptor descriptor)
        {
            return new HttpHeaderBinding(_headerKey);
        }
    }

    public class HttpHeaderBinding : IUberBinding
    {
        private string _headerKey;

        public HttpHeaderBinding(string headerKey)
        {
            _headerKey = headerKey;
        }

        public bool CanBind(Type modelType)
        {
            return modelType == typeof(string);
        }

        public void Bind(UberBindingContext context)
        {
            // look at the header and bind.
            context.Model = context.ActionContext.HttpContext.Request.Headers[_headerKey];
        }
    }

    #endregion

    #region ModelBinder

    public class ModelBinding : IUberBinding
    {
        public void Bind(UberBindingContext context)
        {
            // wrap model binding. 
        }

        public bool CanBind(Type modelType)
        {
            throw new NotImplementedException();
        }
    }

    #endregion


}
