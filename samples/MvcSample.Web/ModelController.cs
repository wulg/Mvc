using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using MvcSample.Web.Models;

namespace MvcSample.Web.RandomNameSpace
{
    public class ModelController
    {
        public void Action(CustomType customType)
        {

        }
    }

    public class CustomType
    {
        public string Name { get; set; }

        public CustomType User { get; set; }
    }
}