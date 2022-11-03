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
using System.Linq;

namespace CGP.Feature.Search.Repositories
{
    public class ProductSearchRepository : ModelRepository, IProductSearchRepository
    {
        private readonly ISiteConfiguration siteConfiguration;
        public readonly ILogger _logger;

        public ProductSearchRepository(ISiteConfiguration siteConfiguration, ILogger logger)
        {
            this.siteConfiguration = siteConfiguration;
            this._logger = logger;
        }
        public ProductListingViewModel GetProductList(InputParameters inputParameters)
        {
            ProductListingViewModel productListingViewModel = new ProductListingViewModel();

            try
            {
                int PageSize = int.Parse(siteConfiguration.GetSiteConfiguration().SearchCriteria.Pagesize);
                inputParameters.SearchCount = PageSize;
                SolrSearchResponse solrSearchResponse = SearchExtensions.InitiateSearch(inputParameters, false, inputParameters.CheckToLoadChildArticlesOnly, new List<string> { StringUtil.RemoveSpecialCharacters(Templates.ProductDetailPageIdString).ToLower() });

                productListingViewModel.GlobalSearchResult = new GlobalSearchResult(solrSearchResponse, siteConfiguration.GetSiteConfiguration().SearchCriteria, inputParameters.FilterOrder)
                {
                    FilterString = inputParameters.FilterString
                };

                SearchHelper.SetTotalCount(productListingViewModel.GlobalSearchResult);
                if (PageSize > 0)
                {
                    SearchHelper.GetNoOfResults(productListingViewModel.GlobalSearchResult, PageSize);
                }
                FillBaseProperties(productListingViewModel);
                SearchHelper.SetActiveFacets(productListingViewModel.GlobalSearchResult, inputParameters.Filters, inputParameters.FilterOrder);
                string gridSelection = HelperExtension.GetValueFromCurrentRenderingParameters(Constants.LoadMore.GridSelection);
                productListingViewModel.GridSelection = !string.IsNullOrEmpty(gridSelection) ? gridSelection : "col-lg-3";
                productListingViewModel.HideDescriptionOnTiles = Convert.ToBoolean(Convert.ToInt32(HelperExtension.GetValueFromCurrentRenderingParameters(Constants.HideDescriptionOnTiles) ?? "0"));
                productListingViewModel.HideTitleOnTiles = Convert.ToBoolean(Convert.ToInt32(HelperExtension.GetValueFromCurrentRenderingParameters(Constants.HideTitleOnTiles) ?? "0"));
            }
            catch (Exception ex)
            {
                _logger.LogError("ERROR occured in ProductSearchRepository.GetProductList() ", ex);
            }
            return productListingViewModel;
        }
    }
}