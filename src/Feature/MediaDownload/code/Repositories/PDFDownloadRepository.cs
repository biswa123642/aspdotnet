using CGP.Feature.MediaDownload.Models;
using CGP.Foundation.ErrorModule.Repositiories;
using System;
using System.IO;

namespace CGP.Feature.MediaDownload.Repositories
{
    public class PDFDownloadRepository : IPDFDownloadRepository
    {
        public readonly ILogger _logger;

        public PDFDownloadRepository(ILogger logger)
        {
            _logger = logger;
        }

        public MediaStreamModel GetPDFStream(string mediaItemID, string db)
        {
            MediaStreamModel mediaStreamModel = new MediaStreamModel();
            Sitecore.Data.Fields.LinkField linkField = Sitecore.Configuration.Factory.GetDatabase(db).GetItem(mediaItemID).Fields["Link"];

            try
            {
                if (linkField != null && linkField.LinkType.ToLower().Equals("internal") && linkField.TargetItem != null)
                {
                    Stream mediaStream = new Sitecore.Data.Items.MediaItem(linkField.TargetItem).GetMediaStream();
                    if (mediaStream != null)
                    {
                        var fileSize = mediaStream.Length;
                        byte[] buffer = new byte[(int)fileSize];
                        mediaStream.Read(buffer, 0, (int)mediaStream.Length);
                        mediaStream.Close();

                        mediaStreamModel = new MediaStreamModel()
                        {
                            FileName = linkField.TargetItem.Name,
                            FileStream = buffer
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("ERROR occured in PDFDownloadRepository.GetPDFStream() ", ex);
            }

            return mediaStreamModel;
        }
    }
}