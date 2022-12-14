using System;
using CGP.Foundation.ErrorModule.Repositiories;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Links.UrlBuilders;
using Sitecore.Resources.Media;

namespace CGP.Foundation.SitecoreExtensions.Utilities
{
    public static class FieldUtil
    {
        public static string ImageUrl(this ImageField imageField)
        {
            if (imageField?.MediaItem == null)
            {
                throw new ArgumentNullException(nameof(imageField));
            }

            var options = MediaUrlBuilderOptions.Empty;
            int width, height;

            if (int.TryParse(imageField.Width, out width))
            {
                options.Width = width;
            }

            if (int.TryParse(imageField.Height, out height))
            {
                options.Height = height;
            }
            return imageField.ImageUrl(options);
        }

        public static string ImageUrl(this ImageField imageField, MediaUrlBuilderOptions options)
        {
            if (imageField?.MediaItem == null)
            {
                throw new ArgumentNullException(nameof(imageField));
            }

            return options == null ? imageField.ImageUrl() : HashingUtils.ProtectAssetUrl(MediaManager.GetMediaUrl(imageField.MediaItem, options));
        }

        public static bool IsChecked(this Field checkboxField)
        {
            if (checkboxField == null)
            {
                throw new ArgumentNullException(nameof(checkboxField));
            }
            return MainUtil.GetBool(checkboxField.Value, false);
        }

        public static bool IsChecked(Item item, string fieldName)
        {
            CheckboxField checkboxField = item.Fields[fieldName];
            return (checkboxField != null && checkboxField.Checked) ? true : false;
        }

        public static string GetLookupItem(Item item, ID fieldId)
        {
            ILogger logger = new Logger();
            string target = string.Empty;
            try
            {
                ReferenceField linkField = item.Fields[fieldId];
                if (linkField != null && linkField.TargetItem != null)
                {
                    target = linkField.TargetItem.Name;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("ERROR in FieldUtil.GetLookupItem() ", ex);
            }
            return target;
        }
    }
}