using CGP.Foundation.SitecoreExtensions.DataItems;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Items;
using System.Linq;

namespace CGP.Foundation.Search.Indexing.ComputedFields
{
    /// <summary>
    /// Computed Field for the Blog Topic
    /// </summary>
    public class ComputedBlogTopic : IComputedIndexField
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
                    if (blogItem.TopicField == null || blogItem.TopicField.TargetIDs.Length == 0)
                    {
                        return string.Empty;
                    }
                    return blogItem.TopicField.GetItems().Select(a => a.Fields["Key"]?.ToString()).Where(a => !string.IsNullOrWhiteSpace(a)).ToArray();
                }
            }
            return null;
        }
    }
}