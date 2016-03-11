using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LMS
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Route to get the parent of the partial upload view.
            routes.MapRoute(
                name: "DocumentsManagement", // Route name
                url: "Documents/{action}/{type}/{id}",  // URL with parameters
                defaults: new { controller = "Documents", action = "Upload", type = UrlParameter.Optional, id = UrlParameter.Optional }  // Parameter defaults
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
