using CGP.Feature.Search.Helper;
using CGP.Feature.Search.Models;
using CGP.Feature.Search.Models.ViewModel;
using CGP.Foundation.ErrorModule.Repositiories;
using CGP.Foundation.Search;
using CGP.Foundation.Search.Extensions;
using CGP.Foundation.Search.Models;
using CGP.Foundation.SitecoreExtensions.Repositories;
using CGP.Foundation.SitecoreExtensions.Utilities;
using Sitecore.XA.Foundation.Mvc.Repositories.Base;
using System;
using System.Collections.Generic;

namespace CGP.Feature.Search.Repositories
{
    public class ArticleSearchRepository : ModelRepository, IArticleSearchRepository
    {
        private readonly ISiteConfiguration siteConfiguration;
        public readonly ILogger _logger;

        public ArticleSearchRepository(ISiteConfiguration siteConfiguration, ILogger logger)
        {
            this.siteConfiguration = siteConfiguration;
            this._logger = logger;
        }
        public ArticleListingViewModel GetArticleList(InputParameters inputParameters)
        {
            ArticleListingViewModel articleListingViewModel = new ArticleListingViewModel();

            try
            {
                int PageSize = int.Parse(siteConfiguration.GetSiteConfiguration().SearchCriteria.Pagesize);
                inputParameters.SearchCount = PageSize;

                SolrSearchResponse solrSearchResponse = SearchExtensions.InitiateSearch(inputParameters, false, inputParameters.CheckToLoadChildArticlesOnly, new List<string> { StringUtil.RemoveSpecialCharacters(Templates.ArticlePageIdString).ToLower() });
                articleListingViewModel.GlobalSearchResult = new GlobalSearchResult(solrSearchResponse, siteConfiguration.GetSiteConfiguration().SearchCriteria, inputParameters.FilterOrder)
                {
                    FilterString = inputParameters.FilterString
                };
                SearchHelper.SetTotalCount(articleListingViewModel.GlobalSearchResult);
                if (PageSize > 0)
                {
                    SearchHelper.GetNoOfResults(articleListingViewModel.GlobalSearchResult, PageSize);
                }
                FillBaseProperties(articleListingViewModel);
                SearchHelper.SetActiveFacets(articleListingViewModel.GlobalSearchResult, inputParameters.Filters, inputParameters.FilterOrder);
                string gridSelection = HelperExtension.GetValueFromCurrentRenderingParameters(Constants.LoadMore.GridSelection);
                articleListingViewModel.GridSelection = !string.IsNullOrEmpty(gridSelection) ? gridSelection : "col-lg-3";
                articleListingViewModel.HideDescriptionOnTiles = Convert.ToBoolean(Convert.ToInt32(HelperExtension.GetValueFromCurrentRenderingParameters(Constants.HideDescriptionOnTiles) ?? "0"));
                articleListingViewModel.HideTitleOnTiles = Convert.ToBoolean(Convert.ToInt32(HelperExtension.GetValueFromCurrentRenderingParameters(Constants.HideTitleOnTiles) ?? "0"));
            }
            catch (Exception ex)
            {
                _logger.LogError("ERROR occured in ArticleSearchRepository.GetArticleList() ", ex);
            }
            return articleListingViewModel;
        }
    }
}