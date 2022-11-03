using System;
using Sitecore.Configuration;
using Sitecore.Data.Items;
using Sitecore.Resources.Media;
using Sitecore.Diagnostics;
using CGP.Foundation.SitecoreExtensions.Utilities;

namespace CGP.Foundation.SitecoreExtensions.Pipelines.MediaValidation
{
    public class ExtendedMediaRequestHandler : MediaRequestHandler
    {
        const string MEDIA_URL_BASE = "-/media/";
        const string MEDIA_PATH_BASE = "sitecore/media library/";

        public override void ProcessRequest(System.Web.HttpContext context)
        {
            MediaRequest request = MediaManager.ParseMediaRequest(context.Request);

            if (request != null)
            {
                Media media = MediaManager.GetMedia(request.MediaUri);

                if (media != null)
                {
                    MediaItem mediaItem = media.MediaData.MediaItem;

                    if (mediaItem != null)
                    {
                        if (!IsValidExtension(request.InnerRequest.FilePath, mediaItem.Extension, Settings.Media.RequestExtension))
                        {
                            //string newPath = request.MediaUri.MediaPath + "." + mediaItem.Extension;
                            string newUrl;
                            bool success;

                            newUrl = CreateMediaUrl(request.MediaUri.MediaPath, mediaItem.Extension, context.Request.Url.Query, out success);

                            if (success)
                            {
                                HttpResponseHelper.RedirectPermanent301(newUrl);
                            }
                            else
                            {
                                Log.Info("Unable to rewrite mismatched media item extension.  Expected extension: \"" + mediaItem.Extension + "\" But got: \"" + request.InnerRequest.CurrentExecutionFilePathExtension + "\" From URL: \"" + context.Request.RawUrl + "\"", this);
                            }
                        }
                    }
                }
            }
            base.ProcessRequest(context);
        }

        private static bool IsValidExtension(string requestExtension, string mediaExtension, string defaultMediaExtension)
        {
            //If the url extension matches the media object extension then you have a match
            if (!String.IsNullOrEmpty(mediaExtension)
                && requestExtension.EndsWith(mediaExtension,
                    StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            //If the url extension matches the default extension then accpet it as a match
            if (!String.IsNullOrEmpty(defaultMediaExtension)
                && requestExtension.EndsWith(defaultMediaExtension,
                    StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }

        private static string CreateMediaUrl(string filePath, string extension, string queryString, out bool success)
        {
            string newUrl = "";

            newUrl = GetMediaUrl(filePath, out success);
            newUrl += "." + extension;
            newUrl += queryString;

            return newUrl;

        }
        private static string GetMediaUrl(string filePath, out bool success)
        {
            string newUrl;

            if (filePath.Contains(MEDIA_PATH_BASE))
            {
                success = true;
            }
            else
            {
                success = false;
            }

            newUrl = filePath.Replace(MEDIA_PATH_BASE, MEDIA_URL_BASE);

            return newUrl;
        }
    }
}
