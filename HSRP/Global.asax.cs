using SharpRaven;
using SharpRaven.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
namespace HSRP
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            var ravenClient = new RavenClient("https://488ae36f6a9c43fabf7961d7266aa3df@o1199066.ingest.sentry.io/6350069");
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            //Addded by Ashok - 5-Aug-22
            //10 = Minutes
            Session.Timeout = 10;
        }
        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            var ravenClient = new RavenClient("https://488ae36f6a9c43fabf7961d7266aa3df@o1199066.ingest.sentry.io/6350069");
            var gotUtl = HttpContext.Current.Request.Url.AbsolutePath.ToString();
            if (HttpContext.Current.Session != null)
            {

                if (HttpContext.Current.Session["UID"] != null)
                {
                    var sessionGot = HttpContext.Current.Session["UID"].ToString();
                    if (!string.IsNullOrEmpty(sessionGot))
                    {
                        var dictionary = new Dictionary<string, string>();
                        dictionary.Add("UserId", sessionGot);
                        ravenClient.CaptureMessage(gotUtl, ErrorLevel.Info, dictionary);

                    }
                    else
                    {
                        ravenClient.CaptureMessage(gotUtl, ErrorLevel.Info);
                    }
                }
                else
                {
                    ravenClient.CaptureMessage(gotUtl, ErrorLevel.Info);
                }
            }

            else
            {
                ravenClient.CaptureMessage(gotUtl, ErrorLevel.Info);
            }
        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            //var ravenClient = new RavenClient("https://488ae36f6a9c43fabf7961d7266aa3df@o1199066.ingest.sentry.io/6350069");
            //var gotUtl = HttpContext.Current.Request.Url.AbsolutePath.ToString();
            //if (HttpContext.Current.Session != null)
            //{

            //    if (HttpContext.Current.Session["UID"] != null)
            //    {
            //        var sessionGot = HttpContext.Current.Session["UID"].ToString();
            //        if (!string.IsNullOrEmpty(sessionGot))
            //        {
            //            var dictionary = new Dictionary<string, string>();
            //            dictionary.Add("UserId", sessionGot);
            //            ravenClient.CaptureMessage(gotUtl, ErrorLevel.Info, dictionary);

            //        }
            //        else
            //        {
            //            ravenClient.CaptureMessage(gotUtl, ErrorLevel.Info);
            //        }
            //    }
            //    else
            //    {
            //        ravenClient.CaptureMessage(gotUtl, ErrorLevel.Info);
            //    }
            //}

            //else
            //{
            //    ravenClient.CaptureMessage(gotUtl, ErrorLevel.Info);
            //}
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var ravenClient = new RavenClient("https://488ae36f6a9c43fabf7961d7266aa3df@o1199066.ingest.sentry.io/6350069");
            var error = Server.GetLastError();
            ravenClient.Capture(new SentryEvent(error));
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}