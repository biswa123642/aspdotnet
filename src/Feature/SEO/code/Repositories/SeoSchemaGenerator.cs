using CGP.Feature.SEO.Models.StructuredData;
using CGP.Foundation.ErrorModule.Repositiories;
using CGP.Foundation.SitecoreExtensions.Repositories;
using CGP.Foundation.SitecoreExtensions.Utilities;
using Newtonsoft.Json;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CGP.Feature.SEO.Repositories
{
    public class SeoSchemaGenerator : ISeoSchemaGenerator
    {
        private readonly ILogger logger;
        private readonly ISiteConfiguration siteConfiguration;
        private static readonly Regex formatRegex = new Regex("<[^>]*>");
        public SeoSchemaGenerator(ILogger logger, ISiteConfiguration siteConfiguration)
        {
            this.logger = logger;
            this.siteConfiguration = siteConfiguration;
        }
        public string CreateBreadcrumbStructuredData(JsonSerializerSettings jsonSettings)
        {
            BreadCrumbListSchema breadCrumbListSchema = new BreadCrumbListSchema();
            try
            {
                var homeItem = HelperExtension.GetHomeItem();
                var contextItem = Sitecore.Context.Item;
                if (homeItem != null)
                {
                    var breadcrumbItems = new List<Item>();
                    breadcrumbItems.Add(contextItem);
                    while (!contextItem.ID.Equals(homeItem.ID))
                    {
                        contextItem = contextItem.Parent;
                        if (contextItem.Visualization.Layout != null)
                        {
                            breadcrumbItems.Add(contextItem);
                        }
                    }
                    int counter = 0;
                    breadcrumbItems.Reverse();
                    breadCrumbListSchema.ItemListElement = breadcrumbItems.Select(item => new ListItem
                    {
                        Name = item.Fields[Constants.SeoSchema.BreadcrumbNavigationTitle].ToString(),
                        Position = (++counter).ToString(),
                        Item = Sitecore.Links.LinkManager.GetItemUrl(item, new Sitecore.Links.UrlBuilders.ItemUrlBuilderOptions() { AlwaysIncludeServerUrl = true }),
                    }).ToList();
                }
                return JsonConvert.SerializeObject(breadCrumbListSchema, jsonSettings);
            }
            catch (Exception ex)
            {
                logger.LogError("ERROR in SeoSchemaGenerator.CreateBreadcrumbStructuredData() | ", ex);
                return string.Empty;
            }
        }
        public string CreateFAQStructuredData(JsonSerializerSettings jsonSettings)
        {
            try
            {
                var contextItem = Sitecore.Context.Item;
                List<Sitecore.Layouts.RenderingReference> faqComponent = contextItem.Visualization.GetRenderings(Sitecore.Context.Device, false)
                    .Where(i => i.WebEditDisplayName == Constants.SeoSchema.Accordian && i.Settings.DataSource != null).ToList();

                if (faqComponent.Count > 0)
                {
                    FAQPageSchema faqPageSchema = new FAQPageSchema
                    {
                        MainEntity = new List<Question>()
                    };
                    for (int i = 0; i < faqComponent.Count; i++)
                    {
                        Item faqDataSourceItem = Sitecore.Context.Database.GetItem(faqComponent[i].Settings.DataSource);
                        foreach (Item accordianItems in faqDataSourceItem.GetChildren())
                        {
                            if (accordianItems != null)
                            {
                                faqPageSchema.MainEntity.Add(new Question()
                                {
                                    Name = formatRegex.Replace(accordianItems.Fields[Constants.SeoSchema.Heading].Value,""),
                                    AcceptedAnswer = new AcceptedAnswer()
                                    {
                                        Text =formatRegex.Replace(accordianItems.Fields[Constants.SeoSchema.Content].Value,"")
                                    },
                                });
                            }
                        }
                    }
                    return (faqPageSchema.MainEntity != null) ? JsonConvert.SerializeObject(faqPageSchema, jsonSettings) : string.Empty;
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                logger.LogError("ERROR in SeoSchemaGenerator.CreateFAQStructuredData() | ", ex);
                return string.Empty;
            }
        }
        public string CreateOrganizationStructuredData(JsonSerializerSettings jsonsettings, string logoUrl)
        {
            try
            {
                var contextItem = Sitecore.Context.Item;
                OrganizationSchema organizationSchema = new OrganizationSchema();
                organizationSchema.Name = Sitecore.Context.GetSiteName();
                organizationSchema.Url = Sitecore.Links.LinkManager.GetItemUrl(contextItem, new Sitecore.Links.UrlBuilders.ItemUrlBuilderOptions() { AlwaysIncludeServerUrl = true });
                organizationSchema.Logo = HelperExtension.GetHostName() + logoUrl;
                return JsonConvert.SerializeObject(organizationSchema, jsonsettings);
            }
            catch (Exception ex)
            {
                logger.LogError("ERROR in SeoSchemaGenerator.CreateFAQStructuredData() | ", ex);
                return string.Empty;
            }
        }
        public string CreateProductStructuredData(JsonSerializerSettings jsonSettings, string brandName, string brandLogoUrl, Item variant)
        {
            try
            {
                Item contextItem = Sitecore.Context.Item;
                List<ProductSchema> productSchemas = new List<ProductSchema>();
                ProductSchema productSchema = new ProductSchema();
                Sitecore.Data.Fields.MultilistField mediaList = variant.Fields[Constants.SeoSchema.ProductMediaList];
                Item[] mediaItems = mediaList.GetItems();
                productSchema.Name = variant.Fields[Constants.SeoSchema.VariantTitle].Value.ToString();
                productSchema.Url = Sitecore.Links.LinkManager.GetItemUrl(contextItem, new Sitecore.Links.UrlBuilders.ItemUrlBuilderOptions() { AlwaysIncludeServerUrl = true });
                productSchema.Description = contextItem.Fields[Constants.SeoSchema.MetaDescription].Value.ToString();
                productSchema.Sku = variant.Fields[Constants.SeoSchema.VariantSKU].Value.ToString();
                productSchema.Gtin12 = variant.Fields[Constants.SeoSchema.VariantUPC].Value.ToString();
                productSchema.Brand = new BrandSchema()
                {
                    Name = brandName,
                    Logo = HelperExtension.GetHostName() + brandLogoUrl
                };
                if(mediaItems.Count() > 0)
                {
                    productSchema.Image = new ImageSchema()
                    {
                        Url = HelperExtension.GetHostName() + mediaItems[0].GetMediaUrl()
                    };
                }
                productSchemas.Add(productSchema);
                return JsonConvert.SerializeObject(productSchemas, jsonSettings);
            }
            catch (Exception ex)
            {
                logger.LogError("ERROR in SeoSchemaGenerator.CreateProductStructuredData() | ", ex);
                return string.Empty;
            }
        }
        public string CreateWebsiteStructuredData(JsonSerializerSettings jsonSettings, string brandName)
        {
            try
            {
                WebsiteSchema webSiteSchema = new WebsiteSchema();
                webSiteSchema.Name = Sitecore.Context.GetSiteName();
                webSiteSchema.Alternatename = brandName;
                webSiteSchema.Url = Sitecore.Links.LinkManager.GetItemUrl(Sitecore.Context.Item, new Sitecore.Links.UrlBuilders.ItemUrlBuilderOptions() { AlwaysIncludeServerUrl = true });
                webSiteSchema.potentialAction = new PotentialAction()
                {
                    Target = webSiteSchema.Url + Constants.SeoSchema.SearchUrl
                };
                return JsonConvert.SerializeObject(webSiteSchema, jsonSettings);
            }
            catch (Exception ex)
            {
                logger.LogError("ERROR occured in SeoSchemaGenerator.CreateWebsiteStructuredData() ", ex);
                return string.Empty;
            }
        }
        public string CreateArticleStructuredData(JsonSerializerSettings jsonSettings, string brandLogoURL, string brandName)
        {
            try
            {
                ArticleSchema articleSchema = new ArticleSchema();
                Item contextItem = Sitecore.Context.Item;
                articleSchema.MainEntityOfPage = new MainEntityOfPageSchema()
                {
                    Id = Sitecore.Links.LinkManager.GetItemUrl(contextItem, new Sitecore.Links.UrlBuilders.ItemUrlBuilderOptions() { AlwaysIncludeServerUrl = true })
                };
                articleSchema.Headline = contextItem.Fields[Constants.SeoSchema.ArticleTitle].Value.ToString();
                articleSchema.Image = HelperExtension.GetHostName() + SitecoreUtil.GetMediaItemUrl(contextItem.Fields[Constants.SeoSchema.ArticleImage]);
                Sitecore.Data.Fields.DateField articlePublishedDate = contextItem.Fields[Constants.SeoSchema.ArticlePublishedDate];
                articleSchema.DatePublished = articlePublishedDate.DateTime.ToString();
                articleSchema.Author = new AuthorSchema()
                {
                    Name = contextItem.Fields[Constants.SeoSchema.ArticleAuthorName].Value.ToString()
                };
                articleSchema.publisher = new PublisherSchema()
                {
                    Name = brandName,
                    Logo = new ImageObjectSchema()
                    {
                        Url = HelperExtension.GetHostName() + brandLogoURL
                    },
                };
                articleSchema.Description = contextItem.Fields[Constants.SeoSchema.MetaDescription].Value.ToString();
                return JsonConvert.SerializeObject(articleSchema, jsonSettings);

            }
            catch(Exception ex)
            {
                logger.LogError("ERROR occured in SeoSchemaGenerator.CreateArticleStructuredData() ", ex);
                return string.Empty;
            }
        }
    }
}