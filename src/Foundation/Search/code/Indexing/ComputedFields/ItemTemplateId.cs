using CGP.Foundation.SitecoreExtensions.Utilities;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Items;

namespace CGP.Foundation.Search.Indexing.ComputedFields
{
    /// <summary>
    /// Get the ItemTemplateId for Search
    /// </summary>
    public class ItemTemplateId : IComputedIndexField
    {
        public string FieldName { get; set; }
        public string ReturnType { get; set; }

        public object ComputeFieldValue(IIndexable indexable)
        {
            var indexableItem = (Item)(indexable as SitecoreIndexableItem);
            if (indexableItem != null)
            {
                return StringUtil.RemoveSpecialCharacters(indexableItem.TemplateID.ToString().ToLower());
            }
            return string.Empty;
        }
    }
}