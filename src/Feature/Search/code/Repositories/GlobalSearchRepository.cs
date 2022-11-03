using CGP.Feature.Search.Models;
using CGP.Foundation.Search;
using CGP.Foundation.Search.Extensions;
using CGP.Foundation.Search.Models;
using CGP.Foundation.SitecoreExtensions.Repositories;
using CGP.Foundation.SitecoreExtensions.Utilities;
using Newtonsoft.Json.Linq;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CGP.Foundation.ErrorModule.Repositiories;
using CGP.Feature.Search.Helper;
using CGP.Feature.Search.Models.ViewModel;
using Sitecore.XA.Foundation.Mvc.Repositories.Base;
using SolrNet;
using System.Collections.ObjectModel;

namespace CGP.Feature.Search.Repositories
{
    public class GlobalSearchRepository : ModelRepository, IGlobalSearchRepository
    {
        private readonly ISiteConfiguration siteConfiguration;
        public readonly ILogger _logger;
        public GlobalSearchRepository(ISiteConfiguration siteConfiguration, ILogger logger)
        {
            this.siteConfiguration = siteConfiguration;
            this._logger = logger;
        }
        public GlobalSearchResultViewModel GetGlobalSearchResults(InputParameters inputParameters)
        {
            GlobalSearchResultViewModel globalSearchResultViewModel = new GlobalSearchResultViewModel();

            try
            {
                int PageSize = int.Parse(siteConfiguration.GetSiteConfiguration().SearchCriteria.Pagesize);
                inputParameters.SearchCount = PageSize;
                SolrSearchResponse solrSearchResponse = SearchExtensions.InitiateSearch(inputParameters, false, inputParameters.CheckToLoadChildArticlesOnly, this.GetSearchableTemplates());
                if (solrSearchResponse.QueryResults.Count > 0)
                {
                    globalSearchResultViewModel.GlobalSearchResult = new GlobalSearchResult(solrSearchResponse, siteConfiguration.GetSiteConfiguration().SearchCriteria, inputParameters.FilterOrder)
                    {
                        FilterString = inputParameters.FilterString
                    };
                    SearchHelper.SetTotalCount(globalSearchResultViewModel.GlobalSearchResult);
                    if (PageSize > 0)
                    {
                        SearchHelper.GetNoOfResults(globalSearchResultViewModel.GlobalSearchResult, PageSize);
                    }
                }
                else
                {
                    globalSearchResultViewModel.GlobalSearchResult = new GlobalSearchResult(solrSearchResponse, siteConfiguration.GetSiteConfiguration().SearchCriteria, inputParameters.FilterOrder);
                    var suggestions = solrSearchResponse.QueryResults.SpellChecking.FirstOrDefault()?.Suggestions;
                    if (suggestions != null && suggestions.Any())
                    {
                        globalSearchResultViewModel.GlobalSearchResult.Suggestions = suggestions.ToList();
                    }
                    SearchHelper.SetTotalCount(globalSearchResultViewModel.GlobalSearchResult);
                    if (PageSize > 0)
                    {
                        SearchHelper.GetNoOfResults(globalSearchResultViewModel.GlobalSearchResult, PageSize);
                    }
                }
                FillBaseProperties(globalSearchResultViewModel);
                SearchHelper.SetActiveFacets(globalSearchResultViewModel.GlobalSearchResult, inputParameters.Filters, inputParameters.FilterOrder);

                string gridSelection = HelperExtension.GetValueFromCurrentRenderingParameters(CGP.Foundation.Search.Constants.LoadMore.GridSelection);
                globalSearchResultViewModel.GridSelection = !string.IsNullOrEmpty(gridSelection) ? gridSelection : "col-lg-3";
                globalSearchResultViewModel.HideDescriptionOnTiles = Convert.ToBoolean(Convert.ToInt32(HelperExtension.GetValueFromCurrentRenderingParameters(CGP.Foundation.Search.Constants.HideDescriptionOnTiles) ?? "0"));
                globalSearchResultViewModel.HideTitleOnTiles = Convert.ToBoolean(Convert.ToInt32(HelperExtension.GetValueFromCurrentRenderingParameters(CGP.Foundation.Search.Constants.HideTitleOnTiles) ?? "0"));
            }
            catch (Exception ex)
            {
                _logger.LogError("ERROR occured in GlobalSearchRepository.GlobalSearchResultViewModel() ", ex);
            }
            return globalSearchResultViewModel;
        }

