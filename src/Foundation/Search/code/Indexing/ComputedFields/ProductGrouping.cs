using CGP.Foundation.SitecoreExtensions.Utilities;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using System.Linq;

namespace CGP.Foundation.Search.Indexing.ComputedFields
{
    public class ProductGrouping : IComputedIndexField
    {
        public string FieldName { get; set; }
        public string ReturnType { get; set; }

        public object ComputeFieldValue(IIndexable indexable)
        {
            var indexableItem = (Item)(indexable as SitecoreIndexableItem);
            if (indexableItem != null && indexableItem.TemplateID.Equals(Templates.ProductDetailPageID))
            {
                MultilistField secondryCategoryList = indexableItem.Fields[Constants.SecondaryCategories];

                if (secondryCategoryList != null)
                {
                    var secondryCategoryItems = secondryCategoryList.GetItems();
                    var output = (secondryCategoryItems == null || secondryCategoryItems.Length == 0)
                        ? null : secondryCategoryItems.Select(x => StringUtil.RemoveSpecialCharacters(x.ID.ToString())).ToList();

                    return output != null ? string.Join(",", output) : string.Empty;
                }
            }
            return null;
        }
    }
}