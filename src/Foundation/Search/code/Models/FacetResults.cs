﻿using System;
using System.Collections.Generic;

namespace CGP.Foundation.Search.Models
{
    public class FacetResults : FacetModel
    {
        public ICollection<FacetValue> FacetValues { get; set; }
    }

    public class FacetValue
    {
        public string Key { get; set; }
        public int Value { get; set; }
        public bool IsActive { get; set; }
    }
}