        public List<GlobalResultItem> GetLoadMoreResults(InputParameters inputParameters, int currentPage, string searchType, string noOfResult)
        {
            inputParameters.Filters.Add(new InputFilter()
            {
                FilterKey = "templateid_s",
            });
            int pageSize = int.Parse(siteConfiguration.GetSiteConfiguration().SearchCriteria.Pagesize);
            int definedNoOfResults = Convert.ToInt32(noOfResult);

            SolrSearchResponse solrSearchResponse = SearchExtensions.InitiateSearch(inputParameters, true, inputParameters.CheckToLoadChildArticlesOnly, this.GetSearchableTemplates());

            if (inputParameters.CurrentItemId != null)
            {
                Item currentItem = Sitecore.Context.Database.GetItem(inputParameters.CurrentItemId);
                solrSearchResponse.CurrentItem = currentItem ?? Sitecore.Context.Item;
            }

            if (solrSearchResponse.QueryResults.Count > 0)
            {
                GlobalSearchResult globalSearchResult = new GlobalSearchResult(solrSearchResponse, siteConfiguration.GetSiteConfiguration().SearchCriteria, inputParameters.FilterOrder);

                switch (searchType)
                {
                    case "product":
                        {
                            if (currentPage == 1)
                                return (globalSearchResult.Products.GroupItems) != null ? globalSearchResult.Products.GroupItems.Skip(currentPage * pageSize).Take(definedNoOfResults).ToList() : null;
                            else
                                return (globalSearchResult.Products.GroupItems) != null ? globalSearchResult.Products.GroupItems.Skip(pageSize + (currentPage - 1) * definedNoOfResults).Take(definedNoOfResults).ToList() : null;
                        }
                    case "article":
                        {
                            if (currentPage == 1)
                                return (globalSearchResult.Articles.GroupItems) != null ? globalSearchResult.Articles.GroupItems.Skip(currentPage * pageSize).Take(definedNoOfResults).ToList() : null;
                            else
                                return (globalSearchResult.Articles.GroupItems) != null ? globalSearchResult.Articles.GroupItems.Skip(pageSize + (currentPage - 1) * definedNoOfResults).Take(definedNoOfResults).ToList() : null;
                        }
                    case "others":
                        {
                            if (currentPage == 1)
                                return (globalSearchResult.Others.GroupItems) != null ? globalSearchResult.Others.GroupItems.Skip(currentPage * pageSize).Take(definedNoOfResults).ToList() : null;
                            else
                                return (globalSearchResult.Others.GroupItems) != null ? globalSearchResult.Others.GroupItems.Skip(pageSize + (currentPage - 1) * definedNoOfResults).Take(definedNoOfResults).ToList() : null;
                        }
                    default:
                        return null;
                }
            }
            return null;
        }

