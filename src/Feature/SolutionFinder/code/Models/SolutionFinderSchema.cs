using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CGP.Feature.SolutionFinder.Models
{
    public class SolutionFinderSchema
    {
        public string SolutionFinderJson { get; set; }
        public string BreadCrumbTemplate { get; set; }
        public string QuestionTemplate { get; set; }
        public string AnswerTemplate { get; set; }
    }
}