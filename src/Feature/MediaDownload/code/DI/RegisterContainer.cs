using CGP.Feature.MediaDownload.Controllers;
using CGP.Feature.MediaDownload.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;

namespace CGP.Feature.MediaDownload.DI
{
    public class RegisterContainer : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<MediaDownloadController>();
            serviceCollection.AddSingleton<IPDFDownloadRepository, PDFDownloadRepository>();
        }
    }
}