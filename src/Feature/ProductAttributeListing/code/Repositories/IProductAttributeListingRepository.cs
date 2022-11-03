using CGP.Feature.ProductAttributeListing.Models;

namespace CGP.Feature.ProductAttributeListing.Repositories
{
    public interface IProductAttributeListingRepository
    {
        ProductAttributeListingViewModel GetProductAttributeListingDetails();
    }
}