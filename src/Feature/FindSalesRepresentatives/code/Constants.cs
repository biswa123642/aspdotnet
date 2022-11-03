using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CGP.Feature.FindSalesRepresentatives
{
    public static class Template
    {
        public static class Templates
        {
            public static readonly ID FindSalesRepresentativesTemplateId = new ID("{C0986AEB-B5B1-460C-A13D-683245F1AAC0}");
            public static readonly ID SalesRepresentativesItemTemplateId = new ID("{6E7840E3-5D65-44E8-B1DD-F418B11653D8}");
        }
    }
    public static class Constants
    {
        public static class FindSalesRep
        {
            public static readonly ID StateNameId = new ID("{89588ACD-1D0D-4092-90C9-8831BE0F26F0}");
            public static readonly ID StateCodeId = new ID("{FE92CF46-537C-4E47-8D4A-E418794D9780}");
            public static readonly ID NameId = new ID("{77EF0051-3303-464F-9C17-EF641AF44D3E}");
            public static readonly ID PhoneId = new ID("{E245BAD2-8002-4973-97E4-4842FAD41516}");
            public static readonly ID EmailId = new ID("{F7F1359A-16E1-4E1C-81A5-67CEE28DCB95}");
            public static readonly ID ImageId = new ID("{F8A933D8-0929-4613-A508-3BC141C66F8F}");
            public static readonly string StatesListing = "StatesList";
            public static readonly string SalesRepListing = "SalesRepList";
            public static readonly string SalesRepImage = "SalesRepImage";
        }
        
    }
}