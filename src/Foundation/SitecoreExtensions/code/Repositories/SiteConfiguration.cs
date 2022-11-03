using CGP.Foundation.SitecoreExtensions.Model;
using CGP.Foundation.SitecoreExtensions.Utilities;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using CGP.Foundation.ErrorModule.Repositiories;
using Sitecore.Data.Fields;
using System.Collections.Specialized;
using Sitecore.Data;
using System.Linq;
using System.Web.Security;

namespace CGP.Foundation.SitecoreExtensions.Repositories
{
    public class SiteConfiguration : ISiteConfiguration
    {
        private readonly ILogger logger;

        public SiteConfiguration(ILogger logger)
        {
            this.logger = logger;
        }
        public SiteConfigurationModel GetSiteConfiguration()
        {
            SiteConfigurationModel siteConfigurationModel = InitializeSiteConfigurationModel();

            try
            {
                Item siteConfigurationItem = HelperExtension.GetChildSiteSettingItem(Templates.SiteConfiguration.Id);
                if (siteConfigurationItem != null)
                {
                    siteConfigurationModel = new SiteConfigurationModel()
                    {
                        PriceSpider =
                        new PriceSpider
                        {
                            DisablePriceSpider = Utilities.FieldUtil.IsChecked(siteConfigurationItem, Templates.SiteConfiguration.Fields.DisablePriceSpider.ToString()),
                            PriceSpiderAccount = Utilities.ItemUtil.GetFieldValue(siteConfigurationItem, Templates.SiteConfiguration.Fields.PriceSpiderAccount),
                            PriceSpiderConfig = Utilities.ItemUtil.GetFieldValue(siteConfigurationItem, Templates.SiteConfiguration.Fields.PriceSpiderConfig),
                            PriceSpiderCountry = Utilities.ItemUtil.GetFieldValue(siteConfigurationItem, Templates.SiteConfiguration.Fields.PriceSpiderCountry),
                            DisableExternalBrand = Utilities.FieldUtil.IsChecked(siteConfigurationItem, Templates.SiteConfiguration.Fields.DisableExternalBrand.ToString()),
                            ExternalBrandConfig = Utilities.ItemUtil.GetFieldValue(siteConfigurationItem, Templates.SiteConfiguration.Fields.ExternalBrandConfig),
                            DisableStoreConfig = Utilities.FieldUtil.IsChecked(siteConfigurationItem, Templates.SiteConfiguration.Fields.DisableStoreConfig.ToString()),
                            StoreConfig = Utilities.ItemUtil.GetFieldValue(siteConfigurationItem, Templates.SiteConfiguration.Fields.StoreConfig)

                        },
                        PowerReview = new PowerReview
                        {
                            EnablePowerReviews = Utilities.FieldUtil.IsChecked(siteConfigurationItem, Templates.SiteConfiguration.Fields.EnablePowerReviews.ToString()),
                            PowerReviewsAPIKey = Utilities.ItemUtil.GetFieldValue(siteConfigurationItem, Templates.SiteConfiguration.Fields.PowerReviewsAPIKey),
                            PowerReviewsLocale = Utilities.ItemUtil.GetFieldValue(siteConfigurationItem, Templates.SiteConfiguration.Fields.PowerReviewsLocale),
                            PowerReviewsMapperURL = GetPowerReviewItemUrl(Utilities.ItemUtil.GetFieldValue(siteConfigurationItem, Templates.SiteConfiguration.Fields.PowerReviewsMapperURL)),
                            PowerReviewsMerchantGroupID = Utilities.ItemUtil.GetFieldValue(siteConfigurationItem, Templates.SiteConfiguration.Fields.PowerReviewsMerchantGroupID),
                            PowerReviewsMerchantID = Utilities.ItemUtil.GetFieldValue(siteConfigurationItem, Templates.SiteConfiguration.Fields.PowerReviewsMerchantID),
                            PowerReviewsSendProductInfo = Utilities.ItemUtil.GetFieldValue(siteConfigurationItem, Templates.SiteConfiguration.Fields.PowerReviewsSendProductInfo)
                        },
                        SeoSettings = new SeoSettings
                        {
                            EnableSeoSchema = Utilities.FieldUtil.IsChecked(siteConfigurationItem, Templates.SiteConfiguration.Fields.EnableSeoSchema.ToString()),
                            SelectSeoSchema = GetSelectedSchema(siteConfigurationItem)
                        },
                        BrandSettings = new BrandSettings
                        {
                            BrandName = Utilities.ItemUtil.GetFieldValue(siteConfigurationItem, Templates.SiteConfiguration.Fields.BrandName),
                            BrandLogo = GetImageUrl(siteConfigurationItem, Templates.SiteConfiguration.Fields.BrandLogo)
                        },
                        GTMSettings = new GTMSettings
                        {
                            DisableGTM = Utilities.FieldUtil.IsChecked(siteConfigurationItem, Templates.SiteConfiguration.Fields.DisableGTM.ToString()),
                            GTMId = Utilities.ItemUtil.GetFieldValue(siteConfigurationItem, Templates.SiteConfiguration.Fields.GTMId),
                        },
                        SearchCriteria = new SearchCriteria
                        {
                            CheckToDisableProductsSection = Utilities.FieldUtil.IsChecked(siteConfigurationItem, Templates.SiteConfiguration.Fields.CheckToDisableProductsSection.ToString()),
                            CheckToDisableArticlesSection = Utilities.FieldUtil.IsChecked(siteConfigurationItem, Templates.SiteConfiguration.Fields.CheckToDisableArticlesSection.ToString()),
                            CheckToDisableOtherSection = Utilities.FieldUtil.IsChecked(siteConfigurationItem, Templates.SiteConfiguration.Fields.CheckToDisableOtherSection.ToString()),
                            AdditionalSuggestedTemplates = GetAdditionalSuggestedTemplate(siteConfigurationItem),
                            ProductAttributes = AttributeList(siteConfigurationItem, Templates.SiteConfiguration.Fields.ProductAttributeLocation, Templates.ProductAttributesFolderTemplateId),
                            PageAttributes = AttributeList(siteConfigurationItem, Templates.SiteConfiguration.Fields.PageAttributeLocation, Templates.PageAttributesFolderTemplateId),
                            BlogPostTopicAttributes = ArticleAttributeList(siteConfigurationItem, Templates.SiteConfiguration.Fields.BlogPostTopicAttributeLocation, Templates.BlogTopicsFolderTemplateId),
                            ArticleTypeAttributes = ArticleAttributeList(siteConfigurationItem, Templates.SiteConfiguration.Fields.ArticleTypeAttributeLocation, Templates.BlogTypesFolderTemplateId),
                            Pagesize = Utilities.ItemUtil.GetFieldValue(siteConfigurationItem, Templates.SiteConfiguration.Fields.PageSize),
                            NumberOfScrolls = Utilities.ItemUtil.GetFieldValue(siteConfigurationItem, Templates.SiteConfiguration.Fields.NumberOfScrolls)
                        },

                        MediaSettings = new MediaSettings
                        {
                            NoImage = GetImageUrl(siteConfigurationItem, Templates.SiteConfiguration.Fields.NoImageID)
                        },
                        SurveyPolls = new SurveyPolls
                        {
                            EnableSurveyPolls = Utilities.FieldUtil.IsChecked(siteConfigurationItem, Templates.SiteConfiguration.Fields.EnableSurveyPolls.ToString()),
                            SurvicateWorkspaceId = Utilities.ItemUtil.GetFieldValue(siteConfigurationItem, Templates.SiteConfiguration.Fields.SurvicateWorkspaceId)
                        },
                        GASettings = new GASettings
                        {
                            DisableGA = Utilities.FieldUtil.IsChecked(siteConfigurationItem, Templates.SiteConfiguration.Fields.DisableGA.ToString()),
                            GAID = Utilities.ItemUtil.GetFieldValue(siteConfigurationItem, Templates.SiteConfiguration.Fields.GAId),
                            GAIDGlobal = Utilities.ItemUtil.GetFieldValue(siteConfigurationItem, Templates.SiteConfiguration.Fields.GAIdGlobal),

                        },
                        CookiesBannerSettings = new CookiesBannerSettings
                        {
                            DisableCookiesBanner = Utilities.FieldUtil.IsChecked(siteConfigurationItem, Templates.SiteConfiguration.Fields.DisableCookiesBanner.ToString()),
                            OneTrustCookiesID = Utilities.ItemUtil.GetFieldValue(siteConfigurationItem, Templates.SiteConfiguration.Fields.OneTrustCookiesID),
                        },
                        GoogleMapsAPIKey = new GoogleMapsAPIKey
                        {
                            OfficeLocationFinderAPIKey = Utilities.ItemUtil.GetFieldValue(siteConfigurationItem, Templates.SiteConfiguration.Fields.OfficeLocationFinderGoogleMapsAPIKey),
                            FoodPlotAPIKey = Utilities.ItemUtil.GetFieldValue(siteConfigurationItem, Templates.SiteConfiguration.Fields.FoodPlotGoogleMapsAPIKey),
                            FoodPlotJSONData = Utilities.ItemUtil.GetFieldValue(siteConfigurationItem, Templates.SiteConfiguration.Fields.FoodPlotJSONData)
                        },
                        UserLogin = new UserLogin
                        {
                            EnableLogin = Utilities.FieldUtil.IsChecked(siteConfigurationItem, Templates.SiteConfiguration.Fields.EnableLogin.ToString()),
                            usersList = UserList(siteConfigurationItem, Templates.SiteConfiguration.Fields.UsersList),
                            PasswordAuth = Utilities.FieldUtil.IsChecked(siteConfigurationItem, Templates.SiteConfiguration.Fields.PasswordAuth.ToString()),
                            PasswordAuthUser = PasswordAuthUser(siteConfigurationItem, Templates.SiteConfiguration.Fields.PasswordAuthUser),
                        },
                        GoogleAdSenseSettings = new GoogleAdSenseSettings
                        {
                            DisableGoogleAdsense = Utilities.FieldUtil.IsChecked(siteConfigurationItem, Templates.SiteConfiguration.Fields.DisableGoogleAds.ToString()),
                            PublisherID = Utilities.ItemUtil.GetFieldValue(siteConfigurationItem, Templates.SiteConfiguration.Fields.PublisherID),
                            AdSlotID = Utilities.ItemUtil.GetFieldValue(siteConfigurationItem, Templates.SiteConfiguration.Fields.AdSlotID),
                        }
                    };
                }
            }

            catch (Exception ex)
            {
                logger.LogError("ERROR in SiteConfiguration.GetSiteConfiguration() ", ex);

            }
            return siteConfigurationModel;
        }

