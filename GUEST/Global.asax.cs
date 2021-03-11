using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ENTITIES.CustomModels;

namespace GUEST
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //using (var stream =
            //    new FileStream(HostingEnvironment.MapPath("/credentials.json"), FileMode.Open, FileAccess.Read))
            //{
            //    string[] Scopes = { DriveService.Scope.DriveFile };
            //    // The file token.json stores the user's access and refresh tokens, and is created
            //    // automatically when the authorization flow completes for the first time.
            //    GlobalCredential.credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
            //        GoogleClientSecrets.Load(stream).Secrets,
            //        Scopes,
            //        "application",
            //        CancellationToken.None).Result;
            //}
        }
    }
}
