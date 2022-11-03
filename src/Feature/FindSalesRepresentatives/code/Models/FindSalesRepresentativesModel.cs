using Sitecore.XA.Foundation.Variants.Abstractions.Models;
using System.Collections.Generic;

namespace CGP.Feature.FindSalesRepresentatives.Models
{
    public class FindSalesRepresentativesViewModel : VariantsRenderingModel
    {
        public List<FindSalesRepDetailsModel> FindSalesRepModel { get; set; }
        public string JSONData { get; set; }
    }
    public class FindSalesRepDetailsModel
    {
        public string StateName { get; set; }
        public string StateCode { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string ImageURL { get; set; }


    }
}