        public List<JObject> GetSuggestions(InputParameters inputParameters, int MaxPredictiveResult = 10, int MaxProductCount = 1, int MaxArticleCount = 1, int MaxOthersCount = 1)
        {
            var autoSuggestResult = new List<JObject>();
            try
            {
                if (!string.IsNullOrEmpty(inputParameters.SearchTerm))
                {

                    var solrParameters = new SolrSearchParameters()
                    {
                        Keyword = inputParameters.SearchTerm,
                        CurrentItem = string.IsNullOrWhiteSpace(inputParameters.CurrentItemId) ? Sitecore.Context.Item : Sitecore.Context.Database.GetItem(inputParameters.CurrentItemId),
                        QueryOptions = new SolrNet.Commands.Parameters.QueryOptions
                        {
                            Rows = MaxPredictiveResult,
                            OrderBy = new Collection<SortOrder>
                                                    {
                                                    new SortOrder("title_alpha_sort", Order.ASC)
                                                    }
                        }
                    };

                    var autoSuggestTemplates = GetSearchableTemplates();
                    if (autoSuggestTemplates.Count <= 0)
                    {
                        return new List<JObject>();
                    }

                    var searchResults = SearchExtensions.GetSearchResults(solrParameters, autoSuggestTemplates, inputParameters.IsNotArticleFilterByPageAttribute, inputParameters.CheckToLoadChildArticlesOnly);

                    if (searchResults.QueryResults.Count > 0)
                    {
                        var searchGroups = searchResults.QueryResults?.Grouping?.Values?.FirstOrDefault()?.Groups;
                        if (searchGroups != null)
                        {
                            int[] resultTakeCount = new int[3] { MaxProductCount, MaxArticleCount, MaxOthersCount };
                            int counter = 0;
                            Dictionary<string, int> searchGroupKeyList = new Dictionary<string, int>();
                            searchGroupKeyList.Add("Product", MaxProductCount);
                            searchGroupKeyList.Add("Article", MaxArticleCount);
                            searchGroupKeyList.Add("Other", MaxOthersCount);

                            foreach (var groupKey in searchGroupKeyList)
                            {
                                var currentGroup = searchGroups.FirstOrDefault(x =>
                                    x.GroupValue.ToLower().Equals(groupKey.Key.ToLower()));
                                counter = 0;
                                if (currentGroup != null)
                                {
                                    foreach (var document in currentGroup.Documents)
                                    {
                                        if (currentGroup != null && autoSuggestResult.Count < MaxPredictiveResult && counter < groupKey.Value)
                                        {
                                            counter++;
                                            string url = !(String.IsNullOrEmpty(document.RedirectUrl)) ? document.RedirectUrl : document.ContentUrl;
                                            JObject json = new JObject();
                                            json["title"] = document.NavigationTitle;
                                            json["link"] = HttpUtility.HtmlEncode((string)url);
                                            autoSuggestResult.Add(json);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("ERROR occured in GlobalSearchRepository.AutoSuggestResult() ", ex);
            }
            return autoSuggestResult;
        }

        public List<string> GetSearchableTemplates()
        {
            List<string> searchableTemplateList = new List<string>()
            {
                StringUtil.RemoveSpecialCharacters(Templates.ProductDetailPageIdString).ToLower(),
                StringUtil.RemoveSpecialCharacters(Templates.ArticlePageIdString).ToLower(),
                StringUtil.RemoveSpecialCharacters(Templates.ContentPageIdString).ToLower(),
                StringUtil.RemoveSpecialCharacters(Templates.ProductGroupPageIdString).ToLower(),
                StringUtil.RemoveSpecialCharacters(Templates.CouponPageIdString).ToLower(),
                StringUtil.RemoveSpecialCharacters(Templates.WriteAReviewPageIdString).ToLower(),
                StringUtil.RemoveSpecialCharacters(Templates.HomePageIdString).ToLower(),
                StringUtil.RemoveSpecialCharacters(Templates.ProductListingPageIdString).ToLower(),
            };

            if (siteConfiguration.GetSiteConfiguration() != null)
            {
                foreach (string item in siteConfiguration.GetSiteConfiguration().SearchCriteria.AdditionalSuggestedTemplates)
                {
                    searchableTemplateList.Add(StringUtil.RemoveSpecialCharacters(item).ToLower());
                }
            }
            return searchableTemplateList.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
        }

        public List<SolrModel> GetLoadMoreResults(InputParameters inputParameters, string searchType)
        {
            List<SolrModel> searchResult = new List<SolrModel>();
            try
            {
                List<string> getContentSearchGroupTemplateId = new List<string>();
                var contextItem = Sitecore.Context.Database.GetItem(inputParameters.CurrentItemId, inputParameters.Language);
                if (contextItem != null && contextItem.TemplateID.ToString().Equals(Templates.SearchPageIdString) && searchType.ToLower().Equals("others"))
                {
                    getContentSearchGroupTemplateId.Add(Templates.ContentPageIdString);
                    getContentSearchGroupTemplateId.Add(Templates.ProductGroupPageIdString);
                    getContentSearchGroupTemplateId.Add(Templates.CouponPageIdString);
                    getContentSearchGroupTemplateId.Add(Templates.HomePageIdString);
                    getContentSearchGroupTemplateId.Add(Templates.WriteAReviewPageIdString);
                    getContentSearchGroupTemplateId.Add(Templates.ProductListingPageIdString);
                    getContentSearchGroupTemplateId = getContentSearchGroupTemplateId.Select(s => (StringUtil.RemoveSpecialCharacters(s.ToLower()))).ToList();
                }
                else
                {
                    getContentSearchGroupTemplateId = SearchExtensions.GetContentSearchGroupTemplateId(searchType);
                }
                searchResult = SearchExtensions.InitiateSearch(inputParameters, true, inputParameters.CheckToLoadChildArticlesOnly, getContentSearchGroupTemplateId).QueryResults;
            }
            catch (Exception ex)
            {
                _logger.LogError("ERROR occured in GlobalSearchRepository.GetLoadMoreResults() ", ex);
            }
            return searchResult;
        }
    }
}