        private string GetImageUrl(Item siteConfigurationItem, ID field)
        {
            try
            {
                ImageField imageField = siteConfigurationItem.Fields[field];
                return (imageField?.MediaItem !=null) ? Sitecore.Resources.Media.MediaManager.GetMediaUrl(imageField.MediaItem) : string.Empty;
            }
            catch (Exception ex)
            {

                logger.LogError("ERROR in SiteConfiguration.GetBrandLogoUri() ", ex);
                return string.Empty;
            }
        }

        private List<string> GetSelectedSchema(Item siteConfigurationItem)
        {
            List<string> schemaList = new List<string>();
            try
            {
                Sitecore.Data.Fields.MultilistField selectedSchemas = siteConfigurationItem.Fields[Templates.SiteConfiguration.Fields.SelectSeoSchema];
                foreach (Item schema in selectedSchemas.GetItems())
                {
                    schemaList.Add(schema.Name);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("ERROR in SiteConfiguration.GetSelectedSchema() ", ex);

            }
            return schemaList;
        }

        private string GetPowerReviewItemUrl(string powerReviewItemId)
        {
            string powerReviewItemUrl = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(powerReviewItemId))
                {
                    var powerReviewItem = HelperExtension.GetItem(powerReviewItemId);
                    powerReviewItemUrl = Sitecore.Links.LinkManager.GetItemUrl(powerReviewItem, new Sitecore.Links.UrlBuilders.ItemUrlBuilderOptions() { AlwaysIncludeServerUrl = true });
                }
            }
            catch (Exception ex)
            {
                logger.LogError("ERROR in SiteConfiguration.GetPowerReviewItemUrl() ", ex);
            }
            return powerReviewItemUrl;
        }

