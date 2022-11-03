using CGP.Feature.Search.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CGP.Feature.Search.Repositories
{
    public interface ILoadMoreRepository
    {
        LoadMoreViewModel GetLoadMoreDetails();
    }
}