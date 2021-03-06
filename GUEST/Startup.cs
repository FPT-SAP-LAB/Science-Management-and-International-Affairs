﻿using BLL;
using BLL.ModelDAL;
using GUEST.Support;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;
using System.Collections.Generic;
using System.Configuration;

[assembly: OwinStartup(typeof(GUEST.Startup))]
namespace GUEST
{
    public class Startup
    {
        public readonly static string GuestURI = ConfigurationManager.AppSettings["GuestURI"];
        public readonly static string ManagerURI = ConfigurationManager.AppSettings["ManagerURI"];
        public readonly static HashSet<string> Staffs = ImportStaff.LoadMail();
        public static string BackgroundURL;
        public void Configuration(IAppBuilder app)
        {
            NotificationRepo.GuestURI = GuestURI;
            NotificationRepo.ManagerURI = ManagerURI;
            HomeRepo homeRepo = new HomeRepo();
            BackgroundURL = homeRepo.GetBackground();
            BackgroundURL = BackgroundURL ?? "../Content/assets/media/bg/bg-10.jpg";
            // Branch the pipeline here for requests that start with "/signalr"
            app.Map("/signalr", map =>
            {
                // Setup the CORS middleware to run before SignalR.
                // By default this will allow all origins. You can 
                // configure the set of origins and/or http verbs by
                // providing a cors options with a different policy.
                map.UseCors(CorsOptions.AllowAll);
                var hubConfiguration = new HubConfiguration
                {
                    // You can enable JSONP by uncommenting line below.
                    // JSONP requests are insecure but some older browsers (and some
                    // versions of IE) require JSONP to work cross domain
                    // EnableJSONP = true
                };
                // Run the SignalR pipeline. We're not using MapSignalR
                // since this branch already runs under the "/signalr"
                // path.
                map.RunSignalR(hubConfiguration);
            });
        }
    }
}