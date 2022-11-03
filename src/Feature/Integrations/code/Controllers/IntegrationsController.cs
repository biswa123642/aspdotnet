using CGP.Foundation.SitecoreExtensions.Repositories;
using System.Web.Mvc;
using Sitecore.XA.Foundation.Mvc.Controllers;
using CGP.Feature.Integrations.Models;
using System.Text;
using Newtonsoft.Json;
using CGP.Feature.Integrations.Repositories;
using CGP.Foundation.SitecoreExtensions.Utilities;
using Sitecore.Mvc.Presentation;
using Sitecore.Collections;
using Sitecore.Data.Items;
using System.Linq;
using Sitecore.XA.Foundation.RenderingVariants.Controllers;

namespace CGP.Feature.Integrations.Controllers
{
    public class IntegrationsController : StandardController
    {
        private readonly ISiteConfiguration siteConfiguration;
        private readonly IIntegrationsRepository integrationsRepository;
        
        public IntegrationsController(ISiteConfiguration siteConfiguration, IIntegrationsRepository integrationsRepository)
        {
            this.siteConfiguration = siteConfiguration;
            this.integrationsRepository = integrationsRepository;
        }

        /// <summary>
        /// Method to load the Scripts in the Head Section of Website
        /// </summary>
        /// <returns></returns>
        public ActionResult LoadIntegrationHeadScripts()
        {
            IntegrationsViewModel integrationsViewModel = new IntegrationsViewModel();
            var getSiteConfiguration = siteConfiguration.GetSiteConfiguration();
            if (getSiteConfiguration.PowerReview.EnablePowerReviews && (Sitecore.Context.Item.TemplateID.Equals(Templates.ProductDetailPageTemplateId) || Sitecore.Context.Item.TemplateID.Equals(Templates.WriteReviewPageTemplateId)))
            {
                integrationsViewModel.EnablePowerReviews = true;
                integrationsViewModel.IsProductDetailPage = true;
                integrationsViewModel.PowerReviews = integrationsRepository.GetPowerReviewIntegrationSettings();
            }
            if (!getSiteConfiguration.PriceSpider.DisablePriceSpider || !getSiteConfiguration.PriceSpider.DisableExternalBrand || !getSiteConfiguration.PriceSpider.DisableStoreConfig)
            {
                integrationsViewModel.EnableWhereToBuy = true;
                integrationsViewModel.WhereToBuy = new WhereToBuy
                {
                    Account = getSiteConfiguration.PriceSpider.PriceSpiderAccount,
                    Config = getSiteConfiguration.PriceSpider.PriceSpiderConfig,
                    Country = getSiteConfiguration.PriceSpider.PriceSpiderCountry,
                    ExternalConfig = getSiteConfiguration.PriceSpider.ExternalBrandConfig,
                    StoreConfig = getSiteConfiguration.PriceSpider.StoreConfig
                };
            }

            if (getSiteConfiguration.SurveyPolls.EnableSurveyPolls)
            {
                integrationsViewModel.EnableSurveyPolls = true;
                integrationsViewModel.SurveyPolls = new SurveyPolls
                {
                    SurvicateWorkspaceID = getSiteConfiguration.SurveyPolls.SurvicateWorkspaceId
                };
            }
            //One Trust Cookies Banner section
            if (!getSiteConfiguration.CookiesBannerSettings.DisableCookiesBanner)
            {
                integrationsViewModel.EnableCookiesBanner = true;
                integrationsViewModel.OneTrustCookiesID = getSiteConfiguration.CookiesBannerSettings.OneTrustCookiesID;
            }
            //Googel Analytics section
            if (!getSiteConfiguration.GASettings.DisableGA)
            {
                integrationsViewModel.EnableGA = true;
                integrationsViewModel.GAID = getSiteConfiguration.GASettings.GAID;
                integrationsViewModel.GAIDGlobal = getSiteConfiguration.GASettings.GAIDGlobal;

            }
            //Googel Tag Manager section
            if (!getSiteConfiguration.GTMSettings.DisableGTM)
            {
                integrationsViewModel.EnableGTM = true;
                integrationsViewModel.GTMId = getSiteConfiguration.GTMSettings.GTMId;
            }
            

            integrationsViewModel.PageHeaderScripts = integrationsRepository.GetHeaderScripts(Sitecore.Context.Item, Fields.PageHeaderScripts);
            return View("~/Views/OneWeb/Integrations/IntegrationsHead.cshtml", integrationsViewModel);
        }

        /// <summary>
        /// Method to load the Scripts in the Body section of website (GTM)
        /// </summary>
        public ActionResult LoadIntegrationBodyScripts()
        {
            IntegrationsViewModel integrationsViewModel = new IntegrationsViewModel();
            var getSiteConfiguration = siteConfiguration.GetSiteConfiguration();
            if (!getSiteConfiguration.GTMSettings.DisableGTM)
            {
                integrationsViewModel.EnableGTM = true;
                integrationsViewModel.GTMId = getSiteConfiguration.GTMSettings.GTMId;
            }
            return View("~/Views/OneWeb/Integrations/IntegrationsBody.cshtml", integrationsViewModel);
        }


        /// <summary>
        /// Method returns html structure to render google ads
        /// </summary>
        /// <returns></returns>
        public ActionResult GetGoogleAds()
        {
            ViewResult viewResult = null;
            IntegrationsViewModel integrationsViewModel = new IntegrationsViewModel();
            var getSiteConfiguration = siteConfiguration.GetSiteConfiguration();
            if (!getSiteConfiguration.GoogleAdSenseSettings.DisableGoogleAdsense)
            {
                integrationsViewModel.DisableGoogleAd = true;
                integrationsViewModel.GoogleAdSense = new GoogleAdSense
                {
                    PublisherID = getSiteConfiguration.GoogleAdSenseSettings.PublisherID,
                    AdSlotID = getSiteConfiguration.GoogleAdSenseSettings.AdSlotID
                };
                viewResult = View("~/Views/OneWeb/Integrations/GoogleAdSenseBody.cshtml", integrationsViewModel);
            }

            return viewResult;
        }
        /// <summary>
        /// Method loads google adsense script to head section
        /// </summary>
        /// <returns></returns>
        public ActionResult LoadGoogleAdSenseHeadScript()
        {
            ViewResult viewResult = null;
            IntegrationsViewModel integrationsViewModel = new IntegrationsViewModel();
            var getSiteConfiguration = siteConfiguration.GetSiteConfiguration();
           
            if (!getSiteConfiguration.GoogleAdSenseSettings.DisableGoogleAdsense)
            {
                integrationsViewModel.DisableGoogleAd = true;
                integrationsViewModel.GoogleAdSense = new GoogleAdSense
                {
                    PublisherID = getSiteConfiguration.GoogleAdSenseSettings.PublisherID
                };
                viewResult = View("~/Views/OneWeb/Integrations/GoogleAdSenseHead.cshtml", integrationsViewModel);
            }
            return viewResult;
        }
    }
}
