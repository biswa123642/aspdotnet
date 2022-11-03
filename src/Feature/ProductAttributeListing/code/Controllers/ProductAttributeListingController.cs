using CGP.Feature.ProductAttributeListing.Repositories;
using Sitecore.XA.Foundation.RenderingVariants.Controllers;


namespace CGP.Feature.ProductAttributeListing.Controllers
{
    public class ProductAttributeListingController : VariantsController
    {
        private readonly IProductAttributeListingRepository productattributelistingRepository;

        public ProductAttributeListingController(IProductAttributeListingRepository productattributelistingRepository)
        {
            this.productattributelistingRepository = productattributelistingRepository;
        }
        protected override object GetModel()
        {
            return productattributelistingRepository.GetProductAttributeListingDetails();
        }
    }
}