        private SiteConfigurationModel InitializeSiteConfigurationModel()
        {
            return new SiteConfigurationModel
            {
                BrandSettings = new BrandSettings(),
                PowerReview = new PowerReview(),
                PriceSpider = new PriceSpider(),
                SeoSettings = new SeoSettings { SelectSeoSchema = new List<string>() },
                GTMSettings = new GTMSettings(),
                SearchCriteria = new SearchCriteria(),
                SurveyPolls = new SurveyPolls(),
                GASettings = new GASettings(),
                CookiesBannerSettings = new CookiesBannerSettings(),
                MediaSettings = new MediaSettings()
            };
        }

        public NameValueCollection GetAdditionalSuggestedTemplate(Item siteConfigurationItem)
        {
            try
            {
                var autoSuggestedField = siteConfigurationItem?.Fields[Templates.SiteConfiguration.Fields.AdditionalSuggestedTemplates];
                if (autoSuggestedField != null && !string.IsNullOrWhiteSpace(autoSuggestedField.Value))
                {
                    return ((NameValueListField)autoSuggestedField).NameValues;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("ERROR in SiteConfiguration.GetAdditionalSuggestedTemplate() ", ex);
            }
            return new NameValueCollection();
        }

        private IDictionary<string, IList<string>> AttributeList(Item item, ID fieldId, ID targetItemTemplate)
        {
            IDictionary<string, IList<string>> attributeList = new Dictionary<string, IList<string>>();
            try
            {
                ReferenceField linkField = item.Fields[fieldId];
                if (linkField != null && linkField.TargetItem != null)
                {
                    Item attributeLocation = linkField.TargetItem;
                    if (attributeLocation != null && attributeLocation.TemplateID.Equals(targetItemTemplate))
                    {
                        foreach (Item attributeItem in attributeLocation.Axes.GetDescendants())
                        {
                            if (attributeItem.TemplateID.Equals(Templates.ProductAttributeTemplateID) || attributeItem.TemplateID.Equals(Templates.PageAttributeTemplateID))
                            {
                                var key = GetFacetKeyValue(attributeItem);
                                var facetValueList = GetFacetValueList(attributeItem);
                                if (attributeList.ContainsKey(key))
                                {
                                    var listValueToUpdate = attributeList[key];
                                    foreach (var facetValue in facetValueList)
                                    {
                                        attributeList[key].Add(facetValue);
                                    }
                                }
                                else
                                {
                                    attributeList.Add(key, facetValueList);
                                }
                            }
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError("ERROR in SiteConfiguration.AttributeList() ", ex);
            }
            return attributeList;
        }
        private List<string> ArticleAttributeList(Item item, ID fieldId, ID targetItemTemplate)
        {
            List<string> attributeList = new List<string>();
            try
            {
                ReferenceField linkField = item.Fields[fieldId];
                if (linkField != null && linkField.TargetItem != null)
                {
                    Item attributeLocation = linkField.TargetItem;
                    if (attributeLocation != null && attributeLocation.TemplateID.Equals(targetItemTemplate))
                    {
                        foreach (Item attributeItem in attributeLocation.Children)
                        {
                            var key = GetFacetKeyValue(attributeItem);

                            if (!string.IsNullOrEmpty(key))
                            {
                                attributeList.Add(key);
                            }
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError("ERROR in SiteConfiguration.ArticleAttributeList() ", ex);
            }
            return attributeList;
        }
        private List<string> UserList(Item item, ID fieldId)
        {
            List<string> usersList = new List<string>();
            try
            {
                MultilistField linkField = item.Fields[fieldId];

                if (linkField != null)
                {
                    var linkedUser = linkField.TargetIDs;

                    if (linkedUser.Count() > 0)
                    {
                        foreach (var user in linkedUser)
                        {
                            var membershipUser = Membership.GetUser(new Guid(user.ToString()));
                            if (membershipUser != null)
                            {
                                usersList.Add(membershipUser.UserName);
                            }
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError("ERROR in SiteConfiguration.ArticleAttributeList() ", ex);
            }
            return usersList;
        }
        private string PasswordAuthUser(Item item, ID fieldId)
        {
            try
            {
                MultilistField linkField = item.Fields[fieldId];

                if (linkField != null)
                {
                    var linkedUser = linkField.TargetIDs;

                    if (linkedUser.Count() > 0)
                    {
                        foreach (var user in linkedUser)
                        {
                            var membershipUser = System.Web.Security.Membership.GetUser(new Guid(user.ToString()));

                            if (membershipUser != null)
                                return membershipUser.UserName;
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError("ERROR in SiteConfiguration.ArticleAttributeList() ", ex);
            }
            return string.Empty;
        }

        private List<string> GetFacetValueList(Item item)
        {
            List<string> facetValueList = new List<string>();
            foreach (Item facetValueItem in item.GetChildren())
            {
                facetValueList.Add(GetFacetKeyValue(facetValueItem));
            }
            return facetValueList;
        }
        private string GetFacetKeyValue(Item item)
        {
            return item[Constants.Key];
        }
        private List<string> GetSortOptionLookup(Item item, ID fieldId)
        {
            List<string> lookupList = new List<string>();
            try
            {
                ReferenceField linkField = item.Fields[fieldId];
                if (linkField != null && linkField.TargetItem != null)
                {
                    Item lookupLocation = linkField.TargetItem;
                    if (lookupLocation != null)
                    {
                        foreach (Item lookupItem in lookupLocation.Children)
                        {
                            lookupList.Add(lookupItem.Name);
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError("ERROR in SiteConfiguration.GetSortOptionLookup() ", ex);
            }
            return lookupList;
        }
    }
}