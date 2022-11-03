using CGP.Feature.ProductAttributeListing.Models;
using CGP.Foundation.ErrorModule.Repositiories;
using CGP.Foundation.SitecoreExtensions.Utilities;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using Sitecore.XA.Foundation.RenderingVariants.Repositories;
using ItemUtil = CGP.Foundation.SitecoreExtensions.Utilities.ItemUtil;
using CGP.Feature.ProductAttributeListing.Repositories;
using Sitecore.Data.Fields;
using System.Linq;
using Sitecore.Mvc.Presentation;
using Sitecore.Data;

namespace CGP.Feature.ProductAttributeListing.Repositories
{
    public class ProductAttributeListingRepository : VariantsRepository, IProductAttributeListingRepository
    {
        private readonly ILogger logger;
        public ProductAttributeListingRepository(ILogger logger)
        {
            this.logger = logger;
        }
        public ProductAttributeListingViewModel GetProductAttributeListingDetails()
        {
            ProductAttributeListingViewModel productAttributeModel = new ProductAttributeListingViewModel();
            try
            {
                FillBaseProperties(productAttributeModel);
                productAttributeModel.ProductAttributesList = new List<Item>();
                productAttributeModel.ProductAttributesList = GetProductAttributeListing(Sitecore.Context.Item);
                string productAttributeValue = RenderingContext.Current.Rendering.Parameters[RenderingParameters.Attributes] != null ? RenderingContext.Current.Rendering.Parameters[RenderingParameters.Attributes].ToString() : string.Empty;
                ID productAttributeGUID = string.IsNullOrWhiteSpace(productAttributeValue) ? null : (new ID(productAttributeValue));
                Item attributeItem = string.IsNullOrWhiteSpace(productAttributeValue) ? null : Sitecore.Context.Database.GetItem(productAttributeGUID);
                productAttributeModel.FilteredProductAttributeList = new List<Item>();
                if (attributeItem != null)
                {
                    productAttributeValue = attributeItem.Name;
                }
                if (productAttributeValue == ProductAttributes.Helps)
                {
                    foreach (var item in productAttributeModel.ProductAttributesList)
                    {
                        if (item.Parent.Name == productAttributeValue)
                        {
                            productAttributeModel.FilteredProductAttributeList.Add(item);
                        }
                    }
                }
                if (productAttributeValue == ProductAttributes.ChewStyle)
                {
                    var item = productAttributeModel.ProductAttributesList.Where(x => x.Parent.Name == ProductAttributes.ChewStyle).FirstOrDefault();
                    if (item != null)
                    {
                        productAttributeModel.FilteredProductAttributeItem = item;
                    }

                }
            }
            catch (Exception ex)
            {
                logger.LogError("ERROR occured in ProductAttributeListingRepository.GetProductAttributeListingDetails() ", ex);
            }

            return productAttributeModel;
        }
        public List<Item> GetProductAttributeListing(Item contextItem)
        {        
            List<Item> ProductAttributeListing = new List<Item>();
            try
            {
                if (contextItem != null)
                {
                    MultilistField selectedProductAttributeListing = contextItem.Fields[Constants.ProductAttributeListing];

                    ProductAttributeListing = selectedProductAttributeListing.GetItems().ToList();

                }
            }
            catch (Exception ex)
            {
                logger.LogError("ERROR occured in ProductAttributeListingRepository.GetProductAttributeListing() ", ex);
            }
            return ProductAttributeListing;
        }
    }
}
