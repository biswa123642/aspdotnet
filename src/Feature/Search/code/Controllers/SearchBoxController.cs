using CGP.Feature.Search.Repositories;
using Sitecore.XA.Foundation.Mvc.Controllers;
using System.Web.Mvc;

namespace CGP.Feature.Search.Controllers
{
    public class SearchBoxController : StandardController
    {
        private readonly ISearchBoxRepository _searchBoxRepository;
        public SearchBoxController(ISearchBoxRepository searchBoxRepository)
        {
            this._searchBoxRepository = searchBoxRepository;
        }
        public ActionResult GlobalSearchBox()
        {
            return PartialView("~/Views/OneWeb/Search/GlobalSearchBox.cshtml", this.GetModel());
        }

        protected override object GetModel()
        {
            return _searchBoxRepository.GetSearchBoxViewModel();
        }
    }
}