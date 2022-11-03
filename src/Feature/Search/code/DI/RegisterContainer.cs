using CGP.Feature.Search.Controllers;
using CGP.Feature.Search.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;

namespace CGP.Feature.Search.DI
{
    public class RegisterContainer : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<SearchController>();
            serviceCollection.AddTransient<ArticleListingController>();
            serviceCollection.AddTransient<CouponListingController>();
            serviceCollection.AddTransient<ProductListingController>();
            serviceCollection.AddTransient<SearchBoxController>();
            serviceCollection.AddTransient<LoadMoreController>();
            serviceCollection.AddSingleton<IGlobalSearchRepository, GlobalSearchRepository>();
            serviceCollection.AddSingleton<ISearchBoxRepository, SearchBoxRepository>();
            serviceCollection.AddSingleton<IProductSearchRepository, ProductSearchRepository>();
            serviceCollection.AddSingleton<IArticleSearchRepository, ArticleSearchRepository>();
            serviceCollection.AddSingleton<ICouponSearchRepository, CouponSearchRepository>();
            serviceCollection.AddSingleton<ILoadMoreRepository, LoadMoreRepository>();
        }
    }
}