using CGP.Feature.Search.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CGP.Feature.Search.Models
{
    public class GlobalLoadMoreResult
    {
        public List<GlobalResultItem> Results { get; set; }
        public string GridSelection { get; set; }
        public bool HideDescriptionOnTiles { get; set; }
        public bool HideTitleOnTiles { get; set; }
    }
}