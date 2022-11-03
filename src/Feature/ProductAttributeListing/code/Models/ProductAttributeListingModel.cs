using Sitecore.Data.Items;
using Sitecore.XA.Foundation.Variants.Abstractions.Models;
using System.Collections.Generic;

namespace CGP.Feature.ProductAttributeListing.Models
{



    public class ProductAttributeListingViewModel : VariantsRenderingModel
    {
        public Item FilteredProductAttributeItem { get; set; }
        public List<Item> FilteredProductAttributeList { get; set; }
        public List<Item> ProductAttributesList { get; set; }

    }
}