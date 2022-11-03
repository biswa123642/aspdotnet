using CGP.Feature.Integrations.Repositories;
using Sitecore.XA.Foundation.RenderingVariants.Controllers;

namespace CGP.Feature.Integrations.Controllers
{
    public class WhereToBuyController : VariantsController
    {
        private readonly IWhereToBuyRepository whereToBuyRepository;

        public WhereToBuyController(IWhereToBuyRepository whereToBuyRepository)
        {
            this.whereToBuyRepository = whereToBuyRepository;
        }

        protected override object GetModel()
        {
            return whereToBuyRepository.GetWhereToBuyDetails();
        }
    }
}
