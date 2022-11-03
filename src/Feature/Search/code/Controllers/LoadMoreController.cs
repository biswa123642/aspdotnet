using CGP.Feature.Search.Repositories;
using Sitecore.XA.Foundation.Mvc.Controllers;
using System.Web.Mvc;

namespace CGP.Feature.Search.Controllers
{
    public class LoadMoreController : StandardController
    {
        private readonly ILoadMoreRepository _loadMoreRepository;
        public LoadMoreController(ILoadMoreRepository loadMoreRepository)
        {
            this._loadMoreRepository = loadMoreRepository;
        }
        public ActionResult GlobalLoadMore()
        {
            return PartialView("~/Views/OneWeb/Search/LoadMore.cshtml", this.GetModel());
        }
        protected override object GetModel()
        {
            return _loadMoreRepository.GetLoadMoreDetails();
        }
    }
}