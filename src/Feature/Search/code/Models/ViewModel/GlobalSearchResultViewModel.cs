using Sitecore.XA.Foundation.Mvc.Models;

namespace CGP.Feature.Search.Models.ViewModel
{
    public class GlobalSearchResultViewModel : RenderingModelBase
    {
        public GlobalSearchResult GlobalSearchResult { get; set; }
        public string GridSelection { get; set; }
        public bool HideDescriptionOnTiles { get; set; }
        public bool HideTitleOnTiles { get; set; }
    }
}