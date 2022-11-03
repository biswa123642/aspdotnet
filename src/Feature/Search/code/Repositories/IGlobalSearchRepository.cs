using CGP.Feature.Search.Models;
using CGP.Feature.Search.Models.ViewModel;
using CGP.Foundation.Search.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace CGP.Feature.Search.Repositories
{
    public interface IGlobalSearchRepository
    {
        GlobalSearchResultViewModel GetGlobalSearchResults(InputParameters inputParameters);
        List<JObject> GetSuggestions(InputParameters inputParameters, int MaxPredictiveResult, int MaxProductCount, int MaxArticleCount, int MaxOthersCount);
        List<SolrModel> GetLoadMoreResults(InputParameters inputParameters, string searchType);
    }
}