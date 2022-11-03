using System.Collections.Generic;

namespace CGP.Feature.SalesRepresentativeMap.Models
{
    public class SalesRepresentativeMapModel
    {
        public string Content { get; set; }
        public List<StateModel> StateModelList { get; set; }
        public string JSONData { get; set; }
    }
    public class StateModel
    {
        public string StateName { get; set; }
        public string StateCode { get; set; }
        public List<SalesRepModel> SalesRepModelList { get; set; }
    }
    public class SalesRepModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string ImageURL { get; set; }
        public string SalesRepTitle { get; set; }
        public string SalesRepDescription { get; set; }
    }
}