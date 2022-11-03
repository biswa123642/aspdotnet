using System;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;

namespace CGP.Foundation.Search.Models
{
    public class SolrField : SearchResultItem
    {
        [IndexField("_created")]
        public DateTime DateCreated { get; set; }

        [IndexField("releasedate")]
        public DateTime ReleasedDate { get; set; }

        [IndexField("contenturl_t")]
        public string ContentUrl { get; set; }

        [IndexField("title")]
        public string Title { get; set; }

        [IndexField("navigationtitle_t")]
        public string NavigationTitle { get; set; }

        [IndexField("opengraphdescription")]
        public string OpengraphDescription { get; set; }

        [IndexField("computedimage_t")]
        public string ComputedImage { get; set; }

        [IndexField("contenttype_t")]
        public string ContentType { get; set; }

        [IndexField("customredirectlink_s")]
        public string RedirectUrl { get; set; }
    }
}