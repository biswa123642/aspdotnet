using CGP.Feature.Integrations.Models;
using CGP.Foundation.ErrorModule.Repositiories;
using CGP.Foundation.SitecoreExtensions.Repositories;
using Newtonsoft.Json;
using Sitecore.Data.Items;
using System;

namespace CGP.Feature.Integrations.Repositories
{
    public class IntegrationsRepository : IIntegrationsRepository
    {
        private readonly ILogger logger;
        private readonly ISiteConfiguration siteConfiguration;

        public IntegrationsRepository(ILogger logger, ISiteConfiguration siteConfiguration)
        {
            this.logger = logger;
            this.siteConfiguration = siteConfiguration;
        }
        public string GetPowerReviewIntegrationSettings()
        {
            PowerReviews powerReview = new PowerReviews();
            try
            {
                var getSiteConfiguration = siteConfiguration.GetSiteConfiguration();
                powerReview.Apikey = getSiteConfiguration.PowerReview.PowerReviewsAPIKey;
                powerReview.Locale = getSiteConfiguration.PowerReview.PowerReviewsLocale;
                powerReview.MerchantGroupID = getSiteConfiguration.PowerReview.PowerReviewsMerchantGroupID;
                powerReview.MerchantID = getSiteConfiguration.PowerReview.PowerReviewsMerchantID;
                var legacyPageID = Sitecore.Context.Item[Constants.PowerReview.LegacyProductGUID];
                powerReview.PageID = !string.IsNullOrEmpty(legacyPageID) ? legacyPageID : Sitecore.Context.Item.ID.ToString().Replace("{", "").Replace("}", "").Replace("-", "");
                if (Sitecore.Context.PageMode.IsExperienceEditor || Sitecore.Context.PageMode.IsPreview)
                {
                    powerReview.ReviewWrapperURL = getSiteConfiguration.PowerReview.PowerReviewsMapperURL + "&pr_page_id=" + powerReview.PageID;
                }
                else
                {
                    powerReview.ReviewWrapperURL = getSiteConfiguration.PowerReview.PowerReviewsMapperURL + "/?pr_page_id=" + powerReview.PageID;
                }
                powerReview.SendProductInfo = getSiteConfiguration.PowerReview.PowerReviewsSendProductInfo;
                if (!Sitecore.Context.Item.TemplateID.Equals(Templates.WriteReviewPageTemplateId))
                {
                    powerReview.Components = new Components()
                    {
                        ReviewDisplay = Constants.PowerReview.ReviewDisplay,
                        ReviewSnippet = Constants.PowerReview.ReviewSnippet
                    };
                }
                else
                {
                    powerReview.Components = new Components()
                    {
                        Write = Constants.PowerReview.Write
                    };
                }
                return JsonConvert.SerializeObject(powerReview, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore});
            }
            catch (Exception ex)
            {
                logger.LogError("ERROR in PowerReviewIntegration.GetPowerReviewIntegrationSettings() | ", ex);
                return string.Empty;
            }
        }
        public string GetHeaderScripts(Item item, Sitecore.Data.ID fieldId)
        {
            return Foundation.SitecoreExtensions.Utilities.ItemUtil.GetFieldValue(item,fieldId);
        }
    }
}