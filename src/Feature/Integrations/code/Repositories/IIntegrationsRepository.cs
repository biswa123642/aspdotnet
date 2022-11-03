using Newtonsoft.Json;
using Sitecore.Data.Items;

namespace CGP.Feature.Integrations.Repositories
{
    public interface IIntegrationsRepository
    {
        string GetPowerReviewIntegrationSettings();
        string GetHeaderScripts(Item item, Sitecore.Data.ID fieldId);
    }
}
