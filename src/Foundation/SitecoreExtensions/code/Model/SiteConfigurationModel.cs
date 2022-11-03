using System.Collections.Generic;
using System.Collections.Specialized;

namespace CGP.Foundation.SitecoreExtensions.Model
{
    public class SiteConfigurationModel
    {
        public PowerReview PowerReview { get; set; }
        public PriceSpider PriceSpider { get; set; }
        public SeoSettings SeoSettings { get; set; }
        public BrandSettings BrandSettings { get; set; }
        public GTMSettings GTMSettings { get; set; }
        public SearchCriteria SearchCriteria { get; set; }
        public MediaSettings MediaSettings { get; set; }
        public SurveyPolls SurveyPolls { get; set; }
        public CookiesBannerSettings CookiesBannerSettings { get; set; }
        public GASettings GASettings { get; set; }
        public GoogleMapsAPIKey GoogleMapsAPIKey { get; set; }
        public UserLogin UserLogin { get; set; }
        public GoogleAdSenseSettings GoogleAdSenseSettings { get; set; }
    }
    public class PowerReview
    {
        public bool EnablePowerReviews { get; set; }
        public string PowerReviewsAPIKey { get; set; }
        public string PowerReviewsLocale { get; set; }
        public string PowerReviewsMerchantGroupID { get; set; }
        public string PowerReviewsMerchantID { get; set; }
        public string PowerReviewsMapperURL { get; set; }
        public string PowerReviewsSendProductInfo { get; set; }
    }
    public class PriceSpider
    {
        public bool DisablePriceSpider { get; set; }
        public string PriceSpiderAccount { get; set; }
        public string PriceSpiderConfig { get; set; }
        public string PriceSpiderCountry { get; set; }
        public bool DisableExternalBrand { get; set; }
        public string ExternalBrandConfig { get; set; }
        public bool DisableStoreConfig { get; set; }
        public string StoreConfig { get; set; }
    }
    public class SeoSettings
    {
        public bool EnableSeoSchema { get; set; }
        public List<string> SelectSeoSchema { get; set; }
    }
    public class BrandSettings
    {
        public string BrandName { get; set; }
        public string BrandLogo { get; set; }
    }

    public class SearchCriteria
    {
        public IDictionary<string, IList<string>> ProductAttributes { get; set; }
        public IDictionary<string, IList<string>> PageAttributes { get; set; }
        public List<string> ArticleTypeAttributes { get; set; }
        public List<string> BlogPostTopicAttributes { get; set; }
        public bool CheckToDisableOtherSection { get; set; }
        public bool CheckToDisableArticlesSection { get; set; }
        public bool CheckToDisableProductsSection { get; set; }
        public NameValueCollection AdditionalSuggestedTemplates { get; set; }
        public string Pagesize { get; set; }
        public string NumberOfScrolls { get; set; }
    }

    public class GTMSettings
    {
        public bool DisableGTM { get; set; }
        public string GTMId { get; set; }
    }

    public class MediaSettings
    {
        public string NoImage { get; set; }
    }

    public class SurveyPolls
    {
        public bool EnableSurveyPolls { get; set; }
        public string SurvicateWorkspaceId { get; set; }
    }
    public class CookiesBannerSettings
    {
        public bool DisableCookiesBanner { get; set; }
        public string OneTrustCookiesID { get; set; }
    }
    public class GASettings
    {
        public bool DisableGA { get; set; }
        public string GAID { get; set; }
        public string GAIDGlobal { get; set; }
    }

    public class GoogleMapsAPIKey
    {
        public string OfficeLocationFinderAPIKey { get; set; }
        public string FoodPlotAPIKey { get; set; }
        public string FoodPlotJSONData { get; set; }

    }

    public class UserLogin
    {
        public bool EnableLogin { get; set; }
        public bool PasswordAuth { get; set; }
        public List<string> usersList { get; set; }
        public string PasswordAuthUser { get; set; }
    }
    public class GoogleAdSenseSettings
    {
        public bool DisableGoogleAdsense { get; set; }
        public string PublisherID { get; set; }
        public string AdSlotID { get; set; }
    }
}