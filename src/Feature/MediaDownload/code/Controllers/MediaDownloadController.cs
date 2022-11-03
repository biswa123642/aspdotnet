using CGP.Feature.MediaDownload.Repositories;
using CGP.Foundation.ErrorModule.Repositiories;
using Sitecore.XA.Foundation.Mvc.Controllers;
using System;
using System.Web.Mvc;

namespace CGP.Feature.MediaDownload.Controllers
{
    public class MediaDownloadController : StandardController
    {
        private readonly IPDFDownloadRepository _pdfDownloadRepository;
        public readonly ILogger _logger;

        public MediaDownloadController(IPDFDownloadRepository pdfDownloadRepository, ILogger logger)
        {
            this._pdfDownloadRepository = pdfDownloadRepository;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult DownloadPdf()
        {
            return RedirectToAction("GetPDF", new { mediaItemID = Sitecore.Context.Item.ID, db = Sitecore.Context.Item.Database });
        }

        [HttpGet]
        public ActionResult GetPDF(string mediaItemID, string db)
        {
            try
            {
                var output = _pdfDownloadRepository.GetPDFStream(mediaItemID, db);
                if (output != null && output.FileStream != null)
                {
                    return File(output.FileStream, "application/octet-stream", string.Format("{0}.pdf", output.FileName));
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("ERROR occured in MediaDownloadController.GetPDF() ", ex);
                return null;
            }
        }
    }
}