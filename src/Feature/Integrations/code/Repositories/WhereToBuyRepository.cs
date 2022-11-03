using CGP.Feature.Integrations.Models;
using CGP.Foundation.ErrorModule.Repositiories;
using CGP.Foundation.SitecoreExtensions.Utilities;
using System;
using Sitecore.XA.Foundation.RenderingVariants.Repositories;
using Sitecore.Mvc.Presentation;
using CGP.Foundation.SitecoreExtensions.Repositories;

namespace CGP.Feature.Integrations.Repositories
{
    public class WhereToBuyRepository : VariantsRepository, IWhereToBuyRepository
    {
        private readonly ILogger logger;
        private readonly ISiteConfiguration siteConfiguration;
        private readonly IIntegrationsRepository integrationsRepository;

        public WhereToBuyRepository(ILogger logger, ISiteConfiguration siteConfiguration, IIntegrationsRepository integrationsRepository)
        {
            this.logger = logger;
            this.siteConfiguration = siteConfiguration;
            this.integrationsRepository = integrationsRepository;
        }

        public WhereToBuy GetWhereToBuyDetails()
        {

            WhereToBuy whereToBuyViewModel = new WhereToBuy();

            FillBaseProperties(whereToBuyViewModel);

            try
            {
                if (siteConfiguration.GetSiteConfiguration().PriceSpider.DisablePriceSpider)
                {
                    return new WhereToBuy();
                }

                string brandType = HelperExtension.GetValueFromCurrentRenderingParameters(Constants.RenderingParameters.Type) ?? string.Empty;

                if (!string.IsNullOrWhiteSpace(brandType) && brandType.ToLower().Equals(Constants.RenderingParameters.WhereToBuy))
                {
                    if (!siteConfiguration.GetSiteConfiguration().PriceSpider.DisablePriceSpider)
                    {
                        whereToBuyViewModel.PSConfig = siteConfiguration.GetSiteConfiguration().PriceSpider.PriceSpiderConfig;
                        var styleAttribute = HelperExtension.GetValueFromCurrentRenderingParameters(Constants.RenderingParameters.Style);
                        whereToBuyViewModel.RenderBuyInStore = styleAttribute != null && styleAttribute.Contains(Constants.RenderingParameters.PsLocalDefault);
                        whereToBuyViewModel.BuyInStoreLabel = HelperExtension.GetValueFromCurrentRenderingParameters(Constants.RenderingParameters.BuyInStoreLabel) ?? "Buy In-Store";
                    }
                }
                else if (!string.IsNullOrWhiteSpace(brandType) && brandType.ToLower() == Constants.RenderingParameters.ExternalBrand)
                {
                    if (!siteConfiguration.GetSiteConfiguration().PriceSpider.DisableExternalBrand)
                    {
                        whereToBuyViewModel.ExternalConfig = siteConfiguration.GetSiteConfiguration().PriceSpider.ExternalBrandConfig;
                    }
                }
                else if (!string.IsNullOrWhiteSpace(brandType) && brandType.ToLower() == Constants.RenderingParameters.StoreLocator)
                {
                    if (!siteConfiguration.GetSiteConfiguration().PriceSpider.DisableStoreConfig)
                    {
                        whereToBuyViewModel.StoreConfig = siteConfiguration.GetSiteConfiguration().PriceSpider.StoreConfig;
                    }
                }
                else
                {
                    return new WhereToBuy();
                }

            }
            catch (Exception ex)
            {
                logger.LogError("ERROR in WhereToBuyRepository.GetWhereToBuyDetails() | ", ex);
            }
            return whereToBuyViewModel;
        }
    }
}