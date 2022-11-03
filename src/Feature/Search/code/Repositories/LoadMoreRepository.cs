using CGP.Feature.Search.Models.ViewModel;
using CGP.Foundation.ErrorModule.Repositiories;
using CGP.Foundation.Search;
using CGP.Foundation.SitecoreExtensions.Repositories;
using Sitecore.Mvc.Presentation;
using Sitecore.XA.Foundation.Mvc.Repositories.Base;
using System;

namespace CGP.Feature.Search.Repositories
{
    public class LoadMoreRepository : ModelRepository, ILoadMoreRepository
    {
        private readonly ISiteConfiguration siteConfiguration;
        public readonly ILogger _logger;
        public LoadMoreRepository(ISiteConfiguration siteConfiguration, ILogger logger)
        {
            this.siteConfiguration = siteConfiguration;
            this._logger = logger;
        }

        public LoadMoreViewModel GetLoadMoreDetails()
        {
            LoadMoreViewModel loadMoreViewModel = new LoadMoreViewModel();
            try
            {
                FillBaseProperties(loadMoreViewModel);
                RenderingParameters parameters = RenderingContext.CurrentOrNull.Rendering.Parameters;
                if (parameters != null)
                {
                    loadMoreViewModel.SearchType = parameters[Constants.LoadMore.SearchType];
                    loadMoreViewModel.PageSize = siteConfiguration.GetSiteConfiguration().SearchCriteria.Pagesize ?? "4";
                    loadMoreViewModel.NumberOfScrolls = GetNumberOfScrolls(parameters);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("ERROR occured in LoadMoreRepository.LoadMoreViewModel() ", ex);
            }

            return loadMoreViewModel; ;
        }

        private string GetNumberOfScrolls(RenderingParameters parameters)
        {
            string numberOfScrolls;
            numberOfScrolls = parameters[Constants.LoadMore.NumberOfScrolls];
            numberOfScrolls = !string.IsNullOrWhiteSpace(numberOfScrolls) ? numberOfScrolls : siteConfiguration.GetSiteConfiguration().SearchCriteria.NumberOfScrolls;
            numberOfScrolls = !string.IsNullOrWhiteSpace(numberOfScrolls) ? numberOfScrolls : "0";
            return numberOfScrolls;
        }
    }
}