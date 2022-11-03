using Sitecore.XA.Foundation.Mvc.Models;

namespace CGP.Feature.Search.Models.ViewModel
{
    public class LoadMoreViewModel : RenderingModelBase
    {
        public string SearchType { get; set; }
        public string PageSize { get; set; }
        public string NumberOfScrolls { get; set; }
    }
}