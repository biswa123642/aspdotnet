using CGP.Feature.Integrations.Controllers;
using CGP.Feature.Integrations.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;

namespace CGP.Feature.Integrations.DI
{
    public class RegisterContainer : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IntegrationsController>();
            serviceCollection.AddTransient<WhereToBuyController>();
            serviceCollection.AddSingleton<IIntegrationsRepository, IntegrationsRepository>();
            serviceCollection.AddSingleton<IWhereToBuyRepository, WhereToBuyRepository>();
        }
    }
}