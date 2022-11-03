using Sitecore.Data;

namespace CGP.Feature.Integrations
{
    public class Templates
    {
        public static readonly ID WriteReviewPageTemplateId = new ID("{574F54F5-6F3D-4CF6-AC3E-225FDD39E963}");
        public static readonly ID ProductDetailPageTemplateId = new ID("{BC41B87D-2A9B-4689-9F51-80BB5B31A6E7}");
    }

    public class Constants
    {
        public class PowerReview
        {
            public static readonly string ReviewSnippet = "pr-reviewsnippet";
            public static readonly string ReviewDisplay = "pr-reviewdisplay";
            public static readonly string Write = "pr-write";
            public static readonly string LegacyProductGUID = "LegacyProductGUID";
            public static readonly string DisableWhereToBuy = "DisableWhereToBuy";
        }
        public class RenderingParameters
        {
            public static readonly string Type = "Type";
            public static readonly string WhereToBuy = "wheretobuy";
            public static readonly string ExternalBrand = "externalbrand";
            public static readonly string StoreLocator = "storelocator";
            public static readonly string Style = "Styles";
            public static readonly string BuyInStoreLabel = "BuyInStoreLabel";
            public static readonly string PsLocalDefault = "{2D061FCA-3022-456C-8129-A54DD2E3B46D}";
        }
    }
    public class Fields
    {
        public static ID PageHeaderScripts = new ID("{543B1D82-A096-4893-90B3-35BBB2AB2B1A}");
    }
    
}