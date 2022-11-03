using CGP.Foundation.SitecoreExtensions.DataObjects;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace CGP.Foundation.SitecoreExtensions.DataItems
{
    public class ProductBaseItem
    {
        public static ID TemplateId = ID.Parse("{BC41B87D-2A9B-4689-9F51-80BB5B31A6E7}");
        public static ID PageAttributesFieldId = ID.Parse("{CAAA6C5B-798C-4D26-8919-FE66D620F798}");
        public static ID ProductPageAttributeFieldid = ID.Parse("{CAAA6C5B-798C-4D26-8919-FE66D620F798}");
        public const string PageAttributesFieldName = "Page Attributes";
        public const string ProductAttributesFieldName = "ProductAttributes";
        private PageAttributes _pageAttributes;
        private PageAttributes _productAttributes;
        private Item _currentItem;
        public ProductBaseItem(Item currentItem)
        {
            _currentItem = currentItem;
        }
        public PageAttributes Attributes
        {
            get
            {
                if (_pageAttributes == null)
                {
                    var attributes = new PageAttributes();
                    attributes.InitPageAttribute(_currentItem);
                    _pageAttributes = attributes;
                }
                return _pageAttributes;
            }
        }

        public PageAttributes ProductAttributes
        {
            get
            {
                if (_productAttributes == null)
                {
                    var attributes = new PageAttributes();
                    attributes.InitProductAttribute(_currentItem);
                    _productAttributes = attributes;
                }
                return _productAttributes;
            }
        }

        public MultilistField PageAttributeField
        {
            get { return this._currentItem.Fields[PageAttributesFieldName]; }
        }

        public MultilistField ProductAttributeField
        {
            get { return this._currentItem.Fields[ProductAttributesFieldName]; }
        }

    }
}