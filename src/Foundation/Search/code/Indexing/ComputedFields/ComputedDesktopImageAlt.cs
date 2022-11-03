using CGP.Foundation.SitecoreExtensions.Utilities;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Items;

namespace CGP.Foundation.Search.Indexing.ComputedFields
{
    /// <summary>
    /// Get the ComputedDesktopImage alternative text for Search
    /// </summary>
    public class ComputedDesktopImageAlt : IComputedIndexField
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
                    case Templates.ArticlePageIdString:
                    case Templates.ContentPageIdString:
                        return HelperExtension.GetMediaItemAlt(indexableItem, Constants.DesktopImage);
                }
            }
            return null;
        }
    }
}