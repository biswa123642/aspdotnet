using CGP.Feature.SalesRepresentativeMap.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;

namespace CGP.Feature.SalesRepresentativeMap.DI
{
    public class RegisterContainer : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<SalesRepresentativeMapController>();
        }
    }
}