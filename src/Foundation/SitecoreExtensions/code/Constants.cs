using Sitecore.Configuration;
using Sitecore.Data;

namespace CGP.Foundation.SitecoreExtensions
{
    public class Templates
    {
        //OneWeb Template IDs
        public static readonly string OneWebDefaultSiteItemID = Settings.GetSetting("OneWebDefaultSiteItemID");
        public static readonly string OneWebTenantTemplateID = Settings.GetSetting("OneWebTenantTemplateID");
        public static readonly string OneWebSiteTemplateID = Settings.GetSetting("OneWebSiteTemplateID");
        public static readonly ID SiteSettingTemplateId = new ID("{3845A334-0D01-4D82-80CF-FFAE3EBD8754}");
        public static readonly ID VariantGroupingTemplateId = new ID("{F9CF5C04-01C3-4060-A143-1E12421C09BE}");
        public static readonly ID ProductVariantTemplateId = new ID("{6659B02D-BD4B-4821-B452-77FCA1DA6B43}");

        public static readonly ID ProductAttributeTemplateID = new ID("{4165F10A-7E0D-4331-B811-8BE903A1CA37}");
        public static readonly ID PageAttributeTemplateID = new ID("{186E0BD4-2905-404A-B92C-868EFC6F42A8}");

        public static readonly ID BlogTopicsFolderTemplateId = new ID("{5DC67FF6-B5D4-489A-BC6F-0E6911A12C61}");
        public static readonly ID BlogTypesFolderTemplateId = new ID("{F50FEC31-FB00-435B-9F95-323004B0F331}");

        public static readonly ID ProductAttributesFolderTemplateId = new ID("{1659C58A-BCF0-4364-9EFF-E2DA49BBDE8B}");
        public static readonly ID PageAttributesFolderTemplateId = new ID("{405D1129-887E-4B12-AF6E-1BE55C2C315F}");
        public static readonly ID ListItemTemplateID = new ID("{62E96C2C-0949-4EEA-997A-D9408A414D7E}");
        public static readonly ID SiteConfigurationItemID = new ID("{592435EE-F314-46C8-96E8-EB41ACF0F00A}");
        public struct SiteConfiguration
        {
            /// <summary>
            /// Defines the Id.
            /// </summary>
            public static readonly ID Id = ID.Parse("{77820909-82A2-4308-9E62-F54E15110EF3}");
            public struct Fields
            {
                public static ID BrandName = new ID("{4762F3C8-900D-44FF-BA2E-D44E57E8F5FB}");
                public static ID BrandLogo = new ID("{C07BE27B-FCB2-44EB-9162-A1E183EE79B9}");
                public static ID EnablePowerReviews = new ID("{ED38D73E-A875-424C-8F31-8E0B0C0D197E}");
                public static ID PowerReviewsAPIKey = new ID("{7BE09131-F96B-4F0E-ACB8-2D50AA27C6E2}");
                public static ID PowerReviewsLocale = new ID("{AFCFBB0D-D8AF-4442-9083-D0F0712ED0A2}");
                public static ID PowerReviewsMerchantGroupID = new ID("{D30967E5-F8C5-4EE6-A5A0-559A6CF5D639}");
                public static ID PowerReviewsMerchantID = new ID("{8F0F4B7C-759C-4871-91AE-D6166BF319EA}");
                public static ID PowerReviewsMapperURL = new ID("{B40EBD0E-FF54-4EE7-8D12-E95A86743552}");
                public static ID PowerReviewsSendProductInfo = new ID("{12773A99-51F6-4C4E-9344-B88BC60A1721}");
                public static ID PriceSpiderAccount = new ID("{9B7BB2EA-CC16-4EF0-862A-B5DA3537A938}");
                public static ID DisablePriceSpider = new ID("{BF6B073F-2C03-4D90-BDA3-FD070FE69665}");
                public static ID PriceSpiderConfig = new ID("{BE547B09-1B5F-4ED9-8772-2F79B5E64D83}");
                public static ID PriceSpiderCountry = new ID("{3D8C2876-9123-4724-92B2-58E7A7DB0452}");
                public static ID SelectSeoSchema = new ID("{F6A9E708-ACDF-411A-A685-9D9CBF9252C1}");
                public static ID EnableSeoSchema = new ID("{EC3719F3-58B4-4504-8545-DF1AF5AEF83D}");
                public static ID DisableExternalBrand = new ID("{0926E173-227E-40C2-9F60-0E5DBF4F45D8}");
                public static ID ExternalBrandConfig = new ID("{BE8F8860-29BC-4C1E-854D-F5C93641E70C}");
                public static ID DisableStoreConfig = new ID("{9382BDCE-C7D0-4CA8-A214-160D80410C08}");
                public static ID StoreConfig = new ID("{DB00C90F-9869-40DE-B06F-31A0FC2C3A45}");


