using Sitecore.Pipelines.HttpRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CGP.Foundation.SitecoreExtensions.Pipelines.LanguageCookie
{

        public class RemoveLanguageCookieProcessor : HttpRequestProcessor
        {
            private const string RemoveLanguageCookieSiteSetting = "removeLanguageCookie";
            public override void Process(HttpRequestArgs args)
            {
                if (Sitecore.Context.Site == null)
                {
                    return;
                }
                if (bool.TryParse(Sitecore.Context.Site.Properties[RemoveLanguageCookieSiteSetting], out var result) && result)
                {
                    string cookieKey = Sitecore.Context.Site.GetCookieKey("lang");
                    //Remove the cookie from the response if it's already set
                    if (args.HttpContext.Response.Cookies.AllKeys.Contains(cookieKey))
                    {
                        args.HttpContext.Response.Cookies.Remove(cookieKey);
                    }
                    //If the user has the cookie set in it's request, invalidate it
                    if (args.HttpContext.Request.Cookies.AllKeys.Contains(cookieKey))
                    {
                        args.HttpContext.Response.Cookies.Set(new System.Web.HttpCookie(cookieKey)
                        {
                            Expires = DateTime.Now.AddYears(-1)
                        });
                    }
                }
            }
        }
    
}