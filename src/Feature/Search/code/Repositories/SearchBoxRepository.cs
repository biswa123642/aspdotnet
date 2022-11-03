using CGP.Feature.Search.Helper;
using CGP.Feature.Search.Models.ViewModel;
using CGP.Foundation.ErrorModule.Repositiories;
using CGP.Foundation.SitecoreExtensions.Utilities;
using Sitecore.XA.Foundation.Mvc.Repositories.Base;
using System;
using Sitecore.Data.Items;
using CGP.Foundation.Search;

namespace CGP.Feature.Search.Repositories
{
    public class SearchBoxRepository : ModelRepository, ISearchBoxRepository
    {
        public readonly ILogger _logger;

        public SearchBoxRepository(ILogger logger)
        {
            this._logger = logger;
        }

        public SearchBoxViewModel GetSearchBoxViewModel()
        {
            SearchBoxViewModel searchBoxViewModel = new SearchBoxViewModel();
            try
            {
                FillBaseProperties(searchBoxViewModel);
                string placeholderText = HelperExtension.GetValueFromCurrentRenderingParameters(Constants.SearchBox.PlaceholderText);
                searchBoxViewModel.PlaceholderText = !string.IsNullOrWhiteSpace(placeholderText) ? placeholderText : "Search here...";

                searchBoxViewModel.DisableAutoSuggestion = Convert.ToBoolean(Convert.ToInt32(HelperExtension.GetValueFromCurrentRenderingParameters(Constants.SearchBox.DisableAutoSugession) ?? "0"));
                searchBoxViewModel.MinSuggestionsTriggerCharacterCount = Convert.ToInt32(HelperExtension.GetValueFromCurrentRenderingParameters(Constants.SearchBox.MinSuggestionsTriggerCharacterCount) ?? "3");
                searchBoxViewModel.MaxPredictiveResultsCount = Convert.ToInt32(HelperExtension.GetValueFromCurrentRenderingParameters(Constants.SearchBox.MaxPredictiveResultsCount) ?? "5"); ;
                searchBoxViewModel.MaxProductResultsCount = Convert.ToInt32(HelperExtension.GetValueFromCurrentRenderingParameters(Constants.SearchBox.MaxProductResultsCount) ?? "2");
                searchBoxViewModel.MaxArticleResultsCount = Convert.ToInt32(HelperExtension.GetValueFromCurrentRenderingParameters(Constants.SearchBox.MaxArticleResultsCount) ?? "2");
                searchBoxViewModel.MaxOthersResultCount = Convert.ToInt32(HelperExtension.GetValueFromCurrentRenderingParameters(Constants.SearchBox.MaxOthersResultCount) ?? "1");

                var searchResultPage = HelperExtension.GetValueFromCurrentRenderingParameters(Constants.SearchBox.SearchResultPage);
                string searchpage = string.Empty;
                if (searchResultPage != null)
                {
                    Item item = HelperExtension.GetItem(searchResultPage);
                    searchBoxViewModel.SearchResultPageUrl = item != null ? SitecoreUtil.GetItemUrl(item, true) : string.Empty;
                }
                searchBoxViewModel.DisableSearchTextBox = !string.IsNullOrEmpty(searchBoxViewModel.SearchResultPageUrl)
                    ? Convert.ToBoolean(Convert.ToInt32(HelperExtension.GetValueFromCurrentRenderingParameters(Constants.SearchBox.DisableSearchTextBox) ?? "0"))
                    : false;
                searchBoxViewModel.CloneSearchBox = Convert.ToBoolean(Convert.ToInt32(HelperExtension.GetValueFromCurrentRenderingParameters(Constants.SearchBox.CloneSearchBox) ?? "0"));
            }
            catch (Exception ex)
            {
                _logger.LogError("ERROR occured in SearchBoxRepository.GetSearchBoxViewModel() ", ex);
            }
            return searchBoxViewModel;
        }
    }
}