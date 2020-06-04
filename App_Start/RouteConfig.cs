using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SERIESREMINDER
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    name: "TmdbApi",
            //    url: "TmdbApi/{id}",
            //    defaults: new { controller = "TmdbApi", action = "GetPerson", id = "" },
            //    constraints: new { id = @"^[0-9]+$" }
            //);

            //routes.MapRoute(
            //    name: "TmdbApiPaging",
            //    url: "TmdbApi/{peopleName}/{page}",
            //    defaults: new { controller = "TmdbApi", action = "Index", peopleName = "", page = "" },
            //    constraints: new { peopleName = @"^[a-zA-Z]+$", page = @"^[0-9]+$" }
            //);

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Login", id = UrlParameter.Optional } // Parameter defaults
            );
        }
    }
}
