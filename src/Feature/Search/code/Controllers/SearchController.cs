using CGP.Feature.Search.Helper;
using CGP.Feature.Search.Models;
using CGP.Feature.Search.Repositories;
using CGP.Foundation.Search;
using CGP.Foundation.Search.Models;
using CGP.Foundation.SitecoreExtensions.Repositories;
using CGP.Foundation.SitecoreExtensions.Utilities;
using Newtonsoft.Json;
using Sitecore.XA.Foundation.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CGP.Feature.Search.Controllers
{
    public class SearchController : StandardController
    {
        private readonly IGlobalSearchRepository _globalSearchRepository;
        private readonly ISiteConfiguration _siteConfiguration;

        public SearchController(IGlobalSearchRepository globalSearchRepository, ISiteConfiguration siteConfiguration)
        {
            this._globalSearchRepository = globalSearchRepository;
            this._siteConfiguration = siteConfiguration;
        }

        public ActionResult GetSuggestions(
            string searchTerm,
            int MaxPredictiveResult = 5,
            int MaxProductCount = 2,
            int MaxArticleCount = 2,
            int MaxOthersCount = 1,
            string currentItemId = null,
            string language = "en"
            )
        {
            var searchParameters = GetQueryStringData(language, currentItemId: currentItemId, searchTerm: searchTerm);
            var result = _globalSearchRepository.GetSuggestions(searchParameters, MaxPredictiveResult, MaxProductCount, MaxArticleCount, MaxOthersCount);
            return Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GlobalLoadMoreResult(GlobalLoadMoreInput loadMoreInput)
        {
            var globalLoadMoreResult = new GlobalLoadMoreResult()
            {
                Results = new List<GlobalResultItem>()
            };
            InputParameters inputParameters = new InputParameters()
            {
                SearchCount = loadMoreInput.SearchCount,
                SkipCount = loadMoreInput.SkipCount,
                CurrentItemId = loadMoreInput.SearchItemId,
                SearchTerm = loadMoreInput.SearchTerm,
                FilterString = loadMoreInput.Filters,
                ArticleListingFacetType = loadMoreInput.ArticleListingFacetType,
                CheckToLoadChildArticlesOnly = loadMoreInput.CheckToLoadChildArticlesOnly,
                FilterOrder = loadMoreInput.FilterOrder,
                ListingSortDirection = SearchHelper.GetSortDirection(loadMoreInput.ListingSortDirection),
                ListingSortOrder = loadMoreInput.ListingSortOrder,
                Language = !string.IsNullOrWhiteSpace(loadMoreInput.Language) ? Sitecore.Globalization.Language.Parse(loadMoreInput.Language) : Sitecore.Globalization.Language.Parse("en"),
                DescriptionLengthLimit = loadMoreInput.DescriptionLengthLimit
            };
            using (new Sitecore.Globalization.LanguageSwitcher(inputParameters.Language))
            {
                inputParameters = SearchHelper.ParseQueryStringParameters(inputParameters, _siteConfiguration.GetSiteConfiguration().SearchCriteria);

                var loadMoreResults = _globalSearchRepository.GetLoadMoreResults(inputParameters, loadMoreInput.Category);
                if (loadMoreResults != null)
                {
                    globalLoadMoreResult.Results = loadMoreResults.Select(x => new GlobalResultItem()
                    {
                        ResultItem = x,
                    }).ToList();
                    globalLoadMoreResult.GridSelection = !string.IsNullOrEmpty(loadMoreInput.GridSelection) ? loadMoreInput.GridSelection : string.Empty;
                    globalLoadMoreResult.HideDescriptionOnTiles = loadMoreInput.HideDescriptionOnTiles;
                    globalLoadMoreResult.HideTitleOnTiles = loadMoreInput.HideTitleOnTiles;
                }
            }
            return Json(new { result = globalLoadMoreResult }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GlobalSearch()
        {
            return PartialView("~/Views/OneWeb/Search/GlobalSearchResult.cshtml", this.GetModel());
        }
        protected override object GetModel()
        {
            var searchParameters = SearchHelper.ParseQueryStringParameters(this.GetQueryStringData(Sitecore.Context.Language.Name), _siteConfiguration.GetSiteConfiguration().SearchCriteria);
            return _globalSearchRepository.GetGlobalSearchResults(searchParameters);
        }
        public ActionResult GlobalSearchWithFilters(GlobalLoadMoreInput loadMoreInput)
        {
            InputParameters inputParameters = new InputParameters();
            inputParameters.SearchCount = loadMoreInput.SearchCount;
            inputParameters.SkipCount = 0;
            inputParameters.CurrentItemId = loadMoreInput.SearchItemId;
            inputParameters.SearchTerm = loadMoreInput.SearchTerm;

            inputParameters.FilterString = loadMoreInput.Filters;

            inputParameters = SearchHelper.ParseQueryStringParameters(inputParameters, _siteConfiguration.GetSiteConfiguration().SearchCriteria);
            var result = _globalSearchRepository.GetGlobalSearchResults(inputParameters);
            return Json(new { result = result }, JsonRequestBehavior.AllowGet);
        }

        private InputParameters GetQueryStringData(string language, string currentItemId = null, string searchTerm = null, string filterString = null)
        {
            InputParameters inputParameters = new InputParameters
            {
                SearchTerm = searchTerm ?? Request.QueryString["q"],
                FilterString = filterString ?? Request.QueryString["f"],
                CurrentItemId = (!string.IsNullOrWhiteSpace(currentItemId)) ? currentItemId : Request.QueryString["currentItemId"],
                Language = !string.IsNullOrWhiteSpace(language) ? Sitecore.Globalization.Language.Parse(language) : Sitecore.Globalization.Language.Parse("en")
            };
            if (!Request.IsAjaxRequest())
            {
                inputParameters.ArticleListingFacetType = HelperExtension.GetValueFromCurrentRenderingParameters(Constants.ArticleListingFacetType) ?? Constants.PageAttributes;
                inputParameters.CheckToLoadChildArticlesOnly = Convert.ToBoolean(Convert.ToInt32(HelperExtension.GetValueFromCurrentRenderingParameters(Constants.CheckToLoadChildArticlesOnly) ?? "0"));
                inputParameters.FilterOrder = HelperExtension.GetValueFromCurrentRenderingParameters(Constants.FilterOrder) ?? Constants.SameAsCMS;
                inputParameters.Language = Sitecore.Context.Language;
            }
            return inputParameters;
        }
    }
}