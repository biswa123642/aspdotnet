using CGP.Feature.Search.Models.ViewModel;
using CGP.Foundation.Search.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CGP.Feature.Search.Repositories
{
    public interface ISearchBoxRepository
    {
        SearchBoxViewModel GetSearchBoxViewModel();
    }
}