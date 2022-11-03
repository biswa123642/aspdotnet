using CGP.Foundation.SitecoreExtensions.DataItems;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Items;

namespace CGP.Foundation.Search.Indexing.ComputedFields
{
    /// <summary>
    /// Computed Field for the BlogContent Type
    /// </summary>
    public class ComputedBlogType : IComputedIndexField
    {
        public string FieldName { get; set; }
        public string ReturnType { get; set; }

        public object ComputeFieldValue(IIndexable indexable)
        {
            var indexableItem = (Item)(indexable as SitecoreIndexableItem);
            if (indexableItem.TemplateID.ToString() == Templates.ArticlePageIdString)
            {
                using (new Sitecore.Globalization.LanguageSwitcher(indexable.Culture.Name))
                {
                    BlogBaseItem blogItem = new BlogBaseItem(indexableItem);
                    if (blogItem.TypeField == null && blogItem.TypeField.TargetItem == null)
                    {
                        return string.Empty;
                    }
                    else
                    {
                        return blogItem.TypeField.TargetItem?.Fields["Key"]?.ToString();
                    }
                }
            }
            return null;
        }
    }
}