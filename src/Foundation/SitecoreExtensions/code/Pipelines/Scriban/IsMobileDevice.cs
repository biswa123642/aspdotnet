using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;
using System.Web;
using Scriban.Runtime;
using Sitecore.XA.Foundation.Scriban.Pipelines.GenerateScribanContext;
using static Sitecore.Configuration.State;
using System.Text.RegularExpressions;

namespace CGP.Foundation.SitecoreExtensions.Pipelines.Scriban
{
    public class IsMobileDevice
    {
        public void Process(GenerateScribanContextPipelineArgs args)
        {
            var getDisplayType = new delegateGetSetting(IsMobileView);
            args.GlobalScriptObject.Import("sc_isMobileDevice", getDisplayType);
        }
        public bool IsMobileView()
        {
            bool isMobileDevice = false;
            HttpRequest httpRequest = HttpContext.Current.Request;
            if (httpRequest.Browser.IsMobileDevice)
            {
                isMobileDevice = true;
            }
            return isMobileDevice;
        }

        private delegate bool delegateGetSetting();
    }
}