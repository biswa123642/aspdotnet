using CGP.Feature.SolutionFinder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sitecore.Mvc;
using Sitecore.Mvc.Presentation;
using Sitecore.Data.Items;
//using Sitecore.Data.Items;

namespace CGP.Feature.SolutionFinder.Controllers
{
    public class SolutionFinderController : Controller
    {
        // GET: SinglePageSolutionFinder
        public ActionResult GetSolutionFinderData()
        {
            try
            {
                var dataSourceId = Sitecore.Mvc.Presentation.RenderingContext.CurrentOrNull.Rendering.DataSource;
                var solutionFinderDataItem = Sitecore.Context.Database.GetItem(dataSourceId);
                var model = new SolutionFinderSchema();
                //var x = articleListItem.Fields["QuestionData"].Value.ToString();
                if (solutionFinderDataItem!=null)
                {
                    model = new SolutionFinderSchema()
                    {
                        SolutionFinderJson = solutionFinderDataItem.Fields["QuestionData"].ToString(),
                        BreadCrumbTemplate = solutionFinderDataItem.Fields["BreadCrumbTemplate"].ToString(),
                        QuestionTemplate = solutionFinderDataItem.Fields["QuestionTemplate"].ToString(),
                        AnswerTemplate = solutionFinderDataItem.Fields["AnswerTemplate"].ToString(),
                    };
                }                
                return View("~/Views/OneWeb/SolutionFinder/SolutionFinder.cshtml", model);
            }
            catch(Exception ex)
            {
                return View("An error has occured in Solution Finder Component - " + ex);
            }
            
        }
    }
}