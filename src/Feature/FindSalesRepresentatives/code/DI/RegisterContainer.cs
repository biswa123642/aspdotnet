using CGP.Feature.FindSalesRepresentatives.Controllers;
using CGP.Feature.FindSalesRepresentatives.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;

namespace CGP.Feature.FindSalesRepresentatives.DI
{
    public class RegisterContainer : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<FindSalesRepresentativesController>();
            serviceCollection.AddSingleton<IFindSalesRepresentativesRepository, FindSalesRepresentativesRepository>();
        }
    }
}