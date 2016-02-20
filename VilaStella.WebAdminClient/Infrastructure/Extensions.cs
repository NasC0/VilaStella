using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace VilaStella.WebAdminClient.Infrastructure
{
    public static class Extensions
    {
        public static RouteValueDictionary ToRouteValueDictionary(this NameValueCollection collection)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();

            foreach (string key in collection)
            {
                routeValueDictionary.Add(key, collection[key]);
            }

            return routeValueDictionary;
        }
    }
}