using CGP.Feature.Search.Helper;
using CGP.Foundation.Search;
using CGP.Foundation.Search.Extensions;
using CGP.Foundation.Search.Models;
using CGP.Foundation.SitecoreExtensions.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CGP.Feature.Search.Models
{
    public class GlobalSearchResult
    {
        public GlobalSearchResult(SolrSearchResponse solrResponse, SearchCriteria searchCriteria, string filterSortOrder = "Alphabetical")
        {
            Products = new GlobalSearchGroup();
            Articles = new GlobalSearchGroup();
            Others = new GlobalSearchGroup();
            Facets = new List<FacetResults>();

            if (solrResponse != null)
            {
                SearchTerm = solrResponse.SearchTerm;
                var searchGroups = solrResponse.QueryResults.Grouping.Values.FirstOrDefault()?.Groups;
                if (searchGroups != null && searchGroups.Any())
                {
                    TotalResultCount = solrResponse.QueryResults.Grouping.Values.FirstOrDefault()?.Matches ?? 0;
                    var productGroup = searchGroups.FirstOrDefault(x => x.GroupValue.ToLower().Equals("product"));
                    if (productGroup != null)
                    {
                        Products = new GlobalSearchGroup()
                        {
                            GroupIdentifier = productGroup.GroupValue,
                            GroupCount = productGroup.NumFound,
                            GroupItems = productGroup.Documents.Select(x => new GlobalResultItem()
                            {
                                ResultItem = x,
                            }).ToList(),
                            ContentType = Sitecore.Globalization.Translate.Text("Product")
                        };

                    }

                    var articleGroup = searchGroups.FirstOrDefault(x => x.GroupValue.ToLower().Equals("article"));
                    if (articleGroup != null)
                    {
                        Articles = new GlobalSearchGroup()
                        {
                            GroupIdentifier = articleGroup.GroupValue,
                            GroupCount = articleGroup.NumFound,
                            GroupItems = articleGroup.Documents.Select(x => new GlobalResultItem()
                            {
                                ResultItem = x,
                            }).ToList(),
                            ContentType = Sitecore.Globalization.Translate.Text("Article")
                        };

                    }

                    var otherGroup = searchGroups.FirstOrDefault(x => x.GroupValue.ToLower().Equals("other"));
                    if (otherGroup != null)
                    {
                        Others = new GlobalSearchGroup()
                        {
                            GroupIdentifier = otherGroup.GroupValue,
                            GroupCount = otherGroup.NumFound,
                            GroupItems = otherGroup.Documents.Select(x => new GlobalResultItem()
                            {
                                ResultItem = x,
                            }).ToList(),
                            ContentType = Sitecore.Globalization.Translate.Text("Other")
                        };
                    }
                }

                if (solrResponse.QueryResults?.FacetFields != null)
                {
                    var facetFields = solrResponse.QueryResults.FacetFields;
                    List<FacetModel> globalFacets = new List<FacetModel>();
                    IDictionary<string, IList<string>> globalFacets2 = new Dictionary<string, IList<string>>();

                    if (solrResponse.IsNotArticleFilterByPageAttribute)
                    {
                        globalFacets = SearchExtensions.GetGlobalFacetsOtherArticleType(solrResponse.CurrentItem, true);
                        globalFacets2 = SearchHelper.GetFacetTopicsandTypes(solrResponse.CurrentItem, searchCriteria.BlogPostTopicAttributes, searchCriteria.ArticleTypeAttributes, true);
                    }
                    else
                    {
                        var currentPageFacetField =
                            solrResponse.CurrentItem.TemplateID.Equals(Templates.SearchPageID)
                            || solrResponse.CurrentItem.TemplateID.Equals(Templates.ArticleLandingPageID)
                            || solrResponse.CurrentItem.TemplateID.Equals(Templates.ArticleCategoryLandingPageID)
                            || solrResponse.CurrentItem.TemplateID.Equals(Templates.OffersPageID)
                            ? Constants.SearchFacetAttributes
                            : Constants.FacetAttributes;

                        globalFacets = SearchExtensions.GetGlobalFacets(solrResponse.CurrentItem, currentPageFacetField);
                        globalFacets2 = solrResponse.CurrentItem.TemplateID.Equals(Templates.SearchPageID)
                            || solrResponse.CurrentItem.TemplateID.Equals(Templates.ArticleLandingPageID)
                            || solrResponse.CurrentItem.TemplateID.Equals(Templates.ArticleCategoryLandingPageID)
                            || solrResponse.CurrentItem.TemplateID.Equals(Templates.OffersPageID)
                            ? searchCriteria.PageAttributes
                            : searchCriteria.ProductAttributes;
                    }

                    if (globalFacets != null)
                    {
                        foreach (var facetKey in globalFacets)
                        {
                            if (facetFields.ContainsKey(facetKey.FacetValue))
                            {
                                var facetCategory = new FacetResults();

                                facetCategory.FacetValue = facetKey.FacetValue;
                                facetCategory.FacetKey = facetKey.FacetKey;
                                if (filterSortOrder.Equals(Constants.Alphabetical))
                                {
                                    facetCategory.FacetValues = facetFields[facetKey.FacetValue].Select(x =>
                                                                    new FacetValue
                                                                    {
                                                                        Key = x.Key,
                                                                        Value = x.Value,
                                                                    }).OrderBy(y => y.Key).ToArray();
                                }
                                else
                                {
                                    var parsedFacet = globalFacets2.FirstOrDefault(x =>
                                                         x.Key.ToLower().Equals(facetKey.FacetKey.ToLower(), StringComparison.InvariantCultureIgnoreCase));
                                    ICollection<KeyValuePair<string, int>> facetCollection1 = facetFields[facetKey.FacetValue];
                                    ICollection<KeyValuePair<string, int>> facetCollection2 = new Collection<KeyValuePair<string, int>>();
                                    if (parsedFacet.Value != null)
                                    {
                                        foreach (var item in parsedFacet.Value)
                                        {
                                            foreach (var item2 in facetCollection1)
                                            {
                                                if (item.Equals(item2.Key))
                                                {
                                                    facetCollection2.Add(item2);
                                                }
                                            }
                                        }
                                        facetCategory.FacetValues = facetCollection2.Select(x =>
                                                                        new FacetValue
                                                                        {
                                                                            Key = x.Key,
                                                                            Value = x.Value,
                                                                        }).ToArray();
                                    }
                                }
                                Facets.Add(facetCategory);
                            }
                        }
                    }
                }
            }
        }

        public GlobalSearchGroup Products { get; set; }
        public GlobalSearchGroup Articles { get; set; }
        public GlobalSearchGroup Others { get; set; }
        public List<FacetResults> Facets { get; set; }
        public int TotalResultCount { get; set; }
        public string FilterString { get; set; }
        public string SearchTerm { get; set; }
        public List<string> Suggestions { get; set; }
    }
}