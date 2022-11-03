using CGP.Feature.FindSalesRepresentatives.Repositories;
using Sitecore.XA.Foundation.RenderingVariants.Controllers;


namespace CGP.Feature.FindSalesRepresentatives.Controllers
{
    public class FindSalesRepresentativesController : VariantsController
    {
        private readonly IFindSalesRepresentativesRepository FindSalesRepresentativesRepository;

        public FindSalesRepresentativesController(IFindSalesRepresentativesRepository FindSalesRepresentativesRepository)
        {
            this.FindSalesRepresentativesRepository = FindSalesRepresentativesRepository;
        }
        protected override object GetModel()
        {
            return FindSalesRepresentativesRepository.GetSalesRepresentativesDetails();
        }
    }
}