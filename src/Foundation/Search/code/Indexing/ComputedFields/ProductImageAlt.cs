using CGP.Foundation.SitecoreExtensions.Utilities;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Items;
using Sitecore.Resources.Media;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CGP.Foundation.Search.Indexing.ComputedFields
{
    /// <summary>
    /// Get the ProductImage alternative text for Search
    /// </summary>
    public class ProductImageAlt : IComputedIndexField
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
                    case Templates.ContentPageIdString:
                    case Templates.ProductGroupPageIdString:
                    case Templates.CouponPageIdString:
                        return HelperExtension.GetMediaItemAlt(indexableItem, Constants.DesktopImage);

                }
            }
            return null;
        }
        private string GetProductDefaultImageUrl(Item productItem)
        {
            Item variantGroupingItem = HelperExtension.GetFirstMatchingChildItem(productItem, Templates.VariantGroupingTemplateId);

            Item defaultVariant = HelperExtension.GetProductVariants(variantGroupingItem).FirstOrDefault();

            if (defaultVariant != null)
            {
                List<Item> mediaItem = SitecoreUtil.GetMultiListFieldValues(defaultVariant, "MediaList");

                if (mediaItem.Any())
                {
                    var media = mediaItem.FirstOrDefault(x => x.TemplateID.ToString() != Templates.VideoTemplateId.ToString());
                    MediaItem image = new MediaItem(media);
                    if (image != null) return image.Alt;
                }
            }
            return String.Empty;
        }
    }
}