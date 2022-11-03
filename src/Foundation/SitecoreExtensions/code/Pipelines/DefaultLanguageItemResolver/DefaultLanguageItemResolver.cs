using Sitecore.Data.Items;
using Sitecore.Globalization;
using Sitecore.Links;
using Sitecore.Pipelines.HttpRequest;
using System;
using System.Linq;
using System.Web.Routing;

namespace CGP.Foundation.SitecoreExtensions.Pipelines.DefaultLanguageItemResolver
{
    public class DefaultLanguageItemResolver : HttpRequestProcessor
    {
        public override void Process(HttpRequestArgs args)
        {
            if (Sitecore.Context.Item == null || Sitecore.Context.Site == null || Sitecore.Context.Database == null || !string.IsNullOrEmpty(Sitecore.Context.Page.FilePath) || RouteTable.Routes.GetRouteData(args.HttpContext) != null || args.PermissionDenied)
                return;

            this.ResolveItemByDefalutLanguage(args);
            RemoveLanguageCookieSiteSetting(args);
        }

        protected void ResolveItemByDefalutLanguage(HttpRequestArgs args)
        {
            var item = GetLanguageVersion(Sitecore.Context.Item, Sitecore.Context.Item.Language.ToString());

            if (item == null || item.Versions.Count == 0)
            {
                Language defalutLanguage = Language.Parse(Constants.DefaultLanguage);
                var defaultItemVersion = GetLanguageVersion(Sitecore.Context.Item, Constants.DefaultLanguage);
                Sitecore.Context.Language = defalutLanguage;
                Sitecore.Context.Item = defaultItemVersion;

                string url = GetPageWithLanguageUrlOptions(defaultItemVersion, Constants.DefaultLanguage);
                if (!string.IsNullOrEmpty(url))
                {
                    args.HttpContext.Response.Redirect(url);
                }
            }
            else if(!Sitecore.Context.Database.ConnectionStringName.Equals(Constants.CoreConnectionStringName))
            {
                var splitURL = args.HttpContext.Request.RawUrl.Split('/');

                if (splitURL[1].Equals("en"))
                {
                    var options = LinkManager.GetDefaultUrlBuilderOptions();
                    string url = LinkManager.GetItemUrl(item, options);
                    args.HttpContext.Response.Redirect(url);
                }
            }
        }

        protected string GetPageWithLanguageUrlOptions(Item item, string language)
        {
            var options = LinkManager.GetDefaultUrlBuilderOptions();
            options.LanguageEmbedding = LanguageEmbedding.Never;
            options.Language = Language.Parse(language);

            return LinkManager.GetItemUrl(item, options);
        }

        protected static Item GetLanguageVersion(Item item, string languageName)
        {
            var language = item.Languages.FirstOrDefault(l => l.Name == languageName);
            if (language != null)
            {
                var languageSpecificItem = Sitecore.Context.Database.GetItem(item.ID, language);
                if (languageSpecificItem != null && languageSpecificItem.Versions.Count > 0)
                {
                    return languageSpecificItem;
                }
            }
            return null;
        }

        public void RemoveLanguageCookieSiteSetting(HttpRequestArgs args)
        {
            if (bool.TryParse(Sitecore.Context.Site.Properties[Constants.RemoveLanguageCookieSiteSetting], out var result) && result)
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