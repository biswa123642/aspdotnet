using CGP.Feature.Search.Models.ViewModel;

namespace CGP.Feature.Search.Models
{
    public class GlobalLoadMoreInput
    {
        public string SearchItemId { get; set; }
        public string SearchTerm { get; set; }
        public string Category { get; set; }
        public int SearchCount { get; set; }
        public int SkipCount { get; set; }
        public string Filters { get; set; }
        public string GridSelection { get; set; }
        public bool HideDescriptionOnTiles { get; set; }
        public bool HideTitleOnTiles { get; set; }
        public string ArticleListingFacetType { get; set; }
        public bool CheckToLoadChildArticlesOnly { get; set; }
        public string FilterOrder { get; set; }
        public string ListingSortDirection { get; set; }
        public string ListingSortOrder { get; set; }
        public int DescriptionLengthLimit { get; set; }
        public string Language { get; set; }
    }
}