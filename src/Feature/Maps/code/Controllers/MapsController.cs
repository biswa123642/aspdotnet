using CGP.Foundation.SitecoreExtensions.Repositories;
using System.Web.Mvc;
using CGP.Feature.Maps.Models;
using Sitecore.XA.Foundation.Mvc.Controllers;
using System.Text;
using CGP.Foundation.SitecoreExtensions.Utilities;


namespace CGP.Feature.Maps.Controllers
{
    public class MapsController : StandardController
    {
        private readonly ISiteConfiguration siteConfiguration;
        public MapsController(ISiteConfiguration siteConfiguration)
        {
            this.siteConfiguration = siteConfiguration;
        }
        public ActionResult LoadFoodPlotMap()
        {
            MapsViewModel integrationsViewModel = new MapsViewModel();
            var getSiteConfiguration = siteConfiguration.GetSiteConfiguration();
            if (getSiteConfiguration.GoogleMapsAPIKey.FoodPlotAPIKey!= null)
            {
                integrationsViewModel.FoodPlotAPIKey = getSiteConfiguration.GoogleMapsAPIKey.FoodPlotAPIKey;
                integrationsViewModel.FoodPlotJSONData = getSiteConfiguration.GoogleMapsAPIKey.FoodPlotJSONData;
            }
                 integrationsViewModel.Mappin = HelperExtension.GetHostName() + "-/media" + SitecoreUtil.GetImageUrlFromMediaLibraryItem(Constants.Mappin);
                 integrationsViewModel.Deerstand = HelperExtension.GetHostName() + "-/media" + SitecoreUtil.GetImageUrlFromMediaLibraryItem(Constants.Deerstand);
                 integrationsViewModel.Deerstandhighlighted = HelperExtension.GetHostName() + "-/media" + SitecoreUtil.GetImageUrlFromMediaLibraryItem(Constants.Deerstandhighlighted);
            if (Sitecore.Context.PageMode.IsExperienceEditor || Sitecore.Context.PageMode.IsPreview)
            {
                integrationsViewModel.Mappin = integrationsViewModel.Mappin.Replace("?sc_site=Default", "");
                integrationsViewModel.Deerstand = integrationsViewModel.Deerstand.Replace("?sc_site=Default", "");
                integrationsViewModel.Deerstandhighlighted = integrationsViewModel.Deerstandhighlighted.Replace("?sc_site=Default", "");
            }
            return View("~/Views/OneWeb/Maps/FoodPlotMap.cshtml", integrationsViewModel);
        }

        public ActionResult LoadOfficeLocationFinder()
        {
            MapsViewModel integrationsViewModel = new MapsViewModel();
            var getSiteConfiguration = siteConfiguration.GetSiteConfiguration();
            
            if (getSiteConfiguration.GoogleMapsAPIKey.OfficeLocationFinderAPIKey != null)
            {
                integrationsViewModel.OfficeLocationFinderAPIKey = getSiteConfiguration.GoogleMapsAPIKey.OfficeLocationFinderAPIKey;
            }
            integrationsViewModel.MarkerCorporateIcon = HelperExtension.GetHostName() + "-/media" + SitecoreUtil.GetImageUrlFromMediaLibraryItem(Constants.MarkerCorporateIcon);
            integrationsViewModel.MarkerDistributionIcon = HelperExtension.GetHostName() + "-/media" + SitecoreUtil.GetImageUrlFromMediaLibraryItem(Constants.MarkerDistributionIcon);
            integrationsViewModel.MarkerManufacturingIcon = HelperExtension.GetHostName() + "-/media" + SitecoreUtil.GetImageUrlFromMediaLibraryItem(Constants.MarkerManufacturingIcon);
            integrationsViewModel.MarkerOfficeIcon = HelperExtension.GetHostName() + "-/media" + SitecoreUtil.GetImageUrlFromMediaLibraryItem(Constants.MarkerOfficeIcon);
            if (Sitecore.Context.PageMode.IsExperienceEditor || Sitecore.Context.PageMode.IsPreview)
            {
                integrationsViewModel.MarkerCorporateIcon = integrationsViewModel.MarkerCorporateIcon.Replace("?sc_site=Default", "");
                integrationsViewModel.MarkerDistributionIcon = integrationsViewModel.MarkerDistributionIcon.Replace("?sc_site=Default", "");
                integrationsViewModel.MarkerManufacturingIcon = integrationsViewModel.MarkerManufacturingIcon.Replace("?sc_site=Default", "");
                integrationsViewModel.MarkerOfficeIcon = integrationsViewModel.MarkerOfficeIcon.Replace("?sc_site=Default", "");
            }
            return View("~/Views/OneWeb/Maps/OfficeLocationFinderMap.cshtml", integrationsViewModel);
        }
    }
}
