using CGP.Feature.Search.Models;
using CGP.Foundation.Search;
using CGP.Foundation.Search.Models;
using CGP.Foundation.SitecoreExtensions.Model;
using CGP.Foundation.SitecoreExtensions.Utilities;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using SolrNet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace CGP.Feature.Search.Helper
{
    public class SearchHelper
    {
        public static string GetValueFromDatasource(string fieldName)
        {

            var dataSourceId = RenderingContext.CurrentOrNull.Rendering.DataSource;
            if (string.IsNullOrEmpty(dataSourceId)) return null;

            var dataSource = Sitecore.Context.Database.GetItem(dataSourceId);
            if (dataSource == null) return null;

            string text = dataSource.Fields[fieldName].ToString();
            return text;
        }
        public static void GetNoOfResults(GlobalSearchResult globalResultItems, int PageSize)
        {
            globalResultItems.Products.GroupItems = globalResultItems.Products.GroupItems?.Take(PageSize).ToList();
            globalResultItems.Articles.GroupItems = globalResultItems.Articles.GroupItems?.Take(PageSize).ToList();
            globalResultItems.Others.GroupItems = globalResultItems.Others.GroupItems?.Take(PageSize).ToList();
        }
        public static void SetTotalCount(GlobalSearchResult globalResultItems)
        {
            globalResultItems.Products.TotalCount = (globalResultItems.Products.GroupItems != null) ? globalResultItems.Products.GroupItems.Count() : 0;
            globalResultItems.Articles.TotalCount = (globalResultItems.Articles.GroupItems != null) ? globalResultItems.Articles.GroupItems.Count() : 0;
            globalResultItems.Others.TotalCount = (globalResultItems.Others.GroupItems != null) ? globalResultItems.Others.GroupItems.Count() : 0;
        }

        public static InputParameters ParseQueryStringParameters(InputParameters inputParameters, SearchCriteria searchCriteria)
        {
            Item currentItem = null;
            if (inputParameters.CurrentItemId != null)
            {
                currentItem = Sitecore.Context.Database.GetItem(inputParameters.CurrentItemId, inputParameters.Language);
            }

            if (currentItem == null)
            {
                currentItem = Sitecore.Context.Item;
                inputParameters.CurrentItemId = currentItem.ID.ToString();
            }

            IDictionary<string, IList<string>> globalFacets = new Dictionary<string, IList<string>>();
            if (currentItem.TemplateID.Equals(Templates.ProductListingPage) || currentItem.TemplateID.Equals(Templates.ProductGroupPage))
            {
                globalFacets = searchCriteria.ProductAttributes;
            }
            else if (currentItem.TemplateID.Equals(Templates.SearchPageID) || currentItem.TemplateID.Equals(Templates.OffersPageID))
            {
                globalFacets = searchCriteria.PageAttributes;
            }
            else if (currentItem.TemplateID.Equals(Templates.ArticleCategoryLandingPageID) || currentItem.TemplateID.Equals(Templates.ArticleLandingPageID))
            {
                if (inputParameters.ArticleListingFacetType.Equals(Constants.PageAttributes))
                {
                    globalFacets = searchCriteria.PageAttributes;
                }
                else
                {
                    var blogTopic = searchCriteria.BlogPostTopicAttributes;
                    var blogType = searchCriteria.ArticleTypeAttributes;
                    globalFacets = GetFacetTopicsandTypes(currentItem, blogTopic, blogType, false);
                    inputParameters.FilterString = inputParameters.FilterString?.Replace(SitecoreUtil.DictionaryValueByLanguage("Filter by Blog Topics", inputParameters.Language), Constants.BlogTopic).Replace(SitecoreUtil.DictionaryValueByLanguage("Filter by Blog Types", inputParameters.Language), Constants.BlogType);
                    inputParameters.IsNotArticleFilterByPageAttribute = true;
                }
            }

            inputParameters.SearchTerm = !string.IsNullOrWhiteSpace(inputParameters.SearchTerm)
                ? Convert.ToString(inputParameters.SearchTerm)
                : string.Empty;
            if (!string.IsNullOrWhiteSpace(inputParameters.FilterString))
            {
                var filterCollection = inputParameters.FilterString.Split(';');
                if (filterCollection.Any())
                {
                    foreach (var filter in filterCollection)
                    {
                        var filterSections = filter.Split(':');
                        var filterKey = filterSections[0];
                        if (filterSections.Length <= 1) break;
                        var filterValues = filterSections[1];
                        if (!string.IsNullOrEmpty(filterKey) && !string.IsNullOrEmpty(filterValues))
                        {
                            var parsedFacet = globalFacets.FirstOrDefault(x =>
                                x.Key.ToLower().Equals(filterKey.ToLower(), StringComparison.InvariantCultureIgnoreCase));
                            if (parsedFacet.Key != null)
                            {
                                var filterObject = new InputFilter()
                                {
                                    FilterKey = parsedFacet.Key.Replace(" ", "_").ToLower() + "_sm",
                                    FilterValues = filterValues.Split('|').ToList(),
                                };
                                inputParameters.Filters.Add(filterObject);
                            }
                        }
                    }
                }
            }
            return inputParameters;
        }

        public static void SetActiveFacets(GlobalSearchResult searchResult, List<InputFilter> filters, string filterOrder)
        {
            foreach (var searchFacet in searchResult.Facets)
            {
                if (searchFacet.FacetValues != null)
                {
                    foreach (var facet in searchFacet.FacetValues)
                    {
                        foreach (var matchedFilter in filters)
                        {
                            if (searchFacet.FacetValue.Equals(matchedFilter.FilterKey) && matchedFilter.FilterValues.Contains(facet.Key))
                            {
                                facet.IsActive = true;
                            }
                        }
                    }
                }
                if (string.IsNullOrWhiteSpace(filterOrder) && filterOrder.Equals(Constants.Alphabetical) && searchFacet.FacetValues != null)
                {
                    var sortedlist = searchFacet.FacetValues;
                    var newList = new Collection<FacetValue>();
                    foreach (var facet2 in searchFacet.FacetValues)
                    {
                        if (Regex.IsMatch(facet2.Key, @"^\d"))
                        {
                            newList.Add(facet2);
                            sortedlist = sortedlist.SkipWhile(x => x.Key.Equals(facet2.Key)).ToList();
                        }
                    }
                    foreach (var facet3 in newList)
                    {
                        sortedlist.Add(facet3);
                    }
                    searchFacet.FacetValues = sortedlist;
                }
            }
        }

        public static IDictionary<string, IList<string>> GetFacetTopicsandTypes(Item currentItem, List<string> blogTopicList, List<string> blogTypeList, bool useDictionary = false)
        {
            IDictionary<string, IList<string>> attributeList = new Dictionary<string, IList<string>>();
            string FilterbyBlogTopic = useDictionary ? SitecoreUtil.DictionaryValueByLanguage("Filter by Blog Topics", currentItem.Language) : Constants.BlogTopic;
            string FilterbyBlogType = useDictionary ? SitecoreUtil.DictionaryValueByLanguage("Filter by Blog Types", currentItem.Language) : Constants.BlogType;
            bool topicAdded = false;
            bool typeAdded = false;

            if (currentItem != null)
            {
                var facetItems =
                    SitecoreUtil.GetMultiListFieldValues(currentItem, Constants.SearchFacetAttributes);
                if (facetItems.Count > 0)
                {
                    foreach (var facet in facetItems)
                    {
                        if (facet.Name.ToLower().Contains(Constants.Topic) && blogTopicList.Count > 0 && !topicAdded)
                        {
                            attributeList.Add(FilterbyBlogTopic, blogTopicList);
                            topicAdded = true;
                        }

                        if (facet.Name.ToLower().Contains(Constants.Type) && blogTypeList.Count > 0 && !typeAdded)
                        {
                            attributeList.Add(FilterbyBlogType, blogTypeList);
                            typeAdded = true;
                        }
                    }
                }
                else
                {
                    if (blogTopicList.Count > 0)
                    {
                        attributeList.Add(FilterbyBlogTopic, blogTopicList);
                    }

                    if (blogTypeList.Count > 0)
                    {
                        attributeList.Add(FilterbyBlogType, blogTypeList);
                    }
                }
            }


            return attributeList;
        }

        public static bool IsSearchFilterDisabled()
        {
            bool status = false;
            var fromRenderingParameter = Convert.ToBoolean(Convert.ToInt32(HelperExtension.GetValueFromCurrentRenderingParameters(Constants.HideSearchFilter) ?? "0"));
            Sitecore.Data.Fields.CheckboxField checkboxField = Sitecore.Context.Item.Fields[Constants.DisableSearchFilter];

            if (checkboxField != null && (checkboxField.Checked || fromRenderingParameter))
            {
                status = true;
            }
            else if (checkboxField == null && fromRenderingParameter)
            {
                status = true;
            }
            return status;
        }

        public static Order GetSortDirection(string direction)
        {
            return (!string.IsNullOrWhiteSpace(direction)) && direction.Trim().ToLower().Equals(Constants.Asc) ? Order.ASC : Order.DESC;
        }
    }
}