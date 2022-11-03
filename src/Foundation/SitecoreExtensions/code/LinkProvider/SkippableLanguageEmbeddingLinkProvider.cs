using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Links.UrlBuilders;
using System.Collections.Generic;
using System.Linq;
using Sitecore.XA.Foundation.Multisite.LinkManagers;

namespace CGP.Foundation.SitecoreExtensions.LinkProvider
{
    public class SkippableLanguageEmbeddingLinkProvider : LocalizableLinkProvider
    {
        private const string SkipEmbedForLanguagesSiteSetting = "skipLanguageEmbeddingInURLForLanguages";
        public SkippableLanguageEmbeddingLinkProvider()
        {
        }
        public override string GetItemUrl(Item item, ItemUrlBuilderOptions options)
        {
            options.LanguageEmbedding = GetShouldEmbedLanguage(options) ? LanguageEmbedding.Always : LanguageEmbedding.Never;
            return base.GetItemUrl(item, options);
        }
        private bool GetShouldEmbedLanguage(ItemUrlBuilderOptions options)
        {
            var languagesToIgnore = options.Site.Properties[SkipEmbedForLanguagesSiteSetting]?.Split('|').Select(l => l.ToLowerInvariant()).ToList() ?? new List<string>();
            if (languagesToIgnore.Contains(options.Language.Name.ToLowerInvariant()))
            {
                return false;
            }
            return true;
        }
    }
}
