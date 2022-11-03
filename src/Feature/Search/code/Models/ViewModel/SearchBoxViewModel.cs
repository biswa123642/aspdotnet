using Sitecore.XA.Foundation.Mvc.Models;

namespace CGP.Feature.Search.Models.ViewModel
{
    public class SearchBoxViewModel : RenderingModelBase
    {
        public string PlaceholderText { get; set; }
        public string SearchResultPageUrl { get; set; }
        public bool DisableSearchTextBox { get; set; }
        public bool CloneSearchBox { get; set; }
        public bool DisableAutoSuggestion { get; set; }
        public int MinSuggestionsTriggerCharacterCount { get; set; }
        public int MaxPredictiveResultsCount { get; set; }
        public int MaxProductResultsCount { get; set; }
        public int MaxArticleResultsCount { get; set; }
        public int MaxOthersResultCount { get; set; }
        public SearchBoxViewModel() { }
    }
}