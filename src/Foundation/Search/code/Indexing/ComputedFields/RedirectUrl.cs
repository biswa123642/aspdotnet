using CGP.Foundation.ErrorModule.Repositiories;
using CGP.Foundation.SitecoreExtensions.Utilities;
using Sitecore.Configuration;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Sites;
using Sitecore.Web;
using System.Linq;

namespace CGP.Foundation.Search.Indexing.ComputedFields
{
    /// <summary>
    /// Get the link of content items
    /// </summary>
    public class RedirectUrl : IComputedIndexField
    {
        public string FieldName { get; set; }
        public string ReturnType { get; set; }
        ILogger logger = new Logger();
        public object ComputeFieldValue(IIndexable indexable)
        {
            Item indexableItem = (Item)(indexable as SitecoreIndexableItem);
            if (indexableItem != null && indexableItem.TemplateID.Equals(Templates.ProductDetailPageID))
            {
                LinkField redirectLink = indexableItem.Fields[Constants.RedirectLink];

                if (redirectLink != null)
                {
                    return redirectLink.Url;
                }
            }
            return null;
        }

        
    }
}