using CGP.Foundation.Search.Models;
using CGP.Foundation.Search.Services;
using CGP.Foundation.SitecoreExtensions.Utilities;
using Sitecore.Configuration;
using Sitecore.Data.Items;
using SolrNet;
using SolrNet.Commands.Parameters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace CGP.Foundation.Search.Extensions
{
    public class SearchExtensions
    {
        public static SolrSearchResponse GetSearchResults(SolrSearchParameters searchParameters, List<string> searchableTemplates, bool isNotArticleFilterByPageAttribute, bool checkToLoadChildArticlesOnly)
        {
            List<FacetModel> globalFacets = new List<FacetModel>();
            if (isNotArticleFilterByPageAttribute)
            {
                globalFacets = GetGlobalFacetsOtherArticleType(searchParameters.CurrentItem);
            }
            else
            {
                var currentPageFacetField =
                    searchParameters.CurrentItem.TemplateID.Equals(Templates.SearchPageID)
                    || searchParameters.CurrentItem.TemplateID.Equals(Templates.ArticleLandingPageID)
                    || searchParameters.CurrentItem.TemplateID.Equals(Templates.ArticleCategoryLandingPageID)
                    || searchParameters.CurrentItem.TemplateID.Equals(Templates.OffersPageID)
                    ? Constants.SearchFacetAttributes
                    : Constants.FacetAttributes;

                globalFacets = GetGlobalFacets(searchParameters.CurrentItem, currentPageFacetField);
            }
            var searchTerm = StringUtil.RemoveSpecialCharactersExceptSpaceQuotes(searchParameters.Keyword).Trim();
            var searchFields = new[]
            {
                "title_t",
                "opengraphtitle_t",
                "opengraphdescription_t",
                "content_t",
                "navigationtitle_t"
            };

            AbstractSolrQuery solrQuery = new SolrQuery(string.Empty);

            foreach (var field in searchFields)
            {
                AbstractSolrQuery fieldQuery = new SolrQueryByField(field, StringUtil.RemoveSpecialCharactersExceptSpace(searchTerm));
                if (field.Equals("title_t"))
                {
                    fieldQuery = new SolrQueryBoost(new SolrQueryByField(field, StringUtil.RemoveSpecialCharactersExceptSpace(searchTerm)), 50f);
                }

                AbstractSolrQuery termQuery = new SolrQuery(string.Empty);
                if (!string.IsNullOrWhiteSpace(searchTerm) && !(searchTerm[0].Equals('"') && searchTerm[searchTerm.Length - 1].Equals('"')) && StringUtil.RemoveSpecialCharactersExceptSpace(searchTerm).Split(' ').Length > 0)
                {
                    foreach (var term in StringUtil.RemoveSpecialCharactersExceptSpace(searchTerm).Split(' '))
                    {
                        string tempTerm = term.Trim();
                        if (field.Equals("title_t"))
                        {
                            termQuery = termQuery || new SolrQueryBoost(new SolrQueryByFieldRegex(field, "/.*" + tempTerm + ".*/"), 5f);
                        }
                        else
                        {
                            termQuery = termQuery || new SolrQueryByFieldRegex(field, "/.*" + tempTerm + ".*/");
                        }
                    }
                }

                fieldQuery = fieldQuery || termQuery;
                solrQuery = solrQuery || fieldQuery;
            }

            var languageQuery = new SolrQueryByField("_language", searchParameters.CurrentItem.Language.Name);

            var completeQuery = new SolrQueryInList("templateid_s", searchableTemplates) && solrQuery && languageQuery;

            if (!searchParameters.IsLoadMore)
            {
                searchParameters.QueryOptions.Grouping = new GroupingParameters()
                {
                    Fields = new[] { "contenttype_s" },
                    Limit = searchParameters.QueryOptions.Rows,
                };
                searchParameters.QueryOptions.Facet = new FacetParameters()
                {
                    Queries =
                        globalFacets.Select(x => (ISolrFacetQuery)new SolrFacetFieldQuery(x.FacetValue)).ToList(),
                };
                searchParameters.QueryOptions.SpellCheck = new SpellCheckingParameters()
                {
                    Collate = true,
                    Count = 5,
                    Query = $"_name:({searchTerm})",
                };
            }

            if (searchParameters.CurrentItem.TemplateID.Equals(Templates.ProductGroupPage))
            {
                var pathQuery = new SolrQueryInList("_path", searchParameters.CurrentItem.ID.ToShortID().ToString().ToLower());
                var secondaryCategoryQuery = new SolrQueryByFieldRegex("secondarycategorylist_t", "/.*" + StringUtil.RemoveSpecialCharacters(searchParameters.CurrentItem.ID.ToString()) + ".*/");
                completeQuery = completeQuery && (pathQuery || secondaryCategoryQuery);
            }
            if (searchParameters.CurrentItem.TemplateID.ToString().Equals(Templates.ArticleLandingPage) || searchParameters.CurrentItem.TemplateID.ToString().Equals(Templates.ArticleCategoryLandingPage))
            {
                if (checkToLoadChildArticlesOnly)
                {
                    var pathQuery = new SolrQueryInList("_path", searchParameters.CurrentItem.ID.ToShortID().ToString().ToLower());
                    completeQuery = completeQuery && pathQuery;
                }
                var categoryQuery = new SolrQueryInList(string.Empty);
                if (searchParameters.CurrentItem.Parent.TemplateID.Equals(Templates.ProductGroupPage))
                {
                    categoryQuery = new SolrQueryInList("filter_by_category_sm", searchParameters.CurrentItem.Parent["NavigationTitle"]);
                }
                completeQuery = completeQuery && (categoryQuery);
            }

            searchParameters.Query = completeQuery;

            var searchService = new SearchService(ParseContextIndex(searchParameters));
            SolrSearchResponse returnObj = searchService.ContentSearchIndex(searchParameters);
            returnObj.IsNotArticleFilterByPageAttribute = isNotArticleFilterByPageAttribute;
            return returnObj;
        }

        private static string ParseContextIndex(SearchParameters searchParameters)
        {
            var indexName = string.Empty;
            if (searchParameters == null)
                return indexName;

            if (!string.IsNullOrEmpty(searchParameters.IndexName))
                indexName = searchParameters.IndexName;
            else
            {
                if (!string.IsNullOrEmpty(searchParameters.SiteName))
                {
                    indexName = searchParameters.SiteName.ToLower() + "_index";
                }
                else
                {
                    if (searchParameters.CurrentItem != null)
                    {
                        var currentSite = Factory.GetSiteInfoList().Where(s => s.RootPath != "" && searchParameters.CurrentItem.Paths.Path.ToLower().StartsWith(s.RootPath.ToLower()))
                            .OrderByDescending(s => s.RootPath.Length)
                            .FirstOrDefault();
                        var siteName = currentSite?.Name;
                        if (!string.IsNullOrEmpty(siteName))
                        {
                            indexName = siteName.ToLower() + "_index";
                        }
                    }
                }

                if (string.IsNullOrEmpty(indexName))
                {
                    var contextSiteName = Sitecore.Context.Site?.Name;
                    if (!string.IsNullOrEmpty(contextSiteName))
                    {
                        indexName = contextSiteName.ToLower() + "_index";
                    }
                }
            }

            return indexName;
        }

        public static List<FacetModel> GetGlobalFacets(Item item, string fieldName)
        {
            if (item != null)
            {
                var facetItems =
                    SitecoreUtil.GetMultiListFieldValues(item, fieldName);
                if (facetItems.Any())
                {
                    return ParseGlobalFacets(facetItems);
                }
            }
            return new List<FacetModel>();
        }
        private static List<FacetModel> ParseGlobalFacets(List<Item> facetItems)
        {
            return facetItems.Where(x => x != null).Select(x => new FacetModel()
            {
                FacetKey = (SitecoreUtil.GetFieldValue(x, Constants.Key)),
                FacetValue = (SitecoreUtil.GetFieldValue(x, Constants.Key).Replace(" ", "_") + "_sm").ToLower(),
            }).ToList();
        }

        public static SolrSearchResponse GetAutoSuggestResults(SolrSearchParameters searchParameters, NameValueCollection autoSuggestTemplates)
        {
            var searchTerm = searchParameters.Keyword;
            if (autoSuggestTemplates != null)
            {
                var searchFields = new[]
                {
                    "title_t",
                    "opengraphtitle_t",
                };

                AbstractSolrQuery solrQuery = new SolrQuery("");

                foreach (var field in searchFields)
                {
                    AbstractSolrQuery fieldQuery = new SolrQueryBoost(new SolrQueryByField(field, searchTerm), 5);
                    AbstractSolrQuery termQuery = new SolrQuery("*:*");
                    foreach (var term in searchTerm.Split(' '))
                    {
                        termQuery = termQuery && new SolrQueryByFieldRegex(field, "/.*" + term + ".*/");
                    }

                    fieldQuery = fieldQuery || termQuery;

                    solrQuery = solrQuery || fieldQuery;
                }

                var languageQuery = new SolrQueryByField("_language", Sitecore.Context.Language.Name);

                var completeQuery = new SolrQueryInList(
                    "templateid_s",
                    autoSuggestTemplates.AllKeys.Select(x => StringUtil.RemoveSpecialCharacters(x).ToLower()))
                                    && solrQuery && languageQuery;


                searchParameters.QueryOptions = new QueryOptions()
                {
                    Grouping = new GroupingParameters()
                    {
                        Fields = new[] { "templateid_s" },
                        Limit = 4,
                    },
                    SpellCheck = new SpellCheckingParameters()
                    {
                        Collate = true,
                        Count = 5,
                        Query = $"_name:({searchTerm})",
                    },
                };
                searchParameters.Query = completeQuery;

                var searchService = new SearchService(ParseContextIndex(searchParameters));
                return searchService.ContentSearchIndex(searchParameters);
            }

            return new SolrSearchResponse();
        }

        public static SolrSearchResponse InitiateSearch(InputParameters inputParameters, bool isLoadMore, bool checkToLoadChildArticlesOnly, List<string> searchableTemplates)
        {
            var filters = inputParameters.Filters.Select(x => (ISolrQuery)new SolrQueryInList(x.FilterKey, x.FilterValues)).ToList();
            var solrParameters = new SolrSearchParameters()
            {
                Keyword = inputParameters.SearchTerm,
                CurrentItem = string.IsNullOrWhiteSpace(inputParameters.CurrentItemId) ? Sitecore.Context.Item : Sitecore.Context.Database.GetItem(inputParameters.CurrentItemId, inputParameters.Language),
                QueryOptions = new QueryOptions()
                {
                    FilterQueries = new List<ISolrQuery>(filters),
                    StartOrCursor = new StartOrCursor.Start(inputParameters.SkipCount),

                },
                IsLoadMore = isLoadMore,
            };

            if (inputParameters.SearchCount != 0)
            {
                solrParameters.QueryOptions.Rows = inputParameters.SearchCount;
            }

            string sortField = inputParameters.ListingSortOrder;
            var sortOrder = inputParameters.ListingSortDirection;
            if (!string.IsNullOrEmpty(sortField))
            {
                solrParameters.QueryOptions.OrderBy = new Collection<SortOrder>
                                                    {
                                                    new SortOrder(sortField, sortOrder)
                                                    };
            }
            return GetSearchResults(solrParameters, searchableTemplates, inputParameters.IsNotArticleFilterByPageAttribute, checkToLoadChildArticlesOnly);
        }

        public static List<string> GetContentSearchGroupTemplateId(string searchType)
        {
            List<string> getContentSearchGroupTemplateId = new List<string>();
            switch (searchType.ToLower())
            {
                case "product":
                    {
                        getContentSearchGroupTemplateId.Add(Templates.ProductDetailPageIdString);
                        break;
                    }
                case "article":
                    {
                        getContentSearchGroupTemplateId.Add(Templates.ArticlePageIdString);
                        break;
                    }
                case "others":
                    {
                        getContentSearchGroupTemplateId.Add(Templates.ContentPageIdString);
                        getContentSearchGroupTemplateId.Add(Templates.ProductGroupPageIdString);
                        break;
                    }
                case "coupon":
                    {
                        getContentSearchGroupTemplateId.Add(Templates.CouponPageIdString);
                        break;
                    }
            }
            return getContentSearchGroupTemplateId.Select(s => (StringUtil.RemoveSpecialCharacters(s.ToLower()))).ToList();
        }

        public static List<FacetModel> GetGlobalFacetsOtherArticleType(Item currentItem, bool useDictionary = false)
        {
            List<FacetModel> globalFacets = new List<FacetModel>();
            string FilterbyBlogTopic = useDictionary ? SitecoreUtil.DictionaryValueByLanguage("Filter by Blog Topics", currentItem.Language) : Constants.BlogTopic;
            string FilterbyBlogType = useDictionary ? SitecoreUtil.DictionaryValueByLanguage("Filter by Blog Types", currentItem.Language) : Constants.BlogType;
            var facetItems =
                 SitecoreUtil.GetMultiListFieldValues(currentItem, Constants.SearchFacetAttributes);
            bool topicAdded = false;
            bool typeAdded = false;

            if (facetItems.Count > 0)
            {
                foreach (var facet in facetItems)
                {
                    if (facet.Name.ToLower().Contains(Constants.Topic) && (!topicAdded))
                    {
                        globalFacets.Add(AddFacet(FilterbyBlogTopic, Constants.BlogTopic.ToLower()));
                        topicAdded = true;
                    }
                    if (facet.Name.ToLower().Contains(Constants.Type) && (!typeAdded))
                    {
                        globalFacets.Add(AddFacet(FilterbyBlogType, Constants.BlogType.ToLower()));
                        typeAdded = true;
                    }
                }
            }
            else
            {
                globalFacets = new List<FacetModel>
                    {
                        new FacetModel()
                        {
                            FacetKey = FilterbyBlogTopic,
                            FacetValue = Constants.BlogTopic.ToLower() + "_sm"
                        },
                        new FacetModel()
                        {
                            FacetKey = FilterbyBlogType,
                            FacetValue = Constants.BlogType.ToLower() + "_sm"
                        }
                    };
            }

            return globalFacets;
        }

        public static FacetModel AddFacet(string key, string value)
        {
            return new FacetModel()
            {
                FacetKey = key,
                FacetValue = value + "_sm"
            };
        }
    }
}