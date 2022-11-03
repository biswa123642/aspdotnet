using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace CGP.Foundation.SitecoreExtensions.DataItems
{
    public class BlogBaseItem
    {
        public static ID TemplateId = ID.Parse("{057AF325-C948-4918-A4E4-2C3F6CF04524}");
        public const string TopicFieldName = "BlogPostTopic";
        public const string TypeFieldName = "ArticleType";
        private readonly Item _currentItem;

        public BlogBaseItem(Item currentItem)
        {
            _currentItem = currentItem;
        }

        public MultilistField TopicField
        {
            get { return _currentItem.Fields[TopicFieldName]; }
        }

        public ReferenceField TypeField
        {
            get { return _currentItem.Fields[TypeFieldName]; }
        }
    }
}