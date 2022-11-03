using Sitecore.XA.Foundation.Variants.Abstractions.Models;
using Newtonsoft.Json;

namespace CGP.Feature.Integrations.Models
{
    public class IntegrationsViewModel
    {
        public WhereToBuy WhereToBuy { get; set; }
        public bool EnableWhereToBuy { get; set; }
        public string PowerReviews { get; set; }
        public bool EnablePowerReviews { get; set; }
        public bool IsProductDetailPage { get; set; }
        public SurveyPolls SurveyPolls { get; set; }
        public bool EnableSurveyPolls { get; set; }
        public bool EnableCookiesBanner { get; set; }
        public string OneTrustCookiesID { get; set; }
        public bool EnableGA { get; set; }
        public string GAID { get; set; }
        public string GAIDGlobal { get; set; }

        public bool EnableGTM { get; set; }
        public string GTMId { get; set; }
        public string PageHeaderScripts { get; set; }
        public GoogleAdSense GoogleAdSense { get; set; }
        public bool DisableGoogleAd { get; set; }
       
    }
    public class WhereToBuy : VariantsRenderingModel
    {
        public string Account { get; set; }
        public string Config { get; set; }
        public string Country { get; set; }
        public string ExternalConfig { get; set; }
        public string PSConfig { get; set; }
        public string StoreConfig { get; set; }
        public bool RenderBuyInStore { get; set; }
        public string BuyInStoreLabel { get; set; }
    }
    public class SurveyPolls
    {
        public string SurvicateWorkspaceID { get; set; }
    }
    public class GoogleAdSense
    {
        public string PublisherID { get; set; }
        public string AdSlotID { get; set; }
    }
}