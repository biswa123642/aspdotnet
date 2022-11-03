using Sitecore.Globalization;
using SolrNet;
using System.Collections.Generic;
using System.Linq;

namespace CGP.Foundation.Search.Models
{
    public class InputParameters
    {
        public InputParameters()
        {
            Filters = new List<InputFilter>();
        }
        public string CurrentItemId { get; set; }
        public string SearchTerm { get; set; }
        public List<InputFilter> Filters { get; set; }
        public string FilterString { get; set; }
        public int SearchCount { get; set; }
        public int SkipCount { get; set; }
        public bool IsNotArticleFilterByPageAttribute { get; set; }
        public string ArticleListingFacetType { get; set; }
        public bool CheckToLoadChildArticlesOnly { get; set; }
        public string FilterOrder { get; set; }
        public Order ListingSortDirection { get; set; }
        public string ListingSortOrder { get; set; } 
        public int DescriptionLengthLimit { get; set; }
        public Language Language { get; set; }
    }

    public class InputFilter
    {
        public string FilterKey { get; set; }
        public List<string> FilterValues { get; set; }
    }
}