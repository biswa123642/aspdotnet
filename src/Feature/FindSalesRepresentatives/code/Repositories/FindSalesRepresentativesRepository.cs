using CGP.Feature.FindSalesRepresentatives.Models;
using CGP.Foundation.ErrorModule.Repositiories;
using CGP.Foundation.SitecoreExtensions.Utilities;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using Sitecore.XA.Foundation.RenderingVariants.Repositories;
using ItemUtil = CGP.Foundation.SitecoreExtensions.Utilities.ItemUtil;
using Sitecore.Data.Fields;
using System.Linq;

namespace CGP.Feature.FindSalesRepresentatives.Repositories
{
    public class FindSalesRepresentativesRepository : VariantsRepository, IFindSalesRepresentativesRepository
    {
        private readonly ILogger logger;

        public FindSalesRepresentativesRepository(ILogger logger)
        {
            this.logger = logger;
        }
        public FindSalesRepresentativesViewModel GetSalesRepresentativesDetails()
        {
            FindSalesRepresentativesViewModel salesRepViewModel = new FindSalesRepresentativesViewModel
            {
                FindSalesRepModel = new List<FindSalesRepDetailsModel>()
            };

            FillBaseProperties(salesRepViewModel);
            List<Item> statesList = new List<Item>();
            try
            {
                var dataSourceId = Sitecore.Mvc.Presentation.RenderingContext.CurrentOrNull.Rendering.DataSource;
                var findSalesRepDataItem = Sitecore.Context.Database.GetItem(dataSourceId);
                MultilistField selectedStatesList = findSalesRepDataItem.Fields[Constants.FindSalesRep.StatesListing];

                statesList = selectedStatesList.GetItems().ToList();
                if (statesList != null)
                {
                    foreach (Item stateItem in statesList)
                    {
                        MultilistField selectedSalesRepList = stateItem.Fields[Constants.FindSalesRep.SalesRepListing];
                        var salesRepItem = selectedSalesRepList.GetItems().ToList().First();
                        if (salesRepItem != null)
                        {
                            FindSalesRepDetailsModel salesRepDetails = new FindSalesRepDetailsModel()
                            {
                                StateCode = ItemUtil.GetFieldValue(stateItem, Constants.FindSalesRep.StateCodeId),
                                StateName = ItemUtil.GetFieldValue(stateItem, Constants.FindSalesRep.StateNameId),
                                Name = ItemUtil.GetFieldValue(salesRepItem, Constants.FindSalesRep.NameId),
                                Email = ItemUtil.GetFieldValue(salesRepItem, Constants.FindSalesRep.EmailId),
                                Phone = ItemUtil.GetFieldValue(salesRepItem, Constants.FindSalesRep.PhoneId),
                                ImageURL = HelperExtension.GetUrlFromImageField(salesRepItem, Constants.FindSalesRep.SalesRepImage)
                            };
                            salesRepViewModel.FindSalesRepModel.Add(salesRepDetails);
                        }
                    }
                }
                salesRepViewModel.JSONData = Newtonsoft.Json.JsonConvert.SerializeObject(salesRepViewModel.FindSalesRepModel);
            }
            catch (Exception ex)
            {
                logger.LogError("ERROR occured in FindSalesRepresentativesRepository.GetSalesRepresentativesDetails() ", ex);
            }
            return salesRepViewModel;
        }
    }
}