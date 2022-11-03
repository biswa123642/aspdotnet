using CGP.Feature.ProductVariant.Models;
using CGP.Foundation.ErrorModule.Repositiories;
using CGP.Foundation.SitecoreExtensions.Utilities;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using Sitecore.XA.Foundation.RenderingVariants.Repositories;
using ItemUtil = CGP.Foundation.SitecoreExtensions.Utilities.ItemUtil;
using System.Text.RegularExpressions;

namespace CGP.Feature.ProductVariant.Repositories
{
    public class ProductVariantRepository : VariantsRepository, IProductVariantRepository
    {
        private readonly ILogger logger;

        public ProductVariantRepository(ILogger logger)
        {
            this.logger = logger;
        }
        public ProductVariantViewModel GetProductVariantDetails()
        {
            ProductVariantViewModel productVariantViewModel = new ProductVariantViewModel
            {
                ProductVariantModel = new ProductVariantModel
                {
                    ProductVariantList = new List<ProductVariantDetails>()
                }
            };

            FillBaseProperties(productVariantViewModel);

            try
            {
                var variantGroupingItem = HelperExtension.GetFirstMatchingChildItem(Sitecore.Context.Item, Template.Templates.VariantGroupingTemplateId);
                if (variantGroupingItem != null)
                {
                    productVariantViewModel.ProductVariantModel.PrimaryTitle = ItemUtil.GetFieldValue(variantGroupingItem, Constants.ProductVariant.PrimaryVariationTitleId).Replace(" ", "");
                    productVariantViewModel.ProductVariantModel.SecondryTitle = ItemUtil.GetFieldValue(variantGroupingItem, Constants.ProductVariant.SecondaryVariationTitleId).Replace(" ", "");
                    productVariantViewModel.ProductVariantModel.TertiaryTitle = ItemUtil.GetFieldValue(variantGroupingItem, Constants.ProductVariant.TertiaryVariationTitleId).Replace(" ", "");

                    GetVariantDetails(HelperExtension.GetProductVariants(variantGroupingItem), productVariantViewModel.ProductVariantModel.ProductVariantList);
                }
                productVariantViewModel.JSONData = Newtonsoft.Json.JsonConvert.SerializeObject(productVariantViewModel.ProductVariantModel);
            }
            catch (Exception ex)
            {
                logger.LogError("ERROR in ProductVariantRepository.GetProductVariantDetails() | ", ex);
            }

            return productVariantViewModel;
        }

        private void GetVariantDetails(List<Item> variantList, List<ProductVariantDetails> productVariantList)
        {
            try
            {
                foreach (Item variantItem in variantList)
                {
                    ProductVariantDetails variantSection = new ProductVariantDetails()
                    {
                        Primary = this.FormatedVariantValue(ItemUtil.GetFieldValue(variantItem, Constants.ProductVariant.PrimaryVariationValueId)),
                        PrimaryText = ItemUtil.GetFieldValue(variantItem, Constants.ProductVariant.PrimaryVariationValueId),
                        Secondary = this.FormatedVariantValue(ItemUtil.GetFieldValue(variantItem, Constants.ProductVariant.SecondaryVariationValueId)),
                        SecondaryText = ItemUtil.GetFieldValue(variantItem, Constants.ProductVariant.SecondaryVariationValueId),
                        Tertiary = this.FormatedVariantValue(ItemUtil.GetFieldValue(variantItem, Constants.ProductVariant.TertiaryVariationValueId)),
                        TertiaryText = ItemUtil.GetFieldValue(variantItem, Constants.ProductVariant.TertiaryVariationValueId),
                        VariantSKU = ItemUtil.GetFieldValue(variantItem, Constants.ProductVariant.VariantSKU),
                        VariantUPC = ItemUtil.GetFieldValue(variantItem, Constants.ProductVariant.VariantUPC),
                        VariantDescription = ItemUtil.GetFieldValue(variantItem, Constants.ProductVariant.VariantDescription)
                    };
                    productVariantList.Add(variantSection);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("ERROR in ProductVariantRepository.GetVariantDetails() | ", ex);
            }
        }
        private string FormatedVariantValue(string value)
        {
            return !string.IsNullOrWhiteSpace(value) ? Regex.Replace(value, "[^A-Za-z0-9]", "") : value;
        }
    }
}