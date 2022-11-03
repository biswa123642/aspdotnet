using Sitecore.XA.Foundation.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CGP.Feature.Search.Models.ViewModel
{
    public class ProductListingViewModel : RenderingModelBase
    {
        public GlobalSearchResult GlobalSearchResult { get; set; }
        public string GridSelection { get; set; }
        public bool HideDescriptionOnTiles { get; set; }
        public bool HideTitleOnTiles { get; set; }
    }
}