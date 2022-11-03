using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CGP.Foundation.SitecoreExtensions.Utilities
{
    public class HttpResponseHelper
    {
        public static void RedirectPermanent301(string newUrl)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Status = "301 Moved Permanently";
            HttpContext.Current.Response.AddHeader("Location", newUrl);
            HttpContext.Current.Response.End();
        }
    }
}