using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Fierce
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapPageRoute(
    "",
    "",
    "~/fierce/DEIntegration/DEOrderApprovedEvent.aspx",
    true, null,
    new RouteValueDictionary { { "outgoing", new Fierce.MvcApplication.MyCustomConstaint() } }
);
            routes.MapPageRoute(
"",
"",
"~/fierce/OrderComplete.aspx",
true, null,
new RouteValueDictionary { { "outgoing", new Fierce.MvcApplication.MyCustomConstaint() } }
);
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                //defaults: new { controller = "CustomBook", action = "OrderComplete", id = UrlParameter.Optional }
                 defaults: new { controller = "DEIntegration", action = "AcceptPunchOutRequest", id = UrlParameter.Optional }
            );
        }
    }
}