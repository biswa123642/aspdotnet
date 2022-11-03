using CGP.Foundation.SitecoreExtensions.Utilities;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CGP.Foundation.Search.Indexing.ComputedFields
{
    /// <summary>
    /// Get the ProductImage for Search
    /// </summary>
    public class ComputedImage : IComputedIndexField
    {
        public string FieldName { get; set; }
        public string ReturnType { get; set; }

        public object ComputeFieldValue(IIndexable indexable)
        {
            var indexableItem = (Item)(indexable as SitecoreIndexableItem);
            if (indexableItem != null)
            {
                switch (indexableItem.TemplateID.ToString())
                {
                    case Templates.ProductDetailPageIdString:
                        return GetProductDefaultImageUrl(indexableItem);
                    case Templates.ArticlePageIdString:
                    case Templates.ContentPageIdString:
                    case Templates.CouponPageIdString:
                    case Templates.ProductGroupPageIdString:
                    case Templates.HomePageIdString:
                    case Templates.WriteAReviewPageIdString:
                    case Templates.ProductListingPageIdString:
                        return GetOtherDefaultImageUrl(indexableItem);
                }
            }
            return null;
        }
        /// <summary>
        /// Get the Default Image Url for the Product
        /// </summary>
        /// <param name="productItem"></param>
        /// <returns></returns>
        private string GetProductDefaultImageUrl(Item productItem)
        {
            string mediaUrl = string.Empty;
            Item variantGroupingItem = HelperExtension.GetFirstMatchingChildItem(productItem, Templates.VariantGroupingTemplateId);

            Item defaultVariant = HelperExtension.GetProductVariants(variantGroupingItem).FirstOrDefault();

            if (defaultVariant != null)
            {
                List<Item> mediaItem = SitecoreUtil.GetMultiListFieldValues(defaultVariant, "MediaList");

                if (mediaItem.Any())
                {
                    mediaUrl = SitecoreUtil.GetMediaItemUrl(mediaItem.FirstOrDefault(x => x.TemplateID.ToString() != Templates.VideoTemplateId.ToString()));
                    mediaUrl = string.IsNullOrWhiteSpace(mediaUrl) ? HelperExtension.GetNoImageUrl(productItem) : mediaUrl;
                    return CreatImageElement(mediaUrl, mediaItem.FirstOrDefault());
                }
            }
            else
            {
                return GetOtherDefaultImageUrl(productItem);
            }
            return string.Empty;
        }

        private string CreatImageElement(string mediaUrl, Item mediaItem)
        {
            var mediaAlt = HelperExtension.GetMediaItemAlt(mediaItem, Constants.DesktopImage);
            mediaAlt = (!string.IsNullOrWhiteSpace(mediaAlt)) ? mediaAlt : mediaItem.Name;
            return string.Format("<img src='{0}' alt='{1}'>", mediaUrl, mediaAlt);
        }

        private string GetOtherDefaultImageUrl(Item otherItem)
        {
            string mediaUrl = HelperExtension.GetUrlFromImageField(otherItem, Constants.DesktopImage);
            return CreatImageElement(mediaUrl, otherItem);
        }
    }
}