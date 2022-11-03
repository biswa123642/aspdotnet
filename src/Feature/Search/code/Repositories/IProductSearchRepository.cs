using CGP.Feature.Search.Models;
using CGP.Feature.Search.Models.ViewModel;
using CGP.Foundation.Search.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Feature.Search.Repositories
{
    public interface IProductSearchRepository
    {
        ProductListingViewModel GetProductList(InputParameters inputParameters);
    }
}