                public static ID PageAttributeLocation = new ID("{75E1AB85-58FA-4E28-9D2A-C7D8836B8FFA}");
                public static ID ProductAttributeLocation = new ID("{2DA26FE5-8B7B-4912-A2FC-F683E630D5BB}");

                public static ID BlogPostTopicAttributeLocation = new ID("{E86000B0-2A64-4152-9178-6591115B5265}");
                public static ID ArticleTypeAttributeLocation = new ID("{F885A9CC-D0C1-431B-8AFE-85B73F80DB86}");


                public static ID CheckToDisableOtherSection = new ID("{09BA3CF1-8C05-4628-B0AE-9AAE26DFF1E7}");
                public static ID CheckToDisableArticlesSection = new ID("{FAF8EEF0-D90D-40A2-8DE0-609152E3FBE3}");
                public static ID CheckToDisableProductsSection = new ID("{17683604-F1C3-47D0-AC8E-A94E0E2ABCB1}");
                public static ID AdditionalSuggestedTemplates = new ID("{CA71498B-C52F-4769-8A96-5269B3231F1F}");

                public static ID PageSize = new ID("{668B8710-8E39-4B7B-87B5-3D7B68A13F5B}");
                public static ID NumberOfScrolls = new ID("{E4ACFB78-8493-4948-AF51-35DA5B417A4D}");
                
                public static ID GTMId = new ID("{D7D037D1-E378-40FE-A22D-7D9E73BA75A3}");
                public static ID DisableGTM = new ID("{D01AB4F5-7DD1-48F4-AE48-A45C1C3BF458}");

                public const string NoImageIdString = "{7BEA2506-C26D-4030-A5EF-E2BD2E8BAA4C}";
                public static readonly ID NoImageID = new ID(NoImageIdString);

                public static ID EnableSurveyPolls = new ID("{41C147D8-4A0A-4627-A517-54FAD45D5229}");
                public static ID SurvicateWorkspaceId = new ID("{C9095AA3-AF8F-436C-B8F1-26BC20C245C6}");

                public static ID DisableCookiesBanner = new ID("{51DEEDC1-0600-4D1D-85EC-DB67CD6F2BAD}");
                public static ID OneTrustCookiesID = new ID("{D8692E4F-D312-490E-881B-84059310D731}");

                public static ID GAId = new ID("{9E24EADA-2DBD-4C64-964D-DA5D755FA5AD}");
                public static ID GAIdGlobal = new ID("{37CBF5A7-E02D-42F8-9CD1-7BBF37E23773}");
                public static ID DisableGA = new ID("{B1AB2B86-16B2-441E-B0D2-404C8C139E3D}");

                public static ID OfficeLocationFinderGoogleMapsAPIKey = new ID("{3AEF89BD-80C4-4DF9-8E5F-038498ED6AC0}");
                public static ID FoodPlotGoogleMapsAPIKey = new ID("{DF838412-CD35-4DAB-92AC-82A5D203CAA4}");
                public static ID FoodPlotJSONData = new ID("{D6FD8D24-F8D7-4AA3-BBCC-58868AD8893C}");

                public static ID EnableLogin = new ID("{D5FFC3A1-1D5E-41B2-B824-3143A122B4E9}");
                public static ID UsersList = new ID("{342E9E22-02AF-42EA-9B73-C725C67E9F0A}");
                public static ID PasswordAuth = new ID("{B3A292CB-7334-4A12-A807-10D38B8DF488}");
                public static ID PasswordAuthUser = new ID("{9D9E1E41-2E26-496B-936E-DFCF5FC5DD80}");

                public static ID DisableGoogleAds = new ID("{F65570F1-60BB-4355-841A-90653E2160DE}");
                public static ID PublisherID = new ID("{45242E6B-9ADF-4F32-BFC1-24135E46F74A}");
                public static ID AdSlotID = new ID("{4E1BCB01-18EE-49A1-9866-E15475DC49A8}");

            }
        }
    }
    public class Constants
    {
        public static readonly string ChooseVariant = "ChooseVariant";
        public static readonly string Key = "Key";
        public static readonly string Value = "Value";
        public static readonly string VariantSKU = "VariantSKU";
        public static readonly string OpenGraphImageUrl = "OpenGraphImageUrl";
        public static readonly string RemoveLanguageCookieSiteSetting = "removeLanguageCookie";
        public static readonly string CoreConnectionStringName = "core";
        public static readonly string DefaultLanguage = "en";
    }
}