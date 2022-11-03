using Sitecore.Data;

namespace CGP.Foundation.Search
{
    public class Constants
    {
        public static readonly string MobileImage = "MobileImage";
        public static readonly string DesktopImage = "DesktopImage";

        public static readonly string SecondaryCategories = "SecondaryCategories";
        public static readonly string ProductAttributes = "ProductAttributes";
        public static readonly string FacetAttributes = "FacetAttributes";
        public static readonly string ProductDetailsAttributes = "ProductDetailsAttributes";
        public static readonly string Key = "Key";
        public static readonly string Value = "Value";
        public static readonly string SearchFacetAttributes = "SearchFacetAttributes";
        public static readonly string PageAttributes = "Page-Attributes";
        public static readonly string TopicsandTypes = "Topics-and-Types";
        public static readonly string HideInSearchResults = "HideInSearchResults";
        public static readonly string HideSearchFilter = "HideSearchFilter";
        public static readonly string HideDescriptionOnTiles = "HideDescriptionOnTiles";
        public static readonly string HideTitleOnTiles = "HideTitleOnTiles";
        public static readonly string DisableSearchBox = "DisableSearchBox";
        public static readonly string DisableSearchFilter = "DisableSearchFilter";

        public static readonly string BlogTopic = "blogtopic";
        public static readonly string BlogType = "blogtype";

        public static readonly string Topic = "topic";
        public static readonly string Type = "type";

        public static readonly string CheckToLoadChildArticlesOnly = "CheckToLoadChildArticlesOnly";
        public static readonly string ArticleListingFacetType = "ArticleListingFacetType";
        public static readonly string FilterOrder = "FilterOrder";
        public static readonly string Alphabetical = "Alphabetical";
        public static readonly string SameAsCMS = "SameAsCMS";
        public static readonly string DescriptionLengthLimit = "DescriptionLengthLimit";

        public static readonly string SortingLookup = "SortingLookup";
        public static readonly string SortDirection = "SortDirection";
        public static readonly string Asc = "asc";
        public static readonly string Desc = "desc";
        public static readonly string RedirectLink = "RedirectLink";

        public class SearchBox
        {
            public static readonly string PlaceholderText = "PlaceholderText";
            public static readonly string SearchResultPage = "SearchResultPage";
            public static readonly string DisableSearchTextBox = "DisableSearchTextBox";
            public static readonly string DisableAutoSugession = "DisableAutoSugession";
            public static readonly string CloneSearchBox = "CloneSearchBox";
            public static readonly string MinSuggestionsTriggerCharacterCount = "MinSuggestionsTriggerCharacterCount";
            public static readonly string MaxPredictiveResultsCount = "MaxPredictiveResultsCount";
            public static readonly string MaxProductResultsCount = "MaxProductResultsCount";
            public static readonly string MaxArticleResultsCount = "MaxArticleResultsCount";
            public static readonly string MaxOthersResultCount = "MaxOthersResultCount";
        }
        public class LoadMore
        {
            public static readonly string SearchType = "SearchType";
            public static readonly string NumberOfScrolls = "NumberOfScrolls";
            public static readonly string GridSelection = "GridSelection";
        }
    }
    public class Templates
    {
        public const string ContentPageIdString = "{AA977B10-C924-4446-8B15-A7D26AA6C93F}";
        public static readonly ID StandardBasePageID = new ID(ContentPageIdString);

        public const string ProductDetailPageIdString = "{BC41B87D-2A9B-4689-9F51-80BB5B31A6E7}";
        public static readonly ID ProductDetailPageID = new ID(ProductDetailPageIdString);

        public const string ArticlePageIdString = "{057AF325-C948-4918-A4E4-2C3F6CF04524}";
        public static readonly ID ArticlePageID = new ID(ArticlePageIdString);

        public const string ArticleCategoryLandingPage = "{4C358B66-2BC5-44DF-88A6-888A0C502E1D}";
        public static readonly ID ArticleCategoryLandingPageID = new ID(ArticleCategoryLandingPage);

        public const string ArticleLandingPage = "{B38986E8-884F-400D-A503-6379538FB556}";
        public static readonly ID ArticleLandingPageID = new ID(ArticleLandingPage);

        public static readonly ID VariantGroupingTemplateId = new ID("{F9CF5C04-01C3-4060-A143-1E12421C09BE}");
        public static readonly ID VideoTemplateId = new ID("{1E9F56B7-FD80-4EAE-9FD4-8AB6242C6DB8}");

        public const string ProductGroupPageIdString = "{7BDF7D45-C98F-4E4D-AC3A-AD8035780BBE}";
        public static readonly ID ProductGroupPage = new ID(ProductGroupPageIdString);

        public const string ProductListingPageIdString = "{80ED8527-6794-4B69-8C25-035C6B33EEEF}";
        public static readonly ID ProductListingPage = new ID(ProductListingPageIdString);

        public const string SearchPageIdString = "{EFBF7280-8143-4B8B-A17F-FA201718310C}";
        public static readonly ID SearchPageID = new ID(SearchPageIdString);

        public const string CouponPageIdString = "{E4D2CB70-90FB-48E3-B8F2-1BAA4E6FB626}";
        public static readonly ID CouponPageID = new ID(SearchPageIdString);

        public const string OffersPageIdString = "{39BCECE9-8EAC-44A4-9831-17AA3067361C}";
        public static readonly ID OffersPageID = new ID(OffersPageIdString);

        public const string WriteAReviewPageIdString = "{574F54F5-6F3D-4CF6-AC3E-225FDD39E963}";
        public static readonly ID WriteAReviewPageID = new ID(WriteAReviewPageIdString);

        public const string HomePageIdString = "{3BC22E05-E1FF-45A6-B8C3-FCF4C8F84A43}";
        public static readonly ID HomePagePageID = new ID(HomePageIdString);
    }
}