using CGP.Feature.ProductAttributeListing.Controllers;
using CGP.Feature.ProductAttributeListing.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;

namespace CGP.Feature.ProductAttributeListing.DI
{
    public class RegisterContainer : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ProductAttributeListingController>();
            serviceCollection.AddSingleton<IProductAttributeListingRepository, ProductAttributeListingRepository>();
        }
    }
}