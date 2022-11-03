using CGP.Feature.SalesRepresentativeMap.Models;
using CGP.Foundation.SitecoreExtensions.Repositories;
using CGP.Foundation.SitecoreExtensions.Utilities;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.XA.Foundation.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ItemUtil = CGP.Foundation.SitecoreExtensions.Utilities.ItemUtil;

namespace CGP.Feature.SalesRepresentativeMap.Controllers
{
    public class SalesRepresentativeMapController : StandardController
    {
        private readonly ISiteConfiguration siteConfiguration;
        public SalesRepresentativeMapController(ISiteConfiguration siteConfiguration)
        {
            this.siteConfiguration = siteConfiguration;
        }
        public ActionResult SalesRepresentativeMap()
        {
            try
            {
                SalesRepresentativeMapModel salesRepresentativeMapModel = new SalesRepresentativeMapModel();

                var dataSourceId = Sitecore.Mvc.Presentation.RenderingContext.CurrentOrNull.Rendering.DataSource;
                var salesRepMapDataItem = Sitecore.Context.Database.GetItem(dataSourceId);

                if (salesRepMapDataItem != null)
                {
                    if (salesRepMapDataItem.Fields[Constants.SalesRepresentativeMap.Content] != null)
                    {
                        salesRepresentativeMapModel.Content = salesRepMapDataItem.Fields[Constants.SalesRepresentativeMap.Content].Value;
                    }

                    StateModel stateModel;
                    List<StateModel> stateModelList = new List<StateModel>();
                    List<Item> statesList = salesRepMapDataItem.GetChildren().ToList();
                    if (statesList != null)
                    {
                        foreach (var state in statesList)
                        {
                            stateModel = new StateModel();

                            if (state.Fields[Constants.SalesRepresentativeMap.StateName] != null)
                            {
                                stateModel.StateName = state.Fields[Constants.SalesRepresentativeMap.StateName].Value;
                            }
                            if (state.Fields[Constants.SalesRepresentativeMap.StateCode] != null)
                            {
                                stateModel.StateCode = state.Fields[Constants.SalesRepresentativeMap.StateCode].Value;
                            }
                            MultilistField selectedSalesRepList = state.Fields[Constants.SalesRepresentativeMap.SalesRepListing];
                            var salesRepItemList = selectedSalesRepList.GetItems().ToList();

                            List<SalesRepModel> salesRepModelList = new List<SalesRepModel>();
                            if (salesRepItemList != null)
                            {
                                foreach (var salesRep in salesRepItemList)
                                {
                                    var salesRepModel = new SalesRepModel();
                                    if (salesRep.Fields[Constants.SalesRepresentativeMap.SalesTeamName] != null)
                                    {
                                        salesRepModel.Name = salesRep.Fields[Constants.SalesRepresentativeMap.SalesTeamName].Value;
                                    }
                                    if (salesRep.Fields[Constants.SalesRepresentativeMap.SalesTeamPhone] != null)
                                    {
                                        salesRepModel.Phone = salesRep.Fields[Constants.SalesRepresentativeMap.SalesTeamPhone].Value;
                                    }
                                    if (salesRep.Fields[Constants.SalesRepresentativeMap.SalesTeamEmail] != null)
                                    {
                                        salesRepModel.Email = salesRep.Fields[Constants.SalesRepresentativeMap.SalesTeamEmail].Value;
                                    }
                                    if (salesRep.Fields[Constants.SalesRepresentativeMap.SalesRepImage] != null)
                                    {
                                        salesRepModel.ImageURL = HelperExtension.GetMediaUrl(salesRep, Constants.SalesRepresentativeMap.SalesRepImage);
                                    }
                                    if (salesRep.Fields[Constants.SalesRepresentativeMap.SalesRepTitle] != null)
                                    {
                                        salesRepModel.SalesRepTitle = salesRep.Fields[Constants.SalesRepresentativeMap.SalesRepTitle].Value;
                                    }
                                    if (salesRep.Fields[Constants.SalesRepresentativeMap.SalesRepDescription] != null)
                                    {
                                        salesRepModel.SalesRepDescription = salesRep.Fields[Constants.SalesRepresentativeMap.SalesRepDescription].Value;
                                    }
                                    salesRepModelList.Add(salesRepModel);
                                }
                            }
                            stateModel.SalesRepModelList = salesRepModelList;

                            stateModelList.Add(stateModel);
                        }
                    }

                    salesRepresentativeMapModel.StateModelList = stateModelList;
                    salesRepresentativeMapModel.JSONData = Newtonsoft.Json.JsonConvert.SerializeObject(salesRepresentativeMapModel.StateModelList);
                    return View("~/Views/OneWeb/SalesRepresentativeMap/SalesRepresentativeMap.cshtml", salesRepresentativeMapModel);
                }
                else
                {
                    return new EmptyResult();
                }
            }
            catch (Exception ex)
            {
                return View("An error has occured in Sales Representative Map Component - " + ex);
            }
        }
    }
}