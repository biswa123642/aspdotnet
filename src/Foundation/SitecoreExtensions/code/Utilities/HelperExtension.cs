using CGP.Foundation.ErrorModule.Repositiories;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using Sitecore.Resources.Media;
using Sitecore.Sites;
using Sitecore.Web;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CGP.Foundation.SitecoreExtensions.Utilities
{
    public static class HelperExtension
    {
        public static Logger _logger = new Logger();
        /// <summary>
        /// Get Context Site
        /// </summary>
        /// <returns></returns>
        public static SiteContext GetContextSite()
        {
            if (Context.PageMode.IsExperienceEditor || Context.PageMode.IsPreview)
            {
                // item ID for page editor and front-end preview mode
                string id = Sitecore.Web.WebUtil.GetQueryString("sc_itemid");

                // by default, get the item assuming Presentation Preview tool (embedded preview in shell)
                var item = Context.Item;

                // if a query string ID was found, get the item for page editor and front-end preview mode
                if (!string.IsNullOrEmpty(id))
                {
                    item = Context.Database.GetItem(id);
                }

                // loop through all configured sites
                foreach (var site in Sitecore.Configuration.Factory.GetSiteInfoList())
                {
                    // get this site's home page item
                    var homePage = Context.Database.GetItem(site.RootPath + site.StartItem);

                    // if the item lives within this site, this is our context site
                    if (homePage != null && item != null && homePage.Axes.IsAncestorOf(item))
                    {
                        return Sitecore.Configuration.Factory.GetSite(site.Name);
                    }
                }

                // fallback and assume context site
                return Context.Site;
            }
            else
            {
                // standard context site resolution via hostname, virtual/physical path, and port number
                return Context.Site;
            }
        }

        /// <summary>
        /// Get item details by item id or path
        /// </summary>
        /// <param name="itemid">Passing sitecore item id or path</param>
        /// <returns>Return item</returns>
        public static Item GetItem(string itemid)
        {
            try
            {
                if (string.IsNullOrEmpty(itemid) && Context.Database == null)
                { return null; }
                return Context.Database.GetItem(itemid);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in HelperExtension.GetItem()", ex);
                return null;
            }
        }
        /// <summary>
        /// Get sitecore root item
        /// </summary>
        /// <returns>Return sitecore item</returns>
        public static Item GetSiteRootItem()
        {
            return GetItem(Context.Site.RootPath);
        }

        /// <summary>
        /// Get child item by parent item context and template id
        /// </summary>
        /// <param name="item">Passing sitecore parent item</param>
        /// <param name="templateId">Passing child template id</param>
        /// <returns>Return child item</returns>
        public static Item GetFirstMatchingChildItem(Item item, ID templateId)
        {
            try
            {
                return item.Children.Where(x => x.TemplateID.Equals(templateId)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in HelperExtension.GetFirstMatchingChildItem()", ex);
                return null;
            }
        }

        public static Item GetHomeItem()
        {
            return GetItem(Context.Site.StartPath);
        }
        public static string GetHostName()
        {
            return Sitecore.Links.LinkManager.GetItemUrl(GetHomeItem(), new Sitecore.Links.UrlBuilders.ItemUrlBuilderOptions() { AlwaysIncludeServerUrl = true });
        }
        public static Item GetChildSiteSettingItem(ID iD, Item rootItem = null)
        {
            Logger logger = new Logger();
            try
            {

                var currentRootItem = (rootItem != null) ? rootItem : HelperExtension.GetSiteRootItem();

                if (currentRootItem != null)
                {
                    var sxaSiteSettingItem = HelperExtension.GetFirstMatchingChildItem(currentRootItem, Templates.SiteSettingTemplateId);
                    if (sxaSiteSettingItem != null)
                    {
                        var settingItem = HelperExtension.GetFirstMatchingChildItem(sxaSiteSettingItem, iD);
                        return settingItem ?? null;
                    }
                    else
                    { return null; }

                }
                else { return null; }
            }
            catch (Exception ex)
            {
                logger.LogError("ERROR occured in HelperExtension.GetChildSiteSettingItem() ", ex);
                return null;
            }
        }
        /// <summary>
        ///  Gets product variants on the basis of variant grouping item
        /// </summary>
        /// <param name="variantGroupingItem"></param>
        /// <returns></returns>
        public static List<Item> GetProductVariants(Item variantGroupingItem)
        {
            Logger logger = new Logger();
            List<Item> variantList = new List<Item>();
            try
            {
                if (variantGroupingItem != null)
                {
                    MultilistField selectedProductVariant = variantGroupingItem.Fields[Constants.ChooseVariant];

                    if (selectedProductVariant.GetItems().Length < 1)
                    {
                        variantList = variantGroupingItem.Children.ToList();
                    }
                    else
                    {
                        variantList = selectedProductVariant.GetItems().ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError("ERROR occured in HelperExtension.GetProductVariants() ", ex);
            }
            return variantList;
        }
        /// <summary>
        /// Get the image URL from first level, if not found take it from OpenGraphImageUrl or else from fall back image field
        /// </summary>
        /// <param name="item">content item</param>
        /// <param name="fieldName"> fieldName</param>
        /// <returns></returns>
        public static string GetUrlFromImageField(Item item, string fieldName)
        {
            string mediaUrl = string.Empty;
            try
            {
                if (item != null && !string.IsNullOrWhiteSpace(fieldName))
                {
                    mediaUrl = GetMediaUrl(item, fieldName);
                    mediaUrl = (string.IsNullOrWhiteSpace(mediaUrl)) ? GetMediaUrl(item, Constants.OpenGraphImageUrl) : mediaUrl;
                    if (string.IsNullOrWhiteSpace(mediaUrl))
                    {
                        mediaUrl = GetNoImageUrl(item);
                    }
                }
                else
                {
                    mediaUrl = GetNoImageUrl(item);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("ERROR in HelperExtension.GetUrlFromImageField() |", ex);
            }
            return mediaUrl;
        }

        public static string GetMediaUrl(Item item, string fieldName)
        {
            string mediaUrl = string.Empty;
            try
            {
                ImageField imageField = item.Fields[fieldName];
                if (imageField != null && imageField.MediaItem != null)
                {
                    mediaUrl = MediaManager.GetMediaUrl(imageField.MediaItem);
                    mediaUrl = mediaUrl.Contains("/sitecore/shell") ? mediaUrl.Replace("/sitecore/shell", string.Empty) : mediaUrl;
                    mediaUrl = HashingUtils.ProtectAssetUrl(mediaUrl);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("ERROR in HelperExtension.GetMediaUrl() | ", ex);
            }
            return mediaUrl;
        }

        public static string GetNoImageUrl(Item item)
        {
            string mediaUrl = string.Empty;
            try
            {
                var rootItem = item.Database.GetItem(GetSiteInfo(item).RootPath);
                Item siteConfiguration = GetChildSiteSettingItem(Templates.SiteConfiguration.Id, rootItem);
                if (siteConfiguration != null)
                {
                    mediaUrl = GetMediaUrl(siteConfiguration, Templates.SiteConfiguration.Fields.NoImageIdString);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("ERROR in HelperExtension.GetNoImageUrl() | ", ex);
            }
            return mediaUrl;
        }
        public static SiteInfo GetSiteInfo(this Item item)
        {
            return SiteContextFactory.Sites
              .Where(s => !string.IsNullOrWhiteSpace(s.RootPath) && item.Paths.Path.StartsWith(s.RootPath, StringComparison.OrdinalIgnoreCase))
              .OrderByDescending(s => s.RootPath.Length)
              .FirstOrDefault();
        }
        public static string GetMediaItemAlt(Item item, string fieldName)
        {
            if (item != null && !string.IsNullOrWhiteSpace(fieldName))
            {
                ImageField imageField = item.Fields[fieldName];
                if (imageField != null && imageField.MediaItem != null)
                {
                    return imageField.Alt;
                }
            }

            return string.Empty;
        }

        public static string GetValueFromCurrentRenderingParameters(string parameterName)
        {
            var rc = RenderingContext.CurrentOrNull;
            if (rc == null || rc.Rendering == null) return (string)null;
            return rc.Rendering.Parameters[parameterName];
        }
    }
}