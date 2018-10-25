using JourneyPortal.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace JourneyPortal
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static readonly object padlock = new object();
        public static List<string> Sessions { get; } = new List<string>();

        protected void Application_Start()
        {
            AjaxHelper.GlobalizationScriptPath = "http://ajax.microsoft.com/ajax/4.0/1/globalization/";
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //ModelBinders.Binders.Add(typeof(decimal), new DecimalModelBinder());
            //ModelBinders.Binders.Add(typeof(decimal?), new DecimalModelBinder());
        }
        protected void Session_Start(object sender, EventArgs e)
        {
            lock (padlock)
            {
                Sessions.Add(Session.SessionID);
            }
        }
        protected void Session_End(object sender, EventArgs e)
        {
            lock (padlock)
            {
                Sessions.Remove(Session.SessionID);
            }
        }
        protected void Application_Error(object sender, EventArgs e)
        {
            var app = (MvcApplication)sender;
            Exception ex = app.Server.GetLastError();
        }
        private string GetQueryString()
        {
            string queryString = "";

            NameValueCollection qs = Request.QueryString;

            foreach (string key in qs.AllKeys)
                foreach (string value in qs.GetValues(key))
                    queryString += Server.UrlEncode(key) + "=" + Server.UrlEncode(value) + "&";

            return queryString.TrimEnd('&');
        }
    }
}
