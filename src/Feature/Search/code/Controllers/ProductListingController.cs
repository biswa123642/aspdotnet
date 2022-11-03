using CGP.Feature.Search.Helper;
using CGP.Feature.Search.Repositories;
using CGP.Foundation.Search;
using CGP.Foundation.Search.Models;
using CGP.Foundation.SitecoreExtensions.Repositories;
using CGP.Foundation.SitecoreExtensions.Utilities;
using Sitecore.XA.Foundation.Mvc.Controllers;
using SolrNet;
using System;
using System.Web.Mvc;

namespace CGP.Feature.Search.Controllers
{
    public class ProductListingController : StandardController
    {
        private readonly IProductSearchRepository _productSearchRepository;
        private readonly ISiteConfiguration _siteConfiguration;
        public ProductListingController(IProductSearchRepository productSearchRepository, ISiteConfiguration siteConfiguration)
        {
            this._productSearchRepository = productSearchRepository;
            this._siteConfiguration = siteConfiguration;
        }

        public ActionResult GetProductList()
        {
            return PartialView("~/Views/OneWeb/Search/ProductList.cshtml", this.GetModel());
        }
        protected override object GetModel()
        {
            var searchParameters = SearchHelper.ParseQueryStringParameters(this.GetQueryStringData(), _siteConfiguration.GetSiteConfiguration().SearchCriteria);

            return _productSearchRepository.GetProductList(searchParameters);
        }
        private InputParameters GetQueryStringData(string currentItemId = null, string searchTerm = null, string filterString = null)
        {
            InputParameters inputParameters = new InputParameters
            {
                SearchTerm = searchTerm ?? Request.QueryString["q"],
                FilterString = filterString ?? Request.QueryString["f"],
                CurrentItemId = (!string.IsNullOrWhiteSpace(currentItemId)) ? currentItemId : Request.QueryString["currentItemId"]
            };
            if (!Request.IsAjaxRequest())
            {
                inputParameters.ArticleListingFacetType = HelperExtension.GetValueFromCurrentRenderingParameters(Constants.ArticleListingFacetType) ?? Constants.PageAttributes;
                inputParameters.CheckToLoadChildArticlesOnly = Convert.ToBoolean(Convert.ToInt32(HelperExtension.GetValueFromCurrentRenderingParameters(Constants.CheckToLoadChildArticlesOnly) ?? "0"));
                inputParameters.FilterOrder = HelperExtension.GetValueFromCurrentRenderingParameters(Constants.FilterOrder) ?? Constants.SameAsCMS;
                inputParameters.ListingSortDirection = SearchHelper.GetSortDirection(HelperExtension.GetValueFromCurrentRenderingParameters(Constants.SortDirection));
                inputParameters.ListingSortOrder = HelperExtension.GetValueFromCurrentRenderingParameters(Constants.SortingLookup);
                inputParameters.Language = Sitecore.Context.Language;
            }
            return inputParameters;
        }
    }
}