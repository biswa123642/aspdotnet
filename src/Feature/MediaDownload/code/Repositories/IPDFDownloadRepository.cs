using CGP.Feature.MediaDownload.Models;

namespace CGP.Feature.MediaDownload.Repositories
{
    public interface IPDFDownloadRepository
    {
        MediaStreamModel GetPDFStream(string mediaItemID, string db);
    }
}
